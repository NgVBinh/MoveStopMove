using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class WeaponInShop
{
    public int index;
    public string weaponName;
    public List<EquipmentSO> weaponInShops;
}

public class UI_ShopWPCT : MonoBehaviour, ISaveManager
{
    public Material testm;
    public List<WeaponInShop> weapons = new List<WeaponInShop>();


    public Transform weaponCenter;
    [SerializeField] private Transform LockTransform;

    [SerializeField] private Transform chooseSkinGroup;

    [SerializeField] private Button rightBtn;
    [SerializeField] private Button leftBtn;
    [SerializeField] private Button buyBtn;
    //[SerializeField] private Button selectBtn;

    [SerializeField] private Button backBtn;

    [SerializeField] private GameObject equipInShop_UI;

    public Button buy;
    public TextMeshProUGUI description;

    public int weaponIndex;

    public GameObject chooseColorBtn;
    public GameObject gridChooseColor;
    public GameObject customTxt;

    public List<Image> imagesColor;
    public Button btnA;
    public Button btnB;
    public Button btnC;
    //public Material materialA;
    //public Material materialB;
    //public Material materialC;
    public List<Material> materialsChoose = new List<Material>();
    private int selectedID;

    public EquipmentSO currentEquipSOSelect;
    private bool isChooseColor;

    private List<Equip> myEquip = new List<Equip>();
    //private GameObject equipPref;
    public void SetSO(EquipmentSO currentEquipSO)
    {
        currentEquipSOSelect = currentEquipSO;
    }
    private void OnEnable()
    {
        weaponIndex = PlayerPrefs.GetInt("currentIndex");
        DisplayWeapon(weaponIndex);

    }

    void Start()
    {
        rightBtn.onClick.AddListener(NextWeaponRight);
        leftBtn.onClick.AddListener(NextWeaponLeft);
        backBtn.onClick.AddListener(BackStartPannel);

        foreach (Image image in imagesColor)
        {
            AddEventTrigger(image.gameObject, image);
        }

        //weapons[0].weaponInShops[0].prefab.GetComponent<MeshRenderer>().material = testm;

        btnA.onClick.AddListener(() => SelectMaterial(0));
        btnB.onClick.AddListener(() => SelectMaterial(1));
        btnC.onClick.AddListener(() => SelectMaterial(2));
    }

