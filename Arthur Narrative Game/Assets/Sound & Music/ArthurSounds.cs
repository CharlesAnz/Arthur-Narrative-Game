using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArthurSounds : MonoBehaviour
{
    [SerializeField] private AudioClip swordHit, swordSwing, ArmourUp, Heal;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int i)
    {
        if (i == 0)
        {
            audioSource.PlayOneShot(swordHit);
        }
        else if (i == 1)
        {
            audioSource.PlayOneShot(swordSwing);
        }
        else if (i == 2)
        {
            audioSource.PlayOneShot(ArmourUp);
        }
        else if (i == 3)
        {
            audioSource.PlayOneShot(Heal);
        }
    }
}
