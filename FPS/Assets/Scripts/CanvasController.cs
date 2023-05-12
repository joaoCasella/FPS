using TMPro;
using UnityEngine;

namespace Fps.Controller
{
    public class CanvasController : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI PlayerHealth { get; set; }

        [field: SerializeField]
        public TextMeshProUGUI PlayerPontuation { get; set; }

        [field: SerializeField]
        public TextMeshProUGUI PlayerAmmo { get; set; }

        private Player Player { get; set; }

        public void Setup(Player player)
        {
            Player = player;
        }

        private void Update()
        {
            if (Player == null)
                return;

            PlayerHealth.text = $"Life: {Mathf.RoundToInt(Player.Health)}/100";
            PlayerPontuation.text = $"Pontuation: {Mathf.RoundToInt(Player.Pontuation)}";
            PlayerAmmo.text = $"Ammo: {Mathf.RoundToInt(Player.AmmoOnMag)}/{Mathf.RoundToInt(Player.StoredAmmo)}";
        }
    }
}