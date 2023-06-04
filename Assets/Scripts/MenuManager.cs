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
                
            }
            else if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
                OpenPauseMenu(); 
            }
            else
            {
                OpenPauseMenu();
                
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

        audioMixer.GetFloat("volume", out float value);
        volumeSlider.value = value;
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
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
        Application.Quit(); //TEMPORARY FIX FOR DEMO PURPOSES - REMOVE BEFORE FINAL RELEASE -
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        UIAsset.SetActive(false); 
        Time.timeScale = 0;
    }

    public void ClosePauseMenu()
    { 
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
        Screen.fullScreen = isFull;
    }
}
