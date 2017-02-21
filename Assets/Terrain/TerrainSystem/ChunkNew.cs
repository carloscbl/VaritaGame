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

        m_Mesh.SetVertices(meshGen.generatedMeshComponents.vertex);
        m_Mesh.SetNormals(meshGen.generatedMeshComponents.normals);
        m_Mesh.SetUVs(0,meshGen.generatedMeshComponents.uvs);
        m_Mesh.subMeshCount = meshGen.materialsNumberList.Count;
        for (int i = 0; i < meshGen.materialsNumberList.Count; i++)
        {
            m_Mesh.SetTriangles(meshGen.generatedMeshComponents.triangles[i], i);
        }
        m_Mesh.RecalculateBounds();
        m_Mesh.RecalculateNormals();
        this.GetComponent<MeshFilter>().sharedMesh = m_Mesh;
    }
    private void assignMaterials()
    {
        materials = new List<Material>();
        Material[] mat = {
        Resources.Load("air", typeof(Material)) as Material,
        Resources.Load("rock", typeof(Material)) as Material,
        Resources.Load("grass", typeof(Material)) as Material,
        Resources.Load("sand", typeof(Material)) as Material,
        Resources.Load("water", typeof(Material)) as Material,
        Resources.Load("bedrock", typeof(Material)) as Material
    };
        this.GetComponent<MeshRenderer>().materials = mat;
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

