using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


class TerrainSystemNew : MonoBehaviour
{
    public static readonly uint heightCubes = 2730;
    public static readonly uint widthCubes = 500;
    public static readonly uint totalChunks = (heightCubes * widthCubes) / totalCubesInChunk; 
    public static readonly ushort rowSize = 20;
    public static readonly ushort collSize = 26;
    public static readonly uint totalCubesInChunk = (uint)(rowSize * collSize);
    public static readonly float cubeSizeMultiplier = 0.35f;

    List<GameObject> chunksList = new List<GameObject>();
    List<Material> materialsList = new List<Material>();
    byte[] assignData;
    private void Start()
    {
        //Initialize GO Getting the Generation
        //Get the data
        assignData = FileIO.blocksA;
        for (int i = 0; i < 3; i++)
        {

            GameObject temp = new GameObject();
            chunksList.Add(temp);
            switch (i)
            {
                case 1:
                    temp.AddComponent<ChunkNew>().setParameters(Dispatcher(0));
                    break;
                case 2:
                    temp.AddComponent<ChunkNew>().setParameters(Dispatcher(1));
                    break;
                case 3:
                    temp.AddComponent<ChunkNew>().setParameters(Dispatcher(501));
                    break;
            }        
        }
    }

    private void Update()
    {
        //We need chunks 0,1,501
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

