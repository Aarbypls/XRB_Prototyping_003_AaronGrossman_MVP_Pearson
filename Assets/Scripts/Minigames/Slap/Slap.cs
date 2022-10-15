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

        private void OnEnable()
        {
            SetCorrectSlappableType();
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
