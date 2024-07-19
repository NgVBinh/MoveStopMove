using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GameData 
{
    public string playerName;
    public int coin;
    public List<Equip> myEquips;
    public GameData() {
        playerName = "You";
        coin = 10000;
        myEquips = new List<Equip>();
        myEquips.Add(new Equip("arrow",true,EquipmentType.WEAPON));
    }
}



[Serializable]
public class Equip
{
    public string equipName;
    public bool used;
    public EquipmentType type;

    public bool isUesOneTime;

    public string[] materialsName;
    public Equip() { }
    public Equip(string equipName, bool used, EquipmentType type, bool isUesOneTime = false)
    {
        this.equipName = equipName;
        this.used = used;
        this.type = type;
        this.isUesOneTime = isUesOneTime;
    }

    public Equip(string equipName, bool used, EquipmentType type, string[] materials)
    {
        this.equipName = equipName;
        this.used = used;
        this.type = type;
        this.materialsName = materials;
    }

}
