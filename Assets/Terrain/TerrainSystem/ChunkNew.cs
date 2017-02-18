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
    private void Start()
    {
        
    }
    public void setParameters(byte [] data )
    {
        this.data = data;
        parseData(data);
        init();
    }
    private void parseData(byte[] data)
    {

    }
    private void init()
    {
        generateMesh();
        assignMaterials();
    }
    private void generateMesh()
    {

    }
    private void assignMaterials()
    {

    }
}

