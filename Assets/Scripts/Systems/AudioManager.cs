using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IManager
{
    [SerializeField]
    AudioSource musicSource;
    [SerializeField]
    AudioSource dialogSource;
    [SerializeField]
    float fadeOutRate;
    bool fadeOut;

    private event EventHandler OnDialogEnd;

    public void Update()
    {
        if(dialogSource.clip != null)
        {
            if (!dialogSource.isPlaying)
            {
                dialogSource.clip = null;
                OnDialogEnd?.Invoke(this, EventArgs.Empty);
            }
        }

        if (fadeOut)
        {
            musicSource.volume -= fadeOutRate * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StopMusic();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartMusic();
        }
    }

    public void StartMusic()
    {
        fadeOut = false;
        musicSource.volume = 1;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        fadeOut = true;
        musicSource.loop = false;
    }

    public EventHandler PlayDialogClip(AudioClip clip)
    {
        dialogSource.PlayOneShot(clip);
        return OnDialogEnd;
    }

    public void Start()
    {
        
    }

    public void Reset()
    {
        
    }
}
