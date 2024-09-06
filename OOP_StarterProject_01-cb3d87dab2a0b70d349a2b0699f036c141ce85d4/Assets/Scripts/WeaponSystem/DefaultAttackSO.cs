using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PA.WeaponSystem;


[CreateAssetMenu(menuName ="Attacks/DefaultAttack")]

public class DefaultAttackSO : AttackPatternSO
{
    //tekli at�� sistemi
     
    public override void Perform(Transform shootingStarPoint)
    {
        Instantiate(projectile, shootingStarPoint.position, shootingStarPoint.rotation );
    }
}
