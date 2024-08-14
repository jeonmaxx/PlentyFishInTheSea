using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound()
    {
        if (sounds.Length == 0)
        {
            Debug.LogWarning("No sounds assigned to the sounds array.");
            return;
        }

        int randomIndex = Random.Range(0, sounds.Length);
        AudioClip randomClip = sounds[randomIndex];

        audioSource.clip = randomClip;
        audioSource.Play();
    }
}
