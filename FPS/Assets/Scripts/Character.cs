using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour {
    public virtual float maxHealth
    {
        get { return 100f; }
    }
    public float minHealth = 0f;
    protected float health;
    protected bool dead = false;
    public Transform bulletSpawnPoint;
    public GameObject bullet;
    public event Action OnDeath;

    // Use this for initialization
    public void CharacterInitialization () {
        health = maxHealth;
        dead = false;
    }
	
	// Update is called once per frame
	public void CheckCharacterHealth () {
        if (health <= minHealth)
        {
            Die();
        }
    }

    public void Hit(float damage)
    {
        health -= damage;
    }

    protected void Shoot()
    {
        GameObject bulletFired = (GameObject)Instantiate(bullet.gameObject, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletFired.transform.rotation = bulletSpawnPoint.transform.rotation;
    }

    protected virtual void Die()
    {
        dead = true;
        OnDeath();
    }

    public float Health()
    {
        return health;
    }
}
