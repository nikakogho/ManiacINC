using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {
    public float health;
    public float moveSpeed;
    public int value;
    public int damage = 1;
    public float deathEffectLifetime = 5;
    public GameObject deathEffect;
    public AudioClip deathSound;
    AudioSource source;
    Rigidbody rb;
    
    void Awake()
    {
        SetupAll();
        GetComponent<Animator>().applyRootMotion = false;

        transform.LookAt(Wall.instance.transform);
    }

    [ContextMenu("Setup All")]
    void SetupAll()
    {
        SetupAudio();
        SetupRigidBody();
    }

    [ContextMenu("Setup Audio")]
    void SetupAudio()
    {
        source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
    }

    [ContextMenu("Setup RB")]
    void SetupRigidBody()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    protected virtual void DoInUpdate()
    {
        rb.MovePosition(rb.position + Vector3.right * moveSpeed * Time.deltaTime);
    }
    
    void Update()
    {
        DoInUpdate();
    }

    void Die()
    {
        WaveLauncher.enemyCount--;

        if (!WaveLauncher.IsWaveInProcess)
        {
            WaveLauncher.instance.EndWave();
        }

        health = 0;
        if(deathSound != null)
        {
            source.clip = deathSound;
            source.Play();
        }
        if (deathEffect != null) Destroy(Instantiate(deathEffect, transform.position, transform.rotation), deathEffectLifetime);
        Destroy(gameObject);
        enabled = false;
    }

    public void GetHit(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            PlayerStats.money += value;
            Die();
        }
    }

    public void HitTheWall()
    {
        PlayerStats.health -= damage;
        Die();
    }
}
