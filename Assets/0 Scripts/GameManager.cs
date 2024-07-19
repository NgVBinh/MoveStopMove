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

    public int enemyCount;
    [SerializeField] private int enemyAlive;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyParent;
    private List<GameObject> enemies= new List<GameObject>();

    [Header("Material")]
    [SerializeField] private List<Material> pantsEnemy;
    [SerializeField] private List<Material> bodyEnemy;

    private ArrowTowardEnemys arrowIndicator;

    public Player player;

    public int amountCharacter { get; private set; }

    // variable game control
    private bool canSpawn = true;

    public bool isWin { get; private set; }

    private int coin;

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
            //get enemy in pool
            GameObject newEnemy = poolObjects.GetObject("enemy");
            //Debug.Log(newEnemy);
            newEnemy.transform.SetParent(enemyParent);

            // enemy properties
            int enemyLevel = Mathf.Clamp(Random.Range(player.GetLevel()-1, player.GetLevel() + 5), 0, player.GetLevel() + 5);
            Material pantMaterial = pantsEnemy[Random.Range(0, pantsEnemy.Count)];

            Material randomEnemyBody = bodyEnemy[Random.Range(0, bodyEnemy.Count)];
            bodyEnemy.Remove(randomEnemyBody);

            // set up enemy
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            enemyScript.InitializeEnemy(randomEnemyBody, pantMaterial, enemyLevel, "Enemy " + (i+1), player);
            enemyScript.OnDeath += OnEnemyDeath;
            newEnemy.SetActive(true);


            // add arrow indicator to enemy
            enemies.Add(newEnemy);
        }
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
        if (canSpawn)
        {
            yield return new WaitForSeconds(1);
            GameObject newEnemy = poolObjects.GetObject("enemy");
            newEnemy.SetActive(true);
            int enemyLevel = Mathf.Clamp(Random.Range(player.GetLevel() - 1, player.GetLevel() + 5), 0, player.GetLevel() + 5);
            newEnemy.GetComponent<Enemy>().level = enemyLevel;
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
