using System;
using UnityEngine;

namespace Minigames.Shoot
{
    public class Shootable : MonoBehaviour
    {
        public ShootableType _shootableType;
        public GameObject _confettiObject;
        
        public void HideAndShootConfetti()
        {
            _confettiObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
