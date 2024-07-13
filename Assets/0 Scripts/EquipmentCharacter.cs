using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCharacter : MonoBehaviour
{
    [SerializeField] private Transform weaponContain;
    [SerializeField] private Transform hairContain;
    [SerializeField] private Transform shieldContain;

    private GameObject weaponEquipped;
    private GameObject hairEquipped;
    private GameObject shieldEquipped;

    
    public string weaponName { get; private set; }

    public void Equipment(EquipmentSO equip)
    {
        // equip contain null?


        switch (equip.equipmentType)
        {
            case EquipmentType.WEAPON:
                GameObject newWeapon = Instantiate(equip.prefab, weaponContain);
                weaponEquipped = newWeapon;
                weaponName = equip.equipName;
                break;

            case EquipmentType.HAIR:
                GameObject newHair =Instantiate(equip.prefab, hairContain);
                 hairEquipped = newHair;
                break;

            case EquipmentType.SHIELD:
                GameObject newShield =Instantiate(equip.prefab, shieldContain);
                 shieldEquipped = newShield;
                break;
            case EquipmentType.PANT:
                break;
        }
        //add modifier
    }

    public void UnEquipment()
    {
        // do something
    }

    public GameObject GetEquipped(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.WEAPON:
                return weaponEquipped;

            case EquipmentType.HAIR:
                return hairEquipped;

            case EquipmentType.SHIELD:
                return shieldEquipped;
            default:
                return null;
        }

    }
}
