using System.Collections;
<<<<<<< HEAD:Assets/0 Scripts/WeaponController.cs
<<<<<<< HEAD:Assets/0 Scripts/WeaponController.cs
=======
using Unity.VisualScripting;
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/WeaponController.cs
=======
using Unity.VisualScripting;
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/WeaponController.cs
using UnityEngine;

public enum WeaponType
{
    STRAIGHT,
    SPIN,
    MULTIBULLET,
    BUMERANG
}


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private WeaponType type;
    [SerializeField] private float speedRotate;

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
        if (isCharacterWeapon)
        {
           
        }

    }
    private void OnEnable()
    {
<<<<<<< HEAD:Assets/0 Scripts/WeaponController.cs
<<<<<<< HEAD:Assets/0 Scripts/WeaponController.cs
=======
=======
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/WeaponController.cs
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        //StartCoroutine(ActiveWeapon());
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/WeaponController.cs

        StartCoroutine(ReturnToPool());
    }
    public void SetupWeapon(Vector3 dir, float force, Entity character)
    {
        myCharacter = character;
        transform.localScale *= (1 + character.GetLevel() / 10f);
        
        rb.isKinematic = false;
        rb.velocity = (dir * force);
<<<<<<< HEAD:Assets/0 Scripts/WeaponController.cs

        transform.up = -dir;

        transform.eulerAngles = new Vector3(90,transform.eulerAngles.y,transform.eulerAngles.z);

=======
        transform.up = -rb.velocity;

        if (canRotate)
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        myCharacter = character;
        transform.localScale *= (1 + character.GetLevel() / 10f);
<<<<<<< HEAD:Assets/0 Scripts/WeaponController.cs
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/WeaponController.cs
=======
>>>>>>> d24970c66c36f5591d8065ab0ad3397a8afbafef:Assets/Scripts/WeaponController.cs
    }

    private void Update()
    {


        if (type==WeaponType.SPIN && !isCharacterWeapon)
        {
            transform.Rotate(-Vector3.forward * speedRotate * Time.deltaTime);
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
