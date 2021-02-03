using UnityEngine;

public class Wall : MonoBehaviour {
    public static Wall instance;

    void Awake()
    {
        instance = this;
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();

        if (e == null) return;
        
        e.HitTheWall();
    }
}
