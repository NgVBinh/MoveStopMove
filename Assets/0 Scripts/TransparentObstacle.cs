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

    public bool isBaseColor;
    private void Start()
    {
        render = GetComponent<Renderer>();
        originalMaterial = render.material;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < transparentDistance)
        {
            Debug.Log("làm mờ");
            render.material = transparentMat;
            isBaseColor = false;
        }

    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < transparentDistance)
        {
            if (distanceToPlayer < transparentDistance)
            {
                if (!isBaseColor) return;

                //Debug.Log("làm mờ");
                render.material = transparentMat;
                isBaseColor = false;
            }

            else
            {
                if (isBaseColor) return;
                else
                {
                    if (isBaseColor) return;

                    //Debug.Log("hủy làm mờ");
                    render.material = originalMaterial;
                    isBaseColor = true;
                }
            }

        }
    }
}
