using UnityEngine;

namespace Minigames.Cut
{
    public class Destroy : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject, 1f);
        }
    }
}
