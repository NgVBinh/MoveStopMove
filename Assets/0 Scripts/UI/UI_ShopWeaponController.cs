using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopWeaponController : MonoBehaviour
{
    [SerializeField] private List<EquipmentSO> weaponEquipment;
    [SerializeField] private Transform weapon;
    [SerializeField] private List<Transform> weaponColorChoose;

    [SerializeField] private Button rightBtn;
    [SerializeField] private Button leftBtn;

    [SerializeField] private TextMeshProUGUI weapponNameTxt;
    [SerializeField] private TextMeshProUGUI statsTxt;

    [SerializeField] private Button backBtn;

    public int weaponIndex;
    void Start()
    {
        DisplayWeapon(weaponIndex);
        rightBtn.onClick.AddListener(NextWeaponRight);
        leftBtn.onClick.AddListener(NextWeaponLeft);
        backBtn.onClick.AddListener(UIManager.instance.InitializedPannel);
    }

   private void DisplayWeapon(int index)
    {
        if (weapon.childCount > 0)
        {
            Destroy(weapon.GetChild(0).gameObject);
        }

        GameObject weaponDisplay = Instantiate(weaponEquipment[weaponIndex].prefab, weapon);
        weaponDisplay.GetComponent<WeaponController>().enabled = false;
        weaponDisplay.transform.localScale = Vector3.one * 10000;

        for (int i=0;i<weaponColorChoose.Count;i++)
        {
            if (weaponColorChoose[i].childCount > 0)
            {
                Destroy(weaponColorChoose[i].GetChild(0).gameObject);
            }

            GameObject weaponDisplayColor = Instantiate(weaponEquipment[weaponIndex].prefab, weaponColorChoose[i]);
            weaponDisplayColor.GetComponent<WeaponController>().enabled = false;
            weaponDisplayColor.GetComponent<MeshRenderer>().material = weaponEquipment[0].materials[i];
            weaponDisplayColor.transform.localScale = Vector3.one * 4000;
        }


        statsTxt.text = weaponEquipment[weaponIndex].description;
        weapponNameTxt.text = weaponEquipment[weaponIndex].equipName;

    }

    private void NextWeaponLeft()
    {
        weaponIndex--;
        weaponIndex=Mathf.Clamp(weaponIndex,0, weaponEquipment.Count-1);
        DisplayWeapon(weaponIndex);
    }

    private void NextWeaponRight()
    {
        weaponIndex++;
        weaponIndex=Mathf.Clamp(weaponIndex, 0, weaponEquipment.Count - 1);
        DisplayWeapon(weaponIndex);
    }
}
