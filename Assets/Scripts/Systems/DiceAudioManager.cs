using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    [RequireComponent(typeof(Dice))]
    public class DiceAudioManager : AudioEmitter
    {
        private Dice dice;

        void Start()
        {

            dice = GetComponent<Dice>();
            dice.OnDiceCollision += EmitSounds;
        }


        private void EmitSounds(object sender, OnCollisionArgs args)
        {
            Debug.Log(args.impactPower);
            source.volume = args.impactPower + 0.2f;
            source.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)]);
        }

    }
}