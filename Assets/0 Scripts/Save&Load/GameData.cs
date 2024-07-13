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
        coin = 1000;
        myEquips = new List<Equip>();
        myEquips.Add(new Equip("1",false));
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

    public Equip() { }
    public Equip(string equipName,bool used) {
        this.equipName = equipName;
        this.used = used;
    }
}