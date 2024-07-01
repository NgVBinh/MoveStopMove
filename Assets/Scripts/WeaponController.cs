using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class WeaponController : MonoBehaviour
{
    private Rigidbody rb;


    // Start is called before the first frame update
    void Awake()
    {
        rb=GetComponent<Rigidbody>();    
    }

    public void AddForceToWeapon(Vector3 dir,float force)
    {
        rb.isKinematic = false;
        rb.velocity = (dir*force);
    }
}
