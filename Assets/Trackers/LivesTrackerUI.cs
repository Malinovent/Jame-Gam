using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesTrackerUI : MonoBehaviour
{
    [SerializeField] private Image[] lives;

    private void OnEnable()
    {
        LivesTracker.onLivesChanged += UpdateUI;
    }

    private void OnDisable()
    {
        LivesTracker.onLivesChanged += UpdateUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        LivesTracker.InitializeLives();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        
        for (int i = 0; i < 5; i++)
        {
            if (i >= LivesTracker.CurrentLives)
            {
                lives[i].enabled = false;
            }
            else
            {
                lives[i].enabled = true;
            }
        }
    }
}
