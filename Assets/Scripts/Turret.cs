using UnityEngine;

public class Turret : MonoBehaviour {
    public LayerMask enemyMask;
    public LayerMask aimAtMask;
    public float range;
    public float fireRate;
    public float countdown = 0;
    public float damage;
    public GameObject bulletEffect;
    public float explosionRange;

    public float minX, maxX;

    Enemy target;

    void GetTarget()
    {
        if (!WaveLauncher.IsWaveInProcess) return;

        Collider closest = null;
        float minDist = float.MaxValue;

        foreach(Collider col in Physics.OverlapSphere(transform.position, range, aimAtMask))
        {
            float dist = transform.position.x - col.transform.position.x;

            if(dist < minDist)
            {
                minDist = dist;
                closest = col;
            }
        }

        if (closest != null) target = closest.GetComponent<Enemy>();
    }

    void Awake()
    {
        InvokeRepeating("GetTarget", 0, 2);
    }

    void Update()
    {
        if (countdown > 0) countdown -= Time.deltaTime;
        else if(target != null)
        {
            Fire();
        }
    }

    void Fire()
    {
        if (bulletEffect != null) Destroy(Instantiate(bulletEffect, target.transform.position, Quaternion.identity), 2);
        target.GetHit(damage);

        if(explosionRange > 0)
        {
            foreach(Collider col in Physics.OverlapSphere(target.transform.position, explosionRange, enemyMask))
            {
                Enemy e = col.GetComponent<Enemy>();

                e.GetHit(damage);
            }
        }

        countdown = 1f / fireRate;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 pos1 = transform.position + Vector3.forward * minX;
        Vector3 pos2 = transform.position + Vector3.forward * maxX;
        Vector3 end1 = pos1 + Vector3.left * range;
        Vector3 end2 = pos2 + Vector3.left * range;

        end1.y = end2.y = 0;

        Gizmos.DrawLine(pos1, end1);
        Gizmos.DrawLine(pos2, end2);
        Gizmos.DrawLine(end1, end2);

        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
