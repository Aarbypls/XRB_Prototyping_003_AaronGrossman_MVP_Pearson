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

        [Header("Prompts")]
        [SerializeField] private AudioClip _basketBallPrompt;
        [SerializeField] private AudioClip _soccerBallPrompt;

        private GameObject _spawned1;
        private GameObject _spawned2;

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

            SetCorrectCuttableType();

            switch (_correctCuttableType)
            {
                case CuttableType.SoccerBall:
                    instructions = "Cut the soccer ball!";
                    break;
                case CuttableType.BasketBall:
                    instructions = "Cut the basketball!";
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

            switch (_correctCuttableType)
            {
                case CuttableType.SoccerBall:
                    audioClip = _soccerBallPrompt;
                    break;
                case CuttableType.BasketBall:
                    audioClip = _basketBallPrompt;
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
            _rightHandObject.SetActive(false);
            _sword.SetActive(true);
        }

        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager._globalGameTimer;
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
            }
            else
            {
                _sfxManager.PlayFailureClip();
                _failureClipPlayed = true;
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
