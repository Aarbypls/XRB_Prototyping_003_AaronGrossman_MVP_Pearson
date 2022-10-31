using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _languageSelectionScreen;

    private bool _quitting;
    
    public void SetStartScreenStatus(bool active)
    {
        if (_quitting)
        {
            return;
        }
        
        _startScreen.SetActive(active);
    }

    public void SetLanguageSelectionScreenStatus(bool active)
    {
        if (_quitting)
        {
            return;
        }
        
        _languageSelectionScreen.SetActive(active);
    }

    public void QuitGameInvoke()
    {
        _quitting = true;
        Invoke(nameof(QuitGame), 2f);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame(int language)
    {
        PlayerPrefs.SetInt("language", (int)language);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("PearsonHD");
    }
}
