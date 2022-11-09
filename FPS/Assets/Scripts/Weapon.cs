using UnityEngine;
using UnityEngine.Serialization;

namespace Fps.Controller
{
    public class Weapon : MonoBehaviour
    {
        const float DefaultShootAnimationTime = 0.2f;

        [field: SerializeField, FormerlySerializedAs("bulletSpawnPoint")]
        private Transform BulletSpawnPoint { get; set; }

        [field: SerializeField, FormerlySerializedAs("bullet")]
        private GameObject Bullet { get; set; }

        [field: SerializeField, FormerlySerializedAs("shotSound")]
        private AudioClip ShotSound { get; set; }

        [field: SerializeField, FormerlySerializedAs("anim")]
        private Animator Anim { get; set; }

        [field: SerializeField, FormerlySerializedAs("audioSource")]
        private AudioSource AudioSource { get; set; }

        [field: SerializeField]
        public float RateOfFire { get; set; }

        private int ShootingAnimParameter { get; set; }

        private void Start()
        {
            ShootingAnimParameter = Animator.StringToHash("Shoot");
            if (Anim != null && RateOfFire < DefaultShootAnimationTime)
                Anim.SetFloat("ShootSpeed", DefaultShootAnimationTime / RateOfFire);
        }

        public void Shoot()
        {
            if (Anim != null)
                Anim.SetTrigger(ShootingAnimParameter);

            if (AudioSource != null)
                AudioSource.PlayOneShot(ShotSound);

            var bulletFired = Instantiate(Bullet, BulletSpawnPoint.transform.position, Quaternion.identity);
            bulletFired.transform.rotation = BulletSpawnPoint.transform.rotation;
        }
    }
}