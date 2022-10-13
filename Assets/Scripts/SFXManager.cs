using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip _successAudioClip;
    [SerializeField] private AudioClip _failureAudioClip;

    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySuccessClip()
    {
        _audioSource.clip = _successAudioClip;
        _audioSource.Play();
    }
    
    public void PlayFailureClip()
    {
        _audioSource.clip = _failureAudioClip;
        _audioSource.Play();
    }
}
