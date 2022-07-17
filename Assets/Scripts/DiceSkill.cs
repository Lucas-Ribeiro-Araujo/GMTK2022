using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Dice))]
    public class DiceSkill : MonoBehaviour
    {
        private Dice dice;

        public GameObject diceFX;

        public float skillRange = 2;

        public float skillDamage = 10;

        public float skillKnockbackForce = 0;

        public LayerMask unitsLayer;


        void Start()
        {
            dice = GetComponent<Dice>();
            dice.OnDiceMaxRoll += Skill;
        }

        void Skill(object sender, OnDiceSkillArgs args)
        {
            Instantiate(diceFX, transform);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, skillRange, unitsLayer);
            List<Unit> hitUnits = GetUnits(hitColliders);

            foreach (Unit unit in hitUnits)
            {
                if (skillKnockbackForce == 0) {
                    unit.TakeDamage(skillDamage);
                    }
                else
                {
                    Knockback knockback = new Knockback();
                    knockback.origin = transform.position;
                    knockback.force =  (transform.position - unit.transform.position).normalized * skillKnockbackForce;
                    unit.TakeDamage(skillDamage, knockback);
                }
            }

        }

        private List<Unit> GetUnits(Collider[] cols)
        {
            List<Unit> units = new List<Unit>();

            foreach (Collider collider in cols)
            {
                units.Add(collider.GetComponent<Unit>());
            }

            return units;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, skillRange);
        }
    }
}