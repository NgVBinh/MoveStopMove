using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ZCGameManager : MonoBehaviour,ISaveManager
{
    public static ZCGameManager instance;


    public Player player;
    public List<Equip> myEquips= new List<Equip>();
    [Header("Enemy")]
    //public Text enemiesAliveTxt;

    [SerializeField] private GameObject zombiePref;

    public int enemyCount;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyParent;
    private List<GameObject> enemies = new List<GameObject>();

    [Header("Material")]
    [SerializeField] private List<Material> zombieMaterials;

    public int numberSpawnOnTime;
    public float spawnCoutndown;
    private float spawnTimer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        player.isZombieCity = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        player.canMove = true;
        SaveManager.instance.LoadComponent(this);

        EquipThePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (enemyCount > 0)
        {
            if (spawnTimer < 0)
            {
                SpawnOnTime(numberSpawnOnTime);
                spawnTimer = spawnCoutndown;
            }
        }
        
    }

    private void SpawnOnTime(int quantity)
    {
        enemyCount -= quantity;
        for (int i = 0; i < quantity; i++)
        {
            //get enemy in pool
            GameObject newEnemy = Instantiate(zombiePref);

            // enemy properties
            Material randomZombieColor = zombieMaterials[Random.Range(0, zombieMaterials.Count)];

            // set up enemy
            ZombieController zombieScript = newEnemy.GetComponent<ZombieController>();
            zombieScript.SetupZombie(randomZombieColor);
            zombieScript.SpawnOnNavMesh();
        }
    }
    public string weaponName { get; private set; }
    public string shieldName { get; private set; }
    public string pantName { get; private set; }
    public string hairName { get; private set; }
    public string setName { get; private set; }
    public void EquipThePlayer()
    {
        weaponName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.WEAPON && tmp.used)?.equipName;
        player.equipController.PlayerEquipped(weaponName);
        hairName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.HAIR && tmp.used)?.equipName;
        pantName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.PANT && tmp.used)?.equipName;
        shieldName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.SHIELD && tmp.used)?.equipName;

        setName = myEquips.FirstOrDefault(tmp => tmp.type == EquipmentType.SET && tmp.used)?.equipName;
        if (setName != null)
        {
            player.equipController.EquipSet(setName);
        }
        else
        {
            player.equipController.UnEquipSet();

            if (hairName != null)
            {
                player.equipController.PlayerEquipped(hairName);
            }
            else
            {
                player.equipController.RemoveHairStat();
            }

            if (pantName != null)
            {
                player.equipController.PlayerEquipped(pantName);
            }
            else
            {
                player.equipController.RemovePantStat();
            }
            player.equipController.PlayerEquipped(shieldName);
        }
        player.CreateWeaponInHand();
    }

    public void LoadData(GameData gameData)
    {
        myEquips = gameData.myEquips;
    }

    public void SaveData(ref GameData gameData)
    {

    }
}
