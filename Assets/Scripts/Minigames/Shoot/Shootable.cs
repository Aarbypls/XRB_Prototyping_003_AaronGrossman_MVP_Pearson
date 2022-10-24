using System;
using UnityEngine;

namespace Minigames.Shoot
{
    public class Shootable : MonoBehaviour
    {
        public ShootableType _shootableType;
        public GameObject _confettiObject;

        public void DisableConfetti()
        {
            _confettiObject.SetActive(false);
        }

        public void HideAndShootConfetti(ShootableType correctShootableType)
        {
            if (correctShootableType == _shootableType)
            {
                _confettiObject.SetActive(true);
            }
            
            gameObject.SetActive(false);
        }
    }
}
