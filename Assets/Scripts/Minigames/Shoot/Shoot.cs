using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

namespace Minigames.Shoot
{
    public enum ShootableType
    {
        Cat =  1,
        Dog = 2,
        Chicken = 3
    }
    
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private ShootableType _correctShootableType;
        [SerializeField] private InputActionReference _cubeActionReference;
        [SerializeField] private GameObject _shootPoint;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private AudioSource _gunShotAudioSource;
        [SerializeField] private GameObject _gun;
        [SerializeField] private GameObject _rightHandObject;

        private float _minigameTimer;
        private bool _failureClipPlayed = false;
        private bool _success = false;
        private bool _ending = false;
        
        private void Update()
        {
            _minigameTimer -= Time.deltaTime;

            if (_minigameTimer <= 0 && !_ending)
            {
                // bool needed as we call EndGame on a slight delay for game feel reasons
                _ending = true;
                
                // only play the failure clip if it HASN'T been played before
                // (i.e., if they ran out of time without doing anything)
                if (!_failureClipPlayed && !_success)
                {
                    _sfxManager.PlayFailureClip();
                }
                
                Invoke(nameof(EndGame), 1f);
            }
        }
        
        private void OnEnable()
        {
            InitializeStartingVariables();
            SetCorrectShootableType();
            _cubeActionReference.action.performed += ShootGun;
            _gun.SetActive(true);
            _rightHandObject.SetActive(false);
        }
        
        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager._globalGameTimer;
            _success = false;
            _ending = false;
        }

        private void SetCorrectShootableType()
        {
            Array types = Enum.GetValues(typeof(ShootableType));
            Random random = new Random();
            _correctShootableType = (ShootableType)types.GetValue(random.Next(types.Length));
        }
        
        private void ShootGun(InputAction.CallbackContext obj)
        {
            _gunShotAudioSource.Play();
            
            if (Physics.Raycast(_shootPoint.transform.position, transform.forward, out RaycastHit hit, 100f))
            {
                if (hit.collider.gameObject.TryGetComponent(out Shootable shootable))
                {
                    RegisterShot(shootable._shootableType);
                }
            }
        }
        
        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            _gun.SetActive(false);
            _rightHandObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
        
        public void RegisterShot(ShootableType shootableType)
        {
            if (shootableType == _correctShootableType)
            {
                _sfxManager.PlaySuccessClip();
                _success = true;
            }
            else
            {
                _sfxManager.PlayFailureClip();
                _failureClipPlayed = true;
            }
        }
    }
}
