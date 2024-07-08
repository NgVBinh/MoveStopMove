using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObstacle : MonoBehaviour
{
    [SerializeField] private Player player;
<<<<<<< HEAD:Assets/0 Scripts/TransparentObstacle.cs
    [SerializeField] private float transparentDistance;
=======
    [SerializeField] private float transparentDistance = 8f;
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/TransparentObstacle.cs
    [SerializeField] private Material transparentMat;

    private Renderer render;
    private Material originalMaterial;

<<<<<<< HEAD:Assets/0 Scripts/TransparentObstacle.cs
    public bool isBaseColor;
=======
    private bool isBaseColor;
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/TransparentObstacle.cs
    private void Start()
    {
        render = GetComponent<Renderer>();
        originalMaterial = render.material;
<<<<<<< HEAD:Assets/0 Scripts/TransparentObstacle.cs

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < transparentDistance)
        {
            Debug.Log("làm mờ");
            render.material = transparentMat;
            isBaseColor = false;
        }

=======
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/TransparentObstacle.cs
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

<<<<<<< HEAD:Assets/0 Scripts/TransparentObstacle.cs
        if (distanceToPlayer < transparentDistance )
        {
            if (!isBaseColor) return;
=======
        if (distanceToPlayer < transparentDistance)
        {
            if (!isBaseColor) return;

>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/TransparentObstacle.cs
            //Debug.Log("làm mờ");
            render.material = transparentMat;
            isBaseColor = false;
        }
<<<<<<< HEAD:Assets/0 Scripts/TransparentObstacle.cs

        else
        {
            if (isBaseColor) return;
=======
        else
        {
            if (isBaseColor) return;

>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/TransparentObstacle.cs
            //Debug.Log("hủy làm mờ");
            render.material = originalMaterial;
            isBaseColor = true;
        }
    }

}
