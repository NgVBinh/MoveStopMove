using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipInShop : MonoBehaviour, IPointerDownHandler
{
    public EquipmentSO myEquipSO;

    public void SetupEquipInShop(EquipmentSO equip)
    {
        myEquipSO = equip;

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
            equipPref.GetComponent<MeshRenderer>().materials = myEquipSO.materials.ToArray();
            equipPref.transform.localScale = Vector3.one * 5000;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        // xem truoc
        UIManager.instance.player.Equipment(myEquipSO);

        if (myEquipSO.equipmentType != EquipmentType.WEAPON)
        {

            UI_SkinController skinShopScript = GetComponentInParent<UI_SkinController>();

            if (skinShopScript != null)
            {
                skinShopScript.buyEquipBtn.GetComponent<UI_BuyBtnController>().SetupBtn(myEquipSO);
                skinShopScript.descriptEquipTxt.text = myEquipSO.description;

            }
            else
            {
                Debug.LogError("UI_SkinController NULL");
            }
        }
        else
        {
            UI_ShopWPCT shopWeponScript = GetComponentInParent<UI_ShopWPCT>();
            if(shopWeponScript != null)
            {
                shopWeponScript.buy.GetComponent<UI_BuyBtnController>().SetupBtn(myEquipSO);
                shopWeponScript.description.text = myEquipSO.description;

                if (shopWeponScript.weaponCenter.childCount > 0)
                {
                    Destroy(shopWeponScript.weaponCenter.GetChild(0).gameObject);

                }
                    GameObject equipPref = Instantiate(myEquipSO.prefab, shopWeponScript.weaponCenter);
                    equipPref.GetComponent<WeaponController>().SetWeaponOfCharacter(true);
                    equipPref.GetComponent<MeshRenderer>().materials = myEquipSO.materials.ToArray();
                    equipPref.transform.localScale = Vector3.one * 10000;

            }
        }
    }

    public void SetFirstSkin(EquipmentSO equip)
    {

        EquipmentSO firstEquip = equip;
        Debug.Log(firstEquip.equipName);

        UI_SkinController skinShopScript = GetComponentInParent<UI_SkinController>();
        if (skinShopScript != null)
        {
            skinShopScript.buyEquipBtn.GetComponent<UI_BuyBtnController>().SetupBtn(firstEquip);
            UIManager.instance.player.Equipment(firstEquip);/////////

            skinShopScript.descriptEquipTxt.text = firstEquip.description;
        }
        else
        {
            Debug.LogError("UI_SkinController NULL");
        }
    }
}
