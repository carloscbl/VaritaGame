using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainSystem : MonoBehaviour
{
    public readonly static float sizeCubesX = 1;
    public readonly static float sizeCubesY = 1;
    public readonly static int cubesForChunk = 25;
    private List<int> IDChunksOnScreen;
    private List<ChunkData> chuncks;
    private List<Chunk> loadedChuncks;
    private Transform cameraTransform;
    private Vector3 cameraLastPosition;

    private Level currentLevel;

    void Start()
    {
        currentLevel = GameObject.Find("Root").GetComponent<Level>();
        IDChunksOnScreen = new List<int>();
        loadedChuncks = new List<Chunk>();
        TerrainGeneration terrain = new TerrainGeneration();
        chuncks = terrain.GenerateTerrain((uint)currentLevel.SizeXLevel, (uint)currentLevel.SizeYLevel, (uint)cubesForChunk, sizeCubesX, sizeCubesY);
        cameraTransform = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().getMainCharacter().transform.Find("Main Camera").GetComponent<Transform>();
        cameraLastPosition = cameraTransform.position;
    }
    void Update()
    {
        if (cameraTransform != null && cameraTransform.position != cameraLastPosition)
        {
            UpdateListVisibleChunks();
            cameraLastPosition = cameraTransform.position;
        }
    }

    void UpdateListVisibleChunks()
    {
        if (loadedChuncks.Count > 50)
        {
            deleteObsoleteObjects(loadedChuncks);
        }
        IDChunksOnScreen.Clear();
        IDChunksOnScreen = PredictNewListOfChunks().Where(x => x != -2).ToList();
        if (IDChunksOnScreen.Count != 0)
        {
            for (int i = 0; i < IDChunksOnScreen.Count; i++)
            {
                if (!loadedChuncks.Any(x => x.getID() == chuncks[IDChunksOnScreen[i]].localChunkId))
                {
                    Chunk newChunk = new Chunk(chuncks[IDChunksOnScreen[i]]);
                    loadedChuncks.Add(newChunk);
                }
            }
        }
    }

    private void deleteObsoleteObjects(List<Chunk> ListToDelete)
    {
        foreach (Chunk LTD in ListToDelete)
        {
            Destroy(LTD.getGameObject());
        }
        System.GC.Collect();
    }

    private int[] PredictNewListOfChunks()
    {
        int row = (int)Mathf.Clamp(cameraTransform.position.x, 0, currentLevel.SizeXLevel);
        int coll = (int)Mathf.Clamp(cameraTransform.position.y, 0, currentLevel.SizeYLevel);
        //Get the 3x3 

        int chunksRow = currentLevel.SizeXLevel / cubesForChunk;
        int chunksCol = currentLevel.SizeYLevel / cubesForChunk;
        int totalIDs = chunksRow * chunksCol;
        int predictedID = (row * currentLevel.SizeYLevel) / (cubesForChunk * cubesForChunk) + coll / cubesForChunk;

        /*
         678
         345 ---> our predicted is 4
         012
         */
        int[] requieredChunks = new int[9];
        requieredChunks[4] = predictedID;
        requieredChunks[1] = requieredChunks[4] - chunksCol;
        requieredChunks[7] = requieredChunks[4] + chunksCol;

        requieredChunks[3] = requieredChunks[4] - 1;
        requieredChunks[5] = requieredChunks[4] + 1;

        requieredChunks[6] = requieredChunks[7] - 1;
        requieredChunks[8] = requieredChunks[7] + 1;

        requieredChunks[0] = requieredChunks[1] - 1;
        requieredChunks[2] = requieredChunks[1] + 1;

        //We need to be sure that is in the array
        //-2 would be our magic number
        if (requieredChunks[4] == 0)//Corner Bottom-Left
        {
            requieredChunks[0] = -2;
            requieredChunks[1] = -2;
            requieredChunks[2] = -2;
            requieredChunks[3] = -2;
            requieredChunks[6] = -2;
        }
        else if (requieredChunks[4] >= 0 && requieredChunks[4] < chunksRow) //Bottom
        {
            requieredChunks[0] = -2;
            requieredChunks[1] = -2;
            requieredChunks[2] = -2;
        }
        else if (requieredChunks[4] > (totalIDs - chunksRow) && requieredChunks[4] < totalIDs)//Top
        {
            requieredChunks[6] = -2;
            requieredChunks[7] = -2;
            requieredChunks[8] = -2;
        }
        else if (requieredChunks[4] % chunksRow == 0)//Right
        {
            requieredChunks[2] = -2;
            requieredChunks[5] = -2;
            requieredChunks[8] = -2;
        }
        else if (requieredChunks[4] % chunksRow == 0 && requieredChunks[4] < totalIDs)//Left
        {
            requieredChunks[0] = -2;
            requieredChunks[3] = -2;
            requieredChunks[6] = -2;
        }
        else if (requieredChunks[4] == totalIDs)//Corner Top-Left
        {
            requieredChunks[0] = -2;
            requieredChunks[3] = -2;
            requieredChunks[6] = -2;
            requieredChunks[7] = -2;
            requieredChunks[8] = -2;
        }
        else if (requieredChunks[4] == chunksRow)//Corner Bottom-Right
        {
            requieredChunks[0] = -2;
            requieredChunks[1] = -2;
            requieredChunks[2] = -2;
            requieredChunks[5] = -2;
            requieredChunks[8] = -2;
        }
        else if (requieredChunks[4] == totalIDs)//Corner Top-Right
        {
            requieredChunks[2] = -2;
            requieredChunks[5] = -2;
            requieredChunks[6] = -2;
            requieredChunks[7] = -2;
            requieredChunks[8] = -2;
        }
        else if (requieredChunks[4] > totalIDs || requieredChunks[4] < 0)
        {
            //Debug.Log("Out of Range");
            requieredChunks[0] = -2;
            requieredChunks[1] = -2;
            requieredChunks[2] = -2;
            requieredChunks[3] = -2;
            requieredChunks[4] = -2;
            requieredChunks[5] = -2;
            requieredChunks[6] = -2;
            requieredChunks[7] = -2;
            requieredChunks[8] = -2;
        }
        //<--To be continued
        //With the list we are ready to ship it
        return requieredChunks;

    }
    public Chunk findChunk(string nameNumber)
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
        return loadedChuncks.Find(p => p.getID() == number);
    }
}

