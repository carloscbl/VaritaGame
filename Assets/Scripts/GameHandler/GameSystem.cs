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
    public GameObject ProjectileSystem;
    private GameStatus gameStatus = GameStatus.StartMenu;

    private void Start()
    {
        initializeProjectileSystem();
        print("holaGamesistem");
    }

    public void requestEnableGameObject(GameObject go)
    {
        //print("Dentro del request");
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
    public void initializeProjectileSystem()
    {
        ProjectileSystem = new GameObject("ProjectileSystem");
        ProjectileSystem.AddComponent<ProjectileSystem>();
        ProjectileSystem.transform.SetParent(this.transform);
    }
}

