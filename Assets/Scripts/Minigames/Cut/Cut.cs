using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Cut
{
    public enum CuttableType
    {
        BasketBall = 1,
        SoccerBall = 2
    }
    
    public class Cut : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private CuttableType _correctCuttableType;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private GameObject _sword;
        [SerializeField] private GameObject _rightHandObject;
        [SerializeField] private GameObject _sphere1;
        [SerializeField] private GameObject _sphere2;
        [SerializeField] private Transform _spawnPoint1;
        [SerializeField] private Transform _spawnPoint2;

        [Header("English Prompts")]
        [SerializeField] private AudioClip _basketBallPromptEnglish;
        [SerializeField] private AudioClip _soccerBallPromptEnglish;

        [Header("Spanish Prompts")]
        [SerializeField] private AudioClip _basketBallPromptSpanish;
        [SerializeField] private AudioClip _soccerBallPromptSpanish;
        
        private GameObject _spawned1;
        private GameObject _spawned2;

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
            string instructionsNonEnglish = String.Empty;

            SetCorrectCuttableType();
            
            switch (_correctCuttableType)
            {
                case CuttableType.SoccerBall:
                    instructionsEnglish = "Cut the soccer ball!";
                    instructionsNonEnglish = "¡Corta el balón de fútbol!";
                    break;
                case CuttableType.BasketBall:
                    instructionsEnglish = "Cut the basketball!";
                    instructionsNonEnglish = "¡Corta el balón de baloncesto!";
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
                    switch (_correctCuttableType)
                    {
                        case CuttableType.SoccerBall:
                            audioClip = _soccerBallPromptEnglish;
                            break;
                        case CuttableType.BasketBall:
                            audioClip = _basketBallPromptEnglish;
                            break;
                        default:
                            Debug.Log("Cuttable type not set correctly!");
                            break;
                    }
                    break;
                case Language.Spanish:
                    switch (_correctCuttableType)
                    {
                        case CuttableType.SoccerBall:
                            audioClip = _soccerBallPromptSpanish;
                            break;
                        case CuttableType.BasketBall:
                            audioClip = _basketBallPromptSpanish;
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
            _rightHandObject.SetActive(false);
            _sword.SetActive(true);
        }

        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager.globalGameTimer;
            _success = false;
            _ending = false;

            _spawned1 = Instantiate(_sphere1, _spawnPoint1.position, Quaternion.identity, transform);
            _spawned2 = Instantiate(_sphere2, _spawnPoint2.position, Quaternion.identity, transform);
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

        private void EndGame()
        {
            Destroy(_spawned1);
            Destroy(_spawned2);
            
            _minigameManager.StartNextMinigame();
            _sword.SetActive(false);
            _rightHandObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
