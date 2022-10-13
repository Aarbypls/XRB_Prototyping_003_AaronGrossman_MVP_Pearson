using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

namespace Minigames.Shoot
{
    public enum ShootableType
    {
        Pizza =  1,
        Hotdog = 2,
        Cake = 3
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
        
        private void Awake()
        {
            SetCorrectShootableType();
            _cubeActionReference.action.performed += ShootGun;
            _gun.SetActive(true);
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
            this.gameObject.SetActive(false);
        }
        
        public void RegisterShot(ShootableType shootableType)
        {
            if (shootableType == _correctShootableType)
            {
                _sfxManager.PlaySuccessClip();
                HandleSuccess();
            }
            else
            {
                _sfxManager.PlayFailureClip();
                HandleFailure();
            }
        }

        private void HandleSuccess()
        {
            EndGame();
        }

        private void HandleFailure()
        {
            EndGame();
        }
    }
}
