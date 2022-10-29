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

        private void Update()
        {
            _minigameTimer -= Time.deltaTime;

            if (_minigameTimer <= 0 && !_ending)
            {
                // bool needed as we call EndGame on a slight delay for game feel reasons
                _ending = true;
                
                // only play the failure clip if it HASN'T been played before
                // (i.e., if they ran out of time without doing anything)
                if (!_failureClipPlayed && !success)
                {
                    _sfxManager.PlayFailureClip();
                }
                
                Invoke(nameof(EndGame), _minigameManager.globalEndOfGameTimer);
            }
        }

        public string SetObjectivesAndGetUIText()
        {
            string instructions = string.Empty;

            SetCorrectSlappableType();

            switch (_minigameManager.language)
            {
                case Language.English:
                    switch (_correctSlappableType)
                    {
                        case SlappableType.YellowRubberDuck:
                            instructions = "Slap the yellow rubber duck!";
                            break;
                        case SlappableType.BlueRubberDuck:
                            instructions = "Slap the blue rubber duck!";
                            break;
                        case SlappableType.RedRubberDuck:
                            instructions = "Slap the red rubber duck!";
                            break;
                        case SlappableType.PurpleRubberDuck:
                            instructions = "Slap the purple rubber duck!";
                            break;
                        case SlappableType.GreenRubberDuck:
                            instructions = "Slap the green rubber duck!";
                            break;
                        case SlappableType.OrangeRubberDuck:
                            instructions = "Slap the orange rubber duck!";
                            break;
                        case SlappableType.PinkRubberDuck:
                            instructions = "Slap the pink rubber duck!";
                            break;
                        case SlappableType.WhiteRubberDuck:
                            instructions = "Slap the white rubber duck!";
                            break;
                        case SlappableType.BlackRubberDuck:
                            instructions = "Slap the black rubber duck!";
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
                            instructions = "¡Dale una palmada al patito de goma amarillo!";
                            break;
                        case SlappableType.BlueRubberDuck:
                            instructions = "¡Dale una palmada al patito de goma azul!";
                            break;
                        case SlappableType.RedRubberDuck:
                            instructions = "¡Dale una palmada al patito de goma rojo!";
                            break;
                        case SlappableType.PurpleRubberDuck:
                            instructions = "¡Dale una palmada al patito de goma morado!";
                            break;
                        case SlappableType.GreenRubberDuck:
                            instructions = "¡Dale una palmada al patito de goma verde!";
                            break;
                        case SlappableType.OrangeRubberDuck:
                            instructions = "¡Dale una palmada al patito de goma naranja!";
                            break;
                        case SlappableType.PinkRubberDuck:
                            instructions = "¡Dale una palmada al patito de goma rosa!";
                            break;
                        case SlappableType.WhiteRubberDuck:
                            instructions = "¡Dale una palmada al patito de goma blanco!";
                            break;
                        case SlappableType.BlackRubberDuck:
                            instructions = "¡Dale una palmada al patito de goma negro!";
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

            return instructions;
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
            }
            else
            {
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
