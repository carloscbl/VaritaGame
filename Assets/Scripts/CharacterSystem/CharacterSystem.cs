﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterSystem : MonoBehaviour {

	GameObject test;
	private List<Character> currentCharactersActivesList = new List<Character>();
	public GameObject mainCharacter;

	enum PlayerType
	{
		Std,
		Elementalist
	}
	//Instanciar un objeto bajo demanda.
	//Añadirlo a la lista
	// Use this for initialization
	void Start () {
        /*
                test = Instantiate(Resources.Load("CharacterBase")) as GameObject;

                test.AddComponent<Character>().mainPlayer = true;
                test.name = "TestPlayerBase";
                currentCharactersActivesList.Add(test.GetComponent<Character>());
                mainCharacter = test;
                */

        //mainCharacter = GameObject.Find("CharacterBase");
       instantiateNewPlayer("Arwin");
        for (int i = 0; i < 5; i++)
        {
           //GameObject temp = instantiateNewPlayer("Dummy");
            //temp.transform.position = new Vector2(i - (i*2)*2f, 2);
        }
        

    }
	public GameObject instantiateNewPlayer(string name)
	{
		GameObject newPlayer = Instantiate(Resources.Load("CharacterBase")) as GameObject;
        /*
        switch ()
        {
            case PlayerType.Std:
                break;
            case PlayerType.Elementalist:
                break;
            default:
                break;
        }
        */
        if (name == "Arwin")
        {
            newPlayer.AddComponent<ArwinFireElementalist>();
            newPlayer.name = "Arwin";
        }
        else if (name == "Test")
        {
            newPlayer.AddComponent<Character>();
            newPlayer.name = "Test";
        }else if (name == "Dummy")
        {
            newPlayer.AddComponent<Dummy>();
            newPlayer.name = "Dummy";
        }
        else
		{
			Debug.Assert(false, "There is not a Character with that name");
		}
		
		currentCharactersActivesList.Add(newPlayer.GetComponent<Character>());
		if (mainCharacter == null)
		{
			newPlayer.GetComponent<Character>().mainPlayer = true;
			mainCharacter = newPlayer;
		}

        return newPlayer;
	}
	public List<Character> getCharacterList()
	{
		return this.currentCharactersActivesList;
	}
	public  GameObject getMainCharacter()
	{
		return mainCharacter;
	}
	// Update is called once per frame
	void Update () {
	   
		
	}
}
