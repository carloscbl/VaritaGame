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
    private void Start()
    {
        
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

        m_Mesh.vertices     = meshGen.generatedMeshComponents.vertex;
        m_Mesh.normals      = meshGen.generatedMeshComponents.normals;
        m_Mesh.uv           = meshGen.generatedMeshComponents.uvs;
        m_Mesh.triangles    = meshGen.generatedMeshComponents.triangles;

        m_Mesh.RecalculateBounds();

        this.GetComponent<MeshFilter>().mesh = m_Mesh;
    }
    private void assignMaterials()
    {
        this.GetComponent<MeshRenderer>().material =Resources.Load("rock", typeof(Material)) as Material;
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
}

