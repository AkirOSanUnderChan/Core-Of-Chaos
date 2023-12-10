using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerWeaponChanger : MonoBehaviour
{
    private GameObject currentWeapon; // Об'єкт поточної зброї

    public Item currentWeaponItem; // Поточний предмет зброї в інвентарі

    void Start()
    {
        currentWeaponItem = InventoryManager.instance.weapon1Slot;
        // При запуску встановлюємо поточну зброю, якщо вона є
        EquipCurrentWeapon();
    }

    void Update()
    {
        // При натисканні на клавішу можна здійснити зміну зброї
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeWeapon();
        }
    }

    void EquipCurrentWeapon()
    {
        // Перевіряємо, чи є поточний предмет зброї
        if (currentWeaponItem is WeaponItem weaponItem)
        {
            // Створюємо зброю та робимо її дочірньою для цього об'єкта
            currentWeapon = Instantiate(weaponItem.weaponModel, transform);
            
        }
    }

    public void ChangeWeapon()
    {
        currentWeaponItem = InventoryManager.instance.weapon1Slot;
        // Перевіряємо, чи є нова зброя в інвентарі
        if (currentWeaponItem != null)
        {
            // Знищуємо поточну зброю
            Destroy(currentWeapon);

            // Змінюємо поточний предмет зброї на наступний в інвентарі (ваша логіка обміну)
            // currentWeaponItem = нова зброя з інвентаря

            // Викликаємо метод для екіпірування нової зброї
            EquipCurrentWeapon();
        }
    }
}
