using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkinController : MonoBehaviour
{
    [SerializeField] private Button hairBtn;
    //[SerializeField] private Button pantBtn;
    //[SerializeField] private Button shieldBtn;
    //[SerializeField] private Button setBtn;

    public Button buyEquipBtn;
    public Button buySelectBtn;
    public TextMeshProUGUI descriptEquipTxt;
    // Start is called before the first frame update
    void Start()
    {
        hairBtn.GetComponent<UI_SkinList>().DisplayEquipInShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }

        if (_menu != null)
            _menu.SetActive(true);
    }
}
