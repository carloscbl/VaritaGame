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
    //public static readonly uint heightCubes = 500;
    public static readonly uint heightCubes = 2730;
    public static readonly uint chunkColl = 105;
    public static readonly uint chunkRow = 105;
    //public static readonly uint chunkRow = 25;
    public static uint totalChunks;
    public static readonly ushort rowSize = 20;
    public static readonly ushort collSize = 26;
    public static readonly uint totalCubesInChunk = (uint)(rowSize * collSize);
    public static readonly float cubeSizeMultiplier = .25f;
    public static int offset = 3;
    public static Material[] mat;

    List<GameObject> chunksList = new List<GameObject>();
    List<uint> chunksNumberList = new List<uint>();
    List<Material> materialsList = new List<Material>();
    List<uint> ObsoleteChunks = new List<uint>();
    public bool ChunkInstantiationsThisFrame = true;
    public bool destroyOldChunks = false;
    public bool chargeWholeStack = false;
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
        if (chargeWholeStack)
        {
            List<uint> totalChunk = new List<uint>();
            for (int i = 0; i < (105 * 75) - 1; i++)
            {

                totalChunk.Add((uint)i);
            }
            float time = Time.realtimeSinceStartup;
            DrawChunks(totalChunk);
            print(Time.realtimeSinceStartup - time);
        }
        
    }
    int count = 0;
    private void Update()
    {
        if (!ChunkInstantiationsThisFrame && destroyOldChunks && count >= 20)
        {
            removeOldChunks();
            count = 0;
        }
        count++;
    }
    private void removeOldChunks()
    {
        int off = (((offset * 2) + 1) * offset * 2) +12;
        //print(off);
        //print(chunksList.Count);
        //print(ObsoleteChunks.Count);
        //print("----");
        //List<GameObject> toRemove = chunksList.All(p => p.GetComponent<ChunkNew>().iAmNumberOfChunk == );
        while (chunksList.Count > (off * 2))
        {
            GameObject temp = chunksList[0];

            chunksNumberList.Remove(chunksList[0].GetComponent<ChunkNew>().iAmNumberOfChunk);
            chunksList.RemoveAt(0);

            DestroyImmediate(temp, true);
        }


        //GameObject temp;
        //    for (int e = 0; e < ObsoleteChunks.Count; e++)
        //    {
        //        temp = chunksList.Find(p => p.GetComponent<ChunkNew>().iAmNumberOfChunk == ObsoleteChunks[e]);
        //        chunksNumberList.Remove(temp.GetComponent<ChunkNew>().iAmNumberOfChunk);
        //        chunksList.Remove(temp);
        //        DestroyImmediate(temp, true);
        //    }
    }
    public void DrawChunks(List<uint> listOfChunks)
    {
        ObsoleteChunks.Clear();
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
                    //ObsoleteChunks.Add(chunksNumberList.Find(item => item != listOfChunks[i]));
                    //print(Time.realtimeSinceStartup - time);
                }
                ChunkInstantiationsThisFrame = true;
            }else
            {
                ChunkInstantiationsThisFrame = false;
            }
        }
    }
    private byte[] Dispatcher(uint numOfChunk)
    {
        byte[] temp = new byte[520];
       
        int data;
        int chunkX = (int)Mathf.Floor(numOfChunk / chunkColl);
        int yBase = rowSize * chunkX;
        int xBase = (int)(collSize * (numOfChunk - (chunkX* chunkColl)));
        int contador = 0;
        for (int y = 0; y < rowSize; y++)
        {
            for (int x = 0; x < collSize; x++)
            {
                data = (int)((widthCubes * y) + (yBase * widthCubes) + x + xBase);
                temp[contador] = assignData[data];
                contador++;
            }
        }
        return temp;
    }
    public void ReportData(byte[] dataToReport,uint numOfChunk)
    {
        int data;
        int chunkX = (int)Mathf.Floor(numOfChunk / chunkColl);
        int yBase = rowSize * chunkX;
        int xBase = (int)(collSize * (numOfChunk - (chunkX * chunkColl)));
        int contador = 0;
        for (int y = 0; y < rowSize; y++)
        {
            for (int x = 0; x < collSize; x++)
            {
                data = (int)((widthCubes * y) + (yBase * widthCubes) + x + xBase);
                assignData[data] = dataToReport[contador];
                contador++;
            }
        }
    }
    
}

