using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Pool")]
    [SerializeField] private PoolObjects poolObjects;
    [Header("Enemy")]
    public Text enemiesAliveTxt;

    [SerializeField] private int enemyCount;
    [SerializeField] private int enemyAlive;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyParent;
    private List<GameObject> enemies= new List<GameObject>();

    [Header("Material")]
    [SerializeField] private List<Material> pantMaterials;
    [SerializeField] private List<Material> bodyMaterials;

    private ArrowTowardEnemys arrowIndicator;

    public Player player;

    private int amountCharacter;

    // variable game control
    private bool canSpawn = true;
    public bool isWin { get; private set; }
    public bool isEnd { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    void Start()
    {
        arrowIndicator = GetComponent<ArrowTowardEnemys>();

        GenerateEnemy();

        Observer.AddObserver("play", AddTargetIndicator);
        amountCharacter = enemyCount + 1;

        DisplayEnemyAlive();
    }

    private void GenerateEnemy()
    {
        for (int i = 0; i < enemyAlive; i++)
        {
            InitializeEnemy();
        }
    }

    private void InitializeEnemy()
    {
        //get enemy in pool
        GameObject newEnemy = poolObjects.GetObject("enemy");
        //Debug.Log(newEnemy);
        newEnemy.transform.SetParent(enemyParent);

        // enemy properties
        int enemyLevel = Mathf.Clamp(Random.Range(player.GetLevel(), player.GetLevel() + 4), 0, player.GetLevel() + 4);
        Material pantMaterial = pantMaterials[Random.Range(0, pantMaterials.Count)];
        Material bodyMaterial = bodyMaterials[Random.Range(0, bodyMaterials.Count)];

        // set up enemy
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.InitializeEnemy(bodyMaterial, pantMaterial, enemyLevel, player);
        enemyScript.OnDeath += OnEnemyDeath;
        newEnemy.SetActive(true);

        // add arrow indicator to enemy
        enemies.Add(newEnemy);

    }

    private void AddTargetIndicator()
    {
        foreach (var enemy in enemies)
        {
            arrowIndicator.AddTargetIndicator(enemy);
        }

    }

    public void OnEnemyDeath()
    {
        amountCharacter--;
        DisplayEnemyAlive();

        // spawn handle
        if (amountCharacter > enemyAlive)
        {
            StartCoroutine(RevivalEnemyCoroutine());
        }
        else
        {
            canSpawn = false;
        }


        if (amountCharacter == 1)
        {
            isEnd = true;
            isWin = true;

            // display player win
            Observer.Notify("PlayerWin");
        }
    }

    private void DisplayEnemyAlive()
    {
        enemiesAliveTxt.text = amountCharacter.ToString();

    }

    private IEnumerator RevivalEnemyCoroutine()
    {
        yield return new WaitForSeconds(1);
        if (canSpawn)
        {

            GameObject newEnemy = poolObjects.GetObject("enemy");
            newEnemy.SetActive(true);
            newEnemy.GetComponent<Enemy>().RevivalEnemy();
            newEnemy.GetComponent<Enemy>().SpawnOnNavMesh();
            //Debug.Log("enemy revival");
        }
    }

    private void OnDisable()
    {
        Observer.RemoveObserver("play", AddTargetIndicator);
    }
}
