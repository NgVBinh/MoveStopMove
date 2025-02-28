using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FollowArrow : MonoBehaviour
{
    private TargetIndicator arrow;

    private TextMeshProUGUI expTxt;
    public float offset;

    private Enemy enemyTarget;

    public void SetupLevelInforImg(TargetIndicator arrow, float offsetArrow, GameObject target, Color color)
    {
        expTxt = GetComponentInChildren<TextMeshProUGUI>();
        this.arrow = arrow;
        this.offsetArrow = offsetArrow;
        enemyTarget = target.GetComponent<Enemy>();
        enemyTarget.OnDeath += DeActiveMySelf;
        expTxt.text = enemyTarget.exp.ToString();

        GetComponent<Image>().color = color;
        enemyTarget.OnLevelUp += DisplayLevel;
    }
    private float offsetArrow;
    private RectTransform imageLevelInfor; // 
    private RectTransform canvasRectTransform;

    void Start()
    {
        imageLevelInfor = GetComponent<RectTransform>();
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 localPointA;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, arrow.transform.position, null, out localPointA);

        Vector2 newPosition = localPointA;

        float leftEdge = (-canvasRectTransform.rect.width / 2);
        float rightEdge = (canvasRectTransform.rect.width / 2);
        float bottomEdge = (-canvasRectTransform.rect.height / 2);
        float topEdge = (canvasRectTransform.rect.height / 2);


        if (newPosition.x - offsetArrow == leftEdge)
        {
            newPosition.x += offset; // Offset to the right
        }
        else if (newPosition.x + offsetArrow == rightEdge)
        {
            newPosition.x -= offset; // Offset to the left
        }

        if (newPosition.y - offsetArrow == bottomEdge)
        {
            newPosition.y += offset; // Offset upwards
        }
        else if (newPosition.y + offsetArrow == topEdge)
        {
            newPosition.y -= offset; // Offset downwards
        }

        // Convert local position back to world position
        Vector3 worldPosition = canvasRectTransform.TransformPoint(newPosition);

        // Set the position of image B
        imageLevelInfor.position = worldPosition;
    }


    private void DisplayLevel()
    {
        expTxt.text = enemyTarget.exp.ToString();
    }

    //private void OnDisable()
    //{
    //    enemyTarget.OnDeath -= DeActiveMySelf;
    //}

    private void DeActiveMySelf()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnDestroy()
    {
        enemyTarget.OnDeath -= DeActiveMySelf;
        enemyTarget.OnLevelUp -= DisplayLevel;

    }
}
