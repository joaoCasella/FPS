using Fps.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour {

    public int reloadAmmo;
    // Use this for initialization
    void Start () {
        reloadAmmo = 20;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().BulletCount += reloadAmmo;
        }
        Destroy(this.gameObject);
    }
}
