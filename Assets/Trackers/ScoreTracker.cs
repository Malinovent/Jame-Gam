using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreTracker
{
    static int score = 0;

    public static Action<int> onScoreChanged;

    public static int Score 
    {
        get => score;
        set 
        {            
            score = value; 
            onScoreChanged?.Invoke(score);
        }
    }
}
