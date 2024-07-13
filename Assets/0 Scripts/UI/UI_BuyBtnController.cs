using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyBtnController : MonoBehaviour, ISaveManager
{
    private EquipmentSO equipSelected;

    public List<Equip> myEquips;
    public int myCoin;

    [SerializeField] private TextMeshProUGUI textBtn;

    private Button myBtn;
    private void Start()
    {
        myBtn = GetComponent<Button>();
    }
    public void LoadData(GameData gameData)
    {
        myEquips.Clear();

        myEquips = gameData.myEquips;
        myCoin = gameData.coin;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.myEquips = myEquips;
        gameData.coin = myCoin;
    }

    public void SetupBtn(EquipmentSO equip)
    {
        equipSelected = equip;

        foreach (Equip equipment in myEquips)
        {
            if (equipSelected.equipName.Equals(equipment.equipName))
            {
                if (equipment.used)
                {

                }
                else
                {

                }
                return;
            }
        }
        
            textBtn.text = equipSelected.cost.ToString();
            myBtn.onClick.RemoveAllListeners();
            myBtn.onClick.AddListener(BuyEquip);
        

    }

    public void BuyEquip()
    {
        myCoin -= equipSelected.cost;
        myEquips.Add(new Equip(equipSelected.equipName, false));
    }


}
