using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipInShop : MonoBehaviour,IPointerDownHandler
{
    public EquipmentSO myEquipSO;

    public void SetupEquipInShop(EquipmentSO equip)
    {
        myEquipSO = equip;

        GameObject equipPref = Instantiate(myEquipSO.prefab,transform);
        if(equip.equipmentType == EquipmentType.PANT)
        {
            MeshRenderer renderer = equipPref.GetComponent<MeshRenderer>();
            renderer.material = equip.materials[0];
            equipPref.transform.localPosition= new Vector3 (0,-140,0);

        }
        else if (equip.equipmentType == EquipmentType.SHIELD)
        {
            equipPref.transform.Rotate(0, -90, 90);
        }
        equipPref.transform.localScale = Vector3.one*200;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(myEquipSO.equipName);
        UI_SkinController skinShopScript = GetComponentInParent<UI_SkinController>();

        if(skinShopScript != null )
        {
            skinShopScript.buyEquipBtn.GetComponent<UI_BuyBtnController>().SetupBtn(myEquipSO);
            skinShopScript.descriptEquipTxt.text = myEquipSO.description;
        }
        else
        {
            Debug.LogError("UI_SkinController NULL");
        }
    }


}
