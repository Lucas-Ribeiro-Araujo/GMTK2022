using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioEmitter : MonoBehaviour
{
    [SerializeField]
    protected AudioClip[] audioClips;

    [SerializeField]
    protected AudioSource source;


}
