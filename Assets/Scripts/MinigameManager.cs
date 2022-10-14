using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _minigames = new List<GameObject>();
    
    private List<GameObject> _randomizedMinigames = new List<GameObject>();
    private static Random _rng = new Random();
    
    void Start()
    {
        // Probably should be moved out of start
        // Ex. on a button press, after a timer, etc.
        CreateMinigameList();
        Invoke(nameof(PlayMinigame), 3f);
    }

    private void CreateMinigameList()
    {
        // Make the new list to use randomized from the total collection
        _randomizedMinigames = _minigames.OrderBy(x => _rng.Next()).ToList();
    }
    
    public void PlayMinigame()
    {
        if (_randomizedMinigames.Count == 0)
        {
            CreateMinigameList();
            Invoke(nameof(PlayMinigame), 3f);
            return;
        }
        
        _randomizedMinigames[0].SetActive(true);
        _randomizedMinigames.RemoveAt(0);
    }

    public void StartNextMinigame()
    {
        Invoke(nameof(PlayMinigame), 3f);
    }
}
