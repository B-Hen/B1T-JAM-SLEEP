using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public enum Weapon
{
    Pistol,
    Shotgun,
    Sword
}

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData pistolData, shotgunData;
    [SerializeField] private Weapon currentWeapon = Weapon.Pistol;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> pistolSFX, shotgunSFX;

    private int shotsFired = 0;
    private bool canShoot = true;
    private float canShootTimer = -1f;

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            HandleWeaponUsed();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            shotsFired = 0;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(currentWeapon == Weapon.Pistol)
            {
                currentWeapon = Weapon.Shotgun;
            }
            else if(currentWeapon == Weapon.Shotgun)
            {
                currentWeapon = Weapon.Pistol;
            }
        }

        if(canShootTimer > 0)
        {
            canShootTimer -= Time.deltaTime;

            if(canShootTimer <= 0)
            {
                canShoot = true;
            }
        }
    }

    private void HandleWeaponUsed()
    {
        switch (currentWeapon)
        {
            case Weapon.Pistol:
                HandlePistol();
                break;
            case Weapon.Shotgun:
                HandleShotgun();
                break;
            default:
                Debug.Log($"Weapon not reconginzed {currentWeapon}");
                break;
        }
    }

    private void HandleAmmo()
    {
        canShoot = true;

        switch(currentWeapon)
        {
            case Weapon.Pistol:
                if (shotsFired >= pistolData.bulletAmmo) canShoot = false;
                break;
            case Weapon.Shotgun:
                if (shotsFired >= shotgunData.bulletAmmo) canShoot = false;
                break;
            default:
                Debug.Log($"Weapon not reconginzed {currentWeapon}");
                break;
        }
    }

    private void HandlePistol()
    {
        //HandleAmmo();

        if (!canShoot) return;

        shotsFired++;

        Bullet bullet = Instantiate(pistolData.bulletPrefab, transform.position, Quaternion.identity);
        bullet.SetData(Input.mousePosition, pistolData.bulletSpeed, pistolData.bulletTime);

        int index = Random.Range(0, pistolSFX.Count);
        audioSource.PlayOneShot(pistolSFX[index], 0.5f);

        canShootTimer = pistolData.weaponCoolDown;
        canShoot = false;
    }

    private void HandleShotgun()
    {
        //HandleAmmo();

        if (!canShoot) return;

        shotsFired++;

        Bullet shotgunBulletLeft = Instantiate(pistolData.bulletPrefab, transform.position, Quaternion.identity);
        Bullet shotgunBullet = Instantiate(pistolData.bulletPrefab, transform.position, Quaternion.identity);
        Bullet shotgunBulletRight = Instantiate(pistolData.bulletPrefab, transform.position, Quaternion.identity);

        shotgunBulletLeft.SetData(Input.mousePosition, pistolData.bulletSpeed, pistolData.bulletTime, -15);
        shotgunBullet.SetData(Input.mousePosition, pistolData.bulletSpeed, pistolData.bulletTime);
        shotgunBulletRight.SetData(Input.mousePosition, pistolData.bulletSpeed, pistolData.bulletTime, 15);

        int index = Random.Range(0, shotgunSFX.Count);
        audioSource.PlayOneShot(shotgunSFX[index], 0.5f);

        canShootTimer = shotgunData.weaponCoolDown;
        canShoot = false;
    }
}
