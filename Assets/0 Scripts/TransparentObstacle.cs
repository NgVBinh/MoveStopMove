using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObstacle : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float transparentDistance = 8f;
    [SerializeField] private Material transparentMat;

    private Renderer render;
    private Material originalMaterial;

    private bool isBaseColor;

    private float distance;
    private void Start()
    {
        render = GetComponent<Renderer>();
        originalMaterial = render.material;

        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < transparentDistance)
        {
            render.material = transparentMat;
            isBaseColor = false;
        }

    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < transparentDistance)
        {
            if (!isBaseColor) return;

            //Debug.Log("làm mờ");
            render.material = transparentMat;
            isBaseColor = false;
        }

        else
        {
            if (isBaseColor) return;

            //Debug.Log("hủy làm mờ");
            render.material = originalMaterial;
            isBaseColor = true;
        }

    }
}
