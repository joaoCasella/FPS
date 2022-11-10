using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fps.Controller
{
    public class Weapon : MonoBehaviour
    {
        const float DefaultShootAnimationTime = 0.2f;
        const float DefaultReloadAnimationTime = 3.3f;

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
        private AudioClip MagazineSlidingSound { get; set; }

        [field: SerializeField]
        private AudioClip MagazineClickingSound { get; set; }

        public float RateOfFire { get; set; }

        private int ShootAnimParameter { get; set; }
        private int ReloadAnimParameter { get; set; }

        private void Start()
        {
            ShootAnimParameter = Animator.StringToHash("Shoot");
            ReloadAnimParameter = Animator.StringToHash("Reload");
            if (Anim != null && RateOfFire < DefaultShootAnimationTime)
                Anim.SetFloat("ShootSpeed", DefaultShootAnimationTime / RateOfFire);
        }

        public void Shoot()
        {
            if (Anim != null)
                Anim.SetTrigger(ShootAnimParameter);

            if (AudioSource != null)
                AudioSource.PlayOneShot(ShotSound);

            var bulletFired = Instantiate(Bullet, BulletSpawnPoint.transform.position, Quaternion.identity);
            bulletFired.transform.rotation = BulletSpawnPoint.transform.rotation;
        }

        public async Task Reload()
        {
            if (Anim != null)
                Anim.SetTrigger(ReloadAnimParameter);

            const int momentMagazineIsRemovedInAnimationMs = 690;
            await Task.Delay(momentMagazineIsRemovedInAnimationMs);
            AudioSource.PlayOneShot(MagazineSlidingSound);

            const int delayUntilMagazineIsInsertedAnimationMs = 1470;
            await Task.Delay(delayUntilMagazineIsInsertedAnimationMs);
            AudioSource.Stop();
            AudioSource.PlayOneShot(MagazineClickingSound);

            await Task.Delay(((int) DefaultReloadAnimationTime * 1000) - (momentMagazineIsRemovedInAnimationMs + delayUntilMagazineIsInsertedAnimationMs));
        }
    }
}