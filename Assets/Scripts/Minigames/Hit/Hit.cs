using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Minigames.Hit
{
    public enum NailType
    {
        Red = 1,
        Blue = 2, 
        Green = 3
    }
    
    public class Hit : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private NailType _correctNailType;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private AudioSource _hitAudioSource;
        [FormerlySerializedAs("_nailHitSFX")] [SerializeField] private AudioClip _hitSFX;
        [SerializeField] private GameObject _weapon;
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
            SetCorrectNailType();
            _weapon.SetActive(true);
            _rightHandObject.SetActive(false);
        }
        
        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager._globalGameTimer;
            _success = false;
            _ending = false;
        }

        private void SetCorrectNailType()
        {
            Array types = Enum.GetValues(typeof(NailType));
            Random random = new Random();
            _correctNailType = (NailType)types.GetValue(random.Next(types.Length));
        }

        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            _weapon.SetActive(false);
            _rightHandObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        public void RegisterHit(NailType nailType)
        {
            if (nailType == _correctNailType)
            {
                PlayHitSFX();
                _sfxManager.PlaySuccessClip();
                _success = true;
            }
            else
            {
                PlayHitSFX();
                _sfxManager.PlayFailureClip();
                _failureClipPlayed = true;
            }
        }

        private void PlayHitSFX()
        {
            _hitAudioSource.clip = _hitSFX;
            _hitAudioSource.Play();
        }
        
    }
}
