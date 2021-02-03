using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject {
    new public string name;
    public AudioClip sound;
    public float fireRate;
    public float damage;
    public bool isAuto = false;
    public float explosionRange = 0;
    public GameObject bulletEffect;
    public float effectLifetime = 5;
    public int ammoPerLoad = 1;
    public float reloadTime;
    [HideInInspector] public bool canFire;
    [HideInInspector] public int ammo;
    [HideInInspector] public bool reloading;
    [HideInInspector] public float reloadCount;
}
