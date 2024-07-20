using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkinList : MonoBehaviour
{
    private Button myBtn;

    //[SerializeField] private Image transparentImg;
    [SerializeField] private GameObject equipInShop_UI;
    [SerializeField] private List<EquipmentSO> equipList;
    [SerializeField] private Transform shopPannel;

    private EquipmentSO equpiped;
    bool setFirst = true;
    void Start()
    {
        myBtn = GetComponent<Button>();

        myBtn.onClick.AddListener(DisplayEquipInShop);

    }
    GameObject firstEquip;

    // attach more in inspector
    public void DisplayEquipInShop()
    {
        string hairName = UIManager.instance.hairName;
        string pantName = UIManager.instance.pantName;
        string shieldName = UIManager.instance.shieldName;
        string setName = UIManager.instance.setName;
        if (shopPannel.childCount > 0)
        {
            for (int i = 0; i < shopPannel.childCount; i++)
            {
                Destroy(shopPannel.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < equipList.Count; i++)
        {
            GameObject equipInShop = Instantiate(equipInShop_UI, shopPannel);
            equipInShop.GetComponentInChildren<UI_EquipInShop>().SetupEquipInShop(equipList[i]);
            if (i == 0)
            {
                firstEquip = equipInShop;
            }
            if (equipList[i].equipName == hairName || equipList[i].equipName == pantName || equipList[i].equipName == shieldName || equipList[i].equipName == setName)
            {
                equipInShop.GetComponentInChildren<UI_EquipInShop>().OnImageSelected();
                equipInShop.GetComponentInChildren<OutlineController>().DisplayOutline();
                equipInShop.GetComponentInChildren<UI_EquipInShop>().SetFirstSkin(equipList[i]);

                setFirst = false;
            }

        }
        if (setFirst)
        {
            firstEquip.GetComponentInChildren<UI_EquipInShop>().SetFirstSkin(equipList[0]);
            firstEquip.GetComponentInChildren<OutlineController>().DisplayOutline();

        }

    }
}
