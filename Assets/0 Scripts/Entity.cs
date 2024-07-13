using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody rb { get; private set; }
    public EquipmentCharacter equipController { get; private set; }

    [Header("Attack")]
    protected string weaponName;
    public float attackCooldown;
    [SerializeField] private Transform attackPoint;
    public float attackDelay;
    public float attackRange;
    public float forceThrow;
    private float attackTimer;

    [Header("Level ui")]
    [SerializeField] private TextMeshProUGUI levelTxt;


    [Header("Equipment")]
    public SkinnedMeshRenderer pant;
    public SkinnedMeshRenderer body;
    [SerializeField] private Transform characterAim;
    protected WeaponController weaponScript;

    // die effect
    [SerializeField] private ParticleSystem dieEfect;
    [SerializeField] private ParticleSystem levelUpEfect;

    private PoolObjects poolObjects;
    [SerializeField] private float rotateSpeed;
    // private var


    private int exp;
    public int level;
    public Action OnLevelUp;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        poolObjects = FindObjectOfType<PoolObjects>();
        equipController = GetComponent<EquipmentCharacter>();
    }

    protected virtual void OnEnable()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {


        DisplayLevelTxt();
        if (level != 0)
        {
            for (int i = 0; i < level; i++)
            {
                LevelUp();
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        attackTimer -= Time.deltaTime;
    }

    //public virtual void InitialWeapon(string weapon)
    //{
    //    //Debug.Log("???");
    //    this.weaponName = weapon;
    //    GameObject myWeapon = poolObjects.GetObjectOutPool(weapon);

    //    myWeapon.transform.SetParent(characterAim);
    //    myWeapon.GetComponent<WeaponController>()?.SetWeaponOfCharacter(true);
    //    myWeapon.transform.localPosition = Vector3.zero;
    //    myWeapon.transform.localRotation = Quaternion.identity;
    //    myWeapon.SetActive(true);
    //    characterAim.gameObject.SetActive(true);

    //    weaponScript = myWeapon.GetComponent<WeaponController>();
    //}
    public virtual void RotateHandle(Vector3 dir)
    {
        //transform.forward = Vector3.RotateTowards(transform.forward, dirTarget, rotateSpeed, Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(dir);
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

    public void Attack(Vector3 dir)
    {

        if(weaponScript.attackType == AttackType.MULTYBULLET)
        {
            ThrowMultiWeapon(dir, weaponScript.amounntBullet,weaponScript.angle);
        }
        else
        {
            ThrowWeapon(dir);
        }
    }

    public virtual void ThrowWeapon(Vector3 dir)
    {
        GameObject weaponThrow = poolObjects.GetObject(weaponName);

        weaponThrow.SetActive(true);
        weaponThrow.transform.position = attackPoint.position;
        weaponThrow.GetComponent<WeaponController>()?.SetupWeapon(dir.normalized, forceThrow, this);

    }

    public virtual void ThrowMultiWeapon(Vector3 dir, int amountBullet, float angle)
    {
        float angleInit = -(amountBullet / 2) * angle;

        for (int i = 0; i < amountBullet; i++)
        {
            GameObject weaponThrow = poolObjects.GetObject(weaponName);
            Quaternion rotationBullet = Quaternion.Euler(0, angleInit + i * angle, 0);
            Vector3 rotatedDirection = rotationBullet * dir;

            weaponThrow.SetActive(true);
            weaponThrow.transform.position = attackPoint.position;
            weaponThrow.GetComponent<WeaponController>()?.SetupWeapon(rotatedDirection.normalized, forceThrow, this);
        }

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

    #region level up & upgrade
    private void IncreaseExp(int amount)
    {
        exp += amount;
        if (exp > 1)
        {
            level++;

            levelUpEfect.GetComponent<Renderer>().material = body.material;
            levelUpEfect.Play();
            OnLevelUp?.Invoke();

            LevelUp();
            exp = 0;
        }
    }
    private void LevelUp()
    {
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
        attackRange *= (1 + percent / 100);
    }

    public int GetLevel()
    {
        return this.level;
    }

    private void DisplayLevelTxt()
    {
        levelTxt.text = level.ToString();
    }

    #endregion
    //action observer tang kich thuoc
    public virtual void ChangeIdleState()
    {
    }
    public virtual void TakeDamage()
    {
        dieEfect.GetComponent<Renderer>().material = body.material;
        dieEfect.Play();
    }

    public virtual void KillCharacter()
    {
        IncreaseExp(1);
        // tang kich thuoc vk
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
