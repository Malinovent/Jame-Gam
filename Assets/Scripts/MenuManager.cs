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
                Time.timeScale = 1;
            }
            else
            {
                OpenPauseMenu();
                Time.timeScale = 0;
            }
        }
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    { 
        //To do - Menu scene
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    { 
        pauseMenu?.SetActive(false);
    }
}
