using UnityEngine;

public class AudioHandler_DataList : MonoBehaviour
{
    [SerializeField] AudioDataList[] _audioDataList;

    public void PlayAudio(int audioIndex, int listIndex = 0)
    {     
        if(audioIndex >= _audioDataList[listIndex].audioData.Length)
        {
            Debug.LogError("Audio index out of range for " + this.gameObject.name);
            return;
        }

        AudioData data = _audioDataList[listIndex].audioData[audioIndex];
        AudioManager_Mali.Instance.PlaySound(data.audioClip, this.transform.position, data.volume);                 
    }

    /// <summary>
    /// Plays a random audioclip from the array
    /// </summary>
    public void PlayRandomAudio(int listIndex = 0)
    { 
        int randomIndex = Random.Range(0, _audioDataList[listIndex].audioData.Length);
        PlayAudio(randomIndex, listIndex);
    }

    /// <summary>
    /// Will play a random audioClip by searching for the name of the list
    /// </summary>
    /// <param name="listName"></param>
    public void PlayRandomAudio(string listName)
    { 
        foreach(AudioDataList list in _audioDataList) 
        {
            if (list.audioListName == listName)
            { 
                //PlayRandomAudio(_audioDataList.IndexOf(list));
                return;
            }
        }
    }

    public void PlayAudio(string listName)
    {
        foreach (AudioDataList list in _audioDataList)
        {
            if (list.audioListName == listName)
            {
                PlayAudio(0);
                return;
            }
        }
    }
}

[System.Serializable]
public class AudioDataList
{
    public string audioListName = "Default";
    public AudioData[] audioData;

}
