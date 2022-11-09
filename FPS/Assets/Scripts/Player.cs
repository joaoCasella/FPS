using UnityEngine;

namespace Fps.Controller
{
    public class Player : Character
    {
        [field: SerializeField]
        private int InitialBulletCount { get; set; } = 10;

        [field: SerializeField]
        private float RateOfFire { get; set; } = 0.2f;

        private int LowestBulletCountPossible { get; set; } = 0;
        private int InitialPontuation { get; set; } = 0;
        private float TimeSinceLastShot { get; set; }

        public int BulletCount { get; set; }
        public int Pontuation { get; set; }

        private void Awake()
        {
            GunController.RateOfFire = RateOfFire;
        }

        private void Start()
        {
            CharacterInitialization();
            SetupInitialPlayerState();
        }

        protected void Update()
        {
            CheckCharacterHealth();

            if (Input.GetMouseButton(0)
                && !Dead
                && TimeSinceLastShot >= RateOfFire
                && BulletCount > LowestBulletCountPossible)
            {
                Shoot();
            }

            TimeSinceLastShot += Time.deltaTime;
        }

        public void SetupInitialPlayerState()
        {
            _health = MaxHealth;
            Dead = false;
            BulletCount = InitialBulletCount;
            Pontuation = InitialPontuation;
            GameController.ShowCursor(false);
            TimeSinceLastShot = RateOfFire;
        }

        protected new void Shoot()
        {
            base.Shoot();
            BulletCount--;
            TimeSinceLastShot = 0f;
        }
    }
}