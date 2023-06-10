using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public float AccelerationOfGravity = 9.8f;

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
