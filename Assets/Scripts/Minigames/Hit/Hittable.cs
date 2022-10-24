using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Minigames.Hit
{
    public class Hittable : MonoBehaviour
    {
        [SerializeField] private Hit _hit;
        [FormerlySerializedAs("_nailType")] [SerializeField] private Type type;

        [SerializeField] private GameObject _splatter;
        [SerializeField] private GameObject _squashedFly;
        //[SerializeField] private Transform _endPoint;

        private bool _beenHit = false;

        private void OnEnable()
        {
            _beenHit = false;
            _splatter.SetActive(false);
            _squashedFly.SetActive(false);
            gameObject.SetActive(true);
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
                _splatter.SetActive(true);
                _squashedFly.SetActive(true);
                _hit.RegisterHit(type);
                gameObject.SetActive(false);
            }
        }
    }
}