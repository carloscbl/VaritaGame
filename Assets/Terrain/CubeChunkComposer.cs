using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CubeChunkComposer : MonoBehaviour
{
    public static void composeCubes(out List<cube> tempContainerList, out List<Material> MatList)
    {
        byte[] terrain;
        FileIO.readFile("test", out terrain);
        MatList = new List<Material>();

        tempContainerList = new List<cube>();
        //cube tempCube = new cube();
        //uint chunks = 0;
        uint totalCubesPerChunk = 520;
        uint totalCubes = (uint)terrain.Length;
        uint totalChunks = totalCubes / totalCubesPerChunk;

        //Materials
        Material air    = Resources.Load("air", typeof(Material)) as Material;
        MatList.Add(air);

        Material rock   = Resources.Load("rock", typeof(Material)) as Material;
        MatList.Add(rock);

        Material sand   = Resources.Load("sand", typeof(Material)) as Material;
        MatList.Add(sand);

        Material grass  = Resources.Load("grass", typeof(Material)) as Material;
        MatList.Add(grass);

        Material water  = Resources.Load("water", typeof(Material)) as Material;
        MatList.Add(water);

        Material bedrock = Resources.Load("bedrock", typeof(Material)) as Material;
        MatList.Add(bedrock);

        //Actual chunk, total cubes less actual id and the result divided to maxcubes 
        for (int i = 0; i < terrain.Length; i++)
        {
            cube tempCube = new cube();
            tempCube.id = (uint)i;
            
            //                                                                      Number of chunk             
            tempCube.inChunkPosition = totalCubesPerChunk - ((totalCubesPerChunk * (totalChunks - ((totalCubes - tempCube.id) / totalCubesPerChunk))) - tempCube.id);
            switch (terrain[i])
            {
                    case (byte)cubeMaterial.air:
                        tempCube.material = air;
                        break;
                    case (byte)cubeMaterial.rock:
                        tempCube.material = rock;
                        break;
                    case (byte)cubeMaterial.sand:
                        tempCube.material = sand;
                        break;
                    case (byte)cubeMaterial.grass:
                        tempCube.material = grass;
                        break;
                    case (byte)cubeMaterial.water:
                        tempCube.material = water;
                        break;
                    case (byte)cubeMaterial.bedrock:
                        tempCube.material = bedrock;
                        break;
                    default:
                        tempCube.material = rock;
                        break;
                }
          
            tempContainerList.Add(tempCube);
        }
        
        
    }
    public static List<ChunkData> composeChunks( List<cube> tempContainerList)
    {
        //A chunk should be composed pasing cubes from the Globalcube list to the instances of chunks
        //We need a global chunk list too for the terrain system
        //List for chunks
        List<ChunkData> chunksContainer = new List<ChunkData>();
        //first get the cubelist
        //List<cube> tempContainerList;
        

        //Componer el primer chunk

        //Debug.Log(tempContainerList.ElementAt(689).material.ToString());
        
        uint totalCubesPerChunk = 520;
        int index = 0;
        int rowCounter = 0;
        int colCounter = 0;
        ChunkData ckData;
        //Bucle
        for (uint i = 0; i< tempContainerList.Count / 520; i++)
            //Once per chunk
        {
            ckData = new ChunkData();
            ckData.active = false;
            ckData.localChunkId = (ushort)i;
            if(i % 210 == 0)
            {
                if (i == 0) { } else { rowCounter++; colCounter = 0; }
                
            }
            
            ckData.worldPos = new Vector2(colCounter*26*TerrainSystem.multiplierFormX, rowCounter*20* TerrainSystem.multiplierFormY);
            ckData.cubes = new List<cube>();
            for (uint e = 0; e < totalCubesPerChunk; e++)
            {
                index = (int)e + ((int)i * (int)totalCubesPerChunk);
               // ckData.cubes = new List<cube>();
                ckData.cubes.Add(tempContainerList[index]);
            }
            colCounter++;
            chunksContainer.Add(ckData);
        }
        return chunksContainer;
    }
}

