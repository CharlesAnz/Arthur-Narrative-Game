using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSounds : MonoBehaviour
{

    [SerializeField] private AudioClip dragonFire, dragonfireBall, dragonHurt;
    [SerializeField] private AudioClip[] dragonSleep;

    private AudioSource audioSource;
    private CharacterCombat cb;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        cb = GetComponent<CharacterCombat>();
    }

    private void Start()
    {
        InvokeRepeating("Roar", 10, 5);
    }

    public void Roar()
    {
        if (!cb.InCombat)
        {
            AudioClip clip = dragonSleep[UnityEngine.Random.Range(0, dragonSleep.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    public void PlaySound(int i)
    {
        if(i == 0)
        {
            audioSource.PlayOneShot(dragonHurt);
        }
        else if(i == 1)
        {
            audioSource.PlayOneShot(dragonfireBall);
        }
        else if(i == 2)
        {
            audioSource.PlayOneShot(dragonFire);
        }
    }
}
