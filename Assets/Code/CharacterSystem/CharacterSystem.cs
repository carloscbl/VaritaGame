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

        test = Instantiate(Resources.Load("CharacterBase")) as GameObject;
        
        test.AddComponent<Character>().mainPlayer = true;
        test.name = "TestPlayerBase";
        currentCharactersActivesList.Add(test.GetComponent<Character>());
        mainCharacter = test;
        
        
        GameObject Arwin = Instantiate(Resources.Load("CharacterBase")) as GameObject;
        Arwin.AddComponent<ArwinFireElementalist>();
        Arwin.name = "Arwin";
        currentCharactersActivesList.Add((Character)Arwin.GetComponent<ArwinFireElementalist>());
        
    
    }
    public void instantiateNewPlayer(string name)
    {

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
        // test.rotatePlayer();
        if (Input.mousePosition.x < Screen.width / 2)
        {
            //Negative X
            test.GetComponent<Character>().body_main.transform.eulerAngles = new Vector3(0, 180, 0);
            test.GetComponent<Character>().WeaponRight.GetComponent<SpriteRenderer>().flipY = true;
            test.GetComponent<Character>().ArmRight.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            //Positive X
            test.GetComponent<Character>().body_main.transform.eulerAngles = new Vector3(0, 0, 0);
            test.GetComponent<Character>().WeaponRight.GetComponent<SpriteRenderer>().flipY = false;
            test.GetComponent<Character>().ArmRight.GetComponent<SpriteRenderer>().flipY = false;
        }
        
    }
}
