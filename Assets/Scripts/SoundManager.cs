using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    public void PlaySound(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayClickSound()
    {
        PlaySound(0);
    }

    public void PlayWinSound()
    {
        PlaySound(1);
    }
    
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
    
    public void ToggleMute()
    {
        audioSource.mute = !audioSource.mute;
    }
}
