using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReportCardItem
{
    public string prompt;
    public string translation;
    public string playerSelection;
    public bool timedOut;
}

public class ReportCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _correctAnswersText;
    [SerializeField] private TextMeshProUGUI _reportCardItems;

    void Start()
    {
        _reportCardItems.text = "";
    }

    public void UpdateCorrectAnswersText(int minigameSuccesses, int minigamesPerLevel)
    {
        if (minigameSuccesses / (float)minigamesPerLevel > .70f)
        {
            _correctAnswersText.SetText($"You got {minigameSuccesses}/{minigamesPerLevel} answers correct!");
        }
        else
        {
            _correctAnswersText.SetText($"You only got {minigameSuccesses}/{minigamesPerLevel} answers correct.");
        }
    }

    public void UpdateReportCardItems(string prompt, string translation, string playerSelection, bool inEnglish, bool timedOut)
    {
        string reportCardItem = $"\"{prompt}\"" + "\n";
        
        if (timedOut)
        {
            reportCardItem += ("\t" + "You ran out of time!" + "\n\n");
        }
        else
        {
            reportCardItem += ("\t" + "Translation: \"" + translation + "\"" + "\n");
            reportCardItem += ("\t" + "Your selection: \"" + playerSelection + "\"" + "\n\n");
        }
        
        _reportCardItems.text += reportCardItem;
    }
}
