using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Fps.UI
{
    public class TableUpdate : MonoBehaviour
    {
        private const string PlaceholderPlayerName = "-";

        [field: SerializeField]
        private GameObject ResultsMenuPanel { get; set; }

        [field: SerializeField]
        public bool GameOver { get; set; }

        private void Start()
        {
            if (GameOver)
            {
                UpdateWinnersTable();
            }
            RenderWinnersTable();
        }

        public void UpdateWinnersTable()
        {
            int i;
            for (i = 0; i < WinnersTable.TotalAmountPlayers; i++)
            {
                if (WinnersTable.Winners[i].Pontuation <= WinnersTable.CurrentPlayer.Pontuation) break;
            }

            if (i < WinnersTable.TotalAmountPlayers)
            {
                WinnersTablePlayerIdentifier[] array = (WinnersTablePlayerIdentifier[])WinnersTable.Winners.Clone();

                for (int j = i; j < WinnersTable.TotalAmountPlayers - 1; j++)
                {
                    WinnersTable.Winners[j + 1].Name = array[j].Name;
                    WinnersTable.Winners[j + 1].Pontuation = array[j].Pontuation;
                }

                WinnersTable.Winners[i].Name = WinnersTable.CurrentPlayer.Name;
                WinnersTable.Winners[i].Pontuation = WinnersTable.CurrentPlayer.Pontuation;
            }
        }

        public void RenderWinnersTable()
        {
            Text[] table = ResultsMenuPanel.GetComponentsInChildren<Text>();

            foreach (Text tableRow in table)
            {
                if (tableRow.name.Contains("Username"))
                {
                    int index = int.Parse(tableRow.name.Substring(tableRow.name.Length - 1, 1)) - 1;
                    tableRow.text = PlayerName(index);
                }
                else if (tableRow.name.Contains("Pontuation"))
                {
                    int index = int.Parse(tableRow.name.Substring(tableRow.name.Length - 1, 1)) - 1;
                    tableRow.text = WinnersTable.Winners[index].Pontuation.ToString();
                }
            }
        }

        private string PlayerName(int position)
        {
            if (!string.IsNullOrEmpty(WinnersTable.Winners[position].Name))
            {
                return WinnersTable.Winners[position].Name;
            }
            else
            {
                return PlaceholderPlayerName;
            }
        }
    }
}