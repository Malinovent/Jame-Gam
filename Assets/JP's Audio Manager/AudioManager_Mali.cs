using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager_Mali : MonoBehaviour
{
    public static AudioManager_Mali Instance { get; private set; }

    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int initialPoolSize = 10;

    private Queue<AudioSource> audioSourcePool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializePool();
    }

    private void InitializePool()
    {
        audioSourcePool = new Queue<AudioSource>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            AudioSource audioSource = Instantiate(audioSourcePrefab, transform);
            audioSource.gameObject.SetActive(false);
            audioSourcePool.Enqueue(audioSource);
        }
    }

    public AudioSource PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        AudioSource audioSource;

        if (audioSourcePool.Count > 0)
        {
            audioSource = audioSourcePool.Dequeue();
            audioSource.transform.position = position;
            audioSource.gameObject.SetActive(true);
        }
        else
        {
            audioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity, transform);
        }

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        StartCoroutine(ReturnToPool(audioSource, audioClip.length));
        return audioSource;
    }

    private AudioSource PlayAndCreateAudioData(AudioData data, Vector3 position)
    {
        AudioSource audioSource;

        if (audioSourcePool.Count > 0)
        {
            audioSource = audioSourcePool.Dequeue();
            audioSource.transform.position = position;
            audioSource.gameObject.SetActive(true);
        }
        else
        {
            audioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity, transform);
        }

        audioSource.clip = data.audioClip;
        audioSource.volume = data.volume; 

        audioSource.Play();

        return audioSource;
    }

    public AudioSource PlayAudioDataOnce(AudioData data, Vector3 position) 
    {
        AudioSource audioSource = PlayAndCreateAudioData(data, position);
        audioSource.loop = false;
        StartCoroutine(ReturnToPool(audioSource, data.GetLength()));
        return audioSource;
    }

    public AudioSource PlayAudioDataLoop(AudioData data, Vector3 position)
    {
        AudioSource audioSource = PlayAndCreateAudioData(data, position);
        audioSource.loop = true;
        return audioSource;
    }

    public void StopAndReturnAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
        audioSourcePool.Enqueue(audioSource);
    }

    private IEnumerator ReturnToPool(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        StopAndReturnAudioSource(audioSource);
    }
}
