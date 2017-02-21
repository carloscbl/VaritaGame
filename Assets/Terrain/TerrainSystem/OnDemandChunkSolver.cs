using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;


class OnDemandChunkSolver : MonoBehaviour
{
    public GameObject currentCamera;
    public Vector2 currentCameraPosition;
    public CharacterSystem CharacterSystem;
    public TerrainSystemNew tsNew;
    [Flags]
    public enum posibleCases
    {
        Zero    = 0,
        bot     = 1 << 0,
        top     = 1 << 1,
        right   = 1 << 2,
        left    = 1 << 3,

        botRight    = bot | right,
        botLeft     = bot | left,
        topRight    = top | right,
        topLeft     = top | left
    }
    public posibleCases Case;
    private void Start()
    {
        CharacterSystem = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>();
        currentCamera = CharacterSystem.getMainCharacter().transform.Find("Main Camera").gameObject;
        tsNew = this.gameObject.GetComponent<TerrainSystemNew>();
        currentCameraPosition = currentCamera.transform.position;
    }
    private void Update()
    {
        //Update the camera in case we change player or camera
        currentCamera = CharacterSystem.getMainCharacter().transform.Find("Main Camera").gameObject;
        currentCameraPosition = currentCamera.transform.position;
        tsNew.DrawChunks(DesignateChunks(currentCameraPosition));
    }
    private List<uint> DesignateChunks(Vector2 position)
    {
        List<uint> chunksNumberList = new List<uint>();

        float y = Mathf.Floor(position.y / (TerrainSystemNew.rowSize * TerrainSystemNew.cubeSizeMultiplier));
        uint centralChunkNumber = (uint)((y * TerrainSystemNew.chunkColl) + Mathf.Floor(position.x / (TerrainSystemNew.collSize * TerrainSystemNew.cubeSizeMultiplier)));
        //print(centralChunkNumber);
        if (centralChunkNumber >= 0 && centralChunkNumber <= TerrainSystemNew.chunkColl * TerrainSystemNew.chunkRow)
        {
            chunksNumberList.Add(centralChunkNumber);
            int mod = (int)centralChunkNumber % (int)TerrainSystemNew.chunkColl;
            Case = posibleCases.Zero;
            //Width
            if (mod == (int)TerrainSystemNew.chunkColl - 1)//Right Border
            {
                Case |= posibleCases.right;
            }
            else if (mod == 0)//Left Border
            {
                Case |= posibleCases.left;
            }
            //Height
            if (centralChunkNumber < (int)TerrainSystemNew.chunkColl - 1)//Bot
            {
                Case |= posibleCases.bot;
            }
            else if (centralChunkNumber > ((int)TerrainSystemNew.chunkColl * (int)TerrainSystemNew.chunkRow) - (int)TerrainSystemNew.chunkColl)//Top
            {
                Case |= posibleCases.top;
            }
            uint collSize = TerrainSystemNew.chunkColl;
            #region switchCases
            switch (Case)
            {
                case posibleCases.bot:
                    chunksNumberList.Add(centralChunkNumber - 1);
                    chunksNumberList.Add(centralChunkNumber + 1);
                    chunksNumberList.Add(centralChunkNumber + collSize);
                    chunksNumberList.Add(centralChunkNumber + collSize - 1);
                    chunksNumberList.Add(centralChunkNumber + collSize + 1);
                    break;
                case posibleCases.top:
                    chunksNumberList.Add(centralChunkNumber - 1);
                    chunksNumberList.Add(centralChunkNumber + 1);
                    chunksNumberList.Add(centralChunkNumber - collSize);
                    chunksNumberList.Add(centralChunkNumber - collSize - 1);
                    chunksNumberList.Add(centralChunkNumber - collSize + 1);
                    break;
                case posibleCases.right:
                    chunksNumberList.Add(centralChunkNumber - 1);
                    chunksNumberList.Add(centralChunkNumber - collSize);
                    chunksNumberList.Add(centralChunkNumber + collSize);
                    chunksNumberList.Add(centralChunkNumber + collSize - 1);
                    chunksNumberList.Add(centralChunkNumber - collSize - 1);
                    break;
                case posibleCases.left:
                    chunksNumberList.Add(centralChunkNumber + 1);
                    chunksNumberList.Add(centralChunkNumber - collSize);
                    chunksNumberList.Add(centralChunkNumber + collSize);
                    chunksNumberList.Add(centralChunkNumber + collSize + 1);
                    chunksNumberList.Add(centralChunkNumber - collSize + 1);
                    break;
                case posibleCases.botRight:
                    chunksNumberList.Add(centralChunkNumber - 1);
                    chunksNumberList.Add(centralChunkNumber + collSize);
                    chunksNumberList.Add(centralChunkNumber + collSize - 1);
                    break;
                case posibleCases.botLeft:
                    chunksNumberList.Add(centralChunkNumber + 1);
                    chunksNumberList.Add(centralChunkNumber + collSize);
                    chunksNumberList.Add(centralChunkNumber + collSize + 1);
                    break;
                case posibleCases.topRight:
                    chunksNumberList.Add(centralChunkNumber - 1);
                    chunksNumberList.Add(centralChunkNumber - collSize);
                    chunksNumberList.Add(centralChunkNumber - collSize - 1);
                    break;
                case posibleCases.topLeft:
                    chunksNumberList.Add(centralChunkNumber + 1);
                    chunksNumberList.Add(centralChunkNumber - collSize);
                    chunksNumberList.Add(centralChunkNumber - collSize + 1);
                    break;
                case posibleCases.Zero://No borders around
                    chunksNumberList.Add(centralChunkNumber + 1);
                    chunksNumberList.Add(centralChunkNumber - 1);
                    chunksNumberList.Add(centralChunkNumber + collSize);
                    chunksNumberList.Add(centralChunkNumber - collSize);
                    chunksNumberList.Add(centralChunkNumber + collSize - 1);
                    chunksNumberList.Add(centralChunkNumber + collSize + 1);
                    chunksNumberList.Add(centralChunkNumber - collSize + 1);
                    chunksNumberList.Add(centralChunkNumber - collSize - 1);
                    break;
            }
            #endregion
        }
        return chunksNumberList;
    }
}

