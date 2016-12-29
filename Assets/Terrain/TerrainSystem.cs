using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class TerrainSystem : MonoBehaviour
{
    public static TerrainSystem TS;
    private uint tileCount = 0;
    public static List<Chunk> chunks;
    public static List<Material> MaterialsList;
    public readonly static float multiplierFormX = 0.25f;
    public readonly static float multiplierFormY = 0.25f;
    bool aa = false;
    List<cube> tempContainerList;
    List<int> ChunkOnScreen = new List<int>();
    List<int> temp;
    List<Chunk> screenGenerated = new List<Chunk>();

    List<ChunkData> chunksStructs;
    List<Chunk> GlobalChunkList = new List<Chunk>();
    Chunk chunk;
    TerrainSystem()
    {
        TS = this;
    }
    ~TerrainSystem()
    {
        //Debug.Log("DesinstanciandoTerrain");
    }
    public Chunk findChunk(String nameNumber)
    {
        uint number;
        try
        {
          number = Convert.ToUInt32(nameNumber);
        }
        catch
        {
            return null;
        }
        return GlobalChunkList.Find(p => p.getID() == number);
    }
    void Start()
    {
        
        CubeChunkComposer.composeCubes(out tempContainerList, out MaterialsList);
        chunksStructs = CubeChunkComposer.composeChunks(tempContainerList);
    }
    void Update()
    {
        if (GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().getMainCharacter().transform.Find("Main Camera").GetComponent<CameraOperands>().hasMoved())
        {
            instantiateListOfChunks();
        }
    }
   
    void instantiateListOfChunks()
    {
        screenGenerated.Clear();
        ChunkOnScreen.Clear();
        int[] aa = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().getMainCharacter().transform.Find("Main Camera").GetComponent<CameraOperands>().computeCameraToListOfChunks();
        //Debug.Log(" , " + aa[0]+ " , " + aa[1] + " , " + aa[2] + " , " + aa[3] + " , " + aa[4] + " , " + aa[5] + " , " + aa[6] + " , " + aa[7] + " , " + aa[8]);
        for(int i = 0; i < aa.Length;i++)
        {
            if (aa[i] != -2)
            {
                ChunkOnScreen.Add(aa[i]);                    
            }
        }
        //ahora hay que evitar la reduplicacion :/
        //Debug.Log(ChunkOnScreen.Count);
        if(ChunkOnScreen.Count != 0)
        {
            for (int e = 0; e < ChunkOnScreen.Count -1; e++)
            {
                if (ChunkOnScreen[e] < 10500)
                {
                    bool repeat = false;
                    //Buscamos que no exista
                    foreach (var GCL in GlobalChunkList)
                    {
                        if (GCL.getID() == chunksStructs[ChunkOnScreen[e]].localChunkId)
                        {
                            screenGenerated.Add(GCL);
                            repeat = true;
                            break;
                        }
                    }
                    if (!repeat)
                    {
                        chunk = new Chunk(chunksStructs[ChunkOnScreen[e]]);
                        screenGenerated.Add(chunk);
                        GlobalChunkList.Add(chunk);
                    }
                }
            }
        }
        if(GlobalChunkList.Count > 50)
        {
            List<Chunk> excludedChunks = new List<Chunk>();
            excludedChunks = GlobalChunkList.Except(screenGenerated).ToList(); 
            deleteObsoleteObjects(excludedChunks);
        }
    }
    private void deleteObsoleteObjects(List<Chunk> ListToDelete)
    {
        foreach (Chunk LTD in ListToDelete)
        {
            LTD.finalize();
            Destroy(LTD.getGameObject(),1.0f);
            
            GlobalChunkList.Remove(LTD);
        }
        //Debug.Log("Colleted");
        System.GC.Collect();
    }
    public uint GetTileCount()
    {
        return tileCount;
    }
    public uint AddTile()
    {
        return tileCount += 1;
    }
}

