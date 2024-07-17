using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndGameController : MonoBehaviour, ISaveManager
{
    [Header("End Game Panel")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;

    [Header("Button")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button tripleReward;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private TextMeshProUGUI killerNameTxt;
    [SerializeField] private TextMeshProUGUI rewardTxt;

    private string killer;
    private int rank;

    private Color killerColor;

    public int coin;
    public int reward;

    public List<Equip> equips = new List<Equip>();
    public void SetupEndLose(string killerName, int rank, Color killerColor)
    {
        killer = killerName;
        this.rank = rank;
        this.killerColor = killerColor;

    }
    private void Start()
    {
        SaveManager.instance.LoadComponent(this);
        reward = (GameManager.instance.enemyCount - rank + 1);


        if (GameManager.instance.isWin)
        {
            reward *= 2;

            losePanel.SetActive(false);
            winPanel.SetActive(true);

            //display
            continueBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Tap to Zone 2";
            rewardTxt.text = reward.ToString();
        }
        else
        {

            losePanel.SetActive(true);
            winPanel.SetActive(false);
            continueBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Tap to countinue";

            //display
            killerNameTxt.text = killer;
            killerNameTxt.color = killerColor;
            rankTxt.text = "#" + rank.ToString();
            rewardTxt.text = reward.ToString();
        }

        CheckEquipOneTime();

        continueBtn.onClick.AddListener(ContinueButton);
        tripleReward.onClick.AddListener(TripleReward);
    }

    private void TripleReward()
    {
        reward *= 3;
        SaveManager.instance.SaveComponent(this);
        UIManager.instance.Home();
    }

    private void ContinueButton()
    {
        SaveManager.instance.SaveComponent(this);
        UIManager.instance.Home();
    }

    public void LoadData(GameData gameData)
    {
        coin = gameData.coin;
        equips = gameData.myEquips;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.coin = coin+reward;
        gameData.myEquips = equips;
    }

    private void CheckEquipOneTime()
    {
        IEnumerable<Equip> equipsOneTime = equips.Where(x => x.used && x.isUesOneTime);
        List<Equip> oneTimes = equipsOneTime.ToList();
        foreach (Equip equip in oneTimes)
        {
            equips.Remove(equip);
        }
    }
}
