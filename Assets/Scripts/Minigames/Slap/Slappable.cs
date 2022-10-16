using System;
using UnityEngine;

namespace Minigames.Slap
{
    public class Slappable : MonoBehaviour
    {
        public SlappableType _SlappableType;

        [SerializeField] private Slap _slap;
        [SerializeField] private float _force = 100f;

        private bool _hasBeenSlapped = false;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private void Awake()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        private void OnEnable()
        {
            _hasBeenSlapped = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.rotation = _initialRotation;
            transform.position = _initialPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_hasBeenSlapped && other.gameObject.GetComponentInParent<Slapper>())
            {
                _hasBeenSlapped = true;
                
                // add force
                Vector3 hitVector = (transform.position - other.transform.position).normalized;
                transform.GetComponent<Rigidbody>().AddForce(hitVector * _force);
                
                // TO-DO: Lucas: play sound of slapped chicken here

                Invoke(nameof(RegisterSlap), 2f);
            }
        }

        private void RegisterSlap()
        {
            _slap.RegisterSlap(_SlappableType);
        }
    }
}
