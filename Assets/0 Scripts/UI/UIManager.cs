using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour,ISaveManager
{
    public static UIManager instance;

    [Header("Pannel")]
    [SerializeField] private GameObject instructionImg;
    [SerializeField] private GameObject startPannel;
    [SerializeField] private GameObject ingamePannel;
    [SerializeField] private GameObject settingPannel;
    [SerializeField] private GameObject shopWeaponPannel;
    [SerializeField] private GameObject shopSkinPannel;

    [Header("End Game")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject joystick;

    [Header("Button")]

    [SerializeField] private Button shopWeaponBtn;
    [SerializeField] private Button shopSkinBtn;
    //endgame
    [SerializeField] private Button playerRevivalBtn;
    [SerializeField] private Button homeLoseBtn;
    [SerializeField] private Button homeWinBtn;

    //setting
    [SerializeField] private Button settingInGameBtn;
    [SerializeField] private Button homeSettingBtn;
    [SerializeField] private Button continueSettingBtn;

    [Header("Coin")]
    [SerializeField] private TextMeshProUGUI coinTxt;

    [Header("Main Camera")]
    [SerializeField] private CameraFollow cameraFollow;

    public Player player;

    private List<Equip> myEquips = new List<Equip>();

    // private
    private Animator cameraAnimator;
    public int coin;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    private void Start()
    {
        cameraAnimator = cameraFollow.GetComponent<Animator>();

        InitializedPannel();

        Observer.AddObserver("PlayerDead", PlayerLose);
        Observer.AddObserver("PlayerWin", PlayerWin);

        shopWeaponBtn.onClick.AddListener(DisplayShopWeapon);
        shopSkinBtn.onClick.AddListener(DisplayShopSkin);

        playerRevivalBtn.onClick.AddListener(PlayerRevial);
        homeLoseBtn.onClick.AddListener(Home);
        homeWinBtn.onClick.AddListener(Home);

        settingInGameBtn.onClick.AddListener(SwitchDisplaySetting);
        homeSettingBtn.onClick.AddListener(Home);
        continueSettingBtn.onClick.AddListener(SwitchDisplaySetting);
    }

    public void InitializedPannel()
    {
        startPannel.SetActive(true);
        joystick.SetActive(false);
        instructionImg.SetActive(false);
        ingamePannel.SetActive(false);
        settingPannel.SetActive(false);
        shopWeaponPannel.SetActive(false);
        shopSkinPannel.SetActive(false);

        player.gameObject.SetActive(true);
        player.ChangeIdleState();

        SaveManager.instance.LoadGame();
        DisplayCoin();
    }
   

    public void PlayBtn()
    {
        ingamePannel.SetActive(true);
        joystick.SetActive(true);
        startPannel.SetActive(false);
        instructionImg.SetActive(true);
        cameraAnimator.SetTrigger("ShrinkCamera");
        Observer.Notify("play");
    }

    void PlayerLose()
    {
        losePanel.SetActive(true);
        joystick.SetActive(false);

    }
    void PlayerWin()
    {
        winPanel.SetActive(true);
        joystick.SetActive(false);

    }

    #region button listeners
    private void PlayerRevial()
    {
        GameManager.instance.player.ChangeIdleState();
        if (losePanel.activeSelf)
        {
            losePanel.SetActive(false);
        }

        if (!joystick.activeSelf)
        {
            joystick.SetActive(true);
        }

    }
    private void SwitchDisplaySetting()
    {
        if (!settingPannel.activeSelf)
        {
            ingamePannel.SetActive(false);
            settingPannel.SetActive(true);
        }
        else
        {
            ingamePannel.SetActive(true);
            settingPannel.SetActive(false);
        }
    }

    private void DisplayShopWeapon()
    {
        shopWeaponPannel.SetActive(true);
        startPannel.SetActive(false);
        player.gameObject.SetActive(false);
    }
    private void DisplayShopSkin()
    {
        player.stateMachine.ChangeState(player.danceSkinState);
        shopSkinPannel.SetActive(true);
        startPannel.SetActive(false);
    }

    // btn
    private void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    public void EquipThePlayer()
    {
        string weaponName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.WEAPON && tmp.used)?.equipName;
        player.equipController.PlayerEquipped(weaponName,player);

        string hairName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.HAIR && tmp.used)?.equipName;
        player.equipController.PlayerEquipped(hairName, player);

        string shieldName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.SHIELD && tmp.used)?.equipName;
        player.equipController.PlayerEquipped(shieldName, player);

        string pantName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.PANT && tmp.used)?.equipName;
        player.equipController.PlayerEquipped(pantName, player);

        player.CreateWeaponInHand();
    }

    private void OnDestroy()
    {
        Observer.RemoveObserver("PlayerDead", PlayerLose);
        Observer.RemoveObserver("PlayerWin", PlayerWin);
    }

    public void DisplayCoin()
    {
        coinTxt.text = coin.ToString();
    }
    


    public void LoadData(GameData gameData)
    {
        this.coin = gameData.coin;
        myEquips = gameData.myEquips;

        EquipThePlayer();


    }

    public void SaveData(ref GameData gameData)
    {
        gameData.coin = this.coin;
    }

}
