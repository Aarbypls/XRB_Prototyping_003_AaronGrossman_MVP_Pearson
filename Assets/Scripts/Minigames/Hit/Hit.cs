using System;
using UnityEngine;
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
        [SerializeField] private AudioClip _woodHitSFX;
        [SerializeField] private AudioClip _nailHitSFX;
        [SerializeField] private GameObject _hammer;

        private void Awake()
        {
            SetCorrectNailType();
            _hammer.SetActive(true);
        }

        private void SetCorrectNailType()
        {
            Array types = Enum.GetValues(typeof(NailType));
            Random random = new Random();
            _correctNailType = (NailType)types.GetValue(random.Next(types.Length));
            Debug.Log(_correctNailType);
        }

        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            _hammer.SetActive(false);
            this.gameObject.SetActive(false);
        }

        public void RegisterHit(NailType nailType)
        {
            if (nailType == _correctNailType)
            {
                PlayNailSFX();
                _sfxManager.PlaySuccessClip();
                HandleSuccess();
            }
            else
            {
                PlayNailSFX();
                _sfxManager.PlayFailureClip();
                HandleFailure();
            }
        }

        private void PlayNailSFX()
        {
            _hitAudioSource.clip = _nailHitSFX;
            _hitAudioSource.Play();
        }

        public void PlayWoodSFX()
        {
            _hitAudioSource.clip = _woodHitSFX;
            _hitAudioSource.Play();
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
