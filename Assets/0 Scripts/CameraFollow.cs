using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private float cameraSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        Vector3 targetPos = new Vector3(target.transform.position.x,target.transform.position.y+offset.y,target.transform.position.z+offset.z);
        //transform.position = Vector3.MoveTowards(transform.position,targetPos , Time.deltaTime*cameraSpeed);
        //transform.position = targetPos;
        //transform.LookAt(target.transform);
        transform.position = targetPos;
    }

    public void SetOffset(float percent)
    {
        this.offset *=(1+percent/100);
    }
}
