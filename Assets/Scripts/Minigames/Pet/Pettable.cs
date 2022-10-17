using System;
using UnityEngine;

namespace Minigames.Pet
{
    public class Pettable : MonoBehaviour
    {
        [SerializeField] private PettableType _pettableType;
        [SerializeField] private Pet _pet;
        
        private Vector3 _lastLeftHandPosition;
        private Vector3 _lastRightHandPosition;
        
        private float _totalPetDistance;
        private float _requiredPettingDistance = 15f;
        private bool _finishedPetting = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Petter petter))
            {
                switch (petter.hand)
                {
                    case Hand.Left:
                        _lastLeftHandPosition = other.gameObject.transform.position;
                        break;
                    case Hand.Right:
                        _lastRightHandPosition = other.gameObject.transform.position;
                        break;
                    default:
                        Debug.Log("Petter hand type not set: " + other.gameObject.name);
                        break;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Petter petter))
            {
                switch (petter.hand)
                {
                    case Hand.Left:
                        _totalPetDistance += Vector3.Distance(_lastLeftHandPosition,
                            other.gameObject.transform.position);
                            break;
                    case Hand.Right:
                        _totalPetDistance += Vector3.Distance(_lastRightHandPosition,
                            other.gameObject.transform.position);
                        break;
                    default:
                        Debug.Log("Petter hand type not set: " + other.gameObject.name);
                        break;
                }
            }
        }

        private void Update()
        {
            Debug.Log(_totalPetDistance);

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