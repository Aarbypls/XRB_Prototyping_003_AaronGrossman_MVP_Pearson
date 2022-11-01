using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

namespace Minigames.Shoot
{
    public enum ShootableType
    {
        Cat =  1,
        Dog = 2,
        Chicken = 3,
        Left = 4,
        Middle = 5,
        Right = 6,
        Lowest = 7,
        Highest = 8
    }
    
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private MinigameManager _minigameManager;
        [SerializeField] private ShootableType _correctShootableType;
        [SerializeField] private InputActionReference _cubeActionReference;
        [SerializeField] private GameObject _shootPoint;
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private AudioSource _gunShotAudioSource;
        [SerializeField] private GameObject _gun;
        [SerializeField] private GameObject _rightHandObject;

        [Header("Shoot Types")] 
        [SerializeField] private GameObject _animalType;
        [SerializeField] private GameObject _position;
        [SerializeField] private GameObject _height;

        [Header("English Prompts")] 
        [SerializeField] private AudioClip _chickenPromptEnglish;
        [SerializeField] private AudioClip _dogPromptEnglish;
        [SerializeField] private AudioClip _catPromptEnglish;
        [SerializeField] private AudioClip _leftPromptEnglish;
        [SerializeField] private AudioClip _middlePromptEnglish;
        [SerializeField] private AudioClip _rightPromptEnglish;
        [SerializeField] private AudioClip _lowestPromptEnglish;
        [SerializeField] private AudioClip _highestPromptEnglish;

        [Header("Spanish Prompts")] 
        [SerializeField] private AudioClip _chickenPromptSpanish;
        [SerializeField] private AudioClip _dogPromptSpanish;
        [SerializeField] private AudioClip _catPromptSpanish;
        [SerializeField] private AudioClip _leftPromptSpanish;
        [SerializeField] private AudioClip _middlePromptSpanish;
        [SerializeField] private AudioClip _rightPromptSpanish;
        [SerializeField] private AudioClip _lowestPromptSpanish;
        [SerializeField] private AudioClip _highestPromptSpanish;
        
        private Vector3 _shootPointPosition;
        private float _minigameTimer;
        private bool _failureClipPlayed = false;
        private bool _success = false;
        private bool _ending = false;
        private ReportCardItem _reportCardItem = new ReportCardItem();
        
