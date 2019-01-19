using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour {

    public float maxHealth, health, interval;
    private int iterations;
    public int damping;
    private bool enemyDead;
    public Transform player, bulletSpawnPoint;
    public GameObject bullet;
    public event Action OnEnemyDeath;

    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        interval = 1f;
        enemyDead = false;
        iterations = 0;
        damping = 1;
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayerMovement();
 
        if (health <= 0) {
            Die();
        } else if (Time.deltaTime * iterations >= interval && !enemyDead) {
            Shoot();
        } else {
            iterations ++;
        }
    }

    void TrackPlayerMovement()
    {
        if (player != null)
        {
            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
    }

    void Shoot()
    {
        GameObject bulletFired = (GameObject)Instantiate(bullet.gameObject, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletFired.transform.rotation = bulletSpawnPoint.transform.rotation;
        iterations = 0;
    }

    void Die()
    {
        enemyDead = true;
        OnEnemyDeath();
        Destroy(this.gameObject);
    }
}
