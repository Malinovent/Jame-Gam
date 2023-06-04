using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioData", menuName = "Whispers/Audio/AudioData", order = 0)]
public class AudioData : ScriptableObject
{
    public AudioClip                audioClip;
    public float                    volume = 1.0f;
    public bool                     loop = false;
    [Range(0,1)] public float                    spatialBlend = 1.0f; // 0 is 2D, 1 is 3D
    public AudioMixerGroup          audioMixerGroup;
    [SerializeField] private float length = 0;

    public void SetLength(float length)
    { 
        this.length = length;
    }

    public float GetLength()
    {
        if (length == 0)
        {
            return audioClip.length;
        }

        return length;
    }
}
