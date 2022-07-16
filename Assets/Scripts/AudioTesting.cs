using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    [SerializeField]
    AudioSource auSource;

    [SerializeField]
    List<AudioClip> clips;

    int currentClip = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentClip == clips.Count) currentClip = 0;
            auSource.PlayOneShot(clips[currentClip]);
            Debug.Log($"Played clip: {clips[currentClip].name}");
            currentClip++;
        }
    }
}
