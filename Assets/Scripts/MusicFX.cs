using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicFX : MonoBehaviour
{
    AudioSource musicPlayer;
    public AudioMixerGroup normalSound;
    public AudioMixerGroup mutedSound;

    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
    }


    public void MuteSong(bool muted)
    {
        if (muted)
        {
            musicPlayer.outputAudioMixerGroup = mutedSound;
        }
        else
        {
            musicPlayer.outputAudioMixerGroup = normalSound;
        }
    }
}
