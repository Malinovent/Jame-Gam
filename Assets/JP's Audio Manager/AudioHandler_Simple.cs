using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler_Simple : MonoBehaviour
{
    public AudioData[] audioDataList;

    public void PlayAudio(int audioIndex)
    {
        if (audioIndex >= audioDataList.Length)
        {
            Debug.LogError("Audio index out of range for " + this.gameObject.name);
            return;
        }

        AudioData data = audioDataList[audioIndex];
        AudioManager_Mali.Instance.PlayAudioDataOnce(data, this.transform.position);
    }

    public void PlayAudio(string audioDataName)
    {
        AudioData data;

        foreach (AudioData audioData in audioDataList)
        {
            if (audioData.name == audioDataName)
            {
                data = audioData;
                AudioManager_Mali.Instance.PlayAudioDataOnce(data, this.transform.position);
                return;
            }
        }  

        Debug.LogWarning(audioDataName + " was not found in " + this.gameObject.name);
    }
}
