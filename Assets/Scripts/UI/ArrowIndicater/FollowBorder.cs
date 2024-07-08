using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBorder : MonoBehaviour
{
    private RectTransform imageParent;
    private RectTransform imageArrow;
    public float offset = 10f; // Khoảng cách từ viền

    private void Start()
    {
        imageParent = transform.parent.GetComponent<RectTransform>();
        imageArrow = transform.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Lấy kích thước của image A
        Vector2 sizeA = imageParent.rect.size;

        // Lấy vị trí hiện tại của image B
        Vector2 positionB = imageArrow.anchoredPosition;

        // Tính toán vị trí mới của image B để nó bám vào viền của image A
        positionB.x = Mathf.Clamp(positionB.x, -sizeA.x / 2 + offset, sizeA.x / 2 - offset);
        positionB.y = Mathf.Clamp(positionB.y, -sizeA.y / 2 + offset, sizeA.y / 2 - offset);

        // Cập nhật vị trí của image B
        imageArrow.anchoredPosition = positionB;
    }
}
