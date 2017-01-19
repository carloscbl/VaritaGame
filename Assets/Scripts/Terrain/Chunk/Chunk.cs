using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum cubeFlags
{
    cf_none = 0,
    cf_immovable = 1 << 0,
    cf_physicalizable = 1 << 1,
    cf_background = 1 << 2
}

public struct ChunkData
{
    public Vector2 worldPos;
    public ushort localChunkId;
    public bool active;
    public List<cube> cubes;
    public List<Material> materials;
}

public class cube
{
    public cubeFlags flags;
    public uint id;
    public uint inChunkPosition;
    public Mesh geometry;
    public Material material;
}

public class Chunk
{
    private uint Totalcubes = 0;
    private bool active = false;
    private Mesh geometries;
    private List<Material> materials;
    private List<cube> chunkCubes;
    private Vector2 worldPos;
    private uint myChunkID;
    private GameObject gameObject;

    public uint getID() { return myChunkID; }
    public bool getActiveStatus() { return active; }
    public Vector2 getWorldPosition() { return worldPos; }

    List<CombineInstance> combineList = new List<CombineInstance>();
    List<GameObject> ListOfGOs = new List<GameObject>();
    Mesh CollMesh = new Mesh();
    Mesh aa = new Mesh();
    Mesh tempMesh = new Mesh();

    CombineInstance combine = new CombineInstance();
    List<CombineInstance> air = new List<CombineInstance>();
    List<CombineInstance> rock = new List<CombineInstance>();
    List<CombineInstance> sand = new List<CombineInstance>();
    List<CombineInstance> grass = new List<CombineInstance>();
    List<CombineInstance> water = new List<CombineInstance>();
    List<CombineInstance> bedrock = new List<CombineInstance>();

    List<List<CombineInstance>> MaterialsCombined = new List<List<CombineInstance>>();

    List<CombineInstance> finalMesh = new List<CombineInstance>();
    List<CombineInstance> ColliderMesh = new List<CombineInstance>();
    CombineInstance cIns = new CombineInstance();
    //Mesh CombinedMeshSubMeshes = new Mesh();
    List<Vector2> temp = new List<Vector2>();
    List<cube> cubes;
    List<int> Materiales = new List<int>();
    List<int> UsedMats = new List<int>();

    public void updateMesh(Vector2 PositionToUpdate)
    {
        temp.Clear();
        temp.Add(PositionToUpdate);
        cubes = getCubesFrom(temp);
        cubes[0].material = TerrainMaterial.Instance.GetMaterial(0);//Air

        //Now the real mesh update
        List<int> UsedMats;// = new List<int>();
        composeGeometry(chunkCubes, out UsedMats, true);

        gameObject.GetComponent<MeshFilter>().mesh = geometries;
        gameObject.GetComponent<MeshCollider>().sharedMesh = CollMesh;
        gameObject.GetComponent<MeshRenderer>().materials = getMats(UsedMats);
    }

    private List<cube> getCubesFrom(List<Vector2> PositionsToUpdate)
    {
        List<cube> toReturn = new List<cube>();
        float collX;
        float rowY;
        int cubeID;
        foreach (Vector2 vec2 in PositionsToUpdate)
        {
            Vector2 tempLoc = vec2 - worldPos;
            collX = (tempLoc.x * 1 / TerrainSystem.sizeCubesX) + 1;
            rowY = (tempLoc.y * 1 / TerrainSystem.sizeCubesX) + 1;//(((0.5f * 100 / TerrainSystem.multiplierFormX)/100) * .5f)  CHANGE THIS IF YOU MODIFIE THE CUBE SIZE
            cubeID = ((((int)rowY) - 1) * TerrainSystem.cubesForChunk) + (((int)collX) - 1);
            toReturn.Add(chunkCubes[cubeID]);
        }
        return toReturn;
    }

