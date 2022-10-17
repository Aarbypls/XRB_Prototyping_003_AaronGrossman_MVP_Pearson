using UnityEngine;

namespace Minigames.Shoot
{
    public class Shootable : MonoBehaviour
    {
        public ShootableType _shootableType;
        
        /*
        [SerializeField] private float _xRotation = 0f;
        [SerializeField] private float _yRotation = 0f;
        [SerializeField] private float _zRotation = 0f; */
        
        // Update is called once per frame
        void Update()
        {
            //transform.Rotate(_xRotation, _yRotation, _zRotation);
            transform.Translate(Vector3.up * (Time.deltaTime/8), Space.Self);
        }
    }
}
