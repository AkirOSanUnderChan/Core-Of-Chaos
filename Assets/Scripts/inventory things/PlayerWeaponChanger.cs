using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerWeaponChanger : MonoBehaviour
{
    private GameObject currentWeapon; // ��'��� ������� ����

    public Item currentWeaponItem; // �������� ������� ���� � ��������

    void Start()
    {
        currentWeaponItem = InventoryManager.instance.weapon1Slot;
        // ��� ������� ������������ ������� �����, ���� ���� �
        EquipCurrentWeapon();
    }

    void Update()
    {
        // ��� ��������� �� ������ ����� �������� ���� ����
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeWeapon();
        }
    }

    void EquipCurrentWeapon()
    {
        // ����������, �� � �������� ������� ����
        if (currentWeaponItem is WeaponItem weaponItem)
        {
            // ��������� ����� �� ������ �� ��������� ��� ����� ��'����
            currentWeapon = Instantiate(weaponItem.weaponModel, transform);
            
        }
    }

    public void ChangeWeapon()
    {
        currentWeaponItem = InventoryManager.instance.weapon1Slot;
        // ����������, �� � ���� ����� � ��������
        if (currentWeaponItem != null)
        {
            // ������� ������� �����
            Destroy(currentWeapon);

            // ������� �������� ������� ���� �� ��������� � �������� (���� ����� �����)
            // currentWeaponItem = ���� ����� � ���������

            // ��������� ����� ��� ���������� ���� ����
            EquipCurrentWeapon();
        }
    }
}
