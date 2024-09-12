using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PA.WeaponSystem;

[CreateAssetMenu(menuName = "Attacks/LazerSO")]


public class LazerAttack : AttackPatternSO
{
    //public Sprite sprite;
    public override void Perform(Transform shootingStarPoint)
    {
     Instantiate(projectile, shootingStarPoint.position, shootingStarPoint.rotation);
        //if (obj.GetComponent<SpriteRenderer>() != null)
        //{
        //    Debug.Log("lazer fire:");
        //    obj.GetComponent<SpriteRenderer>().sprite = null;
        //}

    }
}
