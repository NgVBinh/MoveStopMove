using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    [SerializeField] private float moveSpeed;
    private Vector3 moveDir;
    public float rotateSpeed;

    private bool canMove=true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove) return;
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");
     
        if(moveDir!= Vector3.zero)
        {
            MoveHandle(moveDir*Time.deltaTime*moveSpeed);
            RotationHandle();
            animator.SetBool("IsMove", true);
            animator.SetBool("IsIdle", false);
        }
        else
        {
            PlayerIdle();
        }
    }

    private void MoveHandle(Vector3 moveDir)
    {
        rb.velocity =new Vector3(moveDir.x,rb.velocity.y,moveDir.z);
    }

    private void RotationHandle()
    {
       transform.forward = Vector3.RotateTowards(transform.forward,rb.velocity,rotateSpeed,Time.deltaTime);
    }

    public void PlayerIdle()
    {
        rb.velocity = Vector3.zero;
        animator.SetBool("IsIdle", true);
        animator.SetBool("IsMove",false);
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
