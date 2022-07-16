using UnityEngine;
using UnityEngine.Serialization;

namespace Fps.Controller
{
    public class Weapon : MonoBehaviour
    {
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

        public void Shoot()
        {
            if (Anim != null)
                Anim.SetTrigger("Shoot");

            if (AudioSource != null)
                AudioSource.PlayOneShot(ShotSound);

            var bulletFired = Instantiate(Bullet, BulletSpawnPoint.transform.position, Quaternion.identity);
            bulletFired.transform.rotation = BulletSpawnPoint.transform.rotation;
        }
    }
}