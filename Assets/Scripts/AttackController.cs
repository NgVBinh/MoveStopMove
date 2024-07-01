using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Transform weaponContain;
    public Vector3 dir;
    public float force;

    private PlayerMoveController moveController;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        moveController = GetComponent<PlayerMoveController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))// dau vao de nem vu khi
        {
            StartCoroutine(AttackCorountine(1.1f,0.3f));
        }
    }

    IEnumerator AttackCorountine(float delay,float attackTimer)
    {
        Debug.Log("nem");
        moveController.PlayerIdle();
        moveController.SetCanMove(false);
        animator.SetBool("IsAttack", true);

        yield return new WaitForSeconds(attackTimer);
        // dieu khien weapon
        dir = PlayerManager.instance.transform.forward;
        GameObject weapon = weaponContain.GetChild(0).gameObject;
        weapon.transform.parent = null;
        weapon.GetComponent<WeaponController>()?.AddForceToWeapon(dir.normalized, force);

        yield return new WaitForSeconds(delay);

        animator.SetBool("IsAttack", false);
        moveController.SetCanMove(true);
    }
}
