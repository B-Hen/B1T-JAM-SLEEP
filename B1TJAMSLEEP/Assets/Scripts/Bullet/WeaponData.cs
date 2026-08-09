using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public Bullet bulletPrefab;
    public float bulletSpeed, weaponCoolDown;
    public int bulletAmmo, bulletTime;
}
