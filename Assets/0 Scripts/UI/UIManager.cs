using System.Collections;
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

    // private
    private Animator cameraAnimator;
    private int coin;
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


        DisplayCoin();
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
    }
    private void DisplayShopSkin()
    {
        shopSkinPannel.SetActive(true);
        startPannel.SetActive(false);
    }

    // btn
    private void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    private void OnDestroy()
    {
        Observer.RemoveObserver("PlayerDead", PlayerLose);
        Observer.RemoveObserver("PlayerWin", PlayerWin);
    }

    public void DisplayCoin()
    {
        coinTxt.text = coin.ToString();
    }

    Equip test;

    public void LoadData(GameData gameData)
    {
        coin = gameData.coin;
    }

    public void SaveData(ref GameData gameData)
    {
    }

}
