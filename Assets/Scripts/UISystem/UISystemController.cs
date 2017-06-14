using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

class UISystemController : MonoBehaviour
{
    public GameObject StartGameMenu;
    public GameObject MainMenu;
    public GameObject PauseMenu;
    public GameObject InGameUI;
    public GameObject LoadingUI;
    GameSystem gameSystem;

    void Start()
    {
        gameSystem = GameObject.Find("Root").GetComponent<GameSystem>();
        StartGameMenu.GetComponent<UICharacterSelection>().InitCharacterList(new List<string>() { "Arwin" }); //TODO move this to root
        LoadingUI = this.transform.Find("LoadingScreen").gameObject;
    }

    void Update()
    {
        if (gameSystem.getGameStatus() != GameSystem.GameStatus.StartMenu && Input.GetKeyUp(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
            if (PauseMenu.activeInHierarchy)
            {
                gameSystem.PauseGame();
            }
            else
            {
                gameSystem.ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        gameSystem.ResumeGame();
    }

    public void requestToEndGame()
    {
        Application.Quit();
    }

    public void GoStartGameMenu()
    {
        MainMenu.SetActive(false);
        StartGameMenu.SetActive(true);
    }

    public void CreateWorld(string name)
    {
        MainMenu.SetActive(false);
        StartGameMenu.SetActive(false);
        //LoadingUI.SetActive(true);
        InGameUI.GetComponent<UISpells>().SetSpells(new List<Spell>() { new Spell("Attact1", 100, 0), new Spell("Attact2", 100, 0), new Spell("Attact3", 100, 0), new Spell("Attact4", 100, 0) });//TODO: change this
        InGameUI.SetActive(true);
        
        GameObject.Find("Root").GetComponent<GameSystem>().CreateWorld(name);
        //LoadingUI.SetActive(false);
    }

    public void ActivateMainMenu()
    {
        MainMenu.SetActive(true);
    }
}

