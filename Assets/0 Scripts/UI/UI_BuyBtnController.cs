using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyBtnController : MonoBehaviour, ISaveManager
{
    private EquipmentSO equipSelected;

    public List<Equip> myEquips = new List<Equip>();
    public int myCoin;

    [SerializeField] private TextMeshProUGUI textBtn;
    [SerializeField] private Image coinImage;


    private Button myBtn;
    [Header("Use one Time")]
    [SerializeField] private Button oneTimeBtn;
    [SerializeField] private TextMeshProUGUI useOneTimeTxt;
    private void Awake()
    {
        myBtn = GetComponent<Button>();
    }

    //private void OnEnable()
    //{
    //    SaveManager.instance.LoadGame();
    //}

    public void LoadData(GameData gameData)
    {

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
        if (useOneTimeTxt != null)
            useOneTimeTxt.gameObject.SetActive(false);

        if (oneTimeBtn != null)
            oneTimeBtn.gameObject.SetActive(false);

        Equip myEquipSelected = myEquips.FirstOrDefault(tmp => tmp.equipName == equipSelected.equipName);
        if (myEquipSelected != null)
        {

            // da mua, da trang bi
            if (myEquipSelected.used)
            {
                if (myEquipSelected.type == EquipmentType.WEAPON)
                {
                    myBtn.onClick.RemoveAllListeners();
                    textBtn.text = "Equipped";
                    coinImage.gameObject.SetActive(false);
                    return;
                }

                myBtn.onClick.RemoveAllListeners();
                myBtn.onClick.AddListener(() => UnEquip(ref myEquipSelected));
                textBtn.text = "UnEquip";
                coinImage.gameObject.SetActive(false);

                if (myEquipSelected.isUesOneTime)
                {
                    useOneTimeTxt.gameObject.SetActive(true);
                }
            }
            // da mua, chua trang bi
            else
            {
                textBtn.text = "Select";
                myBtn.onClick.RemoveAllListeners();
                coinImage.gameObject.SetActive(false);
                myBtn.onClick.AddListener(() => SelectEquipBtn(ref myEquipSelected));

                if (myEquipSelected.isUesOneTime)
                {
                    useOneTimeTxt.gameObject.SetActive(true);
                }
            }
            return;

        }

        // chua mua trang bi
        else
        {
            if (oneTimeBtn != null)
            {
                oneTimeBtn.gameObject.SetActive(true);

                oneTimeBtn.onClick.RemoveAllListeners();
                oneTimeBtn.onClick.AddListener(BuyOneTime);
            }

            textBtn.text = equipSelected.cost.ToString();
            coinImage.gameObject.SetActive(true);
            myBtn.onClick.RemoveAllListeners();
            myBtn.onClick.AddListener(BuyEquip);
        }


    }

    public void BuyEquip()
    {
        UIManager.instance.coin -= equipSelected.cost;
        myEquips.Add(new Equip(equipSelected.equipName, false, equipSelected.equipmentType));

        SaveManager.instance.SaveGame();
        SetupBtn(equipSelected);
    }

    private void SelectEquipBtn(ref Equip myEquipSelected)
    {
        GameManager.instance.player.Equipment(equipSelected);
        Equip unEquip = myEquips.FirstOrDefault(tmp => tmp.type == equipSelected.equipmentType && tmp.used);
        if (unEquip != null)
        {
            unEquip.used = false;
        }


        myEquipSelected.used = true;

        SaveManager.instance.SaveGame();
        SetupBtn(equipSelected);
    }

    private void UnEquip(ref Equip myEquipped)
    {
        GameManager.instance.player.equipController.UnEquipment(myEquipped);
        myEquipped.used = false;
        SaveManager.instance.SaveGame();
        SetupBtn(equipSelected);

    }

    private void BuyOneTime()
    {
        myEquips.Add(new Equip(equipSelected.equipName, false, equipSelected.equipmentType, true));

        SaveManager.instance.SaveGame();
        SetupBtn(equipSelected);
    }
}
