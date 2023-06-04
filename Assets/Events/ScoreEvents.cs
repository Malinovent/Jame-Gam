using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ScoreEvents
{
    [SerializeField] int minScoreTarget = 9999;
    [SerializeField] UnityEvent onActivate;

    public bool TryActivate(int score )
    {
        if (score >= minScoreTarget)
        {
            Activate();
            return true;
        }

        return false;
    }

    private void Activate()
    {
        onActivate?.Invoke();
    }
    
}
