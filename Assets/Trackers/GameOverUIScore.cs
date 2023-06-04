using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUIScore : MonoBehaviour
{
    private TMP_Text scoreTextUI;

    private void Start()
    {
        scoreTextUI = GetComponent<TMP_Text>();
        scoreTextUI.text = ScoreTracker.Score.ToString();
    }

}
