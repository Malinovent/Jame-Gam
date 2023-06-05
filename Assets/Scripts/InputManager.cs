using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Singleton;
    public static Action OnStartPainting;

    public static bool redUnlocked = true;
    public static bool blueUnlocked = false;
    public static bool greenUnlocked = false;
    public static bool yellowUnlocked = false;



    //Singleton
    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        SetPaintState();
        ChangeBrushColorAlphaInput();
    }

    void SetPaintState()
    {
        //Paint Mode
        if (Input.GetMouseButtonDown(0))
        {
            OnStartPainting?.Invoke();
            BrushManager.SetBrushState(BrushStates.PAINTING);
        }

        //No mode
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            BrushManager.SetBrushState(BrushStates.NONE);

        }

        //Erase mode
        if (Input.GetMouseButtonDown(1))
        {
            BrushManager.SetBrushState(BrushStates.ERASING);

        }
    }

    public void ChangeBrushColorAlphaInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (redUnlocked)
            {
                BrushManager.SetBrushColor(ColorsEnum.RED);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (greenUnlocked)
            {
                BrushManager.SetBrushColor(ColorsEnum.GREEN);
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (blueUnlocked)
            {
                BrushManager.SetBrushColor(ColorsEnum.BLUE);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (yellowUnlocked)
            {
                BrushManager.SetBrushColor(ColorsEnum.YELLOW);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            redUnlocked = true;
            blueUnlocked = true;
            greenUnlocked = true;
            yellowUnlocked = true;
        }
        else 
        {
            redUnlocked = true;
            blueUnlocked = false;
            greenUnlocked = false;
            yellowUnlocked = false;
        }
    }
}
