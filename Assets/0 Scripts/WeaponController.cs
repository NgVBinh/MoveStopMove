using System.Collections;

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
    // if weapon type Spin
    [SerializeField] private float rotateSpeed=1000;

    // if attack type multi bullet
    public float angle = 25;
    public int amounntBullet = 3;

    private Rigidbody rb;
    private bool isCharacterWeapon;

    private Entity myCharacter;

    private Vector3 lastScale;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lastScale = transform.localScale;
    }

    private void Start()
    {
       
    }
    private void OnEnable()
    {

       // transform.localRotation = Quaternion.identity;
        //transform.localPosition = Vector3.zero;
        //StartCoroutine(ActiveWeapon());


        StartCoroutine(ReturnToPool());
    }
    public void SetupWeapon(Vector3 dir, float force, Entity character)
    {
        myCharacter = character;
        transform.localScale *= (1 + character.GetLevel() / 10f);
        
        rb.isKinematic = false;
        rb.velocity = (dir * force);

        //Quaternion rotationBullet = Quaternion.Euler(-90, transform.eulerAngles.y, transform.eulerAngles.z);
        //Vector3 newRotate = rotationBullet * dir;

        //transform.rotation = Quaternion.Euler(newRotate);
        //transform.up = newRotate;
        //transform.rotation = Quaternion.Euler(-90,transform.eulerAngles.y,transform.eulerAngles.z);

        //transform.Rotate(dir);

        // roatate direction weapon
        Quaternion targetRotation = Quaternion.LookRotation(-dir, Vector3.up);
        transform.rotation = targetRotation * Quaternion.Euler(90, 0, 0); 

    }

    private void Update()
    {

        if (weaponType==WeaponType.SPIN && !isCharacterWeapon)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, myCharacter.transform.position) > myCharacter.attackRange)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(2);
        if (gameObject.activeSelf && !isCharacterWeapon)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetWeaponOfCharacter(bool isCharacterWeapon)
    {
        this.isCharacterWeapon = isCharacterWeapon;
        this.enabled = false;
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
            //Debug.Log(other.name + "   die");
            if (tmp != myCharacter)
            {
                myCharacter.KillCharacter();
                tmp.TakeDamage();
                gameObject.SetActive(false);
            }


        }
    }

    private void OnDisable()
    {
        transform.localScale = lastScale;

    }
}
