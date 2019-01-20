using UnityEngine;

public class Enemy : Character {
    public override float maxHealth
    {
        get { return 30f; }
    }
    public float interval = 1f;
    private int iterations = 0;
    public int damping = 1;
    public Transform player;

    // Use this for initialization
    void Start()
    {
        CharacterInitialization();
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCharacterHealth();

        TrackPlayerMovement();
 
        if (Time.deltaTime * iterations >= interval && !dead) {
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

    protected new void Shoot()
    {
        base.Shoot();
        iterations = 0;
    }

    protected override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }
}
