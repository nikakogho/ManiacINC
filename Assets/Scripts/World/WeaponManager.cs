using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour {
    List<Weapon> weapons = new List<Weapon>();
    public Weapon startWeapon;
    int currentWeaponIndex;
    Weapon currentWeapon;
    public AudioSource source;
    public LayerMask hitMask;
    public static WeaponManager instance;

    public Text weaponNameText, ammoText;
    public Image reloadBar;

    public Texture2D defaultCursor, shotCursor;

    IEnumerator lastReload;

    void Awake()
    {
        instance = this;
        AddWeapon(startWeapon);
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        weapon.canFire = true;
        weapon.reloading = false;
        weapon.reloadCount = 0;
        weapon.ammo = weapon.ammoPerLoad;
        currentWeaponIndex = weapons.Count - 1;
        ChangeWeapon();
    }

    void Update()
    {
        if (!WaveLauncher.IsWaveInProcess) return;

        if (currentWeapon.reloading)
        {
            currentWeapon.reloadCount += Time.deltaTime;
            //reloadBar.fillAmount = currentWeapon.reloadCount / (currentWeapon.reloadTime - ((float)currentWeapon.ammo / currentWeapon.ammoPerLoad));
            reloadBar.fillAmount = currentWeapon.reloadCount / currentWeapon.reloadTime;
        }

        if(Input.GetKeyDown("r") && !currentWeapon.reloading && currentWeapon.ammo < currentWeapon.ammoPerLoad)
        {
            lastReload = Reload(currentWeapon);
            StartCoroutine(lastReload);
        }
        else if (Input.GetKeyDown("q"))
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0) currentWeaponIndex += weapons.Count;
            ChangeWeapon();
        }
        else if (Input.GetKeyDown("e"))
        {
            currentWeaponIndex++;
            if (currentWeaponIndex == weapons.Count) currentWeaponIndex = 0;
            ChangeWeapon();
        }
        else if(!currentWeapon.reloading && currentWeapon.ammo > 0 && currentWeapon.canFire)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire(false);
            }
            else if(currentWeapon.isAuto && Input.GetButton("Fire1"))
            {
                Fire();
            }
        }
    }

    void Fire(bool autoFire = true)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit, 1000, hitMask)) return;

        if (currentWeapon.explosionRange > 0)
        {
            Collider[] cols = Physics.OverlapSphere(hit.point, currentWeapon.explosionRange);

            foreach (Collider col in cols)
            {
                Enemy e = col.GetComponent<Enemy>();
                e.GetHit(currentWeapon.damage);
            }
        }

        if (currentWeapon.bulletEffect != null) Destroy(Instantiate(currentWeapon.bulletEffect, hit.point, Quaternion.identity), currentWeapon.effectLifetime);

        Enemy enemy = hit.transform.GetComponent<Enemy>();

        if (enemy != null)
        {
            StartCoroutine(Shoot(enemy));
        }

        if (!autoFire || !source.isPlaying || source.clip != currentWeapon.sound)
        {
            source.clip = currentWeapon.sound;
            source.Play();
        }

        currentWeapon.ammo--;
        reloadBar.fillAmount -= 1f / currentWeapon.ammoPerLoad;
        ammoText.text = currentWeapon.ammo + "/" + currentWeapon.ammoPerLoad;
        if (currentWeapon.ammo == 0)
        {
            reloadBar.fillAmount = 0;
            lastReload = Reload(currentWeapon);
            StartCoroutine(lastReload);
        }
        else StartCoroutine(FireCountDown());
    }

    void ChangeWeapon()
    {
        if(currentWeapon != null)
        {
            StopCoroutine(lastReload);
        }
        
        currentWeapon = weapons[currentWeaponIndex];

        if (currentWeapon.reloading)
        {
            lastReload = Reload(currentWeapon);
            StartCoroutine(lastReload);
        }

        weaponNameText.text = currentWeapon.name;
        ammoText.text = currentWeapon.ammo + "/" + currentWeapon.ammoPerLoad;
    }

    public void EndWave()
    {
        StopAllCoroutines();

        foreach (Weapon weapon in weapons)
        {
            FinishReloading(weapon);
        }

        ammoText.text = currentWeapon.ammo + "/" + currentWeapon.ammoPerLoad;
        reloadBar.fillAmount = 1;
        Cursor.SetCursor(defaultCursor, Vector2.one * 256, CursorMode.Auto);
    }

    void FinishReloading(Weapon weapon)
    {
        weapon.ammo = weapon.ammoPerLoad;
        weapon.reloading = false;
    }

    IEnumerator FireCountDown()
    {
        currentWeapon.canFire = false;
        yield return new WaitForSeconds(1f / currentWeapon.fireRate);
        currentWeapon.canFire = true;
    }

    IEnumerator Reload(Weapon currentWeapon)
    {
        currentWeapon.reloading = true;
        currentWeapon.reloadCount = (float)currentWeapon.ammo / currentWeapon.ammoPerLoad;
        yield return new WaitForSeconds(currentWeapon.reloadTime - currentWeapon.reloadCount);
        FinishReloading(currentWeapon);
        ammoText.text = currentWeapon.ammo + "/" + currentWeapon.ammoPerLoad;
    }

    IEnumerator Shoot(Enemy e)
    {
        e.GetHit(currentWeapon.damage);
        Cursor.SetCursor(shotCursor, new Vector2(160, 127), CursorMode.Auto);
        yield return new WaitForSeconds(0.5f);
        Cursor.SetCursor(defaultCursor, Vector2.one * 256, CursorMode.Auto);
    }
}
