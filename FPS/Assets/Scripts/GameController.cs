using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Bullet bullet;
    public Player player;
    public Enemy enemy;
    public AmmoBox ammoBox;
    public GameObject sceneTransition;
    public float enemyDamage, playerDamage;
    private GameObject enemy1, enemy2, enemy3, enemy4, ammoBox1, ammoBox2, ammoBox3;
    public float timeUntilNextRound, nextRoundStart;
    public Text playerHealth, playerPontuation, playerAmmo;
    public Transform castle;

    // Use this for initialization
    void Start () {
        player.ResetPlayer();
        player.transform.position = new Vector3(0, 3, 0);
        player.OnPlayerDeath += SavePontuationAndFinishGame;
        nextRoundStart = 10f;
    }
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            playerHealth.text = "Life: " + Mathf.RoundToInt(player.health).ToString() + "/100";
            playerPontuation.text = "Pontuation: " + Mathf.RoundToInt(player.pontuation).ToString();
            playerAmmo.text = "Ammo: " + Mathf.RoundToInt(player.bulletCount).ToString();
        }
        if (AllEnemiesDead())
        {
            if (timeUntilNextRound <= 0)
            {
                timeUntilNextRound = nextRoundStart;
                CreateMoreEnemies();
                CreateAmmoBoxes();

            } else
            {
                timeUntilNextRound -= Time.deltaTime;
            }
        };
	}

    void SavePontuationAndFinishGame()
    {
        sceneTransition.GetComponent<SceneTransitions>().PlayerDeathAndScreenChange(player.pontuation);
    }

    void CreateMoreEnemies()
    {
        enemy1 = (GameObject)Instantiate(enemy.gameObject, new Vector3(-30, 2, 24), Quaternion.identity, castle);
        enemy1.GetComponent<Enemy>().OnEnemyDeath += IncreasePlayerPontuation;
        enemy2 = (GameObject)Instantiate(enemy.gameObject, new Vector3(30, 2, 24), Quaternion.identity, castle);
        enemy2.GetComponent<Enemy>().OnEnemyDeath += IncreasePlayerPontuation;
        enemy3 = (GameObject)Instantiate(enemy.gameObject, new Vector3(30, 2, -12), Quaternion.identity, castle);
        enemy3.GetComponent<Enemy>().OnEnemyDeath += IncreasePlayerPontuation;
        enemy4 = (GameObject)Instantiate(enemy.gameObject, new Vector3(-30, 2, -12), Quaternion.identity, castle);
        enemy4.GetComponent<Enemy>().OnEnemyDeath += IncreasePlayerPontuation;
    }

    void IncreasePlayerPontuation()
    {
        player.pontuation ++;
    }

    void CreateAmmoBoxes()
    {
        if (ammoBox1 == null)
        {
            ammoBox1 = (GameObject)Instantiate(ammoBox.gameObject, new Vector3(-30, 2, 8), Quaternion.identity, castle);
        }
        if (ammoBox2 == null)
        {
            ammoBox2 = (GameObject)Instantiate(ammoBox.gameObject, new Vector3(0, 2, 8), Quaternion.identity, castle);
        }
        if (ammoBox3 == null)
        {
            ammoBox3 = (GameObject)Instantiate(ammoBox.gameObject, new Vector3(30, 2, 8), Quaternion.identity, castle);
        }
    }

    bool AllEnemiesDead()
    {
        return (enemy1 == null && enemy2 == null && enemy3 == null && enemy4 == null);
    }
}
