using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class SFXSoundManager : AudioEmitter
    {

        void Start()
        {

        }


        private void EmitSounds(object sender, OnCollisionArgs args)
        {
            source.volume = args.impactPower + 0.2f;
            source.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)]);
        }

    }
}
