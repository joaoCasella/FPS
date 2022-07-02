using UnityEngine;

namespace Fps.Controller
{
    public class Enemy : Character
    {
        public override float MaxHealth => 30f;

        [field: SerializeField]
        public float Interval { get; set; } = 1f;

        [field: SerializeField]
        public int Damping { get; set; } = 1;

        private int Iterations { get; set; } = 0;
        private Transform Player { get; set; }

        private void Start()
        {
            CharacterInitialization();
            Player = FindObjectOfType<Player>().transform;
        }

        private void Update()
        {
            CheckCharacterHealth();

            TrackPlayerMovement();

            if (Time.deltaTime * Iterations >= Interval && !Dead)
            {
                Shoot();
            }
            else
            {
                Iterations++;
            }
        }

        public Quaternion LookAtPlayerRotation()
        {
            if (Player == null)
                return Quaternion.identity;

            var lookPos = Player.position - transform.position;
            lookPos.y = 0;
            return Quaternion.LookRotation(lookPos);
        }

        private void TrackPlayerMovement()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookAtPlayerRotation(), Time.deltaTime * Damping);
        }

        protected new void Shoot()
        {
            base.Shoot();
            Iterations = 0;
        }

        protected override void Die()
        {
            base.Die();
            Destroy(gameObject);
        }
    }
}