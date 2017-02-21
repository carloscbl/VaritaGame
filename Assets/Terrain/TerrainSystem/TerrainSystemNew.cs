using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


class TerrainSystemNew : MonoBehaviour
{
    public static readonly uint widthCubes = 2730; 
    public static readonly uint heightCubes = 500;
    public static readonly uint chunkColl = 105;
    public static readonly uint chunkRow = 25;
    public static uint totalChunks;
    public static readonly ushort rowSize = 20;
    public static readonly ushort collSize = 26;
    public static readonly uint totalCubesInChunk = (uint)(rowSize * collSize);
    public static readonly float cubeSizeMultiplier = .5f;

    List<GameObject> chunksList = new List<GameObject>();
    List<uint> chunksNumberList = new List<uint>();
    List<Material> materialsList = new List<Material>();
    byte[] assignData;
    private void Start()
    {
        totalChunks = (heightCubes * widthCubes) / totalCubesInChunk;
        //Initialize GO Getting the Generation
        //Get the data
        assignData = FileIO.blocksA;
    }
    private void Update()
    {
        
    }
    public void sayHello()
    {
        print("hello");
    }
    public void DrawChunks(List<uint> listOfChunks)
    {
        for (int i = 0; i < listOfChunks.Count; i++)
        {
            if (chunksNumberList.All(item => item != listOfChunks[i]))
            {
                float time = Time.realtimeSinceStartup;
                GameObject temp = new GameObject(listOfChunks[i].ToString());
                chunksList.Add(temp);
                temp.AddComponent<ChunkNew>().setParameters(Dispatcher(listOfChunks[i]), listOfChunks[i]);
                chunksNumberList.Add(listOfChunks[i]);
                print(Time.realtimeSinceStartup - time);
            }
        }
        
    }
    private byte[] Dispatcher(uint numOfChunk)
    {
        byte[] temp = new byte[520];
        //250 chunk have the cubes on -> numberofchunk + 1 * 520
        for (int i = 0; i < 520; i++)
        {
            temp[i] = assignData[((numOfChunk + 1) * 520)+i];
        }
        return temp;
    }
    
}

