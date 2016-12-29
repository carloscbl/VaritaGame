using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

[Flags]
public enum cubeFlags
{
    cf_none             = 0,
    cf_immovable        = 1 << 0,
    cf_physicalizable   = 1 << 1,
    cf_background       = 1 << 2
}
public enum cubeMaterial : byte
{
    air,rock,sand,grass,water,bedrock
}


public class cube
    {
        public cubeFlags    flags;
        public uint         id;
        public uint         inChunkPosition;
        public Mesh         geometry;
        public Material     material;
   
}
public struct ChunkData
    {
        public Vector2 worldPos;
        public ushort localChunkId;
        public bool active;
        public List<cube> cubes;
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

    public Chunk()
    {
        //geometries = new <Mesh();
    }

    public GameObject getGameObject(){ return gameObject; }
    public void updateMesh(Vector2 PositionToUpdate)
    {
        temp.Clear();
        //cubes.Clear();
        temp.Add(PositionToUpdate);
        cubes = getCubesFrom(temp);

        //Debug.Log(cubes[0].material.ToString());
        cubes[0].material = TerrainSystem.MaterialsList[0];//Air

        //Now the real mesh update
        //UsedMats.Clear();
        List<int> UsedMats;// = new List<int>();
        composeGeometry(chunkCubes, TerrainSystem.MaterialsList,out UsedMats,true);
        //Debug.Log(chunkCubes.Count);
        //Debug.Log(chunkCubes[5].geometry.ToString());
        //Debug.Log(chunkCubes[5].material);
        
        gameObject.GetComponent<MeshFilter>().mesh = geometries;
        gameObject.GetComponent<MeshCollider>().sharedMesh = CollMesh;
        gameObject.GetComponent<MeshRenderer>().materials = getMats(UsedMats);
        //GC.Collect();

    }
    public void updateMesh(List<Vector2> PositionsToUpdate)
    {

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
            collX   = (tempLoc.x * 1 / TerrainSystem.multiplierFormX) + 1;
            rowY    = (tempLoc.y * 1 /TerrainSystem.multiplierFormX ) + 1;//(((0.5f * 100 / TerrainSystem.multiplierFormX)/100) * .5f)  CHANGE THIS IF YOU MODIFIE THE CUBE SIZE

            cubeID = ((((int)rowY) - 1) * 26) + (((int)collX) - 1);
            Vector2 aa = new Vector2(rowY, collX);
            
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
        
        
        float first = Time.realtimeSinceStartup;
        //List<int> Materiales = new List<int>();
        List<int> Materiales;
        composeGeometry(myDataChunk.cubes, TerrainSystem.MaterialsList,out Materiales, false);
        
        gameObject = instatiateGameObject(CollMesh, Materiales);

        float second = Time.realtimeSinceStartup;
        //Debug.Log((second - first).ToString());
    }
    ~Chunk(){  }
    public void finalize()
    {
        chunkCubes = null;
        //Destroy(this.gameObject);
    }

    public void composeGeometry(List<cube> cubeList, List<Material> MatList,out List<int> MaterialesUsados,bool update)
    {
        //Reconstruct the mesh from the private chunkcubes
        int row = 0;
        int col = 0;
        //20 row 26 col

        air.Clear();
        rock.Clear();
        sand.Clear();
        grass.Clear();
        water.Clear();
        bedrock.Clear();
        CollMesh.Clear();// = new Mesh();
        ColliderMesh.Clear();
        //combine.mesh.Clear();
        //MaterialsCombined.Clear();
        aa.Clear();
        MaterialesUsados = new List<int>();
        MaterialsCombined.Clear();
        

        foreach (var itemfor in TerrainSystem.MaterialsList)
        {

            MaterialsCombined.Add(new List<CombineInstance>());
        }

        int counter = 0;
        for (int i=0; i < chunkCubes.Count; i++)
        {
            
            if (update)
            {
                combine.mesh = chunkCubes[i].geometry;
                //Debug.Log(i);
            }
            else
            {
                if (col == 26) { row++; col = 0; }
                counter++;
                //Vector2 ll = new Vector2(worldPos.x + (col * TerrainSystem.multiplierFormX), worldPos.y + (row * TerrainSystem.multiplierFormY));
                //aa = meshCube.doCube(new Vector2(worldPos.x + (col * TerrainSystem.multiplierFormX), worldPos.y + (row * TerrainSystem.multiplierFormY)),true);
                //aa = meshCube.doCube(new Vector2(worldPos.x + (col - TerrainSystem.multiplierFormX), worldPos.y + (row)));
                if(counter < 3)
                {
                    aa = meshCube.doCube(new Vector2(worldPos.x + (col * TerrainSystem.multiplierFormX), worldPos.y + (row * TerrainSystem.multiplierFormY)));
                }else
                {
                    aa = meshCube.doCube(new Vector2(worldPos.x + (col * TerrainSystem.multiplierFormX), worldPos.y + (row * TerrainSystem.multiplierFormY)));
                }
                combine.mesh = aa;
                chunkCubes[i].geometry = aa;
            }
            
            
            //aa = null;


            if (chunkCubes[i].material == MatList[0])
            {
                MaterialsCombined[0].Add(combine);
                //air.Add(combine);
            }
            else if(chunkCubes[i].material == MatList[1])
            {
                MaterialsCombined[1].Add(combine);
                //rock.Add(combine);
            }
            else if (chunkCubes[i].material == MatList[2])
            {
                //sand.Add(combine);
                MaterialsCombined[2].Add(combine);
            }
            else if (chunkCubes[i].material == MatList[3])
            {
                //grass.Add(combine);
                MaterialsCombined[3].Add(combine);
            }
            else if (chunkCubes[i].material == MatList[4])
            {
                //water.Add(combine);
                MaterialsCombined[4].Add(combine);
            }
            else if(chunkCubes[i].material == MatList[5])
            {
                //bedrock.Add(combine);
                MaterialsCombined[5].Add(combine);
            }
            col++;
            
        }
        /*if (update)
        {
            Debug.Log(sand.Count);
            Debug.Log(water.Count);
            Debug.Log(air.Count);
            Debug.Log(rock.Count);
            Debug.Log(grass.Count);
        }
        */
        #region Material_Conditions
        //MaterialsCombined
        int couter = 0;
        finalMesh.Clear();
        //cIns.mesh.Clear();
        //ColliderMesh.Clear();
        //finalMesh = new List<CombineInstance>();
        foreach (List<CombineInstance> item in MaterialsCombined)
        {
            
            //CombinedMeshSubMeshes.Clear();
            if (item.Count!=0)
            {
                Mesh CombinedMeshSubMeshes = new Mesh();
                
                CombinedMeshSubMeshes.CombineMeshes(item.ToArray(), true, false);
                //Debug.Log( CombinedMeshSubMeshes.vertexCount.ToString());
                //cIns = new CombineInstance();
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
            //CombinedMeshSubMeshes.Clear();
            couter++;
        }

        #endregion
       //Debug.Log(finalMesh.Count);
        /**if (update)
        {
            Debug.Log(finalMesh[0].mesh.vertexCount.ToString());
            Debug.Log(finalMesh[1].mesh.vertexCount.ToString());
        }*/

        tempMesh.Clear();
        tempMesh.CombineMeshes(finalMesh.ToArray(),false,false);
        geometries = tempMesh;
        
        

        CollMesh.CombineMeshes(ColliderMesh.ToArray(), true, false);


        //CombinedMeshSubMeshes = null;


    }
    private GameObject instatiateGameObject(Mesh ColliderMesh, List<int> Materiales)
    {
        GameObject ChunkGO = new GameObject();
        ChunkGO.SetActive(true);
        ChunkGO.name = myChunkID.ToString();
        ChunkGO.AddComponent<MeshFilter>().mesh = geometries;
        ChunkGO.AddComponent<MeshCollider>().sharedMesh = ColliderMesh;
        ChunkGO.AddComponent<MeshRenderer>().materials = getMats(Materiales);
        
        //debug.LogWarning(this.myChunkID.ToString());
        return ChunkGO;

    }
    Material[] getMats(List<int> Materiales)
    {
        List<Material> temp = new List<Material>();
        foreach (var Mat in Materiales)
        {
            temp.Add(TerrainSystem.MaterialsList[Mat]);
        }
        return temp.ToArray();
    }
}

