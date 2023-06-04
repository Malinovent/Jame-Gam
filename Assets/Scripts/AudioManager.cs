using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [SerializeField]
    private AudioSource sourceA;
    [SerializeField]
    private AudioSource sourceB;

    public bool inGame = false; 

    private void Start()
    {
        if (inGame)
        {
            sourceA.Play(); 
            StartCoroutine(MainTheme());
        }
        else
        {
            Play("MainTheme"); 
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    IEnumerator MainTheme()
    {
        yield return new WaitForSeconds(8f);
        sourceB.Play(); 
    }
}
