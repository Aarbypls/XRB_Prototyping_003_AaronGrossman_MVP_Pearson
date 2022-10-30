using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Feed
{
    public enum FeedableType
    {
        Pig = 1,
        Cow = 2,
        Self = 3
    }

    public enum FoodType
    {
        Orange = 1,
        Pepper = 2,
        Tomato = 3
    }
    
    public class Feed : MonoBehaviour
    {
        public FoodType correctFoodType;
        public FeedableType correctFeedableType;

        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private List<Food> _foodList;
        
        [Header("English Prompts")]
        [SerializeField] private AudioClip _cowOrangePromptEnglish;
        [SerializeField] private AudioClip _cowPepperPromptEnglish;
        [SerializeField] private AudioClip _cowTomatoPromptEnglish;
        [SerializeField] private AudioClip _pigOrangePromptEnglish;
        [SerializeField] private AudioClip _pigPepperPromptEnglish;
        [SerializeField] private AudioClip _pigTomatoPromptEnglish;
        [SerializeField] private AudioClip _selfOrangePromptEnglish;
        [SerializeField] private AudioClip _selfPepperPromptEnglish;
        [SerializeField] private AudioClip _selfTomatoPromptEnglish;
        
        [Header("Spanish Prompts")]
        [SerializeField] private AudioClip _cowOrangePromptSpanish;
        [SerializeField] private AudioClip _cowPepperPromptSpanish;
        [SerializeField] private AudioClip _cowTomatoPromptSpanish;
        [SerializeField] private AudioClip _pigOrangePromptSpanish;
        [SerializeField] private AudioClip _pigPepperPromptSpanish;
        [SerializeField] private AudioClip _pigTomatoPromptSpanish;
        [SerializeField] private AudioClip _selfOrangePromptSpanish;
        [SerializeField] private AudioClip _selfPepperPromptSpanish;
        [SerializeField] private AudioClip _selfTomatoPromptSpanish;
        
        private float _minigameTimer;
        private bool _failureClipPlayed = false;
        private bool _success = false;
        private bool _ending = false;
        private ReportCardItem _reportCardItem = new ReportCardItem();

        private void Update()
        {
            _minigameTimer -= Time.deltaTime;

            if (_minigameTimer <= 0 && !_ending)
            {
                // bool needed as we call EndGame on a slight delay for game feel reasons
                _ending = true;
                
                // only play the failure clip if it HASN'T been played before
                // (i.e., if they ran out of time without doing anything)
                if (!_success)
                {
                    if (!_failureClipPlayed)
                    {
                        _sfxManager.PlayFailureClip();
                    }
                    
                    _reportCardItem.timedOut = true;
                    _minigameManager.AddReportCardItemToList(_reportCardItem);
                }
                
                Invoke(nameof(EndGame), _minigameManager.globalEndOfGameTimer);
            }
        }
        
        public string SetObjectivesAndGetUIText()
        {
            string instructions = String.Empty;

            SetCorrectFeedableType();
            SetCorrectFoodType();

            switch (_minigameManager.language)
            {
                case Language.English:
                    switch (correctFeedableType)
                    {
                        case FeedableType.Cow:
                            instructions = "Feed the cow the ";
                            break;
                        case FeedableType.Pig:
                            instructions = "Feed the pig the ";
                            break;
                        case FeedableType.Self:
                            instructions = "Feed yourself the ";
                            break;
                        default:
                            Debug.Log("Feedable type not set correctly!");
                            break;
                    }
            
                    switch (correctFoodType)
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
                    break;
                case Language.Spanish:
                    switch (correctFeedableType)
                    {
                        case FeedableType.Cow:
                            instructions = "¡Alimenta a la vaca ";
                            break;
                        case FeedableType.Pig:
                            instructions = "¡Alimenta al cerdo ";
                            break;
                        case FeedableType.Self:
                            switch (correctFoodType)
                            {
                                case FoodType.Orange:
                                    instructions = "¡Come la naranja!";
                                    break;
                                case FoodType.Pepper:
                                    instructions = "¡Come el pimiento!";
                                    break;
                                case FoodType.Tomato:
                                    instructions = "¡Come el tomate!";
                                    break;
                            }
                            break;
                        default:
                            Debug.Log("Feedable type not set correctly!");
                            break;
                    }

                    // Need this logic due to the grammatical nature of Spanish
                    if (correctFeedableType != FeedableType.Self)
                    {
                        switch (correctFoodType)
                        {
                            case FoodType.Orange:
                                instructions += "con la naranja!";
                                break;
                            case FoodType.Pepper:
                                instructions += "con el pimiento!";
                                break;
                            case FoodType.Tomato:
                                instructions += "con el tomate!";
                                break;
                            default:
                                Debug.Log("Food type not set correctly!");
                                break;
                        }
                    }
                    break;
                default:
                    Debug.Log("Language not properly set in the MinigameManager!");
                    break;               
            }
            
            _reportCardItem.prompt = instructions;
            _reportCardItem.translation = "";

            return instructions;
        }

        public AudioClip GetPromptAudioClip()
        {
            AudioClip audioClip = null;

            switch (_minigameManager.language)
            {
                case Language.English:
                    switch (correctFeedableType)
                    {
                        case FeedableType.Cow:
                            if (correctFoodType == FoodType.Orange)
                            {
                                audioClip = _cowOrangePromptEnglish;
                            }
                            else if (correctFoodType == FoodType.Pepper)
                            {
                                audioClip = _cowPepperPromptEnglish;
                            }
                            else if (correctFoodType == FoodType.Tomato)
                            {
                                audioClip = _cowTomatoPromptEnglish;
                            }
                            break;
                        case FeedableType.Pig:
                            if (correctFoodType == FoodType.Orange)
                            {
                                audioClip = _pigOrangePromptEnglish;
                            }
                            else if (correctFoodType == FoodType.Pepper)
                            {
                                audioClip = _pigPepperPromptEnglish;
                            }
                            else if (correctFoodType == FoodType.Tomato)
                            {
                                audioClip = _pigTomatoPromptEnglish;
                            }
                            break;
                        case FeedableType.Self:
                            if (correctFoodType == FoodType.Orange)
                            {
                                audioClip = _selfOrangePromptEnglish;
                            }
                            else if (correctFoodType == FoodType.Pepper)
                            {
                                audioClip = _selfPepperPromptEnglish;
                            }
                            else if (correctFoodType == FoodType.Tomato)
                            {
                                audioClip = _selfTomatoPromptEnglish;
                            }
                            break;
                        default:
                            Debug.Log("Feedable type not set correctly!");
                            break;
                    }
                    break;
                case Language.Spanish:
                    switch (correctFeedableType)
                    {
                        case FeedableType.Cow:
                            if (correctFoodType == FoodType.Orange)
                            {
                                audioClip = _cowOrangePromptSpanish;
                            }
                            else if (correctFoodType == FoodType.Pepper)
                            {
                                audioClip = _cowPepperPromptSpanish;
                            }
                            else if (correctFoodType == FoodType.Tomato)
                            {
                                audioClip = _cowTomatoPromptSpanish;
                            }
                            break;
                        case FeedableType.Pig:
                            if (correctFoodType == FoodType.Orange)
                            {
                                audioClip = _pigOrangePromptSpanish;
                            }
                            else if (correctFoodType == FoodType.Pepper)
                            {
                                audioClip = _pigPepperPromptSpanish;
                            }
                            else if (correctFoodType == FoodType.Tomato)
                            {
                                audioClip = _pigTomatoPromptSpanish;
                            }
                            break;
                        case FeedableType.Self:
                            if (correctFoodType == FoodType.Orange)
                            {
                                audioClip = _selfOrangePromptSpanish;
                            }
                            else if (correctFoodType == FoodType.Pepper)
                            {
                                audioClip = _selfPepperPromptSpanish;
                            }
                            else if (correctFoodType == FoodType.Tomato)
                            {
                                audioClip = _selfTomatoPromptSpanish;
                            }
                            break;
                        default:
                            Debug.Log("Feedable type not set correctly!");
                            break;
                    }
                    break;
                default:
                    Debug.Log("Language not properly set in the MinigameManager!");
                    break;               
            }

            return audioClip;
        }

        private void OnEnable()
        {
            _reportCardItem = new ReportCardItem();
            _minigameManager.HideInstructionsText();
            InitializeStartingVariables();
            EnableFood();
        }

        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager.globalGameTimer;
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
            correctFeedableType = (FeedableType)types.GetValue(random.Next(types.Length));
        }

        private void SetCorrectFoodType()
        {
            Array types = Enum.GetValues(typeof(FoodType));
            Random random = new Random();
            correctFoodType = (FoodType)types.GetValue(random.Next(types.Length));
        }
        
        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            this.gameObject.SetActive(false);
        }

        public void RegisterFoodAndFeedable(FoodType foodType, FeedableType feedableType)
        {
            if (foodType == correctFoodType && feedableType == correctFeedableType)
            {
                _sfxManager.PlaySuccessClip();
                _success = true;
                _minigameTimer = 1f;
                _minigameManager.RegisterSuccess();
            }
            else
            {
                _reportCardItem.timedOut = false;
                _minigameManager.AddReportCardItemToList(_reportCardItem);
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
