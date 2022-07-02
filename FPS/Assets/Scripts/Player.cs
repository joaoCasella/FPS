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
        private int LastShotIteration { get; set; } = 0;
        private int InitialPontuation { get; set; } = 0;

        public int BulletCount { get; set; }
        public int Pontuation { get; set; }

        private void Start()
        {
            CharacterInitialization();
            SetupInitialPlayerState();
        }

        private void Update()
        {
            CheckCharacterHealth();

            if (Input.GetMouseButton(0)
                && !Dead
                && Time.deltaTime * LastShotIteration >= RateOfFire
                && BulletCount > LowestBulletCountPossible)
            {
                Shoot();
            }
            LastShotIteration++;
        }

        public void SetupInitialPlayerState()
        {
            _health = MaxHealth;
            Dead = false;
            BulletCount = InitialBulletCount;
            Pontuation = InitialPontuation;
            GameController.ShowCursor(false);
            LastShotIteration = 1000;
        }

        protected new void Shoot()
        {
            base.Shoot();
            BulletCount--;
            LastShotIteration = 0;
        }
    }
}