using System.Collections;
using UnityEngine;

public class CardSound : AudioEmitter
{
    public void EmitSounds()
    {
        source.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)]);
    }
}
