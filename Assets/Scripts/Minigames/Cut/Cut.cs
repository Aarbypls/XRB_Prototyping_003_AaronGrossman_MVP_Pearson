using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Minigames.Cut
{
    public enum CuttableType
    {
        RedSphere = 1,
        BlueSphere = 2
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
                
                Invoke(nameof(EndGame), 1f);
            }
        }
        
        private void OnEnable()
        {
            InitializeStartingVariables();
            SetCorrectCuttableType();
            _sword.SetActive(true);
            _rightHandObject.SetActive(false);
        }
        
        private void InitializeStartingVariables()
        {
            _minigameTimer = _minigameManager._globalGameTimer;
            _success = false;
            _ending = false;

            _spawned1 = Instantiate(_sphere1, _spawnPoint1.position, Quaternion.identity);
            _spawned2 = Instantiate(_sphere2, _spawnPoint2.position, Quaternion.identity);
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
