using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour {

    public float expireRate, bulletSpeed;
    private float currentTimer;
    public float enemyDamage;

    // Use this for initialization
    void Start () {
		
	}
	
	void FixedUpdate () {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        currentTimer += 1 * Time.deltaTime;

        if (currentTimer >= expireRate)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().health -= enemyDamage;
        }
        Destroy(this.gameObject);
    }
}
