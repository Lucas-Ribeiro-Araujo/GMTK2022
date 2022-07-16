using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class UnitAudioManager : AudioEmitter
    {
        [SerializeField]
        private Unit unit;

        void Start()
        {
            unit.OnWalk += EmitSounds;
        }

        
        private void EmitSounds(object sender, EventArgs args)
        {
            source.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)]);
        }

    }
}