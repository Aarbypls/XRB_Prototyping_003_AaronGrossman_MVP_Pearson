using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slap : MonoBehaviour
{
    [SerializeField] private MinigameManager _minigameManager;
    
    private void OnEnable()
    {
        Invoke(nameof(EndGame), 5f);
    }

    private void EndGame()
    {
        _minigameManager.StartNextMinigame();
        this.gameObject.SetActive(false);
    }
}
