using System;
using UnityEngine;

namespace Minigames.Pet
{
    public class Pettable : MonoBehaviour
    {
        [SerializeField] private PettableType _pettableType;
        [SerializeField] private Pet _pet;
        
        private Transform _lastLeftHandTransform = null;
        private Transform _lastRightHandTransform = null;
        private float _totalPetDistance;
        private float _requiredPettingDistance = 1f;
        private bool _finishedPetting = false;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Petter petter))
            {
                switch (petter.hand)
                {
                    case Hand.Left:
                        if (!_lastLeftHandTransform)
                        {
                            _lastLeftHandTransform = other.gameObject.transform;
                        }
                        else
                        {
                            _totalPetDistance += Vector3.Distance(_lastLeftHandTransform.position,
                                other.gameObject.transform.position);
                        }
                        break;
                    case Hand.Right:
                        if (_lastRightHandTransform)
                        {
                            _lastRightHandTransform = other.gameObject.transform;
                        }
                        else
                        {
                            _totalPetDistance += Vector3.Distance(_lastRightHandTransform.position,
                                other.gameObject.transform.position);
                        }
                        break;
                    default:
                        Debug.Log("Petter hand type not set: " + other.gameObject.name);
                        break;
                }
            }
        }

        private void Update()
        {
            if (_finishedPetting)
            {
                return;
            }

            if (_totalPetDistance >= _requiredPettingDistance)
            {
                _finishedPetting = true;
                _pet.RegisterPet(_pettableType);
            }
        }
    }
}