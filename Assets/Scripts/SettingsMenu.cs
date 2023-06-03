using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; 

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioManager soundManager;
    public Toggle fullScreen;
    public Slider volumeSlider;

    private void Start()
    {
        if (Screen.fullScreen == true)
        {
            fullScreen.isOn = true;
        }
        else
        {
            fullScreen.isOn = false;
        }

        audioMixer.GetFloat("volume", out float value);
        volumeSlider.value = value;

    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
        ClickSound(); 
    }

    public void ClickSound()
    {
        soundManager.Play("ButtonClick");
    }
}
