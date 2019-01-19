using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBullet : MonoBehaviour {

    public float expireRate, bulletSpeed;
    private float currentTimer;
    public float playerDamage;

    // Use this for initialization
    void Start () {
		
	}
	
	void FixedUpdate () {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        currentTimer += Time.deltaTime;

        if (currentTimer >= expireRate)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().health -= playerDamage;
        }
        Destroy(this.gameObject);
    }
}
