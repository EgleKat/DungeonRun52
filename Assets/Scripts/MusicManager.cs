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
    public AudioClip slash;
    public AudioClip laser;
    public AudioClip plasma;
    public AudioClip shot;
    public AudioClip weaponSwap;
    public AudioClip itemSwap;
    public AudioClip shieldHit;
    public AudioClip glassBreak;






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
        else if (audioName == "laser")
        {
            audioSourceEffect.clip = laser;
        }
        else if (audioName == "slash")
        {
            audioSourceEffect.clip = slash;
        }
        else if (audioName == "plasma")
        {
            audioSourceEffect.clip = plasma;
        }
        else if (audioName == "shot")
        {
            audioSourceEffect.clip = shot;
        }
        else if (audioName == "weaponSwap")
        {
            audioSourceEffect.clip = weaponSwap;
        }
        else if (audioName == "itemSwap")
        {
            audioSourceEffect.clip = itemSwap;
        }
        else if (audioName == "shieldHit")
        {
            audioSourceEffect.clip = shieldHit;
        }
        else if (audioName == "glassBreak")
        {
            audioSourceEffect.clip = glassBreak;
        }
        audioSourceEffect.Play();
    }
}
