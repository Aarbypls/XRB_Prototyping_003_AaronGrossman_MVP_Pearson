using UnityEngine;

namespace Minigames.Cut
{
    public class Cuttable : MonoBehaviour
    {
        public CuttableType _CuttableType;
        
        [SerializeField] private float _speed = 1f;
        private Vector3 _position1;
        private Vector3 _position2;
        private Vector3 _target;
        private float _minDistance = .01f;

        private void Awake()
        {
            _position1 = transform.position;
            _position2 = new Vector3(_position1.x, _position1.y + 2, _position1.z);
            _target = _position2;
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, _position1) <= _minDistance)
            {
                _target = _position2;
            }
            else if (Vector3.Distance(transform.position, _position2) <= _minDistance)
            {
                _target = _position1;
            }

            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        }
    }
}
