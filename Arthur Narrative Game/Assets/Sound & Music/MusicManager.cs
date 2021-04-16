using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip musicLoop;
    [SerializeField] private AudioClip musicEnd;

    private AudioSource audioSource;
    public static MusicManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void StartLoop()
    {
        audioSource.clip = musicLoop;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopLoop()
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
}
