using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ISaveManager
{
    public static UIManager instance;
    [SerializeField] private GameObject coinImg;

    [Header("Pannel")]
    [SerializeField] private GameObject instructionImg;
    [SerializeField] private GameObject startPannel;
    [SerializeField] private GameObject ingamePannel;
    [SerializeField] private GameObject settingPannel;
    [SerializeField] private GameObject shopWeaponPannel;
    [SerializeField] private GameObject shopSkinPannel;
    [SerializeField] private GameObject revivalPanel;
    [SerializeField] private GameObject endgamePanel;

    [Header("Button")]
    [SerializeField] private Button shopWeaponBtn;
    [SerializeField] private Button shopSkinBtn;

    [SerializeField] private Button closeRevivalBtn;


    //setting
    [SerializeField] private Button settingInGameBtn;
    [SerializeField] private Button homeSettingBtn;
    [SerializeField] private Button continueSettingBtn;

    [Header("ZombieCity")]
    [SerializeField] private Button PlayZombieCity;

    [Header("Joystick")]
    [SerializeField] private GameObject joystick;

    [Header("Coin")]
    [SerializeField] private TextMeshProUGUI coinTxt;

    [Header("Player")]
    public Player player;
    [Header("Main Camera")]
    [SerializeField] private CameraFollow cameraFollow;

    public UI_EndGameController endgameController;
    private List<Equip> myEquips = new List<Equip>();

    // private
    private Animator cameraAnimator;
    private Animator uiAnimator;
    public int coin;

    // equipped
    public string weaponName { get; private set; }
    public string shieldName { get; private set; }
    public string pantName { get; private set; }
    public string hairName { get; private set; }
    public string setName { get; private set; }

    public UI_ShopWPCT shopWeapon;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        startPannel.SetActive(true);
        shopSkinPannel.SetActive(true);
        shopWeaponPannel.SetActive(true);
    }
    private void Start()
    {
        cameraAnimator = cameraFollow.GetComponent<Animator>();
        uiAnimator = GetComponent<Animator>();

        InitializedPannel();

        Observer.AddObserver("PlayerTakeDamage", DisplayRevivalPanel);
        Observer.AddObserver("PlayerWin", DisplayEndgame);

        shopWeaponBtn.onClick.AddListener(DisplayShopWeapon);
        //shopSkinBtn.onClick.AddListener(DisplayShopSkin);

        settingInGameBtn.onClick.AddListener(SwitchDisplaySetting);
        homeSettingBtn.onClick.AddListener(Home);
        continueSettingBtn.onClick.AddListener(SwitchDisplaySetting);

        //ZC
        PlayZombieCity.onClick.AddListener(PlayZombieCityMode);

        closeRevivalBtn.onClick.AddListener(DisplayEndgame);
    }

    public void InitializedPannel()
    {
        coinImg.SetActive(true);
        startPannel.SetActive(true);
        instructionImg.SetActive(false);
        ingamePannel.SetActive(false);
        settingPannel.SetActive(false);
        shopWeaponPannel.SetActive(false);
        shopSkinPannel.SetActive(false);
        revivalPanel.SetActive(false);
        endgamePanel.SetActive(false);

        player.gameObject.SetActive(true);
        player.ChangeIdleState();

        uiAnimator.SetTrigger("test3");

        SaveManager.instance.LoadComponent(this);
        EquipThePlayer();

        DisplayCoin(coin);
    }


    public void PlayBtn()
    {
        coinImg.SetActive(false);

        ingamePannel.SetActive(true);
        startPannel.SetActive(false);
        instructionImg.SetActive(true);
        cameraAnimator.SetTrigger("ShrinkCamera");
        Observer.Notify("play");
    }

    private void DisplayRevivalPanel()
    {

        revivalPanel.SetActive(true);
        joystick.SetActive(false);
    }

    #region button listeners

    private void SwitchDisplaySetting()
    {
        if (!settingPannel.activeSelf)
        {
            uiAnimator.SetTrigger("test2");
            ingamePannel.SetActive(false);
            settingPannel.SetActive(true);
        }
        else
        {
            uiAnimator.SetTrigger("test1");

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

    // add onclick in inspector
    public void DisplayShopSkin()
    {
        player.stateMachine.ChangeState(player.danceSkinState);
        shopSkinPannel.SetActive(true);
        startPannel.SetActive(false);
    }

    public void PlayerRevial()
    {
        // add ADs
        GameManager.instance.player.PlayerRevival();
        if (revivalPanel.activeSelf)
        {
            revivalPanel.SetActive(false);
        }

        if (!joystick.activeSelf)
        {
            joystick.SetActive(true);
        }

    }

    public void PlayerRevialByCoin()
    {
        if (coin < 150)
        {
            Debug.LogWarning("haven't money");
            return;
        }
        else
        {
            coin -= 150;
            SaveManager.instance.SaveComponent(this);
            PlayerRevial();

        }
    }

    public void DisplayEndgame()
    {
        revivalPanel.SetActive(false);
        endgamePanel.SetActive(true);
    }

    // btn
    public void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    public void EquipThePlayer()
    {
        weaponName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.WEAPON && tmp.used)?.equipName;
        player.equipController.PlayerEquipped(weaponName);
        hairName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.HAIR && tmp.used)?.equipName;
        pantName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.PANT && tmp.used)?.equipName;
        shieldName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.SHIELD && tmp.used)?.equipName;

        setName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.SET && tmp.used)?.equipName;
        if (setName != null)
        {
            player.equipController.EquipSet(setName);
        }
        else
        {
            player.equipController.UnEquipSet();

            if (hairName != null)
            {
                player.equipController.PlayerEquipped(hairName);
            }
            else
            {
                player.equipController.RemoveHairStat();
            }

            if (pantName != null)
            {
                player.equipController.PlayerEquipped(pantName);
            }
            else
            {
                player.equipController.RemovePantStat();
            }
            player.equipController.PlayerEquipped(shieldName);
        }
        player.CreateWeaponInHand();
    }


    public void DisplayCoin(int coin)
    {
        coinTxt.text = coin.ToString();
    }

    public void ChangePlayerName(string name)
    {
        player.characterName = name;
    }

    private void PlayZombieCityMode()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadData(GameData gameData)
    {
        this.coin = gameData.coin;
        myEquips = gameData.myEquips;

    }

    public void SaveData(ref GameData gameData)
    {
        gameData.coin = this.coin;
    }

    private void OnDestroy()
    {
        Observer.RemoveObserver("PlayerTakeDamage", DisplayRevivalPanel);
        Observer.RemoveObserver("PlayerWin", DisplayEndgame);

    }
}
