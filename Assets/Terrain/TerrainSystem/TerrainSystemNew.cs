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
    public static Material[] mat;

    List<GameObject> chunksList = new List<GameObject>();
    List<uint> chunksNumberList = new List<uint>();
    List<Material> materialsList = new List<Material>();
    byte[] assignData;
    private void Start()
    {
        totalChunks = (heightCubes * widthCubes) / totalCubesInChunk;
        mat = new Material[]{
        Resources.Load("air",       typeof(Material)) as Material,
        Resources.Load("rock",      typeof(Material)) as Material,
        Resources.Load("grass",     typeof(Material)) as Material,
        Resources.Load("sand",      typeof(Material)) as Material,
        Resources.Load("water",     typeof(Material)) as Material,
        Resources.Load("bedrock",   typeof(Material)) as Material
        };
        //Initialize GO Getting the Generation
        //Get the data
        assignData = FileIO.blocksA;
        //print(assignData.Length);
        //print(FileIO.blocksA);
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
                if(listOfChunks[i] >=0 && listOfChunks[i] <= (chunkColl * chunkRow) - 1)
                {
                    //float time = Time.realtimeSinceStartup;
                    GameObject temp = new GameObject(listOfChunks[i].ToString());
                    chunksList.Add(temp);
                    //print(listOfChunks[i]);
                    temp.AddComponent<ChunkNew>().setParameters(Dispatcher(listOfChunks[i]), listOfChunks[i]);
                    chunksNumberList.Add(listOfChunks[i]);
                    //print(Time.realtimeSinceStartup - time);
                }
            }
        }
        
    }
    private byte[] Dispatcher(uint numOfChunk)
    {
        byte[] temp = new byte[520];
        //250 chunk have the cubes on -> numberofchunk + 1 * 520
        //for (int i = 0; i < 520; i++)
        //{
        //    temp[i] = assignData[((numOfChunk ) * 520)+i];
        //}
        int data;
        int chunkX = (int)Mathf.Floor(numOfChunk / chunkColl);
        int yBase = rowSize * chunkX;
        int xBase = (int)(collSize * (numOfChunk - (chunkX* chunkColl)));
        int contador = 0;
        print(numOfChunk + ": YBase" + yBase);
        print(numOfChunk + ": XBase" + xBase);
        print((int)((widthCubes * 0) + yBase + 0 + xBase));
        print((int)((widthCubes * (rowSize-1)) + yBase + (collSize-1) + xBase -26));
        for (int y = 0; y < rowSize; y++)
        {
            for (int x = 0; x < collSize; x++)
            {
                data = (int)((widthCubes * y) + (yBase * widthCubes) + x + xBase);
                temp[contador] = assignData[data];
                contador++;
                //((numOfChunk * collSize) + x) + (y * widthCubes)
            }
        }
        return temp;
    }
    
}

