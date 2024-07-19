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


    [SerializeField] private Button myBtn;
    [Header("Use one Time")]
    [SerializeField] private Button oneTimeBtn;
    [SerializeField] private TextMeshProUGUI useOneTimeTxt;
    private void Awake()
    {
    }

    private void OnEnable()
    {
        //SaveManager.instance.LoadComponent(this);
    }

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

        // lay ra equip cung ten neu co
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
                else
                {
                    myBtn.onClick.RemoveAllListeners();
                    myBtn.onClick.AddListener(() => UnEquip(myEquipSelected));
                    textBtn.text = "UnEquip";
                    coinImage.gameObject.SetActive(false);
                }


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
                myBtn.onClick.AddListener(() => SelectEquipBtn(myEquipSelected));

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
        myCoin -= equipSelected.cost;
        myEquips.Add(new Equip(equipSelected.equipName, true, equipSelected.equipmentType));

        // bo trang bi cu
        Equip unEquip = myEquips.FirstOrDefault(tmp => tmp.type == equipSelected.equipmentType && tmp.used);
        if (unEquip != null)
        {
            unEquip.used = false;
        }
        SaveManager.instance.SaveComponent(this);

        // check
        if (equipSelected.equipmentType == EquipmentType.WEAPON)
        {
            UI_ShopWPCT shopWeaponScript= GetComponentInParent<UI_ShopWPCT>();
            if (PlayerPrefs.GetInt("currentIndex") != shopWeaponScript.weaponIndex)
            {
                shopWeaponScript.SaveIndex();
                shopWeaponScript.DisplayWeapon(PlayerPrefs.GetInt("currentIndex"));

                myEquips.Add(new Equip(shopWeaponScript.weapons[PlayerPrefs.GetInt("currentIndex")].weaponInShops[2].equipName, false, equipSelected.equipmentType));
                myEquips.Add(new Equip(shopWeaponScript.weapons[PlayerPrefs.GetInt("currentIndex")].weaponInShops[1].equipName, false, equipSelected.equipmentType));

            }
        }

        SetupBtn(equipSelected);

        UIManager.instance.DisplayCoin(myCoin);
    }

    private void SelectEquipBtn(Equip myEquipSelected)
    {
        GameManager.instance.player.Equipment(equipSelected);
        Equip unEquip = myEquips.FirstOrDefault(tmp => tmp.type == equipSelected.equipmentType && tmp.used);
        if (unEquip != null)
        {
            unEquip.used = false;
        }
        myEquipSelected.used = true;

        GetComponentInParent<UI_ShopWPCT>()?.SaveIndex();
        //GetComponentInParent<UI_ShopWPCT>()?.BackStartPannel();

        SaveManager.instance.SaveComponent(this);
        SetupBtn(equipSelected);
    }

    private void UnEquip(Equip myEquipped)
    {
        GameManager.instance.player.equipController.UnEquipment(myEquipped);
        myEquipped.used = false;
        SaveManager.instance.SaveComponent(this);
        SetupBtn(equipSelected);

    }

    private void BuyOneTime()
    {
        myEquips.Add(new Equip(equipSelected.equipName, false, equipSelected.equipmentType, true));

        SaveManager.instance.SaveComponent(this);
        SetupBtn(equipSelected);

    }
}
