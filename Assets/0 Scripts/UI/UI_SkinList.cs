using System.Collections;
using System.Collections.Generic;
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


    void Start()
    {
        myBtn = GetComponent<Button>();

        myBtn.onClick.AddListener(DisplayEquipInShop);
    }

    public void DisplayEquipInShop()
    {

        if (shopPannel.childCount > 0)
        {
            for(int i = 0;i<shopPannel.childCount;i++)
            {
                Destroy(shopPannel.GetChild(i).gameObject);
            }
        }

        for(int i = 0;i<equipList.Count;i++)
        //foreach(EquipmentSO equip in equipList)
        {
            GameObject equipInShop = Instantiate(equipInShop_UI, shopPannel);
            equipInShop.GetComponent<UI_EquipInShop>().SetupEquipInShop(equipList[i]);
            if (i == 0)
            {
                equipInShop.GetComponent<UI_EquipInShop>().SetFirstSkin(equipList[i]);

            }
        }


        //transparentImg.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        //transparentImg.gameObject.SetActive(true);
        //Debug.Log("?");

    }
}
