using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class CameraOperands : MonoBehaviour
{
    public Camera currentCamera;
    private Vector2 lastPosition;

    void Start()
    {
        currentCamera = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().getMainCharacter().transform.Find("Main Camera").GetComponent<Camera>();
        lastPosition = new Vector2();
    }
    private void refreshCameraPosition()
    {
        lastPosition = currentCamera.transform.position;
    }

    public bool hasMoved() {

        if((Vector2)currentCamera.transform.position == lastPosition)
        {
            //refreshCameraPosition();
            return false;
        }else
        {
            refreshCameraPosition();
            return true;
        }
    }
      
   
    public int[] computeCameraToListOfChunks()
    {
       float x = currentCamera.transform.position.x;
       float y = currentCamera.transform.position.y;
        //Get the 3x3 

        int coll = (int)(x / (26 * TerrainSystem.multiplierFormX));
        int row = (int)(y / (20 * TerrainSystem.multiplierFormY));

        int predictedName = (row * 210) + coll;
       
        int [] requieredChunks = new int[9];
        /*
         789
         456 ---> our predicted is 5
         123
         */

        requieredChunks[5 - 1] = predictedName;
        requieredChunks[2 - 1] = requieredChunks[5 - 1] - 210;
        requieredChunks[8 - 1] = requieredChunks[5 - 1] + 210;

        requieredChunks[4 - 1] = requieredChunks[5 - 1] - 1;
        requieredChunks[7 - 1] = requieredChunks[4 - 1] + 210;
        requieredChunks[1 - 1] = requieredChunks[4 - 1] - 210;

        requieredChunks[6 - 1] = requieredChunks[5 - 1] + 1;
        requieredChunks[9 - 1] = requieredChunks[6 - 1] + 210;
        requieredChunks[3 - 1] = requieredChunks[6 - 1] - 210;
        //We need to be sure that is in the array
        //-2 would be our magic number
        if(requieredChunks[5-1] <= 0)//Corner Bottom-Left
        {
            //Debug.Log("Corner Bottom-Left");
            requieredChunks[1 - 1] = -2;
            requieredChunks[2 - 1] = -2;
            requieredChunks[3 - 1] = -2;
            requieredChunks[4 - 1] = -2;
            requieredChunks[7 - 1] = -2;
        }else if(requieredChunks[5-1] >= 1 && requieredChunks[5 - 1] < 209) //Bottom
        {
            //Debug.Log("Bottom");
            requieredChunks[1 - 1] = -2;
            requieredChunks[2 - 1] = -2;
            requieredChunks[3 - 1] = -2;
        }else if (requieredChunks[5 - 1] > 10290 && requieredChunks[5 - 1] < 10499)//Top
        {
            //Debug.Log("Top");
            requieredChunks[7 - 1] = -2;
            requieredChunks[8 - 1] = -2;
            requieredChunks[9 - 1] = -2;
        }else if(requieredChunks[5-1] % 209 == 0)//Right
        {
            //Debug.Log("Right");
            requieredChunks[3 - 1] = -2;
            requieredChunks[6 - 1] = -2;
            requieredChunks[9 - 1] = -2;
        }else if(requieredChunks[5 - 1] % 210 == 0 && requieredChunks[5 - 1] < 10499)//Left
        {
            //Debug.Log("Left");
            requieredChunks[1 - 1] = -2;
            requieredChunks[4 - 1] = -2;
            requieredChunks[7 - 1] = -2;
        }else if(requieredChunks[5 - 1] == 10290)//Corner Top-Left
        {
            //Debug.Log("Corner Top-Left");
            requieredChunks[1 - 1] = -2;
            requieredChunks[4 - 1] = -2;
            requieredChunks[7 - 1] = -2;
            requieredChunks[8 - 1] = -2;
            requieredChunks[9 - 1] = -2;
        }else if (requieredChunks[5 - 1] == 209)//Corner Bottom-Right
        {
            //Debug.Log("Corner Bottom-Right");
            requieredChunks[1 - 1] = -2;
            requieredChunks[2 - 1] = -2;
            requieredChunks[3 - 1] = -2;
            requieredChunks[6 - 1] = -2;
            requieredChunks[9 - 1] = -2;
        }else if(requieredChunks[5 - 1] == 10499)//Corner Top-Right
        {
            //Debug.Log("Corner Top-Right");
            requieredChunks[3 - 1] = -2;
            requieredChunks[6 - 1] = -2;
            requieredChunks[7 - 1] = -2;
            requieredChunks[8 - 1] = -2;
            requieredChunks[9 - 1] = -2;
        }else if(requieredChunks[5 - 1] > 10499)
        {
            //Debug.Log("Out of Range");
            requieredChunks[1 - 1] = -2;
            requieredChunks[2 - 1] = -2;
            requieredChunks[3 - 1] = -2;
            requieredChunks[4 - 1] = -2;
            requieredChunks[5 - 1] = -2;
            requieredChunks[6 - 1] = -2;
            requieredChunks[7 - 1] = -2;
            requieredChunks[8 - 1] = -2;
            requieredChunks[9 - 1] = -2;
        }
        //<--To be continued
        //With the list we are ready to ship it
        return requieredChunks;

    }
    
}

