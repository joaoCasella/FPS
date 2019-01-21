using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Transform bulletSpawnPoint;
    public GameObject bullet;
    private Animator anim;
    private AudioSource audioSource;
    public AudioClip shotSound;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if(anim != null) {
            anim.SetTrigger("Shoot");
        }
        if(audioSource != null)
        {
            audioSource.PlayOneShot(shotSound);
        }
        GameObject bulletFired = (GameObject)Instantiate(bullet.gameObject, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletFired.transform.rotation = bulletSpawnPoint.transform.rotation;
    }
}
