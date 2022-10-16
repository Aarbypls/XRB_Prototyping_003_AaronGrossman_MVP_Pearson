using System;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Feed
{
    public enum FeedableType
    {
        Pig = 1,
        Cow = 2
    }

    public enum FoodType
    {
        Peach = 1,
        Pepper = 2,
        Tomato = 3
    }
    
    public class Feed : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private FeedableType _correctFeedableType;
        [SerializeField] private FoodType _correctFoodType;
        [SerializeField] private SFXManager _sfxManager;

        private void OnEnable()
        {
            SetCorrectFeedableType();
            SetCorrectFoodType();
        }

        private void SetCorrectFeedableType()
        {
            Array types = Enum.GetValues(typeof(FeedableType));
            Random random = new Random();
            _correctFeedableType = (FeedableType)types.GetValue(random.Next(types.Length));
        }

        private void SetCorrectFoodType()
        {
            Array types = Enum.GetValues(typeof(FoodType));
            Random random = new Random();
            _correctFoodType = (FoodType)types.GetValue(random.Next(types.Length));
        }
        
        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            this.gameObject.SetActive(false);
        }

        public void RegisterFoodAndFeedable(FoodType foodType, FeedableType feedableType)
        {
            if (foodType == _correctFoodType && feedableType == _correctFeedableType)
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
