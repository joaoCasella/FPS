using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {
    public float maxHealth = 100f;
    public float minHealth = 0f;
    public float health;
    private bool playerDead;
    public int initialBulletCount = 10;
    public int lowestBulletCountPossible = 0;
    public float rateOfFire = 0.2f;
    public int lastShotIteration = 0;
    public int initialPontuation = 0;
    public int bulletCount, pontuation;
    public Transform bulletSpawnPoint;
    public GameObject bullet;
    public event Action OnPlayerDeath;

    // Use this for initialization
    void Start () {
        SetupInitialPlayerState();
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= minHealth) {
            Die();
        } else if (Input.GetMouseButton(0) && !playerDead) {
            if (Time.deltaTime * lastShotIteration >= rateOfFire)
            {
                if (bulletCount > lowestBulletCountPossible) Shoot();
            }
        }
        lastShotIteration++;
	}

    public void SetupInitialPlayerState()
    {
        health = maxHealth;
        playerDead = false;
        bulletCount = initialBulletCount;
        pontuation = initialPontuation;
        GameController.showCursor(false);
        lastShotIteration = 1000;
    }

    public void Hit(float damage)
    {
        health -= damage;
    }

    void Shoot()
    {
        GameObject bulletFired = (GameObject)Instantiate(bullet.gameObject, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletFired.transform.rotation = bulletSpawnPoint.transform.rotation;
        bulletCount--;
        lastShotIteration = 0;
    }

    void Die()
    {
        playerDead = true;
        OnPlayerDeath();
    }
}
