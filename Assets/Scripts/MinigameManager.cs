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
using UnityEngine.Serialization;
using Random = System.Random;

public enum Language
{
    English = 1,
    Spanish = 2
}

public class MinigameManager : MonoBehaviour
{
    public Language language;
    public float globalGameTimer = 8f;
    public float globalEndOfGameTimer = .1f;
    
    [SerializeField] private List<GameObject> _minigames = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI _minigameInstructions;
    [SerializeField] private AudioSource _promptAudioSource;
    [SerializeField] private int _minigamesPerLevel = 18;
    [SerializeField] private int _minigamesPlayed = 0;
    [SerializeField] private int _minigameSuccesses = 0;
    [SerializeField] private ReportCard _reportCard;
    [SerializeField] private List<ReportCardItem> _reportCardItems = new List<ReportCardItem>();
    
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
        if (_minigamesPlayed >= _minigamesPerLevel)
        {
            HandleEndGameState();
        }
        else
        {
            if (_randomizedMinigames.Count == 0)
            {
                CreateMinigameList();
                Invoke(nameof(PlayMinigame), 1f);
                return;
            }
        
            SetMinigameInstructionsUIText();
            Invoke(nameof(SetMinigameActive),  _promptAudioSource.clip == null ? 2f : _promptAudioSource.clip.length + 1.5f);
        }
    }

    private void SetMinigameActive()
    {
        _randomizedMinigames[0].SetActive(true);
        _randomizedMinigames.RemoveAt(0);
    }

    public void StartNextMinigame()
    {
        Invoke(nameof(PlayMinigame), .1f);
    }
    
    public void SetMinigameInstructionsUIText()
    {
        GameObject nextMinigame = _randomizedMinigames[0];
        string instructions = "";
        _promptAudioSource.clip = null;

        // very hacky way to do this, would refactor in larger project
        if (nextMinigame.TryGetComponent(out Cut cut))
        {
            instructions = cut.SetObjectivesAndGetUIText();
            _promptAudioSource.clip = cut.GetPromptAudioClip();            
        }
        else if (nextMinigame.TryGetComponent(out Slap slap))
        {
            instructions = slap.SetObjectivesAndGetUIText();
            _promptAudioSource.clip = slap.GetPromptAudioClip();
        }
        else if (nextMinigame.TryGetComponent(out Hit hit))
        {
            instructions = hit.SetObjectivesAndGetUIText();
            _promptAudioSource.clip = hit.GetPromptAudioClip();
        }
        else if (nextMinigame.TryGetComponent(out Shoot shoot))
        {
            instructions = shoot.SetObjectivesAndGetUIText();
            _promptAudioSource.clip = shoot.GetPromptAudioClip();
        }
        else if (nextMinigame.TryGetComponent(out Feed feed))
        {
            instructions = feed.SetObjectivesAndGetUIText();
            _promptAudioSource.clip = feed.GetPromptAudioClip();
        }
        else if (nextMinigame.TryGetComponent(out Pet pet))
        {
            instructions = pet.SetObjectivesAndGetUIText();
            _promptAudioSource.clip = pet.GetPromptAudioClip();
        }
        
        _minigameInstructions.gameObject.SetActive(true);
        _minigameInstructions.SetText(instructions);

        if (_promptAudioSource)
        {
            _promptAudioSource.Play();
        }
    }

    public void HideInstructionsText()
    {
        _minigameInstructions.SetText("");
        _minigameInstructions.gameObject.SetActive(false);
    }

    public void RegisterSuccess()
    {
        _minigameSuccesses++;
    }

    public void AddReportCardItemToList(ReportCardItem reportCardItem)
    {
        _reportCardItems.Add(reportCardItem);
    }
    
    private void HandleEndGameState()
    {
        SetUpReportCard();
    }

    private void SetUpReportCard()
    {
        _reportCard.UpdateCorrectAnswersText(_minigameSuccesses, _minigamesPerLevel);

        foreach (ReportCardItem reportCardItem in _reportCardItems)
        {
            _reportCard.UpdateReportCardItems(reportCardItem.prompt, reportCardItem.translation, reportCardItem.playerSelection, 
                language == Language.English, reportCardItem.timedOut);
        }
        
        _reportCard.gameObject.SetActive(true);
    }
}
