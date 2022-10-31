using System;
using System.Collections.Generic;
using Minigames.Hit;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Minigames.Slap
{
    public enum SlappableType
    {
        YellowRubberDuck = 1,
        BlueRubberDuck = 2,
        RedRubberDuck = 3,
        PurpleRubberDuck = 4,
        GreenRubberDuck = 5,
        OrangeRubberDuck = 6,
        PinkRubberDuck = 7,
        WhiteRubberDuck = 8,
        BlackRubberDuck = 9
    }
    
    public class Slap : MonoBehaviour
    {
        [FormerlySerializedAs("_success")] public bool success = false;
        
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private SlappableType _correctSlappableType;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private List<Slappable> _slappables;
        
        [Header("English Prompts")]
        [SerializeField] private AudioClip _yellowRubberDuckPromptEnglish;
        [SerializeField] private AudioClip _blueRubberDuckPromptEnglish;
        [SerializeField] private AudioClip _redRubberDuckPromptEnglish;
        [SerializeField] private AudioClip _purpleRubberDuckPromptEnglish;
        [SerializeField] private AudioClip _greenRubberDuckPromptEnglish;
        [SerializeField] private AudioClip _orangeRubberDuckPromptEnglish;
        [SerializeField] private AudioClip _pinkRubberDuckPromptEnglish;
        [SerializeField] private AudioClip _whiteRubberDuckPromptEnglish;
        [SerializeField] private AudioClip _blackRubberDuckPromptEnglish;

        [Header("Spanish Prompts")]
        [SerializeField] private AudioClip _yellowRubberDuckPromptSpanish;
        [SerializeField] private AudioClip _blueRubberDuckPromptSpanish;
        [SerializeField] private AudioClip _redRubberDuckPromptSpanish;
        [SerializeField] private AudioClip _purpleRubberDuckPromptSpanish;
        [SerializeField] private AudioClip _greenRubberDuckPromptSpanish;
        [SerializeField] private AudioClip _orangeRubberDuckPromptSpanish;
        [SerializeField] private AudioClip _pinkRubberDuckPromptSpanish;
        [SerializeField] private AudioClip _whiteRubberDuckPromptSpanish;
        [SerializeField] private AudioClip _blackRubberDuckPromptSpanish;
        
        private float _minigameTimer;
        private bool _failureClipPlayed = false;
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
                if (!success)
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
                
            SetCorrectSlappableType();

            switch (_correctSlappableType)
            {
                case SlappableType.YellowRubberDuck:
                    instructionsEnglish = "Slap the yellow rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma amarillo!";
                    break;
                case SlappableType.BlueRubberDuck:
                    instructionsEnglish = "Slap the blue rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma azul!";
                    break;
                case SlappableType.RedRubberDuck:
                    instructionsEnglish = "Slap the red rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma rojo!";
                    break;
                case SlappableType.PurpleRubberDuck:
                    instructionsEnglish = "Slap the purple rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma morado!";
                    break;
                case SlappableType.GreenRubberDuck:
                    instructionsEnglish = "Slap the green rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma verde!";
                    break;
                case SlappableType.OrangeRubberDuck:
                    instructionsEnglish = "Slap the orange rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma naranja!";
                    break;
                case SlappableType.PinkRubberDuck:
                    instructionsEnglish = "Slap the pink rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma rosa!";
                    break;
                case SlappableType.WhiteRubberDuck:
                    instructionsEnglish = "Slap the white rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma blanco!";
                    break;
                case SlappableType.BlackRubberDuck:
                    instructionsEnglish = "Slap the black rubber duck!";
                    instructionsNonEnglish = "¡Dale una palmada al patito de goma negro!";
                    break;
                default:
                    Debug.Log("Cuttable type not set correctly!");
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
                    switch (_correctSlappableType)
                    {
                        case SlappableType.YellowRubberDuck:
                            audioClip = _yellowRubberDuckPromptEnglish;
                            break;
                        case SlappableType.BlueRubberDuck:
                            audioClip = _blueRubberDuckPromptEnglish;
                            break;
                        case SlappableType.RedRubberDuck:
                            audioClip = _redRubberDuckPromptEnglish;
                            break;
                        case SlappableType.PurpleRubberDuck:
                            audioClip = _purpleRubberDuckPromptEnglish;
                            break;
                        case SlappableType.GreenRubberDuck:
                            audioClip = _greenRubberDuckPromptEnglish;
                            break;
                        case SlappableType.OrangeRubberDuck:
                            audioClip = _orangeRubberDuckPromptEnglish;
                            break;
                        case SlappableType.PinkRubberDuck:
                            audioClip = _pinkRubberDuckPromptEnglish;
                            break;
                        case SlappableType.WhiteRubberDuck:
                            audioClip = _whiteRubberDuckPromptEnglish;
                            break;
                        case SlappableType.BlackRubberDuck:
                            audioClip = _blackRubberDuckPromptEnglish;
                            break;
                        default:
                            Debug.Log("Cuttable type not set correctly!");
                            break;
                    }
                    break;
                case Language.Spanish:
                    switch (_correctSlappableType)
                    {
                        case SlappableType.YellowRubberDuck:
                            audioClip = _yellowRubberDuckPromptSpanish;
                            break;
                        case SlappableType.BlueRubberDuck:
                            audioClip = _blueRubberDuckPromptSpanish;
                            break;
                        case SlappableType.RedRubberDuck:
                            audioClip = _redRubberDuckPromptSpanish;
                            break;
                        case SlappableType.PurpleRubberDuck:
                            audioClip = _purpleRubberDuckPromptSpanish;
                            break;
                        case SlappableType.GreenRubberDuck:
                            audioClip = _greenRubberDuckPromptSpanish;
                            break;
                        case SlappableType.OrangeRubberDuck:
                            audioClip = _orangeRubberDuckPromptSpanish;
                            break;
                        case SlappableType.PinkRubberDuck:
                            audioClip = _pinkRubberDuckPromptSpanish;
                            break;
                        case SlappableType.WhiteRubberDuck:
                            audioClip = _whiteRubberDuckPromptSpanish;
                            break;
                        case SlappableType.BlackRubberDuck:
                            audioClip = _blackRubberDuckPromptSpanish;
                            break;
                        default:
                            Debug.Log("Cuttable type not set correctly!");
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
            foreach (Slappable slappable in _slappables)
            {
                if (slappable._SlappableType == _correctSlappableType)
                {
                    slappable.gameObject.SetActive(true);
                }
            }

            _minigameTimer = _minigameManager.globalGameTimer;
            success = false;
            _ending = false;
        }

        private void SetCorrectSlappableType()
        {
            Array types = Enum.GetValues(typeof(SlappableType));
            Random random = new Random();
            _correctSlappableType = (SlappableType)types.GetValue(random.Next(types.Length));
        }

        public void RegisterSlap(SlappableType slappableType)
        {
            if (slappableType == _correctSlappableType)
            {
                _sfxManager.PlaySuccessClip();
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
        
        private void EndGame()
        {
            foreach (Slappable slappable in _slappables)
            {
                slappable.gameObject.SetActive(false);
            }
            
            _minigameManager.StartNextMinigame();
            this.gameObject.SetActive(false);
        }
    }
}
