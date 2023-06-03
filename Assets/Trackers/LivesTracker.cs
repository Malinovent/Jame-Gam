using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LivesTracker
{
    private static int startingLives = 3;
    private static int currentLives;

    public static Action onLivesChanged;

    public static int CurrentLives 
    { 
        get => currentLives;
    }

    public static int StartingLives { get => startingLives; }

    public static void InitializeLives()
    { 
        currentLives = StartingLives;
        onLivesChanged?.Invoke();
    }

    public static void LoseLife()
    {
        currentLives -= 1;
        onLivesChanged?.Invoke();
    }

    
}
