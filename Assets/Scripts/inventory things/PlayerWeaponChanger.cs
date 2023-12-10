using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWeaponChanger : MonoBehaviour
{
    public static PlayerWeaponChanger instance;
    private GameObject currentWeapon;

    public Item currentWeaponItem;

    void Start()
    {
        currentWeaponItem = InventoryManager.instance.weapon1Slot;
        EquipCurrentWeapon();
    }

    void Update()
    {
        
            
        
    }

    void EquipCurrentWeapon()
    {

        if (currentWeaponItem is WeaponItem weaponItem)
        {
            currentWeapon = Instantiate(weaponItem.weaponModel, transform);
            
        }
    }

    public void ChangeWeapon()
    {
        currentWeaponItem = InventoryManager.instance.weapon1Slot;
        if (currentWeaponItem != null)
        {
            Destroy(currentWeapon);
            EquipCurrentWeapon();
        }
        else
        {
            Destroy(currentWeapon);
        }
    }
}
