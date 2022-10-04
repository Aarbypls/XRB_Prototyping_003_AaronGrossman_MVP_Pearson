using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{   
    [SerializeField] private InputActionReference _restartSceneActionReference;

    [Header("Accessibility")] 
    public Handed _handedness;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _bgmSource;
    
    private PlayerInput _playerInput;
    private FirstPersonController _firstPersonController;

    private void Awake()
    {
        _handedness = (Handed)PlayerPrefs.GetInt("handedness");
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        
        if (player)
        {
            _playerInput = player.GetComponent<PlayerInput>();
            _firstPersonController = player.GetComponent<FirstPersonController>();
        }
        else
        {
            Debug.LogWarning("There is no player (or object with tag \"Player\" in the scene.");
        }

        _restartSceneActionReference.action.performed += RestartSceneInputAction;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void PlayBGM(AudioClip newBgm)
    {
        if (_bgmSource.clip == newBgm)
        {
            return;
        }
        
        _bgmSource.clip = newBgm;
        _bgmSource.volume = .25f;
        _bgmSource.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    private void RestartSceneInputAction(InputAction.CallbackContext obj)
    {
        RestartScene();
    }

    public void RestartScene()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);    
    }

    public void ToggleDominantHand()
    {
        if (_handedness == Handed.Right)
        {
            _handedness = Handed.Left;
        }
        else
        {
            _handedness = Handed.Right;
        }
        
        PlayerPrefs.SetInt("handedness", (int)_handedness);
        PlayerPrefs.Save();
        
        RestartScene();
    }
}
