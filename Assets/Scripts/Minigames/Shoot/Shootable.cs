using System;
using UnityEngine;

namespace Minigames.Shoot
{
    public class Shootable : MonoBehaviour
    {
        public ShootableType _shootableType;
        
        [SerializeField] private float _xRotation = 0f;
        [SerializeField] private float _yRotation = 0f;
        [SerializeField] private float _zRotation = 0f;

        private float _rotateSpeed = 1f;

        private void OnEnable()
        {
            _rotateSpeed = 1f;
        }

        public void SpeedUpRotation()
        {
            _rotateSpeed = 10f;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(_xRotation * _rotateSpeed, _yRotation * _rotateSpeed, _zRotation * _rotateSpeed);
        }
    }
}
