using UnityEngine;

namespace Fps.Controller
{
    public class Bullet : MonoBehaviour
    {
        protected virtual string OpponentTagIdentifier => "Enemy";

        [field: SerializeField]
        private float Lifetime { get; set; }

        [field: SerializeField]
        private float BulletSpeed { get; set; }

        [field: SerializeField]
        private float Damage { get; set; }

        private float CurrentTimer { get; set; }

        private void FixedUpdate()
        {
            transform.Translate(BulletSpeed * Time.deltaTime * Vector3.forward);
            CurrentTimer += Time.deltaTime;

            if (CurrentTimer >= Lifetime && this != null && gameObject != null)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(OpponentTagIdentifier) && other.TryGetComponent(out Character character))
                character.Hit(Damage, Vector3.forward);

            Destroy(gameObject);
        }
    }
}