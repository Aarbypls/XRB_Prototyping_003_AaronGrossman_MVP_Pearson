using System;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Cut
{
    public enum CuttableType
    {
        RedSphere = 1,
        BlueSphere = 2
    }
    
    public class Cut : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private CuttableType _correctCuttableType;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private GameObject _sword;
        [SerializeField] private GameObject _rightHandObject;
        
        private void OnEnable()
        {
            SetCorrectCuttableType();
            _sword.SetActive(true);
            _rightHandObject.SetActive(false);
        }

        private void SetCorrectCuttableType()
        {
            Array types = Enum.GetValues(typeof(CuttableType));
            Random random = new Random();
            _correctCuttableType = (CuttableType)types.GetValue(random.Next(types.Length));
        }

        public void RegisterCut(CuttableType cuttableType)
        {
            if (cuttableType == _correctCuttableType)
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

        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            _sword.SetActive(false);
            _rightHandObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
