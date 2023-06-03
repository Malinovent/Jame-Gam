using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTrackerUI : MonoBehaviour
{
    private TMP_Text textUI;

    private void OnEnable()
    {
        ScoreTracker.onScoreChanged += UpdateUI;
    }

    private void OnDisable()
    {
        ScoreTracker.onScoreChanged -= UpdateUI;
    }

    private void Start()
    {
        textUI = GetComponent<TMP_Text>();
    }

    void UpdateUI(int amount)
    { 
        textUI.text = amount.ToString();
    }
}
