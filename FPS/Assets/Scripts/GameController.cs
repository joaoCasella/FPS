using Fps.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fps.Controller
{
    public class GameController : MonoBehaviour
    {
        private readonly Vector3 PlayerStartPosition = new Vector3(26f, 7.01f, -26f);

        [field: SerializeField]
        private Player Player { get; set; }

        [field: SerializeField]
        private AmmoBox AmmoBox { get; set; }

        [field: SerializeField]
        private SceneTransitions SceneTransition { get; set; }

        [field: SerializeField]
        private Enemy EnemyPrefab { get; set; }

        [field: SerializeField]
        public float TimeUntilNextRound { get; set; }

        [field: SerializeField]
        private float NextRoundStart { get; set; } = 10f;

        [field: SerializeField]
        public Transform Castle { get; set; }

        private List<Vector3> EnemiesPositions { get; } = new List<Vector3>()
        {
            new Vector3(-15, 7.01f, 20),
            //new Vector3(15, 7.01f, 20),
            //new Vector3(15, 7.01f, -20),
            //new Vector3(-15, 7.01f, -20),
        };

        private List<Vector3> AmmoBoxesPositions { get; } = new List<Vector3>()
        {
            new Vector3(-15, 6.01f, 0),
            new Vector3(0, 6.01f, 0),
            new Vector3(15, 6.01f, 0),
        };

        private List<Enemy> Enemies { get; set; }

        private void Start()
        {
            Player.SetupInitialPlayerState();
            Player.transform.position = PlayerStartPosition;
            Player.OnDeath += SavePontuationAndFinishGame;
        }

        private void Update()
        {
            if (Player != null)
            {
                PlayerHealth.text = $"Life: {Mathf.RoundToInt(Player.Health)}/100";
                PlayerPontuation.text = $"Pontuation: {Mathf.RoundToInt(Player.Pontuation)}";
                PlayerAmmo.text = $"Ammo: {Mathf.RoundToInt(Player.BulletCount)}";
            }

            if (!AllEnemiesDead())
                return;

            if (TimeUntilNextRound > 0)
            {
                TimeUntilNextRound -= Time.deltaTime;
                return;
            }

            TimeUntilNextRound = NextRoundStart;
            CreateMoreEnemies();
            CreateAmmoBoxes();
        }

        private void SavePontuationAndFinishGame()
        {
            SceneTransition.PlayerDeathAndScreenChange(Player.Pontuation);
        }

        private void CreateMoreEnemies()
        {
            if (Enemies != null)
                Enemies.Clear();
            else
                Enemies = new List<Enemy>();

            foreach (var position in EnemiesPositions)
            {
                var enemyInstantiated = Instantiate(EnemyPrefab, position, Quaternion.identity, Castle);
                enemyInstantiated.transform.rotation = enemyInstantiated.LookAtPlayerRotation();
                enemyInstantiated.OnDeath += IncreasePlayerPontuation;
                Enemies.Add(enemyInstantiated);
            }
        }

        private void IncreasePlayerPontuation()
        {
            Player.Pontuation++;
        }

        private void CreateAmmoBoxes()
        {
            foreach (var position in AmmoBoxesPositions)
            {
                Instantiate(AmmoBox.gameObject, position, Quaternion.identity, Castle);
            }
        }

        private bool AllEnemiesDead()
        {
            return Enemies?.All(e => e == null) != false;
        }

        public void ShowDamageIndicator(Vector3 impactPoint)
        {

        }

        public static void ShowCursor(bool shouldShow)
        {
            Cursor.visible = shouldShow;
            Cursor.lockState = shouldShow ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}