using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APathAttach : MonoBehaviour {

	// Use this for initialization
	void Start () {
        

    }
    int counter = 0;
	// Update is called once per frame
	void Update () {
        //print(GameObject.Find("Root").transform.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.name);
        AstarPath.active.astarData.gridGraph.center = GameObject.Find("Root").transform.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.position;
        AstarPath.active.astarData.gridGraph.GenerateMatrix();
        AstarPath.active.transform.position = GameObject.Find("Root").transform.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.position;
        //print(GameObject.Find("Root").transform.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.position.ToString());
        

        if (counter > 5)
        {
            AstarPath.active.Scan();
            AstarPath.active.astarData.gridGraph.maxSlope = 180;
            counter = 0;
        }
        counter++;

    }
}
