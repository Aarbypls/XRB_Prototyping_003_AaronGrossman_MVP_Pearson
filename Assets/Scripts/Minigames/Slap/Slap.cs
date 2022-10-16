using System;
using Minigames.Hit;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Slap
{
    public enum SlappableType
    {
        Chicken = 1
    }
    
    public class Slap : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private SlappableType _correctSlappableType;
        [SerializeField] private SFXManager _sfxManager;

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
            SetCorrectSlappableType();
        }
        
        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager._globalGameTimer;
            _success = false;
            _ending = false;
        }

        private void SetCorrectSlappableType()
        {
            Array types = Enum.GetValues(typeof(SlappableType));
            Random random = new Random();
            _correctSlappableType = (SlappableType)types.GetValue(random.Next(types.Length));
        }
        
        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            this.gameObject.SetActive(false);
        }

        public void RegisterSlap(SlappableType slappableType)
        {
            if (slappableType == _correctSlappableType)
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
