using UnityEngine;

namespace Fps.Controller
{
    public class AmmoBox : MonoBehaviour
    {
        [field: SerializeField]
        private int ReloadAmmo { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
            {
                player.StoredAmmo += ReloadAmmo;
            }
        }
    }
}