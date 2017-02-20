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
    public static readonly float cubeSizeMultiplier = 1;

    List<GameObject> chunksList = new List<GameObject>();
    List<Material> materialsList = new List<Material>();
    byte[] assignData;
    private void Start()
    {
        totalChunks = (heightCubes * widthCubes) / totalCubesInChunk;
        //Initialize GO Getting the Generation
        //Get the data
        assignData = FileIO.blocksA;
        float time = Time.realtimeSinceStartup;
        for (int i = 0; i < 9; i++)
        {

            GameObject temp = new GameObject(i.ToString());
            //temp.transform.SetParent(this.transform);
            chunksList.Add(temp);/*
            switch (i)
            {
                case 0:
                    float time = Time.realtimeSinceStartup;
                    temp.AddComponent<ChunkNew>().setParameters(Dispatcher(0),0);
                    float newTime = Time.realtimeSinceStartup - time;
                    print(newTime);
                    break;
                case 1:
                    temp.AddComponent<ChunkNew>().setParameters(Dispatcher(25), 25);
                    break;
                case 2:
                    temp.AddComponent<ChunkNew>().setParameters(Dispatcher(105), 105);
                    break;
                case 3:
                    temp.AddComponent<ChunkNew>().setParameters(Dispatcher(1), 1);
                    break;
            }      */
            temp.AddComponent<ChunkNew>().setParameters(Dispatcher((uint)i), (uint)i);
        }
        float newTime = Time.realtimeSinceStartup - time;
        print(newTime);
    }
    private void Update()
    {
        
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

