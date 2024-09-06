using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    //ikili atýþ sistemi
    [CreateAssetMenu(menuName = "Attack/DoubleBarrelAttack")]
    public class DoubleBarrelAttackSO : AttackPatternSO
    {
        [SerializeField] private float offsetromShootinPoint = 0.3f;
        public override void Perform(Transform shootingStarPoint)
        {
            Vector3 offsetVector = shootingStarPoint.rotation * new Vector3(offsetromShootinPoint, 0, 0);//tam ortasýnda
            Vector3 point1 = shootingStarPoint.position + offsetVector;//sagýnda 
            Vector3 point2 = shootingStarPoint.position - offsetVector;//solunda

            Instantiate(projectile, point1, shootingStarPoint.rotation);
            Instantiate(projectile, point2, shootingStarPoint.rotation);
        }
 
    }
}