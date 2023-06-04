using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<ScoreEvents> events = new List<ScoreEvents>();

    private void OnEnable()
    {
        ScoreTracker.onScoreChanged += UpdateEvents;
    }

    private void OnDisable()
    {
        ScoreTracker.onScoreChanged -= UpdateEvents;
    }

    private void UpdateEvents(int score)
    { 
        foreach(var e in events) 
        {
            e.TryActivate(score);
        }
    }
}
