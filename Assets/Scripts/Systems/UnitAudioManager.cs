using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    [RequireComponent(typeof(Unit))]
    public class UnitAudioManager : AudioEmitter
    {
        private Unit unit;

        void Start()
        {
            unit = GetComponent<Unit>();
            unit.OnUnitCollision += EmitSounds;
        }


        private void EmitSounds(object sender, OnCollisionArgs args)
        {
            source.volume = args.impactPower / 2;

            if (args.collisionType == CollisionType.Table)
            {
                source.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)]);
            }
        }

    }
}