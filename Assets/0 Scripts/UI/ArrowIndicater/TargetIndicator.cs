using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private Image offScreenTargetIndicator;
    [SerializeField] private float outOfSightOffset = 20f;

    private float outOfSightOffest { get { return outOfSightOffset; } }

    private GameObject target;
    private Camera mainCamera;

    private RectTransform canvasRect;
    private RectTransform rectTransform;


    private ArrowTowardEnemys arrowTowardEnemys;
    private Image myImage;

    [SerializeField] private bool canRotate;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        myImage = GetComponent<Image>();
    }

    public void InitialTargetIndicator(GameObject target, Camera mainCamera, Canvas canvas, ArrowTowardEnemys arrowTowardEnemys)
    {
        this.target = target;
        this.mainCamera = mainCamera;
        canvasRect = canvas.GetComponent<RectTransform>();
        this.arrowTowardEnemys = arrowTowardEnemys;

        myImage.color = target.GetComponent<Enemy>().body.material.color;
        target.GetComponent<Enemy>().OnDeath += RemoveIndicator;
    }

    public void UpdateTargetIndicator()
    {
        SetIndicatorPos();
    }

    private void SetIndicatorPos()
    {
        Vector3 indicatorPos = mainCamera.WorldToScreenPoint(target.transform.position);

        if (indicatorPos.z < 0)
        {
            indicatorPos *= -1;
        }

        bool isOffScreen = indicatorPos.x < 0 || indicatorPos.x > Screen.width || indicatorPos.y < 0 || indicatorPos.y > Screen.height;
        if (isOffScreen)
        {
            indicatorPos = AdjustIndicatorPosition(indicatorPos);
            rectTransform.position = indicatorPos;
            TargetOutOffSight(true, indicatorPos);
        }
        else
        {
            TargetOutOffSight(false, indicatorPos);
        }
    }

    private Vector3 AdjustIndicatorPosition(Vector3 indicatorPos)
    {
        indicatorPos.z = 0;

        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2, canvasRect.rect.height / 2, 0) * canvasRect.localScale.x;
        indicatorPos -= canvasCenter;

        //float aspectRatio = canvasRect.rect.width / canvasRect.rect.height;

        float normalizedX = Mathf.Abs(indicatorPos.x) / (canvasRect.rect.width * 0.5f - outOfSightOffset);
        float normalizedY = Mathf.Abs(indicatorPos.y) / (canvasRect.rect.height * 0.5f - outOfSightOffset);

        if (normalizedX > normalizedY)
        {
            indicatorPos.x = Mathf.Sign(indicatorPos.x) * (canvasRect.rect.width * 0.5f - outOfSightOffset) * canvasRect.localScale.x;
            indicatorPos.y = indicatorPos.y / Mathf.Abs(indicatorPos.x) * (canvasRect.rect.width * 0.5f - outOfSightOffset) * canvasRect.localScale.x;
        }
        else
        {
            indicatorPos.y = Mathf.Sign(indicatorPos.y) * (canvasRect.rect.height * 0.5f - outOfSightOffset) * canvasRect.localScale.y;
            indicatorPos.x = indicatorPos.x / Mathf.Abs(indicatorPos.y) * (canvasRect.rect.height * 0.5f - outOfSightOffset) * canvasRect.localScale.y;
        }

        indicatorPos += canvasCenter;

        //Đảm bảo vị trí không vượt quá giới hạn của canvas
        indicatorPos.x = Mathf.Clamp(indicatorPos.x, outOfSightOffset, canvasRect.rect.width * canvasRect.localScale.x - outOfSightOffset);
        indicatorPos.y = Mathf.Clamp(indicatorPos.y, outOfSightOffset, canvasRect.rect.height * canvasRect.localScale.y - outOfSightOffset);

        return indicatorPos;
    }

    private void TargetOutOffSight(bool oos, Vector3 indicatorPos)
    {
        if (oos)
        {
            if (!offScreenTargetIndicator.gameObject.activeSelf)
            {
                offScreenTargetIndicator.gameObject.SetActive(true);
            }
            
            offScreenTargetIndicator.rectTransform.rotation = Quaternion.Euler(CalculateRotation(indicatorPos));
        }
        else
        {
            if (offScreenTargetIndicator.gameObject.activeSelf)
            {
                offScreenTargetIndicator.gameObject.SetActive(false);
            }
        }
    }

    private Vector3 CalculateRotation(Vector3 indicatorPos)
    {
        Vector3 canvasCenter = new Vector3(canvasRect.rect.width / 2, canvasRect.rect.height / 2, 0) * canvasRect.localScale.x;
        float angle = Vector3.SignedAngle(Vector3.up, indicatorPos - canvasCenter, Vector3.forward);
        return new Vector3(0, 0, angle);
    }

    public void RemoveIndicator()
    {
        arrowTowardEnemys.RemoveTargetIndicator(this);
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }


}
