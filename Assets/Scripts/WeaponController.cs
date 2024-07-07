using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private bool canRotate;
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
            this.enabled = false;
            gameObject.tag = "Untagged";
        }

    }
    private void OnEnable()
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        //StartCoroutine(ActiveWeapon());

        StartCoroutine(ReturnToPool());
    }
    public void SetupWeapon(Vector3 dir, float force, Entity character)
    {
        rb.isKinematic = false;
        rb.velocity = (dir * force);
        transform.up = -rb.velocity;

        if (canRotate)
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        myCharacter = character;
        transform.localScale *= (1 + character.GetLevel() / 10f);
    }

    private void Update()
    {
        if (canRotate && !isCharacterWeapon)
        {
            transform.Rotate(-Vector3.forward * speedRotate * Time.deltaTime);
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
    }

    //private IEnumerator ActiveWeapon()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    rb.isKinematic = true;
    //    rb.useGravity = true;
    //}

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
