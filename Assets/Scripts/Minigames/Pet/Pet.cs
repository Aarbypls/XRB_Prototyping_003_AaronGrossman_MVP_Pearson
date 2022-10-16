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

        private void OnEnable()
        {
            SetCorrectPettableType();
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
