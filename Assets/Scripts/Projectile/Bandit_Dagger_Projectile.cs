using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bandit_Dagger_Projectile : BaseProjectile
{
    protected override void Start()
    {
        
    }

    protected override void Hit()
    {
        
    }

    public override void DestroyProjectile()
    {
        base.DestroyProjectile();
    }
    public override Vector3 CalculateSpawnPoint(Camera mainCamera, Vector3 firingPointPosition, Vector3 targetPosition)
    {

        // Lật hướng nếu cần
        if (targetPosition.x < firingPointPosition.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        return firingPointPosition;
    }

}
