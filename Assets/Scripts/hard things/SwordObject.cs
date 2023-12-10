using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwordObject : MonoBehaviour
{
    public PlayerWeaponChanger playerWC;
    public int currentWeaponDamage;
    public float currentWeaponSpeedAtack;


    void Start()
    {
        playerWC = GetComponentInParent<PlayerWeaponChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWC.currentWeaponItem != null)
        {
            var item = playerWC.currentWeaponItem;
            if (item is WeaponItem weaponItem)
            {
                currentWeaponDamage = weaponItem.weaponDamage;
                currentWeaponSpeedAtack = weaponItem.weaponSpeed;

            }
        }
    }
}
