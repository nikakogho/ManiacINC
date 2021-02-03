using UnityEngine;

public class ShooterEnemy : Enemy {
    public float range;
    public float shootTime;
    float countdown = 0;
    Transform wall;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        wall = Wall.instance.transform;
    }

    void Fire()
    {
        countdown = shootTime;
        PlayerStats.health -= damage;
        anim.SetTrigger("shoot");
    }

    protected override void DoInUpdate()
    {
        if(wall.position.x - transform.position.x <= range)
        {
            if (countdown > 0) countdown -= Time.deltaTime;
            else
            {
                Fire();
            }
        }
        else
        {
            base.DoInUpdate();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
