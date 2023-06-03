using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static BrushStates CurrentBrushState;
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
    }

    void SetPaintState()
    {
        //Paint Mode
        if (Input.GetMouseButtonDown(0))
        {
            CurrentBrushState = BrushStates.PAINTING;
            OnStartPainting?.Invoke();
        }

        //No mode
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            CurrentBrushState = BrushStates.NONE;
        }

        //Erase mode
        if (Input.GetMouseButtonDown(1))
        {
            CurrentBrushState = BrushStates.ERASING;
        }
    }
}
public enum BrushStates
{
    NONE,
    PAINTING,
    ERASING
}