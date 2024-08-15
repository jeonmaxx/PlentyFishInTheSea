using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSound : MonoBehaviour
{
    public AudioClip introSound;
    public AudioClip loopSound;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!source.isPlaying && source.clip == introSound)
        {
            source.clip = loopSound;
            source.loop = true;
            source.Play();
        }
    }
}