    //Para construir el objeto ncesitamos, tener los datos minimos para construir el mesh
    public Chunk(ChunkData myDataChunk)
    {
        worldPos = myDataChunk.worldPos;
        myChunkID = myDataChunk.localChunkId;
        chunkCubes = myDataChunk.cubes;
        List<int> Materiales;
        composeGeometry(myDataChunk.cubes, out Materiales, false);
        gameObject = instatiateGameObject(CollMesh, Materiales);
    }

    private void composeGeometry(List<cube> cubeList, out List<int> MaterialesUsados, bool update)
    {
        //Reconstruct the mesh from the private chunkcubes
        int row = 0;
        int col = 0;

        air.Clear();
        rock.Clear();
        sand.Clear();
        grass.Clear();
        water.Clear();
        bedrock.Clear();
        CollMesh.Clear();
        ColliderMesh.Clear();
        aa.Clear();
        MaterialesUsados = new List<int>();
        MaterialsCombined.Clear();

        for (int i = 0; i < TerrainMaterial.Instance.TotalMaterials(); i++)
        {
            MaterialsCombined.Add(new List<CombineInstance>());
        }
        for (int i = 0; i < chunkCubes.Count; i++)
        {
            if (update)
            {
                combine.mesh = chunkCubes[i].geometry;
            }
            else
            {
                if (col == TerrainSystem.cubesForChunk) { row++; col = 0; }
                aa = meshCube.doCube(new Vector2(worldPos.x + (col * TerrainSystem.sizeCubesX), worldPos.y + (row * TerrainSystem.sizeCubesY)));
                combine.mesh = aa;
                chunkCubes[i].geometry = aa;
            }
            int pos;
            if ((pos = TerrainMaterial.Instance.GetMaterialPosition(chunkCubes[i].material)) != -1)
            {
                MaterialsCombined[pos].Add(combine);
            }
            col++;
        }
        #region Material_Conditions
        //MaterialsCombined
        int couter = 0;
        finalMesh.Clear();
        foreach (List<CombineInstance> item in MaterialsCombined)
        {
            if (item.Count != 0)
            {
                Mesh CombinedMeshSubMeshes = new Mesh();
                CombinedMeshSubMeshes.CombineMeshes(item.ToArray(), true, false);
                cIns.mesh = CombinedMeshSubMeshes;
                finalMesh.Add(cIns);
                switch (couter)
                {
                    case 0://Air
                        MaterialesUsados.Add(0);
                        break;
                    case 1://Air
                        MaterialesUsados.Add(1);
                        ColliderMesh.Add(cIns);
                        break;
                    case 2://Air
                        MaterialesUsados.Add(2);
                        ColliderMesh.Add(cIns);
                        break;
                    case 3://Air
                        MaterialesUsados.Add(3);
                        ColliderMesh.Add(cIns);
                        break;
                    case 4://Air
                        MaterialesUsados.Add(4);//water
                        break;
                    case 5://Air
                        MaterialesUsados.Add(5);
                        ColliderMesh.Add(cIns);
                        break;
                }
            }
            couter++;
        }
        #endregion
        tempMesh.Clear();
        tempMesh.CombineMeshes(finalMesh.ToArray(), false, false);
        geometries = tempMesh;
        CollMesh.CombineMeshes(ColliderMesh.ToArray(), true, false);
    }

    private GameObject instatiateGameObject(Mesh ColliderMesh, List<int> Materiales)
    {
        GameObject ChunkGO = new GameObject();
        ChunkGO.SetActive(true);
        ChunkGO.name = myChunkID.ToString();
        ChunkGO.AddComponent<MeshFilter>().mesh = geometries;
        ChunkGO.AddComponent<MeshCollider>().sharedMesh = ColliderMesh;
        ChunkGO.AddComponent<MeshRenderer>().materials = getMats(Materiales);
        return ChunkGO;
    }

    private Material[] getMats(List<int> Materiales)
    {
        List<Material> temp = new List<Material>();
        for (int i = 0; i < TerrainMaterial.Instance.TotalMaterials(); i++)
        {
            temp.Add(TerrainMaterial.Instance.GetMaterial(i));
        }
        return temp.ToArray();
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }
}