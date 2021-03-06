﻿using System;
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
    public GameObject FileIO;
    public GameObject ProjectileSystem;
    public GameObject EnemySystem;
    public GameObject LoadingUI;
    private GameStatus gameStatus = GameStatus.StartMenu;

    private void Start()
    {
        initializeProjectileSystem();
        //print("holaGamesistem");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Player"));
        FileIO = GameObject.Find("Root").transform.Find("FileIO").gameObject;
    }
    byte firstFrame = 0;
    private void Update()
    {
        if (firstFrame == 0)
        {
            //transform.Find("TerrainSystem").gameObject.SetActive(false);
            firstFrame = 1;
        }
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

        LoadingUI.SetActive(true);
        CharacterSystem.GetComponent<CharacterSystem>().instantiateNewPlayer(nameCharacter);
        FileIO.SetActive(true);
        TerrainSystem.SetActive(true);
        EnemySystem.SetActive(true);
        LoadingUI.SetActive(false);
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

