using UnityEngine;

public class Bandit_Archer_Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        //Debug.Log("Electrocutioner Projectile spawned!");
    }

    protected override void Hit()
    {
        base.Hit();
    }

    public override void DestroyProjectile()
    {
        base.DestroyProjectile();
    }
}
