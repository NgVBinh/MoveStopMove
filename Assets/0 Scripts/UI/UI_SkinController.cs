using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkinController : MonoBehaviour
{
    [SerializeField] private Button hairBtn;

    public Button buyEquipBtn;
    public TextMeshProUGUI descriptEquipTxt;

    [SerializeField] private Button closeBtn;
    // Start is called before the first frame update
    void Start()
    {
        //hairBtn.GetComponent<UI_SkinList>().DisplayEquipInShop();
        closeBtn.onClick.AddListener(UIManager.instance.InitializedPannel);
    }


}
