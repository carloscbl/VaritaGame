using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class ChunkNew : MonoBehaviour
{
    Mesh m_Mesh;
    byte[] data;
    uint iAmNumberOfChunk;
    MeshFilter m_MeshFilter;
    MeshComposer meshGen;
    List<Material> materials;
    List<uint> StackOfChangesToRemove = new List<uint>();
    int counterO = 0;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (counterO == 8)
        {
            if (StackOfChangesToRemove.Count != 0)
            {
                //float time = Time.realtimeSinceStartup;
                for (int i = 0; i < StackOfChangesToRemove.Count; i++)
                {
                    this.data[StackOfChangesToRemove[i]] = 0;
                }
                //print(StackOfChangesToRemove.Count);
                StackOfChangesToRemove.Clear();
                //print("refresco");
                refreshChunk();
                //print(Time.realtimeSinceStartup - time);
            }
            counterO = 0;
        }
        counterO++;
    }
    public void setParameters(byte [] data,uint iAmNumberOfChunk )
    {
        this.data = data;
        this.iAmNumberOfChunk = iAmNumberOfChunk;
        parseData(data);
        init();
    }
    private void parseData(byte[] data)
    {

    }
    private void init()
    {
        AddComponets();
        generateMesh();
        assignMaterials();
        SetLocation(iAmNumberOfChunk);
        gameObject.name = iAmNumberOfChunk.ToString();
    }
    private void AddComponets()
    {
        this.gameObject.AddComponent<MeshFilter>();
        this.gameObject.AddComponent<MeshRenderer>();
        this.gameObject.AddComponent<MeshCollider>();
    }
    private void getMeshGen()
    {
        meshGen = new MeshComposer(data);
    }
    private void generateMesh()
    {
        
        getMeshGen();
        m_Mesh = new Mesh();
        m_Mesh.Clear();

        m_Mesh.SetVertices(meshGen.generatedMeshComponents.vertex);
        m_Mesh.SetNormals(meshGen.generatedMeshComponents.normals);
        m_Mesh.SetUVs(0,meshGen.generatedMeshComponents.uvs);
        m_Mesh.subMeshCount = meshGen.materialsNumberList.Count;
        //print(meshGen.materialsNumberList.Count);
        for (int i = 0; i < meshGen.materialsNumberList.Count; i++)
        {
            m_Mesh.SetTriangles(meshGen.generatedMeshComponents.triangles[i], i);
            //print(meshGen.generatedMeshComponents.triangles[i].Count);
        }
        m_Mesh.RecalculateBounds();
        m_Mesh.RecalculateNormals();
        this.GetComponent<MeshFilter>().sharedMesh = m_Mesh;
        GetComponent<MeshCollider>().sharedMesh = m_Mesh;
    }
    private void assignMaterials()
    {
        materials = new List<Material>();
        for (int i = 0; i < meshGen.materialsNumberList.Count; i++)
        {
            materials.Add(TerrainSystemNew.mat[meshGen.materialsNumberList[i]]);
        }
        this.GetComponent<MeshRenderer>().materials = materials.ToArray();
    }
    private void SetLocation(uint NumberOfChunk)
    {
        float localYNumber = Mathf.Ceil(NumberOfChunk / TerrainSystemNew.chunkColl);
        float y = TerrainSystemNew.rowSize * localYNumber * TerrainSystemNew.cubeSizeMultiplier;
        //print(y);
        float localXNumber = Mathf.Ceil(NumberOfChunk / TerrainSystemNew.chunkRow);
        float x = ((NumberOfChunk - (localYNumber * TerrainSystemNew.chunkColl)) *TerrainSystemNew.cubeSizeMultiplier) * TerrainSystemNew.collSize;
        //print(x+":"+NumberOfChunk + ":" + localYNumber + ":" +TerrainSystemNew.chunkColl + ":" +TerrainSystemNew.cubeSizeMultiplier);
        //print(NumberOfChunk+"->"+x + ":"+ y);
        this.transform.Translate(  new Vector3(x, y),Space.World);
        this.transform.localScale = new Vector3(1,1,1);
    }
    public uint parsePositionToCube(Vector2 position)
    {
        Vector2 localPos = position - (Vector2)this.gameObject.transform.position;
        int y = (int)Mathf.Floor((localPos.y * TerrainSystemNew.rowSize) / (TerrainSystemNew.rowSize * TerrainSystemNew.cubeSizeMultiplier));
        int x = (int)Mathf.Floor((localPos.x * TerrainSystemNew.collSize) / (TerrainSystemNew.collSize * TerrainSystemNew.cubeSizeMultiplier));
        int cubeID = (y * TerrainSystemNew.collSize) + x;
        //print(cubeID+ " Altura: " + y + " Anchote: "+x);
        return (uint)cubeID;
    }
    public void removeCube(uint cube)
    {
        //print(cube + ":"+ gameObject.name);
        if(cube >= 0 && cube <= 519)
        {
            if (data[cube] != 0)
            {
                StackOfChangesToRemove.Add(cube);
            } 
        }
    }
    public void removeCube(List<uint> listCubes)
    {
        for (int i = 0; i < listCubes.Count; i++)
        {
            if (data[i] != 0 && listCubes[i] >= 0 && listCubes[i] <= 519)
            {
                StackOfChangesToRemove.AddRange(listCubes);
            }
        }
    }
    private void refreshChunk()
    {
        getMeshGen();
        generateMesh();
        assignMaterials();
    }
}

