using UnityEngine;

public class Enemy : Character
{
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

        if (Time.deltaTime * iterations >= interval && !dead)
        {
            Shoot();
        }
        else
        {
            iterations++;
        }
    }

    public Quaternion LookAtPlayerRotation()
    {
        if (player == null)
            return Quaternion.identity;

        var lookPos = player.position - transform.position;
        lookPos.y = 0;
        return Quaternion.LookRotation(lookPos);
    }

    void TrackPlayerMovement()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, LookAtPlayerRotation(), Time.deltaTime * damping);
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
