using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GameData 
{
    public int coin;
    public List<Equip> myEquips;
    public GameData() {
        coin = 10000;
        myEquips = new List<Equip>();
        myEquips.Add(new Equip("arrow",true,EquipmentType.WEAPON));
    }
    public GameData(int coin) {
        this.coin = coin;
        myEquips = new List<Equip>();
    }
}

[Serializable]
public class Equip
{
    public string equipName;
    public bool used;
    public EquipmentType type;
    public Equip() { }
    public Equip(string equipName,bool used, EquipmentType type)
    {
        this.equipName = equipName;
        this.used = used;
        this.type = type;
    }
}