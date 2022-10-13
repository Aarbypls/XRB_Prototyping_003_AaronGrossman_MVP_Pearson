using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Hit
{
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

            if (other.gameObject.CompareTag("Hammer"))
            {
                _beenHit = true;

                gameObject.transform.position = _endPoint.position;
                
                _hit.RegisterHit(_nailType);
            }
        }
    }
}