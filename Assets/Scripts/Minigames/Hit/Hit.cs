using System;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Hit
{
    public class Hit : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private NailType _correctNailType;
        
        private void Awake()
        {
            SetCorrectNailType();
        }

        private void SetCorrectNailType()
        {
            Array types = Enum.GetValues(typeof(NailType));
            Random random = new Random();
            _correctNailType = (NailType)types.GetValue(random.Next(types.Length));
        }

        private void EndGame()
        {
            _minigameManager.PlayMinigame();
            this.gameObject.SetActive(false);
        }

        public void RegisterHit(NailType nailType)
        {
            if (nailType == _correctNailType)
            {
                HandleSuccess();
            }
            else
            {
                HandleFailure();
            }
        }

        private void HandleSuccess()
        {
            EndGame();
        }

        private void HandleFailure()
        {
            throw new NotImplementedException();
        }
    }
}
