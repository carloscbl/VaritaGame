using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


class GameSystem : MonoBehaviour
{
    public enum GameStatus
    {
        StartMenu, Pause, Running
    };

    public GameObject TerrainSystem;
    public GameObject CharacterSystem;
    private GameStatus gameStatus = GameStatus.StartMenu;

    public void requestEnableGameObject(GameObject go)
    {
        print("Dentro del request");
        go.SetActive(true);
    }
    public GameStatus getGameStatus()
    {
        return gameStatus;
    }

    public void SetGameStatus(GameStatus newStatus)
    {
        gameStatus = newStatus;
    }

    public void CreateWorld(string nameCharacter)
    {
        gameStatus = GameStatus.Running;
        CharacterSystem.GetComponent<CharacterSystem>().instantiateNewPlayer(nameCharacter);
        TerrainSystem.SetActive(true);
    }

    public void PauseGame()
    {

    }

    public void ResumeGame()
    {

    }
}

