using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PA.WeaponSystem;

[CreateAssetMenu(menuName = "Attacks/BombBullet")]

public class BombBulletSO : AttackPatternSO
{
    

    public override void Perform(Transform shootingStarPoint)
    {
        Instantiate(projectile, shootingStarPoint.position, shootingStarPoint.rotation);

    }
}
