using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody rb { get; private set; }
    public EquipmentCharacter equipController;

    [HideInInspector]
    public string characterName;

    public float moveSpeed;

    [Header("Attack")]
    protected string weaponName;
    public float attackCooldown;
    [SerializeField] private Transform attackPoint;
    public float attackDelay;
    public float attackRange;
    public float attackSpeed;
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
    public int[] expToUgrade = {0,2,5,10,18,30,45,60};

    //[HideInInspector]
    public int exp;
    public int curentLevel;
    public int lastLevel;
    public Action OnLevelUp;

    [HideInInspector]
    public bool usedWeapon;
    [HideInInspector]
    public bool usedHair;
    [HideInInspector]
    public bool usedShield;
    [HideInInspector]
    public bool usedPant;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        poolObjects = FindObjectOfType<PoolObjects>();
        equipController = GetComponent<EquipmentCharacter>();
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        levelUpEfect.GetComponent<Renderer>().material = body.material;
        dieEfect.GetComponent<Renderer>().material = body.material;

        if (exp != 0)
        {
            IncreaseExp(exp);
        }

        DisplayExpTxt();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        attackTimer -= Time.deltaTime;
    }

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
        weaponThrow.GetComponent<WeaponController>()?.SetupWeapon(dir.normalized, attackSpeed, this);

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
            weaponThrow.GetComponent<WeaponController>()?.SetupWeapon(rotatedDirection.normalized, attackSpeed, this);
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
    public void IncreaseExp(int amount)
    {
        exp += amount;
        DisplayExpTxt();
        curentLevel = GetCharacterLevel(exp);
        if(curentLevel>lastLevel)
        {
            for(int i = lastLevel; i  < curentLevel; i++)
            {
                // level up
                LevelUp(10);
                
                levelUpEfect.Play();
                OnLevelUp?.Invoke();
            }
            lastLevel = curentLevel;

        }
    }
    private void LevelUp(float percentScale)
    {
        IncreaseScaleCharacter(percentScale);
        IncreaseRange(percentScale);

    }
    public virtual void IncreaseScaleCharacter(float percent)
    {
        transform.localScale *= (1 + percent / 100f);

    }
    public virtual void IncreaseRange(float percent)
    {
        attackRange *= (1 + percent / 100f);
    }
    public virtual void IncreaseSpeed(float percent)
    {
        moveSpeed *= (1 + percent / 100f);
    }

    public virtual void IncreaseAttackSpeed(float percent)
    {
        attackSpeed *= (1 + percent / 100f);
    }

    public int GetLevel()
    {
        return this.curentLevel;
    }

    private void DisplayExpTxt()
    {
        levelTxt.text = exp .ToString();
    }

    #endregion
    //action observer tang kich thuoc
    public virtual void ChangeIdleState()
    {
    }
    public virtual void TakeDamage()
    {
        dieEfect.Play();
    }

    public virtual void KillCharacter(int exp)
    {
        IncreaseExp(exp);
        // tang kich thuoc vk
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    public virtual int GetCharacterLevel(int exp)
    {
        for(int i = 0;i<expToUgrade.Length;i++)
        {
            if (exp>=expToUgrade[i] && exp < expToUgrade[i+1])
            {
                return i;
            }
        }

        return -1;
    }
}
