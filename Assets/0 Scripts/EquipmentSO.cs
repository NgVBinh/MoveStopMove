using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    WEAPON,
    PANT,
    SHIELD,
    HAIR
}


[System.Serializable]
public class WeaponMaterialSet
{
    public string setName; 
    public List<Material> materials; 
}

[CreateAssetMenu(fileName = "Equipment",menuName ="Item/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public string equipName;
    public int cost;
    public EquipmentType equipmentType;
    public GameObject prefab;
    
    public List<Material> materials = new List<Material>();

    public List<WeaponMaterialSet> weaponMaterialSets = new List<WeaponMaterialSet>();

    public string description;

    [Range(0,100)]
    public int atkSpeedModifier;
    [Range(0, 100)]
    public int moveSpeedModifier;
    [Range(0, 100)]
    public int rangeModifier;

}
