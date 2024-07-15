using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentCharacter : MonoBehaviour
{
    [SerializeField] private Transform weaponContain;
    [SerializeField] private Transform hairContain;
    [SerializeField] private Transform shieldContain;

    [SerializeField] private List<EquipmentSO> allEquipment;

    private GameObject weaponEquipped;
    private GameObject hairEquipped;
    private GameObject shieldEquipped;
    private GameObject pantEquipped;

    public string weaponName { get; private set; }

    [SerializeField] private SkinnedMeshRenderer pantRenderer;
    public Material defaultPantMaterial;
    private void Awake()
    {
        if (pantRenderer != null)
            defaultPantMaterial = pantRenderer.material;
    }

    public void PlayerEquipped(string name, Entity character)
    {
        EquipmentSO equip = allEquipment.FirstOrDefault(tmp => tmp.equipName.Equals(name));

        if (equip == null)
        {
            //Debug.LogWarning(name + " don't exits");
            if (weaponContain.childCount > 0)
            {
                Destroy(weaponContain.GetChild(0).gameObject);
            }
            if (hairContain.childCount > 0)
            {
                Destroy(hairContain.GetChild(0).gameObject);
            }
            if (shieldContain.childCount > 0)
            {
                Destroy(shieldContain.GetChild(0).gameObject);
            }
            if (pantRenderer.material != defaultPantMaterial)
            {
                pantRenderer.material = defaultPantMaterial;
            }
            return;
        }

        Equipment(equip, character);
    }

    public void Equipment(EquipmentSO equip, Entity character)
    {
        // equip contain null?
        // add modifier

        switch (equip.equipmentType)
        {
            case EquipmentType.WEAPON:
                if (weaponContain.childCount > 0)
                {
                    Destroy(weaponContain.GetChild(0).gameObject);
                    //character.usedWeapon = false;
                }

                GameObject newWeapon = Instantiate(equip.prefab, weaponContain);
                weaponEquipped = newWeapon;
                weaponName = equip.equipName;
                AddModifier(equip, character);

                break;
            case EquipmentType.HAIR:
                if (hairContain.childCount > 0)
                {
                    Destroy(hairContain.GetChild(0).gameObject);
                    //character.usedHair = false;

                }

                GameObject newHair = Instantiate(equip.prefab, hairContain);
                hairEquipped = newHair;
                AddModifier(equip, character);

                break;

            case EquipmentType.SHIELD:
                if (shieldContain.childCount > 0)
                {
                    Destroy(shieldContain.GetChild(0).gameObject);
                    //character.usedShield = false;   
                }

                GameObject newShield = Instantiate(equip.prefab, shieldContain);
                shieldEquipped = newShield;
                AddModifier(equip, character);

                break;
            case EquipmentType.PANT:

                GameObject newPant = Instantiate(equip.prefab);
                pantEquipped = newPant;
                newPant.SetActive(false);
                Material pantMaterial = equip.materials[0];
                pantRenderer.material = pantMaterial;
                AddModifier(equip, character);

                break;
        }

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
            case EquipmentType.PANT:
                return pantEquipped;
            default:
                return null;
        }

    }


    private void AddModifier(EquipmentSO equip, Entity character)
    {
        if (GetEquipped(EquipmentType.WEAPON) && !character.usedWeapon)
        {
            if (equip.atkSpeedModifier != 0)
            {
                character.IncreaseAttackSpeed(equip.atkSpeedModifier);
            }
            if (equip.rangeModifier != 0)
            {
                character.IncreaseRange(equip.rangeModifier);
            }
            character.usedWeapon = true;
        }
        if (GetEquipped(EquipmentType.HAIR) != null && !character.usedHair)
        {
            character.IncreaseRange(equip.rangeModifier);
            character.usedHair = true;
        }
        if (GetEquipped(EquipmentType.SHIELD) && !character.usedShield)
        {
            // +25% coin
            character.usedShield = true;
        }
        if (GetEquipped(EquipmentType.PANT) && !character.usedPant)
        {
            character.IncreaseSpeed(equip.moveSpeedModifier);
            character.usedPant = true;

        }
        else
        {
        }
    }
}
