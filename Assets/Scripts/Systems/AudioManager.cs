using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource musicSource;
    [SerializeField]
    AudioSource dialogSource;

    private event EventHandler OnDialogEnd;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(dialogSource.clip != null)
        {
            if (!dialogSource.isPlaying)
            {
                dialogSource.clip = null;
                OnDialogEnd?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void SetMusicClip(AudioClip clip)
    {
        musicSource.clip = clip;
    }

    public EventHandler PlayDialogClip(AudioClip clip)
    {
        dialogSource.PlayOneShot(clip);
        return OnDialogEnd;
    }
}
