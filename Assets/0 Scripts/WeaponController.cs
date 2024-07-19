using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    STRAIGHT,
    SPIN,
    BUMERANG
}

public enum AttackType
{
    SINGLE,
    MULTYBULLET
}

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class WeaponController : MonoBehaviour
{
    public WeaponType weaponType;
    public AttackType attackType;
    // if weaponCenter type Spin
    [SerializeField] private float rotateSpeed = 1000;

    // if attack type multi bullet
    public float angle = 25;
    public int amounntBullet = 3;

    // color & material
    private MeshRenderer meshRenderer;
    //public Material[] materials = new Material[] { };

    private Rigidbody rb;
    public bool isCharacterWeapon;

    private Entity myCharacter;

    private Vector3 lastScale;

    private bool returnCharacter;

    Vector3 startPos;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        //materials = meshRenderer.materials;
        lastScale = transform.localScale;
    }
    private void OnEnable()
    {
        StartCoroutine(ReturnToPool());

    }

    public void SetColor(List<Material> a)
    {
        meshRenderer.materials = a.ToArray();
    }

    public void SetupWeapon(Vector3 dir, float force, Entity character)
    {
        myCharacter = character;
        transform.localScale *= (1 + character.GetLevel() / 10f);

        rb.isKinematic = false;
        rb.velocity = (dir * force);

        // roatate direction weaponCenter
        Quaternion targetRotation = Quaternion.LookRotation(-dir, Vector3.up);
        transform.rotation = targetRotation * Quaternion.Euler(90, 0, 0);

        startPos = transform.position;

    }

    private void Update()
    {
        if (isCharacterWeapon) return;

        if (weaponType == WeaponType.SPIN)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }

        BumerangLogic();

        if (Vector3.Distance(transform.position, startPos) > myCharacter.attackRange && weaponType != WeaponType.BUMERANG)
        {
            gameObject.SetActive(false);
        }
    }

    private void BumerangLogic()
    {
        if (weaponType == WeaponType.BUMERANG && !returnCharacter)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) > myCharacter.attackRange)
            {
                returnCharacter = true;
                rb.velocity = Vector3.zero;
            }
        }
        if (returnCharacter)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            Vector3 dir = myCharacter.transform.position - transform.position;
            transform.position += dir.normalized * 0.1f;

            if (Vector3.Distance(transform.position, myCharacter.transform.position) < 1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(1);

        if (weaponType!=WeaponType.BUMERANG&& !isCharacterWeapon && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetWeaponOfCharacter(bool isCharacterWeapon)
    {
        this.isCharacterWeapon = isCharacterWeapon;
        gameObject.tag = "Untagged";
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCharacterWeapon) return;

        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            Entity tmp = other.GetComponent<Entity>();

            if (tmp != myCharacter)
            {
                if (other.GetComponent<Player>() != null)
                {
                    //Debug.Log(myCharacter.characterName);
                    UIManager.instance.endgameController.SetupEndLose(myCharacter.characterName, GameManager.instance.amountCharacter, myCharacter.body.material.color);
                }

                myCharacter.KillCharacter();
                tmp.TakeDamage();

                gameObject.SetActive(false);
            }


        }
    }

    private void OnDisable()
    {
        transform.localScale = lastScale;
        returnCharacter = false;
        startPos = Vector3.zero;

    }
}
