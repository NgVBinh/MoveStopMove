using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Material")]
    [SerializeField] private List<Material> pantMaterials;
    [SerializeField] private List<Material> bodyMaterials;

    [Header("ArrowIndicator")]
    [SerializeField] private ArrowTowardEnemys arrowIndicator;

    [Header("UI")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject joystick;

    [SerializeField] private Player player;

    private int amountCharacter;

    private string[] weapons = { "bua", "riu", "keo" };

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
        GenerateEnemy();

        player.OnDeath += PlayerLose;
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

        // enemy properties
        int enemyLevel = Mathf.Clamp(Random.Range(player.GetLevel(), player.GetLevel() + 4), 0, player.GetLevel() + 4);
        Material pantMaterial = pantMaterials[Random.Range(0, pantMaterials.Count)];
        Material bodyMaterial = bodyMaterials[Random.Range(0, bodyMaterials.Count)];

        // set up enemy
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.InitializeEnemy(weapons[Random.Range(0, weapons.Length)], bodyMaterial, pantMaterial, enemyLevel,player);
        enemyScript.OnDeath += OnEnemyDeath;
        newEnemy.SetActive(true);

        // add arrow indicator to enemy
        arrowIndicator.AddTargetIndicator(newEnemy);
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
            PlayerWin();
        }
    }

    private void DisplayEnemyAlive()
    {
        enemiesAliveTxt.text = amountCharacter.ToString();

    }

    void PlayerLose()
    {
        losePanel.SetActive(true);
        joystick.SetActive(false);

    }
    void PlayerWin()
    {
        winPanel.SetActive(true);
        joystick.SetActive(false);

    }
    public void PlayerRevial()
    {
        player.ChangeIdleState();
        if (losePanel.activeSelf)
        {
            losePanel.SetActive(false);
        }

        if (!joystick.activeSelf)
        {
            joystick.SetActive(true);
        }

    }

    // btn replay
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}
