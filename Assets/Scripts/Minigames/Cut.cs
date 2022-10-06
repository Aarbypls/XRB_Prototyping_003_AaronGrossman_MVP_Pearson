using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour
{
    [SerializeField] private MinigameManager _minigameManager;
    
    private void Awake()
    {
        Invoke(nameof(EndGame), 5f);
    }

    private void EndGame()
    {
        _minigameManager.PlayMinigame();
        this.gameObject.SetActive(false);
    }
}
