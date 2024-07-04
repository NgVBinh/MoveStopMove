using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 dir; // Hướng di chuyển

    void Update()
    {
        rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.z);

        // Nếu có di chuyển
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }


    //
    //public Vector3 targetDirection; // Vector chỉ hướng

    //void Update()
    //{
    //    // Tính toán góc quay trên trục Y cần thiết để hướng theo targetDirection
    //    Vector3 direction = new Vector3(targetDirection.x, 0, targetDirection.z);

    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);

    //        // Lấy góc hiện tại của GameObject
    //        Vector3 currentRotation = transform.eulerAngles;

    //        // Chỉ cập nhật góc quay trên trục Y
    //        transform.rotation = Quaternion.Euler(currentRotation.x, targetRotation.eulerAngles.y, currentRotation.z);
    //    }
    //}
}
