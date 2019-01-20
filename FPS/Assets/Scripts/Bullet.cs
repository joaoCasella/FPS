using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float expireRate, bulletSpeed;
    private float currentTimer;
    public float damage;

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        currentTimer += Time.deltaTime;

        if (currentTimer >= expireRate)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(OpponentTagIdentifier()))
        {
            other.gameObject.GetComponent<Character>().Hit(damage);
        }
        Destroy(this.gameObject);
    }

    protected virtual string OpponentTagIdentifier()
    {
        return "Enemy";
    }
}
