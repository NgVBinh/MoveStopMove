using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyBtnController : MonoBehaviour, ISaveManager
{
    private EquipmentSO equipSelected;

    public List<Equip> myEquips = new List<Equip>();
    //public int myCoin;

    [SerializeField] private TextMeshProUGUI textBtn;

    private Button myBtn;
    private void Awake()
    {
        myBtn = GetComponent<Button>();
    }
    public void LoadData(GameData gameData)
    {

        myEquips = gameData.myEquips;
        //myCoin = gameData.coin;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.myEquips = myEquips;
        gameData.coin = UIManager.instance.coin;
    }

    public void SetupBtn(EquipmentSO equip)
    {

        equipSelected = equip;

        Equip myEquipped = myEquips.FirstOrDefault(tmp => tmp.equipName == equipSelected.equipName);
        if (myEquipped != null)
        {
            if (myEquipped.used)
            {
                textBtn.text = "Equipped";
            }
            else
            {
                textBtn.text = "Select";
                myBtn.onClick.RemoveAllListeners();
                myBtn.onClick.AddListener(()=>SelectEquipBtn(ref myEquipped));
            }
            return;
        }
        else
        {
            textBtn.text = equipSelected.cost.ToString();
            myBtn.onClick.RemoveAllListeners();
            myBtn.onClick.AddListener(BuyEquip);
        }


    }

    public void BuyEquip()
    {
        UIManager.instance.coin -= equipSelected.cost;
        myEquips.Add(new Equip(equipSelected.equipName, false, equipSelected.equipmentType));
        SetupBtn(equipSelected);

        SaveManager.instance.SaveGame();
    }

    private void SelectEquipBtn(ref Equip myequipped)
    {
        Equip unEquip = myEquips.FirstOrDefault(tmp => tmp.type == equipSelected.equipmentType && tmp.used);
        if(unEquip != null)
        {
            unEquip.used = false;
        }


        myequipped.used = true;
        SetupBtn(equipSelected);

        SaveManager.instance.SaveGame();

    }


}
