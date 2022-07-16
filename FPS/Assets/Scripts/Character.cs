using System;
using UnityEngine;

namespace Fps.Controller
{
    public class Character : MonoBehaviour
    {
        public virtual float MaxHealth => 100f;

        [field: SerializeField]
        private float MinHealth { get; set; } = 0f;

        [field: SerializeField]
        private Weapon GunController { get; set; }

        protected bool Dead { get; set; } = false;
        public event Action OnDeath;

        protected float _health;
        public float Health => Mathf.Clamp(_health, MinHealth, MaxHealth);

        public void CharacterInitialization()
        {
            _health = MaxHealth;
            Dead = false;
        }

        public void CheckCharacterHealth()
        {
            if (_health > MinHealth)
                return;

            Die();
        }

        public void Hit(float damage)
        {
            _health -= damage;
        }

        protected void Shoot()
        {
            GunController.Shoot();
        }

        protected virtual void Die()
        {
            Dead = true;
            OnDeath?.Invoke();
            OnDeath = null;
        }
    }
}