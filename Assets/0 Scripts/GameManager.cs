using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Enemy")]
    public Text enemiesAliveTxt;

    [SerializeField] private int enemyCount;
    [SerializeField] private int enemyAlive;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyParent;
    public List<Transform> enemies = new List<Transform>();

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

    private string[] weapons = { "bua", "riu", "keo","ten" };
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
        
        DisplayEnemyAlive();
        player.OnDeath += PlayerLose;
        amountCharacter = enemyCount + 1;
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
        Vector3 ramdomPos = new Vector3(Random.Range(-49, 50), 0, Random.Range(-49, 50));
        GameObject newEnemy = Instantiate(enemyPrefab, ramdomPos, Quaternion.identity, enemyParent);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.InitialWeapon(weapons[Random.Range(0,weapons.Length)]);
        int enemyLevel = Mathf.Clamp(Random.Range(player.GetLevel(), player.GetLevel() + 4), 0, player.GetLevel() + 4);
        enemyScript.level = enemyLevel;
        enemyScript.OnDeath += OnEnemyDeath;
        enemyScript.pant.material = pantMaterials[Random.Range(0, pantMaterials.Count)];
        enemyScript.body.material = bodyMaterials[Random.Range(0, bodyMaterials.Count)];

        //enemies.Add(newEnemy.transform);

        arrowIndicator.AddTargetIndicator(newEnemy);
    }

    void OnEnemyDeath()
    {
        amountCharacter--;
        DisplayEnemyAlive();
        //
        if (amountCharacter > enemyAlive)
        {
            InitializeEnemy();
        }
        if (amountCharacter == 1)
        {
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

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
