using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour {

    public float maxHealth = 30f, interval = 1f;
    private float health;
    private int iterations = 0;
    public int damping = 1;
    private bool enemyDead = false;
    public Transform player, bulletSpawnPoint;
    public GameObject bullet;
    public event Action OnEnemyDeath;

    // Use this for initialization
    void Start()
    {
        health = maxHealth;
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

    public void Hit(float damage)
    {
        health -= damage;
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
