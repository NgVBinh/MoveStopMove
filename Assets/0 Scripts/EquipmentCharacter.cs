using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentCharacter : MonoBehaviour
{
    [SerializeField] private Transform weaponContain;
    [SerializeField] private Transform hairContain;
    [SerializeField] private Transform shieldContain;

    // set Equip
    [SerializeField] private Transform wingContain;


    [SerializeField] private List<EquipmentSO> allEquipment;

    private GameObject weaponEquipped;
    private GameObject hairEquipped;
    private GameObject shieldEquipped;
    private GameObject pantEquipped;

    public string weaponName { get; private set; }

    [SerializeField] private SkinnedMeshRenderer bodyRenderer;

    [SerializeField] private SkinnedMeshRenderer pantRenderer;
    public Material defaultPantMaterial;
    private Material defaultBodyMaterial;

    private Entity character;
    private float attackSpeedIncreased;
    private float attackRangeIncreased;
    private float attackRangeByHairIncreased;
    private float moveSpeedIncreased;
    private void Awake()
    {
        if (pantRenderer != null)
            defaultPantMaterial = pantRenderer.material;
        if (bodyRenderer != null)
            defaultBodyMaterial = bodyRenderer.material;
        character = GetComponent<Entity>();
    }

    public void PlayerEquipped(string name)
    {
        EquipmentSO equip = allEquipment.FirstOrDefault(tmp => tmp.equipName.Equals(name));
        if (equip == null) return;

        Equipment(equip);
        AddStat(equip, character);

    }

    public void Equipment(EquipmentSO equip)
    {
        // equip contain null?
        // add modifier

        switch (equip.equipmentType)
        {
            case EquipmentType.WEAPON:
                RemoveWeapon();

                GameObject newWeapon = Instantiate(equip.prefab, weaponContain);
                weaponEquipped = newWeapon;
                weaponName = equip.equipName;

                break;
            case EquipmentType.HAIR:
                RemoveHair();

                GameObject newHair = Instantiate(equip.prefab, hairContain);
                hairEquipped = newHair;

                break;

            case EquipmentType.SHIELD:
                RemoveShield();

                GameObject newShield = Instantiate(equip.prefab, shieldContain);
                shieldEquipped = newShield;

                break;
            case EquipmentType.PANT:

                GameObject newPant = Instantiate(equip.prefab);
                pantEquipped = newPant;
                newPant.SetActive(false);
                Material pantMaterial = equip.materials[0];
                pantRenderer.material = pantMaterial;

                break;
        }

    }

    public void UnEquipment(Equip equip)
    {
        // do something
        switch (equip.type)
        {
            case EquipmentType.HAIR:
                if (hairContain.childCount > 0)
                {
                    Destroy(hairContain.GetChild(0).gameObject);
                    //character.usedHair = false;
                }
                break;
            case EquipmentType.PANT:
                Material pantMaterial = defaultPantMaterial;
                pantRenderer.material = pantMaterial;
                break;
            case EquipmentType.SHIELD:
                if (shieldContain.childCount > 0)
                {
                    Destroy(shieldContain.GetChild(0).gameObject);
                    //character.usedShield = false;   
                }
                break;
        }
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


    private void AddStat(EquipmentSO equip, Entity character)
    {

        if (!character.usedHair && equip.equipmentType == EquipmentType.HAIR)
        {
            attackRangeByHairIncreased = character.attackRange * (equip.rangeModifier / 100f);
            character.IncreaseRange(equip.rangeModifier);
            character.usedHair = true;

        }

        if (weaponContain.childCount > 0 && equip.equipmentType == EquipmentType.WEAPON)
        {
            RemoveWeaponStat();
            if (equip.atkSpeedModifier != 0)
            {
                attackSpeedIncreased = character.attackSpeed * (equip.atkSpeedModifier / 100f);
                character.IncreaseAttackSpeed(equip.atkSpeedModifier);
            }
            if (equip.rangeModifier != 0)
            {
                attackRangeIncreased = character.attackRange * (equip.rangeModifier / 100f);
                character.IncreaseRange(equip.rangeModifier);
            }
            character.usedWeapon = true;
        }
        if (shieldContain && !character.usedShield)
        {
            // +25% coin
            character.usedShield = true;
        }
        if ( !character.usedPant  &&  equip.equipmentType == EquipmentType.PANT)
        {
            moveSpeedIncreased = character.moveSpeed * (equip.moveSpeedModifier / 100f);
            character.IncreaseSpeed(equip.moveSpeedModifier);
            character.usedPant = true;
        }

    }

    public void RemoveHairStat()
    {
        RemoveHair();
        if (character.usedHair)
        {
            character.usedHair = false;
            character.attackRange -= attackRangeByHairIncreased;
        }
    }

    public void RemovePantStat()
    {
        RemmovePant();
        if (character.usedPant)
        {
            character.usedPant = false;
            character.moveSpeed -= moveSpeedIncreased;

        }
    }

    public void RemoveWeaponStat()
    {
        character.attackRange -= attackRangeIncreased;
        character.attackSpeed -= attackSpeedIncreased;

        attackSpeedIncreased = 0;
        attackRangeIncreased = 0;
    }

    private void RemoveHair()
    {
        if (hairContain.childCount > 0)
        {
            Destroy(hairContain.GetChild(0).gameObject);
            hairEquipped = null;
        }
    }

    private void RemoveWeapon()
    {
        if (weaponContain.childCount > 0)
        {
            Destroy(weaponContain.GetChild(0).gameObject);
        }
    }

    private void RemmovePant()
    {
        if (pantRenderer.material != defaultPantMaterial)
        {
            pantRenderer.material = defaultPantMaterial;
        }
    }

    private void RemoveShield()
    {
        if (shieldContain.childCount > 0)
        {
            Destroy(shieldContain.GetChild(0).gameObject);
            shieldEquipped = null;
        }
    }
    private void RemoveWing()
    {
        if (wingContain.childCount > 0)
        {
            Destroy(wingContain.GetChild(0).gameObject);
        }
    }
    public void EquipSet(string name)
    {
        EquipmentSO set = allEquipment.FirstOrDefault(tmp => tmp.equipName.Equals(name));

        bodyRenderer.material = set.materials[1];
        pantRenderer.enabled = false;
        RemoveHair();
        RemoveShield();
        RemoveWing();

        if (!set.equipOfSets[0].IsUnityNull())
        {
            GameObject newHair = Instantiate(set.equipOfSets[0], hairContain);
        }
        if (!set.equipOfSets[1].IsUnityNull())
        {
            GameObject newWing = Instantiate(set.equipOfSets[1], wingContain);
        }
        if (!set.equipOfSets[2].IsUnityNull())
        {
            GameObject newShield = Instantiate(set.equipOfSets[2], shieldContain);
        }

    }

    public void UnEquipSet()
    {
        RemoveHair();
        RemoveShield();
        RemoveWing();
        pantRenderer.enabled = true;
        pantRenderer.material = defaultPantMaterial;
        bodyRenderer.material = defaultBodyMaterial;
    }


}
