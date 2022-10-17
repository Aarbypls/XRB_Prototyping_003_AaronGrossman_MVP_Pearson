using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Minigames.Hit
{
    public enum Type
    {
        Fly = 1,
        Ladybug = 2, 
        Cockroach = 3
    }
    
    public class Hit : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [FormerlySerializedAs("_correctNailType")] [SerializeField] private Type correctType;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private AudioSource _hitAudioSource;
        [SerializeField] private AudioClip _surfaceHitSFX;
        [FormerlySerializedAs("_nailHitSFX")] [SerializeField] private AudioClip _hitSFX;
        [FormerlySerializedAs("_hammer")] [SerializeField] private GameObject _weapon;
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
                
                Invoke(nameof(EndGame), _minigameManager._globalEndOfGameTimer);
            }
        }

        public string SetObjectivesAndGetUIText()
        {
            string instructions = String.Empty;

            correctType = Type.Fly;
            //SetCorrectType();

            switch (correctType)
            {
                case Type.Fly:
                    instructions = "Hit the Fly!";
                    break;
                case Type.Ladybug:
                    instructions = "Hit the Ladybug!";
                    break;
                case Type.Cockroach:
                    instructions = "Hit the Cockroach!";
                    break;
                default:
                    Debug.Log("Hittable type not set correctly");
                    break;
            }
            
            return instructions;
        }
        
        private void OnEnable()
        {
            _minigameManager.HideInstructionsText();
            InitializeStartingVariables();
            _weapon.SetActive(true);
            _rightHandObject.SetActive(false);
        }
        
        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager._globalGameTimer;
            _success = false;
            _ending = false;
        }

        private void SetCorrectType()
        {
            Array types = Enum.GetValues(typeof(Type));
            Random random = new Random();
            correctType = (Type)types.GetValue(random.Next(types.Length));
        }

        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            _weapon.SetActive(false);
            _rightHandObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        public void RegisterHit(Type type)
        {
            if (type == correctType)
            {
                PlayNailSFX();
                _sfxManager.PlaySuccessClip();
                _success = true;
                _minigameTimer = 1f;
            }
            else
            {
                PlayNailSFX();
                _sfxManager.PlayFailureClip();
                _failureClipPlayed = true;
            }
        }

        private void PlayNailSFX()
        {
            _hitAudioSource.clip = _hitSFX;
            _hitAudioSource.Play();
        }

        public void PlayWoodSFX()
        {
            _hitAudioSource.clip = _surfaceHitSFX;
            _hitAudioSource.Play();
        }
    }
}
