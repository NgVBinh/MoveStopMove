using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipInShop : MonoBehaviour, IPointerDownHandler
{
    public EquipmentSO myEquipSO;
    private bool canChooseColor;

    public static UI_EquipInShop currentlyEquipped;
    public GameObject equippedTxt;

    void Start()
    {
        
    }

    public void SetupEquipInShop(EquipmentSO equip, bool canChooseColor = false)
    {
        myEquipSO = equip;
        this.canChooseColor = canChooseColor;

        if (equip.equipmentType == EquipmentType.SET)
        {
            GetComponent<Image>().color = Color.white;

            GetComponent<Image>().sprite = equip.setIcon;
            GetComponent<Image>().preserveAspect = true;
            return;
        }

        GameObject equipPref = Instantiate(myEquipSO.prefab, transform);
        equipPref.transform.localScale = Vector3.one * 200;
        if (equip.equipmentType == EquipmentType.PANT)
        {
            MeshRenderer renderer = equipPref.GetComponent<MeshRenderer>();
            renderer.material = myEquipSO.materials[0];
            equipPref.transform.localPosition = new Vector3(0, -140, 0);

        }
        else if (myEquipSO.equipmentType == EquipmentType.SHIELD)
        {
            equipPref.transform.Rotate(0, -90, 90);
        }
        else if (myEquipSO.equipmentType == EquipmentType.WEAPON)
        {

            equipPref.GetComponent<WeaponController>().SetWeaponOfCharacter(true);
            //equipPref.GetComponentInChildren<MeshRenderer>().materials = myEquipSO.materials.ToArray();

            equipPref.transform.GetChild(0).localPosition = Vector3.zero;
            equipPref.transform.localRotation = Quaternion.Euler(0, 0, 140);
            equipPref.transform.localScale = Vector3.one * 100;

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {


        if (myEquipSO.equipmentType != EquipmentType.WEAPON)
        {
            ClickSkinInShop();

        }
        else
        {
            ClickWeaponInShop(myEquipSO);
            if (canChooseColor)
            {
                UIManager.instance.shopWeapon.SetSO(myEquipSO);
                UIManager.instance.shopWeapon.DisplayChooseColor(true);

            }
            else
            {

                UIManager.instance.shopWeapon.DisplayChooseColor(false);
            }
        }
    }

    private void ClickSkinInShop()
    {
        // xem truoc
        if (myEquipSO.equipmentType != EquipmentType.SET)
            UIManager.instance.player.Equipment(myEquipSO);
        else
        {
            UIManager.instance.player.equipController.EquipSet(myEquipSO.equipName);
            Debug.Log(">");
        }
        UI_SkinController skinShopScript = GetComponentInParent<UI_SkinController>();

        if (skinShopScript != null)
        {
            skinShopScript.buyEquipBtn.GetComponent<UI_BuyBtnController>().SetupBtn(myEquipSO,this);
            skinShopScript.descriptEquipTxt.text = myEquipSO.description;

        }
        else
        {
            Debug.LogError("UI_SkinController NULL");
        }
    }

    public void ClickWeaponInShop(EquipmentSO weaponClick)
    {
        UI_ShopWPCT shopWeponScript = GetComponentInParent<UI_ShopWPCT>();
        if (shopWeponScript != null)
        {
            shopWeponScript.buy.GetComponent<UI_BuyBtnController>().SetupBtn(weaponClick);
            shopWeponScript.description.text = weaponClick.description;

            if (shopWeponScript.weaponCenter.childCount > 0)
            {
                Destroy(shopWeponScript.weaponCenter.GetChild(0).gameObject);

            }
            GameObject equipPref = Instantiate(weaponClick.prefab, shopWeponScript.weaponCenter);
            equipPref.GetComponent<WeaponController>().SetWeaponOfCharacter(true);
            equipPref.transform.GetChild(0).localPosition = Vector3.zero;
            //equipPref.GetComponentInChildren<MeshRenderer>().materials = weaponClick.materials.ToArray();
            equipPref.transform.localScale = Vector3.one * 200;

        }
        else
        {
            Debug.LogWarning("shop weapon Null");
        }
    }

    public void SetFirstSkin(EquipmentSO equip)
    {
        //currentlyEquipped = this;

        EquipmentSO firstEquip = equip;
        if (firstEquip.equipmentType != EquipmentType.WEAPON)
        {
            UI_SkinController skinShopScript = GetComponentInParent<UI_SkinController>();
            if (skinShopScript != null)
            {

                skinShopScript.buyEquipBtn.GetComponent<UI_BuyBtnController>().SetupBtn(firstEquip,this);
                
                //UIManager.instance.player.Equipment(firstEquip);/////////

                if (myEquipSO.equipmentType != EquipmentType.SET)
                    UIManager.instance.player.Equipment(firstEquip);
                else
                {
                    UIManager.instance.player.equipController.EquipSet(firstEquip.equipName);
                }

                skinShopScript.descriptEquipTxt.text = firstEquip.description;
            }
            else
            {
                Debug.LogError("UI_SkinController NULL");
            }
        }
        else
        {
            UI_ShopWPCT weaponShopScript = GetComponentInParent<UI_ShopWPCT>();

            ClickWeaponInShop(equip);

            if (weaponShopScript != null)
            {
                weaponShopScript.buy.GetComponent<UI_BuyBtnController>().SetupBtn(firstEquip);

                weaponShopScript.description.text = firstEquip.description;
            }
            else
            {
                Debug.LogError("UI_SkinController NULL");
            }
        }

    }

    public void OnImageSelected()
    {
        if (currentlyEquipped != null)
        {
            currentlyEquipped.equippedTxt.SetActive(false); 
        }

        currentlyEquipped = this;
        equippedTxt.SetActive(true); 
    }
}