    public void DisplayWeapon(int index)
    {
        if (weaponIndex > PlayerPrefs.GetInt("maxIndex"))
        {
            if (weaponIndex > PlayerPrefs.GetInt("maxIndex") + 1)
            {
                chooseSkinGroup.gameObject.SetActive(false);
                LockTransform.gameObject.SetActive(true);
                buy.gameObject.SetActive(false);
            }
            else
            {
                chooseSkinGroup.gameObject.SetActive(false);
                LockTransform.gameObject.SetActive(true);
                buy.gameObject.SetActive(true);
            }

            customTxt.SetActive(false);
            chooseSkinGroup.gameObject.SetActive(false);
            LockTransform.gameObject.SetActive(true);

            // tao vu khi o giua
            buy.GetComponent<UI_BuyBtnController>().SetupBtn(weapons[index].weaponInShops[0]);
            description.text = weapons[index].weaponInShops[0].description;

            if (weaponCenter.childCount > 0)
            {
                Destroy(weaponCenter.GetChild(0).gameObject);

            }
            GameObject equipPref = Instantiate(weapons[index].weaponInShops[0].prefab, weaponCenter);
            equipPref.GetComponent<WeaponController>().SetWeaponOfCharacter(true);
            equipPref.transform.GetChild(0).localPosition = Vector3.zero;

            //equipPref.GetComponentInChildren<MeshRenderer>().materials = weapons[index].weaponInShops[0].materials.ToArray();
            equipPref.transform.localScale = Vector3.one * 200;

        }
        else
        {
            chooseSkinGroup.gameObject.SetActive(true);
            LockTransform.gameObject.SetActive(false);
            customTxt.SetActive(true);

            if (chooseSkinGroup.childCount > 0)
            {
                for (int i = 0; i < chooseSkinGroup.childCount; i++)
                {
                    Destroy(chooseSkinGroup.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < weapons[index].weaponInShops.Count; i++)
            {
                GameObject equipInShop = Instantiate(equipInShop_UI, chooseSkinGroup);
                equipInShop.GetComponentInChildren<UI_EquipInShop>().SetupEquipInShop(weapons[index].weaponInShops[i]);
                if (i == 0)
                {
                    equipInShop.GetComponentInChildren<UI_EquipInShop>().SetupEquipInShop(weapons[index].weaponInShops[i], true);

                }

            }

            if (!isChooseColor)
            {
                EquipmentSO equip = weapons[weaponIndex].weaponInShops.FirstOrDefault(a => a.equipName.Equals(UIManager.instance.weaponName));
                if (equip != null)
                {
                    GetComponentInChildren<UI_EquipInShop>()?.SetFirstSkin(equip);
                }
                else
                {
                    GetComponentInChildren<UI_EquipInShop>()?.SetFirstSkin(weapons[weaponIndex].weaponInShops[0]);

                }
            }

            else
            {
                GetComponentInChildren<UI_EquipInShop>()?.SetFirstSkin(weapons[weaponIndex].weaponInShops[0]);

            }


        }
    }

    private void NextWeaponLeft()
    {
        weaponIndex--;
        weaponIndex = Mathf.Clamp(weaponIndex, 0, weapons.Count - 1);
        DisplayWeapon(weaponIndex);

        DisplayChooseColor(false);
        selectedID = 0;
    }

    private void NextWeaponRight()
    {
        weaponIndex++;
        weaponIndex = Mathf.Clamp(weaponIndex, 0, weapons.Count - 1);
        DisplayWeapon(weaponIndex);
        DisplayChooseColor(false);
        selectedID = 0;

    }

    public void SaveIndex()
    {
        PlayerPrefs.SetInt("currentIndex", weaponIndex);
        if (weaponIndex > PlayerPrefs.GetInt("maxIndex"))
            PlayerPrefs.SetInt("maxIndex", weaponIndex);
    }

    public void BackStartPannel()
    {
        UIManager.instance.InitializedPannel();
    }
    public void DisplayChooseColor(bool canDisplay)
    {
        RectTransform rectBuyBtn = buy.GetComponent<RectTransform>();
        Vector3 newBuyPos = rectBuyBtn.localPosition;

        if (canDisplay)
        {
            isChooseColor = true;
            //selectBtn.gameObject.SetActive(true) ;
            chooseColorBtn.SetActive(true);
            gridChooseColor.SetActive(true);

            newBuyPos.y = -800;
            rectBuyBtn.localPosition = newBuyPos;
            // hien thi cac nut chon mau
            for (int i = 0; i < chooseColorBtn.transform.childCount; i++)
            {
                chooseColorBtn.transform.GetChild(i).gameObject.SetActive(false);
            }

            Debug.Log(currentEquipSOSelect.materials.Count);
            for (int i = 0; i < currentEquipSOSelect.materials.Count; i++)
            {
                chooseColorBtn.transform.GetChild(i).gameObject.SetActive(true);
                materialsChoose = weapons[weaponIndex].weaponInShops[0].materials;
            }
        }

        else
        {
            isChooseColor = false;
            //selectBtn.gameObject.SetActive(false) ;
            chooseColorBtn.SetActive(false);
            gridChooseColor.SetActive(false);

            newBuyPos.y = -500;
            rectBuyBtn.localPosition = newBuyPos;


        }

    }

    #region choose color

    private void AddEventTrigger(GameObject target, Image image)
    {
        EventTrigger trigger = target.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = target.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnImageClick(image); });

        trigger.triggers.Add(entry);
    }

    private void OnImageClick(Image image)
    {
        Material material = image.material;
        if (material != null)
        {
            switch (selectedID)
            {
                case 0:
                    materialsChoose[0] = material;
                    weapons[weaponIndex].weaponInShops[0].prefab.GetComponentInChildren<MeshRenderer>().materials = materialsChoose.ToArray();
                    //equipPref.GetComponent<MeshRenderer>().materials = materialsChoose.ToArray();
                    DisplayWeapon(weaponIndex);
                    break;
                case 1:
                    materialsChoose[1] = material;
                    weapons[weaponIndex].weaponInShops[0].prefab.GetComponentInChildren<MeshRenderer>().materials = materialsChoose.ToArray();
                    DisplayWeapon(weaponIndex);


                    break;
                case 2:
                    materialsChoose[2] = material;
                    weapons[weaponIndex].weaponInShops[0].prefab.GetComponentInChildren<MeshRenderer>().materials = materialsChoose.ToArray();
                    DisplayWeapon(weaponIndex);


                    break;

            }
        }
        else
        {
            Debug.LogWarning("Image does not have a material assigned.");
        }
    }
    private void SelectMaterial(int id)
    {
        selectedID = id;
    }

    public void LoadData(GameData gameData)
    {
        myEquip = gameData.myEquips;
    }

    public void SaveData(ref GameData gameData)
    {

    }

    #endregion
}
