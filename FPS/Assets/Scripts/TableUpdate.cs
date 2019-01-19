using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableUpdate : MonoBehaviour
{
    public GameObject resultsMenuPanel;
    public bool gameOver;

    // Use this for initialization
    void Start()
    {
        if (gameOver)
        {
            UpdateWinnersTable();
        }
        RenderWinnersTable();
    }

    public static void UpdateWinnersTable()
    {
        int i, j;
        PlayerIdentifier[] array;
        for (i = 0; i < 5; i++)
        {
            if (WinnersTable.Winners[i].pontuation <= WinnersTable.CurrentPlayer.pontuation) break;
        }

        if (i < 5)
        {
            array = (PlayerIdentifier[])WinnersTable.Winners.Clone();

            for (j = i; j < 4; j++)
            {
                WinnersTable.Winners[j + 1].name = array[j].name;
                WinnersTable.Winners[j + 1].pontuation = array[j].pontuation;
            }

            WinnersTable.Winners[i].name = WinnersTable.CurrentPlayer.name;
            WinnersTable.Winners[i].pontuation = WinnersTable.CurrentPlayer.pontuation;
        }
    }

    public void RenderWinnersTable()
    {
        Text[] table = resultsMenuPanel.GetComponentsInChildren<Text>();
        for (int i = 0; i <= 12; i++)
        {
            if (table[i].name == "FirstUserIdentifierHeading")
            {
                table[i].text = WinnersTable.Winners[0].name;
            }
            else if (table[i].name == "FirstUserPontuationHeading")
            {
                table[i].text = WinnersTable.Winners[0].pontuation.ToString();
            }
            else if (table[i].name == "SecondUserIdentifierHeading")
            {
                table[i].text = WinnersTable.Winners[1].name;
            }
            else if (table[i].name == "SecondUserPontuationHeading")
            {
                table[i].text = WinnersTable.Winners[1].pontuation.ToString();
            }
            else if (table[i].name == "ThirdUserIdentifierHeading")
            {
                table[i].text = WinnersTable.Winners[2].name;
            }
            else if (table[i].name == "ThirdUserPontuationHeading")
            {
                table[i].text = WinnersTable.Winners[2].pontuation.ToString();
            }
            else if (table[i].name == "FourthUserIdentifierHeading")
            {
                table[i].text = WinnersTable.Winners[3].name;
            }
            else if (table[i].name == "FourthUserPontuationHeading")
            {
                table[i].text = WinnersTable.Winners[3].pontuation.ToString();
            }
            else if (table[i].name == "FifthUserIdentifierHeading")
            {
                table[i].text = WinnersTable.Winners[4].name;
            }
            else if (table[i].name == "FifthUserIdentifierHeading")
            {
                table[i].text = WinnersTable.Winners[4].pontuation.ToString();
            }
        }
    }
}
