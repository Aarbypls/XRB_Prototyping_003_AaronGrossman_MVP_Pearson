using System;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Pet
{
    public enum PettableType
    {
        Chicken = 1
    }
    
    public class Pet : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private PettableType _correctPettableType;
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
            SetCorrectPettableType();
        }
        
        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager._globalGameTimer;
            _success = false;
            _ending = false;
        }

        private void SetCorrectPettableType()
        {
            Array types = Enum.GetValues(typeof(PettableType));
            Random random = new Random();
            _correctPettableType = (PettableType)types.GetValue(random.Next(types.Length));
        }

        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            this.gameObject.SetActive(false);
        }

        public void RegisterPet(PettableType pettableType)
        {
            if (pettableType == _correctPettableType)
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
