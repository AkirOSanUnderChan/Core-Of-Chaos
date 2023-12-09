using UnityEngine;

[CreateAssetMenu(fileName = "New weapon object", menuName = "My/Inventory System/weapons")]

public class WeaponItem : Item
{
    public int weaponDamage;
    public float weaponSpeed;
    public bool itsWeapon;
    public GameObject weaponModel;

}
