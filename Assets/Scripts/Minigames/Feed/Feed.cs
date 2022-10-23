using System;
using System.Collections.Generic;
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
        Orange = 1,
        Pepper = 2,
        Tomato = 3
    }
    
    public class Feed : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private FeedableType _correctFeedableType;
        [SerializeField] private FoodType _correctFoodType;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private List<Food> _foodList;
        
        [Header("Prompts")]
        [SerializeField] private AudioClip _cowOrangePrompt;
        [SerializeField] private AudioClip _cowPepperPrompt;
        [SerializeField] private AudioClip _cowTomatoPrompt;
        [SerializeField] private AudioClip _pigOrangePrompt;
        [SerializeField] private AudioClip _pigPepperPrompt;
        [SerializeField] private AudioClip _pigTomatoPrompt;

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

            SetCorrectFeedableType();
            SetCorrectFoodType();

            switch (_correctFeedableType)
            {
                case FeedableType.Cow:
                    instructions = "Feed the cow the ";
                    break;
                case FeedableType.Pig:
                    instructions = "Feed the pig the ";
                    break;
                default:
                    Debug.Log("Feedable type not set correctly!");
                    break;
            }
            
            switch (_correctFoodType)
            {
                case FoodType.Orange:
                    instructions += "orange!";
                    break;
                case FoodType.Pepper:
                    instructions += "pepper!";
                    break;
                case FoodType.Tomato:
                    instructions += "tomato!";
                    break;
                default:
                    Debug.Log("Food type not set correctly!");
                    break;
            }
            
            return instructions;
        }

        public AudioClip GetPromptAudioClip()
        {
            AudioClip audioClip = null;

            switch (_correctFeedableType)
            {
                case FeedableType.Cow:
                    if (_correctFoodType == FoodType.Orange)
                    {
                        audioClip = _cowOrangePrompt;
                    }
                    else if (_correctFoodType == FoodType.Pepper)
                    {
                        audioClip = _cowPepperPrompt;
                    }
                    else if (_correctFoodType == FoodType.Tomato)
                    {
                        audioClip = _cowTomatoPrompt;
                    }
                    break;
                case FeedableType.Pig:
                    if (_correctFoodType == FoodType.Orange)
                    {
                        audioClip = _pigOrangePrompt;
                    }
                    else if (_correctFoodType == FoodType.Pepper)
                    {
                        audioClip = _pigPepperPrompt;
                    }
                    else if (_correctFoodType == FoodType.Tomato)
                    {
                        audioClip = _pigTomatoPrompt;
                    }
                    break;                    break;
                default:
                    Debug.Log("Feedable type not set correctly!");
                    break;
            }
            
            return audioClip;
        }

        private void OnEnable()
        {
            _minigameManager.HideInstructionsText();
            InitializeStartingVariables();
            EnableFood();
        }

        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager._globalGameTimer;
            _success = false;
            _ending = false;
        }
        
        private void EnableFood()
        {
            // Necessary as the food objects are deactivated when fed
            foreach (Food food in _foodList)
            {
                food.gameObject.SetActive(true);
                food.gameObject.transform.parent = transform;
            }
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
                _success = true;
                _minigameTimer = 1f;
            }
            else
            {
                _sfxManager.PlayFailureClip();
                _failureClipPlayed = true;
            }
        }

        private void OnDisable()
        {
            foreach (Food food in _foodList)
            {
                food.gameObject.SetActive(false);
            }
        }
    }
}
