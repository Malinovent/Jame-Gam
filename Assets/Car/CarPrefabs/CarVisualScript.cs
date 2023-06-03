using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarVisualScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    private void Start()
    { 
        StartCoroutine(AudioNext()); 
    }

    IEnumerator AudioNext()
    {
        yield return new WaitForSeconds(1.5f);
        source.Play(); 
    }
}
