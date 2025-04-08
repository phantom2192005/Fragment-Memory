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
    public override Vector3 CalculateSpawnPoint(Camera mainCamera, Transform firingPoint, Transform targetTransform)
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found!");
                return Vector3.zero;
            }
        }

        if (targetTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                targetTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player not found!");
                return Vector3.zero;
            }
        }

        float cameraHeight = mainCamera.orthographicSize * 2;
        float x_spawn = targetTransform.position.x;
        float y_spawn = mainCamera.transform.position.y + (cameraHeight / 2);

        return new Vector3(x_spawn, y_spawn, 0);
    }
}