        private void Update()
        {
            _shootPointPosition = _shootPoint.transform.position;

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

        private void Awake()
        {
            _cubeActionReference.action.performed += ShootGun;
        }

        public string SetObjectivesAndGetUIText()
        {
            string instructionsEnglish = string.Empty;
            string instructionsNonEnglish = string.Empty;

            SetCorrectShootableType();
            
            switch (_correctShootableType)
            {
                case ShootableType.Cat:
                    instructionsEnglish = "Shoot the cat shaped hot air balloon!";
                    instructionsNonEnglish = "¡Dispara al globo aerostático con forma de gato!";
                    break;
                case ShootableType.Dog:
                    instructionsEnglish = "Shoot the dog shaped hot air balloon!";
                    instructionsNonEnglish = "¡Dispara al globo aerostático con forma de perro!";
                    break;
                case ShootableType.Chicken:
                    instructionsEnglish = "Shoot the chicken shaped hot air balloon!";
                    instructionsNonEnglish = "¡Dispara al globo aerostático con forma de pollo!";
                    break;
                case ShootableType.Left:
                    instructionsEnglish = "Shoot the balloon on the left!";
                    instructionsNonEnglish = "¡Dispara al globo de la izquierda!";
                    break;
                case ShootableType.Middle:
                    instructionsEnglish = "Shoot the balloon in the middle!";
                    instructionsNonEnglish = "¡Dispara al globo en el medio!";
                    break;
                case ShootableType.Right:
                    instructionsEnglish = "Shoot the balloon on the right!";
                    instructionsNonEnglish = "¡Dispara al globo de la derecha!";
                    break;
                case ShootableType.Lowest:
                    instructionsEnglish = "Shoot the lowest hot air balloon!";
                    instructionsNonEnglish = "¡Dispara al globo aerostático más bajo!";
                    break;
                case ShootableType.Highest:
                    instructionsEnglish = "Shoot the highest hot air balloon!";
                    instructionsNonEnglish = "¡Dispara al globo aerostático más alto!";
                    break;
                default:
                    Debug.Log("Shootable type not set correctly!");
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
                    switch (_correctShootableType)
                    {
                        case ShootableType.Cat:
                            audioClip = _catPromptEnglish;
                            break;
                        case ShootableType.Dog:
                            audioClip = _dogPromptEnglish;
                            break;
                        case ShootableType.Chicken:
                            audioClip = _chickenPromptEnglish;
                            break;
                        case ShootableType.Left:
                            audioClip = _leftPromptEnglish;
                            break;
                        case ShootableType.Middle:
                            audioClip = _middlePromptEnglish;
                            break;
                        case ShootableType.Right:
                            audioClip = _rightPromptEnglish;
                            break;
                        case ShootableType.Lowest:
                            audioClip = _lowestPromptEnglish;
                            break;
                        case ShootableType.Highest:
                            audioClip = _highestPromptEnglish;
                            break;
                        default:
                            Debug.Log("Shootable type not set correctly!");
                            break;
                    }
                    break;
                case Language.Spanish:
                    switch (_correctShootableType)
                    {
                        case ShootableType.Cat:
                            audioClip = _catPromptSpanish;
                            break;
                        case ShootableType.Dog:
                            audioClip = _dogPromptSpanish;
                            break;
                        case ShootableType.Chicken:
                            audioClip = _chickenPromptSpanish;
                            break;
                        case ShootableType.Left:
                            audioClip = _leftPromptSpanish;
                            break;
                        case ShootableType.Middle:
                            audioClip = _middlePromptSpanish;
                            break;
                        case ShootableType.Right:
                            audioClip = _rightPromptSpanish;
                            break;
                        case ShootableType.Lowest:
                            audioClip = _lowestPromptSpanish;
                            break;
                        case ShootableType.Highest:
                            audioClip = _highestPromptSpanish;
                            break;
                        default:
                            Debug.Log("Shootable type not set correctly!");
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
            _gun.SetActive(true);
            _rightHandObject.SetActive(false);
        }
        
        private void InitializeStartingVariables()
        {
            if ((int)_correctShootableType >= 1 && (int)_correctShootableType <= 3)
            {
                SetChildShootablesActiveStatus(_animalType, true);
                _animalType.SetActive(true);
            }
            else if ((int)_correctShootableType >= 4 && (int)_correctShootableType <= 6)
            {
                SetChildShootablesActiveStatus(_position, true);
                _position.SetActive(true);
            }
            else if ((int)_correctShootableType >= 7 && (int)_correctShootableType <= 8)
            {
                SetChildShootablesActiveStatus(_height, true);
                _height.SetActive(true);
            }
            
            _minigameTimer = _minigameManager.globalGameTimer;
            _success = false;
            _ending = false;
        }

        private void SetChildShootablesActiveStatus(GameObject shootType, bool isActive)
        {
            List<Shootable> shootablesInShootType = shootType.GetComponentsInChildren<Shootable>(true).ToList();
            foreach (Shootable shootable in shootablesInShootType)
            {
                if (isActive)
                {
                    shootable.DisableConfetti();
                }
                
                shootable.gameObject.SetActive(isActive);
            }
        }

        private void SetCorrectShootableType()
        {
            Array types = Enum.GetValues(typeof(ShootableType));
            Random random = new Random();
            _correctShootableType = (ShootableType)types.GetValue(random.Next(types.Length));
        }
        
        private void ShootGun(InputAction.CallbackContext obj)
        {
            _gunShotAudioSource.Play();
            
            if (Physics.Raycast(_shootPointPosition, _shootPoint.transform.forward, out RaycastHit hit, 100f))
            {
                if (hit.collider.gameObject.TryGetComponent(out Shootable shootable))
                {
                    shootable.HideAndShootConfetti(_correctShootableType);
                    RegisterShot(shootable._shootableType);
                }
            }
        }
        
        private void EndGame()
        {
            _minigameManager.StartNextMinigame();
            _gun.SetActive(false);
            _rightHandObject.SetActive(true);
            
            SetChildShootablesActiveStatus(_animalType, true);
            SetChildShootablesActiveStatus(_position, true);
            SetChildShootablesActiveStatus(_height, true);

            _animalType.SetActive(false);
            _position.SetActive(false);
            _height.SetActive(false);
            
            this.gameObject.SetActive(false);
        }
        
        public void RegisterShot(ShootableType shootableType)
        {
            if (shootableType == _correctShootableType)
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
