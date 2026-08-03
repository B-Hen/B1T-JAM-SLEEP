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
    [SerializeField] WeaponData pistolData, shotgunData;
    [SerializeField] Weapon currentWeapon = Weapon.Pistol;

    private int shotsFired = 0;
    private bool canShoot = true;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HandleWeaponUsed();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            shotsFired = 0;
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
        HandleAmmo();

        if (!canShoot) return;

        shotsFired++;

        Bullet bullet = Instantiate(pistolData.bulletPrefab, transform.position, Quaternion.identity);
        bullet.SetData(Input.mousePosition, pistolData.bulletSpeed, pistolData.bulletTime);
    }

    private void HandleShotgun()
    {
        HandleAmmo();

        if (!canShoot) return;

        shotsFired++;

        Bullet shotgunBulletLeft = Instantiate(pistolData.bulletPrefab, transform.position, Quaternion.identity);
        Bullet shotgunBullet = Instantiate(pistolData.bulletPrefab, transform.position, Quaternion.identity);
        Bullet shotgunBulletRight = Instantiate(pistolData.bulletPrefab, transform.position, Quaternion.identity);

        shotgunBulletLeft.SetData(Input.mousePosition, pistolData.bulletSpeed, pistolData.bulletTime, -15);
        shotgunBullet.SetData(Input.mousePosition, pistolData.bulletSpeed, pistolData.bulletTime);
        shotgunBulletRight.SetData(Input.mousePosition, pistolData.bulletSpeed, pistolData.bulletTime, 15);
    }
}
