using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Pet
{
    public enum PettableType
    {
        Chicken = 1,
        Duck = 2,
        Pig = 3
    }
    
    public class Pet : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private PettableType _correctPettableType;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private List<Pettable> _pettables;

        [Header("English Prompts")] 
        [SerializeField] private AudioClip _chickenPromptEnglish;
        [SerializeField] private AudioClip _duckPromptEnglish;
        [SerializeField] private AudioClip _pigPromptEnglish;
        
        [Header("Spanish Prompts")] 
        [SerializeField] private AudioClip _chickenPromptSpanish;
        [SerializeField] private AudioClip _duckPromptSpanish;
        [SerializeField] private AudioClip _pigPromptSpanish;  
        
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
            string instructionsEnglish = string.Empty;
            string instructionsNonEnglish = string.Empty;

            SetCorrectPettableType();
            
            switch (_correctPettableType)
            {
                case PettableType.Chicken:
                    instructionsEnglish = "Pet the chicken!";
                    instructionsNonEnglish = "??Acaricia el pollo!";
                    break;
                case PettableType.Duck:
                    instructionsEnglish = "Pet the duck!";
                    instructionsNonEnglish = "??Acaricia al pato!";
                    break;
                case PettableType.Pig:
                    instructionsEnglish = "Pet the pig!";
                    instructionsNonEnglish = "??Acaricia al cerdo!";
                    break;
                default:
                    Debug.Log("Pettable type not set correctly!");
                    break;
            }

            _reportCardItem.prompt = _minigameManager.language == Language.English ? instructionsEnglish : instructionsNonEnglish;
            _reportCardItem.translation = instructionsEnglish;
            
            return _minigameManager.language == Language.English ? instructionsEnglish : instructionsNonEnglish;
        }

        public AudioClip GetPromptAudioClip()
        {
            AudioClip audioClip = null;

            switch (_minigameManager.language)
            {
                case Language.English:
                    switch (_correctPettableType)
                    {
                        case PettableType.Chicken:
                            audioClip = _chickenPromptEnglish;
                            break;
                        case PettableType.Duck:
                            audioClip = _duckPromptEnglish;
                            break;
                        case PettableType.Pig:
                            audioClip = _pigPromptEnglish;
                            break;
                        default:
                            Debug.Log("Pettable type not set correctly!");
                            break;
                    }
                    break;
                case Language.Spanish:
                    switch (_correctPettableType)
                    {
                        case PettableType.Chicken:
                            audioClip = _chickenPromptSpanish;
                            break;
                        case PettableType.Duck:
                            audioClip = _duckPromptSpanish;
                            break;
                        case PettableType.Pig:
                            audioClip = _pigPromptSpanish;
                            break;
                        default:
                            Debug.Log("Pettable type not set correctly!");
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
            _minigameManager.HideInstructionsText();
            InitializeStartingVariables();
        }
        
        private void InitializeStartingVariables()
        {
            foreach (Pettable pettable in _pettables)
            {
                if (pettable._pettableType == _correctPettableType)
                {
                    pettable.gameObject.SetActive(true);
                }
            }
            
            _minigameTimer = _minigameManager.globalGameTimer;
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
            foreach (Pettable pettable in _pettables)
            {
                pettable.gameObject.SetActive(false);
            }
            
            _minigameManager.StartNextMinigame();
            this.gameObject.SetActive(false);
        }

        public void RegisterPet(PettableType pettableType)
        {
            if (pettableType == _correctPettableType)
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
                _ending = true;
                Invoke(nameof(EndGame), 1.1f);
            }
        }
    }
}
