using UnityEngine;
using UnityEngine.UI;

public class TableUpdate : MonoBehaviour
{
    public GameObject resultsMenuPanel;
    public bool gameOver;
    private readonly string placeholderPlayerName = "-";
    private readonly int totalAmountPlayers = 5;

    // Use this for initialization
    void Start()
    {
        if (gameOver)
        {
            UpdateWinnersTable();
        }
        RenderWinnersTable();
    }

    public void UpdateWinnersTable()
    {
        int i;
        for (i = 0; i < totalAmountPlayers; i++)
        {
            if (WinnersTable.Winners[i].pontuation <= WinnersTable.CurrentPlayer.pontuation) break;
        }

        if (i < totalAmountPlayers)
        {
            PlayerIdentifier[] array = (PlayerIdentifier[])WinnersTable.Winners.Clone();

            for (int j = i; j < totalAmountPlayers - 1; j++)
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

        foreach (Text tableRow in table)
        {
            if(tableRow.name.Contains("Username"))
            {
                int index = int.Parse(tableRow.name.Substring(tableRow.name.Length - 1, 1)) - 1;
                tableRow.text = PlayerName(index);
            } else if (tableRow.name.Contains("Pontuation"))
            {
                int index = int.Parse(tableRow.name.Substring(tableRow.name.Length - 1, 1)) - 1;
                tableRow.text = WinnersTable.Winners[index].pontuation.ToString();
            }
        }
    }

    private string PlayerName(int position)
    {
        if (!string.IsNullOrEmpty(WinnersTable.Winners[position].name))
        {
            return WinnersTable.Winners[position].name;
        }
        else
        {
            return placeholderPlayerName;
        }
    }
}
