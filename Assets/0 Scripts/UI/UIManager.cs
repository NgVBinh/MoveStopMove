using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject instructionImg;
    [SerializeField] private GameObject startPannel;
    [SerializeField] private GameObject ingamePannel;

    [SerializeField] private CameraFollow cameraFollow;
    private Animator cameraAnimator;
    private void Start()
    {
        cameraAnimator = cameraFollow.GetComponent<Animator>();
    }

    public void PlayBtn()
    {
        ingamePannel.SetActive(true);
        startPannel.SetActive(false);
        instructionImg.SetActive(true);
        cameraAnimator.SetTrigger("ShrinkCamera");
        Observer.Notify("play");
    }


}
