using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerIdentifier
{
    public int pontuation;
    public string name;

    public PlayerIdentifier(int pontuation, string name)
    {
        this.pontuation = pontuation;
        this.name = name;
    }
}

public class WinnersTable : MonoBehaviour {
    private static PlayerIdentifier[] winners = new PlayerIdentifier[5];
    private static PlayerIdentifier currentPlayer;

    public static PlayerIdentifier[] Winners
    {
        get
        {
            return winners;
        }
        set
        {
            winners = value;
        }
    }

    public static PlayerIdentifier CurrentPlayer
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
}
