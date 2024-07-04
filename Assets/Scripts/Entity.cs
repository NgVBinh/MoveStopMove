using System.Collections;
using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody rb { get; private set; }


    [Header("Attack")]
    [SerializeField] private string weaponName;
    public float attackCooldown;
    [SerializeField] private Transform attackPoint;
    public float attackDelay;
    public float attackRange;
    public float forceThrow;
    private float attackTimer;

    [Header("Level ui")]
    [SerializeField] private TextMeshProUGUI levelTxt;


    [SerializeField] private Transform characterAim;
    
    // private var
    [SerializeField] private float rotateSpeed;

    [Header("Equipment")]
    public GameObject pant;
    public GameObject body;

    private int exp;
    public int level;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitialWeapon();
        DisplayLevelTxt();

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        attackTimer -= Time.deltaTime;
    }


    protected virtual void InitialWeapon()
    {
        //Instantiate(characterWeapon, characterAim);
        GameObject myWeapon = PoolObjects.Instance.GetObjectOutPool(weaponName);

        myWeapon.transform.SetParent(characterAim);
        myWeapon.transform.localPosition = Vector3.zero;
        myWeapon.transform.localRotation = Quaternion.identity;
        myWeapon.GetComponent<WeaponController>()?.SetWeaponOfCharacter(true);
        myWeapon.SetActive(true);


    }
    public virtual void RotateHandle(Vector3 dirTarget)
    {
        //transform.forward = Vector3.RotateTowards(transform.forward, rb.velocity, rotateSpeed, Time.deltaTime);
        transform.forward = Vector3.RotateTowards(transform.forward, dirTarget, rotateSpeed, Time.deltaTime);

    }

    #region attack
    protected virtual Transform GetTargetInRange(LayerMask layerCheck)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, attackRange, layerCheck);

        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider target in targets)
        {
            if (target.gameObject != gameObject)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target.transform;
                }
                //Debug.Log(closestTarget.name);

                return closestTarget;
            }
        }

        return null;
    }
    public virtual void ThrowWeapon(Vector3 dir)
    {
        GameObject weaponThrow = PoolObjects.Instance.GetObject(weaponName);
        weaponThrow.SetActive(true);
        weaponThrow.transform.position = attackPoint.position;
        weaponThrow.GetComponent<WeaponController>()?.SetupWeapon(dir.normalized, forceThrow,this);
    }

    public virtual bool CheckAttackCooldown()
    {
        if (attackTimer < 0) return true;

        return false;
    }
    public virtual void SetCooldown()
    {
        attackTimer = attackCooldown;
    }

    public virtual IEnumerator HideWeaponOnAttack(float time)
    {
        if (characterAim.gameObject.activeSelf)
        {
            characterAim.gameObject.SetActive(false);
            yield return new WaitForSeconds(time);
            characterAim.gameObject.SetActive(true);
        }

    }
    #endregion
    protected virtual void ChangeIdleState()
    {
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public virtual void TakeDamage()
    {
    }

    public virtual void KillCharacter()
    {
        IncreaseExp(1);
        // tang kich thuoc vk

    }
    
    private void IncreaseExp(int amount)
    {
        exp += amount;
        if(exp > 1)
        {
            LevelUp();
            exp = 0;
        }
    }
    private void LevelUp()
    {
        level++;
        DisplayLevelTxt();
        IncreaseScaleCharacter(10);
    }
    protected virtual void IncreaseScaleCharacter(float percent)
    {
        transform.localScale *= (1 + percent / 100);
        SetRange(percent);

    }
    private void SetRange(float percent)
    {
        attackRange *= (1+ percent/100); 
    }

    public int GetLevel()
    {
        return this.level;
    }

    private void DisplayLevelTxt()
    {
        levelTxt.text = level.ToString();
    }

    //action observer tang kich thuoc
}
