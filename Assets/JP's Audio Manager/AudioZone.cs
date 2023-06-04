using UnityEngine;

public class AudioZone : MonoBehaviour
{
    [SerializeField] private AudioData audioData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //AudioManager.Instance.PlayAudio(audioData);
        }
    }
}
