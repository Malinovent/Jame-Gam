using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Singleton;
    public static Action OnStartPainting;

    //Singleton
    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
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
            BrushManager.SetBrushColor(ColorsEnum.RED);         
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {          
            BrushManager.SetBrushColor(ColorsEnum.GREEN);

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BrushManager.SetBrushColor(ColorsEnum.BLUE);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BrushManager.SetBrushColor(ColorsEnum.YELLOW);
        }
    }
}
