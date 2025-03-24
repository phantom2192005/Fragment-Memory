using UnityEngine;

public class Electrocutioner_Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        //Debug.Log("Electrocutioner Projectile spawned!");
    }

    protected override void Hit()
    {
        base.Hit();
        //Debug.Log("Electrocutioner projectile hit the target!");
    }
    public override void DestroyProjectile()
    {
        base.DestroyProjectile();
    }
}
