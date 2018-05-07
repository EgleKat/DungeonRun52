using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource audioSourceBackground;
    AudioSource audioSourceEffect;

    public AudioClip hit;
    public AudioClip kick;
    public AudioClip backgroundMusic;

    private void Start()
    {
        audioSourceBackground = GetComponents<AudioSource>()[0];
        audioSourceEffect = GetComponents<AudioSource>()[1];

    }

    public void ChangeMusic(String audioName)
    {
        audioSourceBackground.Stop();
        // yield return new WaitForSeconds(1);

        if (audioName == "backgroundMusic")
        {
            audioSourceBackground.clip = backgroundMusic;
        }
       
        if (audioName != "stop")
            audioSourceBackground.Play();
    }
    public void PlaySound(object nameAudio)
    {
        String audioName = (String)nameAudio;
        audioSourceEffect.volume = 1;

        if (audioName == "hit")
        {
            audioSourceEffect.clip = hit;
        }
        else if (audioName == "kick")
        {
            audioSourceEffect.clip = kick;
        }
        audioSourceEffect.Play();
    }
}
