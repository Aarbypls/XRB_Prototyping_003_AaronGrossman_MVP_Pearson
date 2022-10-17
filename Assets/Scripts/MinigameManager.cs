using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Cut;
using Minigames.Feed;
using Minigames.Hit;
using Minigames.Pet;
using Minigames.Shoot;
using Minigames.Slap;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class MinigameManager : MonoBehaviour
{
    public float _globalGameTimer = 10f;
    
    [SerializeField] private List<GameObject> _minigames = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI _minigameInstructions;
    
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
            Invoke(nameof(PlayMinigame), 1f);
            return;
        }
        
        SetMinigameInstructionsUIText();
        Invoke(nameof(SetMinigameActive), 3f);
    }

    private void SetMinigameActive()
    {
        _randomizedMinigames[0].SetActive(true);
        _randomizedMinigames.RemoveAt(0);
    }

    public void StartNextMinigame()
    {
        Invoke(nameof(PlayMinigame), 1f);
    }
    
    public void SetMinigameInstructionsUIText()
    {
        GameObject nextMinigame = _randomizedMinigames[0];
        string instructions = "";
        
        // very hacky way to do this, would refactor in larger project
        if (nextMinigame.TryGetComponent(out Cut cut))
        {
            instructions = cut.SetObjectivesAndGetUIText();
        }
        else if (nextMinigame.TryGetComponent(out Slap slap))
        {
            instructions = slap.SetObjectivesAndGetUIText();
        }
        else if (nextMinigame.TryGetComponent(out Hit hit))
        {
            instructions = hit.SetObjectivesAndGetUIText();
        }
        else if (nextMinigame.TryGetComponent(out Shoot shoot))
        {
            instructions = shoot.SetObjectivesAndGetUIText();
        }
        else if (nextMinigame.TryGetComponent(out Feed feed))
        {
            instructions = feed.SetObjectivesAndGetUIText();
        }
        else if (nextMinigame.TryGetComponent(out Pet pet))
        {
            instructions = pet.SetObjectivesAndGetUIText();
        }
        
        _minigameInstructions.gameObject.SetActive(true);
        _minigameInstructions.SetText(instructions);
    }

    public void HideInstructionsText()
    {
        _minigameInstructions.SetText("");
        _minigameInstructions.gameObject.SetActive(false);
    }
}
