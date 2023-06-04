using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioManager sound;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
