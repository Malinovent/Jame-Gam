using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioManager sound;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject tutorialMenu; 

    [SerializeField]
    private List<GameObject> Tutorials;
    private int CurrentIndex = 0; 

    public void PlayGame()
    {
        SceneManager.LoadScene("MainFinal");
        ClickSound();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClickSound()
    {
        sound.Play("ButtonClick");
    }

    public void CycleTutorialsForward()
    {
        ClickSound();
        Tutorials[CurrentIndex].SetActive(false);
        CurrentIndex++;

        if (CurrentIndex >= 4)
        {
            PlayGame();
        }
        else
        {
            Tutorials[CurrentIndex].SetActive(true);
        }
    }

    public void CycleTutorialsBack()
    {
        ClickSound(); 
        Tutorials[CurrentIndex].SetActive(false);
        CurrentIndex--; 

        if(CurrentIndex <= 0)
        {
            tutorialMenu.SetActive(false);
            menu.SetActive(true);
            CurrentIndex = 0; 
        }
        else
        {
            Tutorials[CurrentIndex].SetActive(true); 
        }
    }
}
