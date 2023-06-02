using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameModeManager : MonoBehaviour
{
    private GameModesEnum _currentGameMode = GameModesEnum.Drawing;

    public static event Action OnDrawingMode;
    public static event Action OnErasingMode;
    public static event Action OnPausedMode;

    public void SetDrawingMode()
    { 
        _currentGameMode = GameModesEnum.Drawing;
        OnDrawingMode?.Invoke();
    }

    public void SetErasingMode()
    {
        _currentGameMode = GameModesEnum.Erasing;
        OnErasingMode?.Invoke();
    }

    public void SetPausedMode()
    {
        _currentGameMode = GameModesEnum.Paused;
        OnPausedMode?.Invoke();
    }

}
