using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingController : MonoBehaviour
{
    public bool activeVolume;

    [SerializeField] private Image onVolumeImg;
    [SerializeField] private Image offVolumeImg;

    public bool activeVibration;

    [SerializeField] private Image onVibraImg;
    [SerializeField] private Image offVibraImg;
    public void SwitchStateVolume()
    {
        if(activeVolume)
        {
            onVolumeImg.gameObject.SetActive(!activeVolume);
            offVolumeImg.gameObject.SetActive(activeVolume);
            activeVolume = false;
        }
        else
        {
            onVolumeImg.gameObject.SetActive(!activeVolume);
            offVolumeImg.gameObject.SetActive(activeVolume);
            activeVolume = true;
        }
    }

    public void SwitchStateVibration()
    {
        if (activeVibration)
        {
            onVibraImg.gameObject.SetActive(!activeVibration);
            offVibraImg.gameObject.SetActive(activeVibration);
            activeVibration = false;
        }
        else
        {
            onVibraImg.gameObject.SetActive(!activeVibration);
            offVibraImg.gameObject.SetActive(activeVibration);
            activeVibration = true;
        }
    }
}
