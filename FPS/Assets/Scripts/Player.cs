using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    public float maxHealth, health;
    private bool playerDead;
    public int bulletCount, pontuation;
    public Transform bulletSpawnPoint;
    public GameObject bullet;
    public event Action OnPlayerDeath;

    // Use this for initialization
    void Start () {
        maxHealth = 100;
        health = maxHealth;
        playerDead = false;
        bulletCount = 10;
        pontuation = 0;
        GameController.showCursor(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0f) {
            Die();
        } else if (Input.GetMouseButtonDown(0) && !playerDead) {
            if (bulletCount > 0) Shoot();
        }
	}

    public void ResetPlayer()
    {
        maxHealth = 100;
        health = maxHealth;
        playerDead = false;
        bulletCount = 10;
        pontuation = 0;
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
