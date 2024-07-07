using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text enemiesAliveTxt;

    [SerializeField] private int enemyCount; 
    [SerializeField] private int enemyAlive;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyParent;
    //public List<GameObject> enemies = new List< GameObject>();

    [SerializeField] private List<Material> pantMaterials;
    [SerializeField] private List<Material> bodyMaterials;

    [SerializeField] private ArrowTowardEnemys arrowIndicator;

    [SerializeField] private Player player;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        GenerateEnemy();

    }

    void Start()
    {
        DisplayEnemyAlive();
    }

    private void GenerateEnemy()
    {
        for (int i = 0; i < enemyAlive; i++)
        {
            InitializeEnemy();
            //enemies.Add(newEnemy);
        }
    }

    private void InitializeEnemy()
    {
        Vector3 ramdomPos = new Vector3(Random.Range(-49, 50), 0, Random.Range(-49, 50));
        GameObject newEnemy = Instantiate(enemyPrefab, ramdomPos, Quaternion.identity, enemyParent);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        int enemyLevel = Mathf.Clamp(Random.Range(player.GetLevel(), player.GetLevel() + 2), 0, player.GetLevel() + 4);
        enemyScript.level = enemyLevel;
        enemyScript.OnDeath += OnEnemyDeath;
        enemyScript.pant.material = pantMaterials[Random.Range(0, pantMaterials.Count)];
        enemyScript.body.material = bodyMaterials[Random.Range(0, bodyMaterials.Count)];

        arrowIndicator.AddTargetIndicator(newEnemy);
    }

    void OnEnemyDeath()
    {
        enemyCount--;
        //
        DisplayEnemyAlive();
        if (enemyCount >= enemyAlive)
            InitializeEnemy();
    }

    private void DisplayEnemyAlive()
    {
        enemiesAliveTxt.text = enemyCount.ToString();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            //Vector3 ramdomPos = new Vector3(Random.Range(-49, 50), 0, Random.Range(-49, 50));
            //PlayerManager.instance.player.transform.position = ramdomPos;

            PlayerManager.instance.player.ChangeIdleState();
        }
    }
}
