using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    [CreateAssetMenu(menuName = "Attack/SpreadAttack")]
    public class SpreadAttackSO : AttackPatternSO
    {
        //a��sal bir farkl�l�g� olan bir at�� sistemi 3 l� at�� sistemi 
 
        [SerializeField] private float angleDegress = 5;
        public override void Perform(Transform shootingStarPoint)
        {
            Instantiate(projectile, shootingStarPoint.position,shootingStarPoint.rotation);
            Instantiate(projectile, shootingStarPoint.position,shootingStarPoint.rotation *Quaternion.Euler(Vector3.forward * angleDegress));
            Instantiate(projectile, shootingStarPoint.position,shootingStarPoint.rotation *Quaternion.Euler(Vector3.forward * -angleDegress));
        }
    }
}