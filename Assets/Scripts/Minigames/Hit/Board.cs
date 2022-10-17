using UnityEngine;

namespace Minigames.Hit
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Hit _hit;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Hammer"))
            {
                //_hit.PlayWoodSFX();
            }
        }
    }
}
