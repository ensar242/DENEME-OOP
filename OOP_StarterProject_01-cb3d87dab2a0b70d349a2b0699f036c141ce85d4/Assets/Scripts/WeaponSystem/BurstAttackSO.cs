using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    [CreateAssetMenu(menuName = "Attack/BurstAttack")]
    public class BurstAttackSO : AttackPatternSO
    {
        [SerializeField] private float offset = 0.1f;

        //art arda 3 tane arka arkaya atan atýþ sistemi
        public override void Perform(Transform shootingStarPoint)
        {
            Vector3 offsetDirection = shootingStarPoint.rotation * new Vector3(0,offset,0);

            Instantiate(projectile, shootingStarPoint.position, shootingStarPoint.rotation);
            Instantiate(projectile, shootingStarPoint.position + offsetDirection, shootingStarPoint.rotation);
            Instantiate(projectile, shootingStarPoint.position - offsetDirection, shootingStarPoint.rotation);
        }
    }
} 