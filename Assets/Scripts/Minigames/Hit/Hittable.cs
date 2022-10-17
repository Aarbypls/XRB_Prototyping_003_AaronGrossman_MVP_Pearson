using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Minigames.Hit
{
    public class Hittable : MonoBehaviour
    {
        [SerializeField] private Hit _hit;
        [FormerlySerializedAs("_nailType")] [SerializeField] private Type type;
        //[SerializeField] private Transform _endPoint;

        private bool _beenHit = false;

        private void OnEnable()
        {
            _beenHit = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_beenHit)
            {
                return;
            }

            if (other.gameObject.CompareTag("Weapon"))
            {
                _beenHit = true;

                // gameObject.transform.position = _endPoint.position;
                
                _hit.RegisterHit(type);

            }
        }
    }
}