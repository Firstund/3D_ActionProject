using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private Player.Player currentPlayer = null;
    public Player.Player CurrentPlayer
    {
        get
        {
            return currentPlayer;
        }

        set
        {
            currentPlayer = value;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
