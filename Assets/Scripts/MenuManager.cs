using System;
using UnityEngine.UI;
using UnityEngine.Audio; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject UIAsset; 
    [SerializeField] GameObject optionsMenu;
    [SerializeField] AudioManager AudioManager; 

    public static Action OnGameOver;

    public AudioMixer audioMixer;
    public Toggle fullScreen;
    public Slider volumeSlider;

    private void OnEnable()
    {
        OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        OnGameOver -= GameOver;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                ClosePauseMenu();
                ButtonClick(); 
            }
            else if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
                OpenPauseMenu();
                ButtonClick(); 
            }
            else
            {
                OpenPauseMenu();
                ButtonClick(); 
            }
        }
    }

    private void Start()
    {
        if (Screen.fullScreen == true)
        {
            fullScreen.isOn = true;
        }
        else
        {
            fullScreen.isOn = false;
        }

        UIAsset.SetActive(true); 

        audioMixer.GetFloat("volume", out float value);
        volumeSlider.value = value;
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        UIAsset.SetActive(false);
        AudioManager.GameOver(); 
        Time.timeScale = 0;
    }

    public void GoToNextScene()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        //To do - Menu scene
        Debug.Log("Quitting");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void OpenPauseMenu()
    {
        ButtonClick(); 
        pauseMenu.SetActive(true);
        UIAsset.SetActive(false); 
        Time.timeScale = 0;
    }

    public void ClosePauseMenu()
    {
        ButtonClick(); 
        pauseMenu?.SetActive(false);
        UIAsset.SetActive(true); 
        Time.timeScale = 1;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullScreen(bool isFull)
    {
        ButtonClick(); 
        Screen.fullScreen = isFull;
    }

    public void ButtonClick()
    {
        AudioManager.Play("ButtonClick"); 
    }
}
