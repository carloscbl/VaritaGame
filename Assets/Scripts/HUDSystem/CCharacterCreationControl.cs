using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCharacterCreationControl : MonoBehaviour {
    GameObject selectButton;
    GameObject selectionPanel;
    GameObject PlayerCharacterImg;
    GameObject NaNText;
    // Use this for initialization
    void Start () {

        selectButton = gameObject.transform.Find("SelectPlayerButton").gameObject;
        selectionPanel = gameObject.transform.Find("SelectionPanel").gameObject;
        PlayerCharacterImg = gameObject.transform.Find("PlayerCharacterImg").gameObject;
        NaNText = gameObject.transform.Find("Text").gameObject;
        PlayerCharacterImg.SetActive(false);
        selectButton.GetComponent<Button>().interactable = false;
        //First of all check if there is created players and charge it

        //If not, we make player create new one


        //Then we able it to be selected


    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void setPlayerImg(GameObject imgGOComponent)
    {
        NaNText.SetActive(false);
        PlayerCharacterImg.SetActive(true);
        print(imgGOComponent.GetComponent<RawImage>().texture.name);
        PlayerCharacterImg.GetComponent<RawImage>().texture = imgGOComponent.GetComponent<RawImage>().texture;
        selectButton.GetComponent<Button>().interactable = true;
    }
}
