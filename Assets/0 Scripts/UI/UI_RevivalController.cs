using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_RevivalController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownTxt;
    [SerializeField] private int countDown;
    private float time;

    private void OnEnable()
    {
        time = countDown;

    }

    private void Update()
    {
        time -= Time.deltaTime;
        countDownTxt.text = Mathf.RoundToInt(time).ToString();

        if(time < 0)
        {
            UIManager.instance.DisplayEndgame();
        }
    }
}
