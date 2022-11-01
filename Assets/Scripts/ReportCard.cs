using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReportCardItem
{
    public string prompt;
    public string translation;
    public bool timedOut;
}

public class ReportCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _correctAnswersText;
    [SerializeField] private TextMeshProUGUI _reportCardItems;
    
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

    public void UpdateReportCardItems(string prompt, string translation, bool inEnglish, bool timedOut)
    {
        string reportCardItem = $"\"{prompt}\"" + "\n";
        
        if (!inEnglish)
        {
            reportCardItem += ("\t" + "Translation: \"" + translation + "\"" + "\n");
        }
        
        if (timedOut)
        {
            reportCardItem += ("\t" + "You ran out of time!" + "\n");
        }

        reportCardItem += "\n";

        _reportCardItems.text += reportCardItem;
    }
}
