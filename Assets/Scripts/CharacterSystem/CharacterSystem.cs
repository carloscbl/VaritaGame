using UnityEngine;
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
		
		
		
	
	}
	public void instantiateNewPlayer(string name)
	{
		GameObject newPlayer = Instantiate(Resources.Load("CharacterBase")) as GameObject;

		if (name == "Arwin")
		{
			newPlayer.AddComponent<ArwinFireElementalist>();
			newPlayer.name = "Arwin";
		}
		else if(name == "Test")
		{
			newPlayer.AddComponent<Character>();
			newPlayer.name = "Test";
		}else
		{
			Debug.Assert(false, "There is not a Character with that name");
		}
		
		currentCharactersActivesList.Add(newPlayer.GetComponent<Character>());
		if (mainCharacter == null)
		{
			newPlayer.GetComponent<Character>().mainPlayer = true;
			mainCharacter = newPlayer;
		}
	}
	public List<Character> getCharacterList()
	{
		return this.currentCharactersActivesList;
	}
	public GameObject getMainCharacter()
	{
		return this.mainCharacter;
	}
	// Update is called once per frame
	void Update () {
	   
		
	}
}
