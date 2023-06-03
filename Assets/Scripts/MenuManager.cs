using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;

    public static Action OnGameOver;

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
            else
            {
                OpenPauseMenu();
                
            }
        }
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
        Time.timeScale = 0;
    }

    public void ClosePauseMenu()
    { 
        pauseMenu?.SetActive(false);
        Time.timeScale = 1;
    }
}
