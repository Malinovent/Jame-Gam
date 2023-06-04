using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrushManager
{
    public static event Action<ColorsEnum> OnColorChanged;
    public static Action<BrushStates> OnBrushStateChanged;

    public static BrushStates CurrentBrushState { get; private set; }
    public static ColorsEnum CurrentBrushColor { get; private set; }


    public static void SetBrushState(BrushStates newBrushState)
    {
        CurrentBrushState = newBrushState;
        //Debug.Log("Switching to brush state: " + CurrentBrushState.ToString());
        OnBrushStateChanged?.Invoke(newBrushState);
    }

    public static void SetBrushColor(ColorsEnum newBrushColor)
    {
        CurrentBrushColor = newBrushColor;
        //Debug.Log("Switching to brush color: " + CurrentBrushColor.ToString());
        OnColorChanged?.Invoke(newBrushColor);
    }
}

public enum BrushStates
{
    NONE,
    PAINTING,
    ERASING
}
