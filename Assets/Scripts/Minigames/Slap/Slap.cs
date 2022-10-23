using System;
using System.Collections.Generic;
using Minigames.Hit;
using UnityEngine;
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
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private SlappableType _correctSlappableType;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private List<Slappable> _slappables;
        
        [Header("Prompts")]
        [SerializeField] private AudioClip _yellowRubberDuckPrompt;
        [SerializeField] private AudioClip _blueRubberDuckPrompt;
        [SerializeField] private AudioClip _redRubberDuckPrompt;
        [SerializeField] private AudioClip _purpleRubberDuckPrompt;
        [SerializeField] private AudioClip _greenRubberDuckPrompt;
        [SerializeField] private AudioClip _orangeRubberDuckPrompt;
        [SerializeField] private AudioClip _pinkRubberDuckPrompt;
        [SerializeField] private AudioClip _whiteRubberDuckPrompt;
        [SerializeField] private AudioClip _blackRubberDuckPrompt;

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
            string instructions = string.Empty;

            SetCorrectSlappableType();

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

            return instructions;
        }

        public AudioClip GetPromptAudioClip()
        {
            AudioClip audioClip = null;
            
            switch (_correctSlappableType)
            {
                case SlappableType.YellowRubberDuck:
                    audioClip = _yellowRubberDuckPrompt;
                    break;
                case SlappableType.BlueRubberDuck:
                    audioClip = _blueRubberDuckPrompt;
                    break;
                case SlappableType.RedRubberDuck:
                    audioClip = _redRubberDuckPrompt;
                    break;
                case SlappableType.PurpleRubberDuck:
                    audioClip = _purpleRubberDuckPrompt;
                    break;
                case SlappableType.GreenRubberDuck:
                    audioClip = _greenRubberDuckPrompt;
                    break;
                case SlappableType.OrangeRubberDuck:
                    audioClip = _orangeRubberDuckPrompt;
                    break;
                case SlappableType.PinkRubberDuck:
                    audioClip = _pinkRubberDuckPrompt;
                    break;
                case SlappableType.WhiteRubberDuck:
                    audioClip = _whiteRubberDuckPrompt;
                    break;
                case SlappableType.BlackRubberDuck:
                    audioClip = _blackRubberDuckPrompt;
                    break;
                default:
                    Debug.Log("Cuttable type not set correctly!");
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

            _minigameTimer = _minigameManager._globalGameTimer;
            _success = false;
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
                _success = true;
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
