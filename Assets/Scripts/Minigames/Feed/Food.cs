using UnityEngine;

namespace Minigames.Feed
{
    public class Food : MonoBehaviour
    {
        [SerializeField] private FoodType _foodType;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        
        private void Awake()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        private void OnEnable()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.rotation = _initialRotation;
            transform.position = _initialPosition;
        }
        
        public FoodType GetFoodType()
        {
            return _foodType;
        }
    }
}
