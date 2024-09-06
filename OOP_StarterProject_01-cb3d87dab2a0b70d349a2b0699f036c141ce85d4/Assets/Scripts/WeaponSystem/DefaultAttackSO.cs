using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PA.WeaponSystem;


[CreateAssetMenu(menuName ="Attacks/DefaultAttack")]

public class DefaultAttackSO : AttackPatternSO
{
    //tekli atýþ sistemi
     
    public override void Perform(Transform shootingStarPoint)
    {
        Instantiate(projectile, shootingStarPoint.position, shootingStarPoint.rotation );
    }
}
