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
    bool firstTimeGame;
    bool startingGame;
    bool pause;
    void Start()
    {
        startingGame = true;
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

