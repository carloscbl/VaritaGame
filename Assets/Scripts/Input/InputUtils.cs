using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class InputUtils
{
    public static Vector3 getMousePosition()
    {
        Camera camera = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.Find("Main Camera").gameObject.GetComponent<Camera>();
        //Camera camera = Camera.current;
        Vector3 mousePosition2 = Input.mousePosition;
        //Grab the current mouse position on the screen
        Vector3 mousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - camera.transform.position.z));
        return mousePosition;
    }
}

