using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


class CGameSystem : MonoBehaviour
{
    public enum gameStatus{
        StartMenu,Pause,Running
    };
    static GameObject TerrainSystem;
    bool firstTimeGame;
    bool startingGame;
    bool pause;
    void Awake()
    {
        startingGame = true;
        TerrainSystem = GameObject.Find("Root").transform.Find("TerrainSystem").gameObject;
    }
    public static GameObject getTerrainSystem()
    {
        return TerrainSystem;
    }
    public void requestEnableGameObject(GameObject go )
    {
        print("Dentro del request");
        go.SetActive(true);
    }
    public gameStatus getGameStatus() {
        if (startingGame)
        {
            return gameStatus.StartMenu;
        }else if (pause)
        {
            return gameStatus.Pause;
        }else
        {
            return gameStatus.Running;
        }
    }
}

