using UnityEngine;

public class Player : Character {
    public int initialBulletCount = 10;
    public int lowestBulletCountPossible = 0;
    public float rateOfFire = 0.2f;
    public int lastShotIteration = 0;
    public int initialPontuation = 0;
    public int bulletCount, pontuation;

    // Use this for initialization
    void Start () {
        CharacterInitialization();
        SetupInitialPlayerState();
    }
	
	// Update is called once per frame
	void Update () {
        CheckCharacterHealth();

        if (Input.GetMouseButton(0) && !dead) {
            if (Time.deltaTime * lastShotIteration >= rateOfFire)
            {
                if (bulletCount > lowestBulletCountPossible) Shoot();
            }
        }
        lastShotIteration++;
	}

    public void SetupInitialPlayerState()
    {
        health = maxHealth;
        dead = false;
        bulletCount = initialBulletCount;
        pontuation = initialPontuation;
        GameController.ShowCursor(false);
        lastShotIteration = 1000;
    }

    protected new void Shoot()
    {
        base.Shoot();
        bulletCount--;
        lastShotIteration = 0;
    }
}
