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
    public Transform gun;
    private Weapon gunController;
    public event Action OnDeath;

    // Use this for initialization
    public void CharacterInitialization () {
        health = maxHealth;
        dead = false;
        gunController = gun.GetComponent<Weapon>();
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
        gunController.Shoot();
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
