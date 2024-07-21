using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZC_UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject preparePanel;
    public GameObject instrucPanel;
    public GameObject joystick;

    public Button homeBasicGame;
    public Button playBtn;
    public Button selectAbilityBtn;

    // prepare count down
    private bool canCountDown;
    [SerializeField] private TextMeshProUGUI countDownTxt;
    [SerializeField] private int countDown;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        homeBasicGame.onClick.AddListener(HomeBasicGame);
        playBtn.onClick.AddListener(Playgame);
        selectAbilityBtn.onClick.AddListener(Playgame);
    }

    // Update is called once per frame
    void Update()
    {
        if(canCountDown)
        {
            timer -= Time.deltaTime;
            countDownTxt.text = Mathf.RoundToInt(timer).ToString();

            if (timer < 0)
            {
                // display instruc
                instrucPanel.SetActive(true);
                joystick.SetActive(true);
                canCountDown = false;
            }
        }
    }

    public void Initialize()
    {
        startPanel.SetActive(true);
        joystick.SetActive(false);
        preparePanel.SetActive(false);
        instrucPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    private void Playgame()
    {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        preparePanel.SetActive(true );
        canCountDown = true;
        timer = countDown;
    }

    private void HomeBasicGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
