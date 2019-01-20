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
            if (bulletCount > lowestBulletCountPossible) Shoot();
        }
	}

    public void SetupInitialPlayerState()
    {
        health = maxHealth;
        playerDead = false;
        bulletCount = initialBulletCount;
        pontuation = initialPontuation;
        GameController.showCursor(false);
    }

    void Shoot()
    {
        GameObject bulletFired = (GameObject)Instantiate(bullet.gameObject, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletFired.transform.rotation = bulletSpawnPoint.transform.rotation;
        bulletCount--;
    }

    void Die()
    {
        playerDead = true;
        OnPlayerDeath();
    }
}
