using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

class CHUDSystem : MonoBehaviour
{
    GameObject StartMenu;
        GameObject MainMenu;

    GameObject PauseMenu;

    GameObject ExitButton;

    Button batten;

    CGameSystem GameSystem;

    List<GameObject> currentShowingHUDs;
    //GameObject currentShowingHUD;

    void Start()
    {
        currentShowingHUDs = new List<GameObject>();
        StartMenu = gameObject.transform.Find("StartMenu").gameObject;
        MainMenu = StartMenu.transform.Find("MainMenu").gameObject;
        PauseMenu = gameObject.transform.Find("PauseMenu").gameObject;
        GameSystem = GameObject.Find("GameSystem").GetComponent<CGameSystem>();
        requestToShowHUD(StartMenu);
        ExitButton  = gameObject.transform.Find("PauseMenu/Canvas/ExitButton").gameObject;

        Debug.Log("hola");
    }

    void Update()
    {
        batten = ExitButton.GetComponent<Button>();

        ColorBlock cb = batten.colors;

        cb.normalColor = Color.green;

        batten.colors = cb;
    }

    public void requestToShowHUD(GameObject HUDObject)
    {
            HUDObject.SetActive(true);
            currentShowingHUDs.Add( HUDObject);
    }
    public void requestToHideHUD(GameObject HUDObject)
    {
        HUDObject.SetActive(false);
        currentShowingHUDs.Remove(HUDObject);
    }

    public void requestToStartNewGame()
    {
        GameObject.Find("EnviromentSystem").transform.Find("EnviromentStds").gameObject.SetActive(true);
        GameObject.Find("Root").transform.Find("CharacterSystem").gameObject.SetActive(true);
        //.SetActive(true);
        StartMenu.SetActive(false);
    }
    public void requestToEndGame()
    {
        Application.Quit();
    }
    void OnGUI()
    {
        if (!StartMenu.activeInHierarchy) {
            if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Escape.ToString())))
            {
                PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
            }
        }
    }
}

