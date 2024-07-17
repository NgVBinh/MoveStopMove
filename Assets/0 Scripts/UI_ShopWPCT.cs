using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WeaponInShop
{
    public int index;
    public string weaponName;
    public List<EquipmentSO> weaponInShops;
}

public class UI_ShopWPCT : MonoBehaviour
{
    public List<WeaponInShop> weapons = new List<WeaponInShop>();


    public Transform weaponCenter;
    [SerializeField] private Transform weaponColorChoose;

    [SerializeField] private Button rightBtn;
    [SerializeField] private Button leftBtn;
    [SerializeField] private Button buyBtn;

    [SerializeField] private Button backBtn;

    [SerializeField] private GameObject equipInShop_UI;

    public Button buy;
    public TextMeshProUGUI description;


    public int weaponIndex;

    private void OnEnable()
    {
        DisplayWeapon(weaponIndex);

    }

    void Start()
    {
        rightBtn.onClick.AddListener(NextWeaponRight);
        leftBtn.onClick.AddListener(NextWeaponLeft);
        backBtn.onClick.AddListener(UIManager.instance.InitializedPannel);
    }

    private void DisplayWeapon(int index)
    {
        if (weaponCenter.childCount > 0)
        {
            Destroy(weaponCenter.GetChild(0).gameObject);
        }
        if (weaponColorChoose.childCount > 0)
        {
            for (int i = 0; i < weaponColorChoose.childCount; i++)
            {
                Destroy(weaponColorChoose.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < weapons[index].weaponInShops.Count; i++)
        {
            GameObject equipInShop = Instantiate(equipInShop_UI, weaponColorChoose);
            equipInShop.GetComponentInChildren<UI_EquipInShop>().SetupEquipInShop(weapons[index].weaponInShops[i]);

        }

    }

    private void NextWeaponLeft()
    {
        weaponIndex--;
        weaponIndex = Mathf.Clamp(weaponIndex, 0, weapons.Count - 1);
        DisplayWeapon(weaponIndex);
    }

    private void NextWeaponRight()
    {
        weaponIndex++;
        weaponIndex = Mathf.Clamp(weaponIndex, 0, weapons.Count - 1);
        DisplayWeapon(weaponIndex);
    }
}
