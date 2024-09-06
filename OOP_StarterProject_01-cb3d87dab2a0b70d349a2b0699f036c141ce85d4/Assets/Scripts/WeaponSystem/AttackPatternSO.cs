using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    public abstract class AttackPatternSO : ScriptableObject
    {
        [SerializeField] protected float attackdealy = 0.2f;
        public float AttackDelay => attackdealy;

        [SerializeField] protected GameObject projectile;


        [SerializeField] AudioClip weaponSFX;

        public AudioClip AudioSFX => weaponSFX;

        public abstract void Perform(Transform shootingStarPoint);
    }
}