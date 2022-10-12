using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Hit
{
    public enum NailType
    {
        Red = 1,
        Blue = 2, 
        Green = 3
    }

    public class Nail : MonoBehaviour
    {
        [SerializeField] private Hit _hit;
        [SerializeField] private NailType _nailType;
        [SerializeField] private Transform _endPoint;

        private bool _beenHit = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_beenHit)
            {
                return;
            }

            _beenHit = true;

            gameObject.transform.position = _endPoint.position;
            
            if (other.gameObject.CompareTag("Hammer"))
            {
                _hit.RegisterHit(_nailType);
            }
        }
    }
}