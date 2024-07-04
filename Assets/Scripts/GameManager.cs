using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text enemiesAliveTxt;
    [SerializeField] private int enemyCount;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyParent;

    [SerializeField] private List<Material> pantMaterials;
    [SerializeField] private List<Material> bodyMaterials;

    void Start()
    {
        DisplayEnemyAlive();
        GenerateEnemy();
    }

    private void GenerateEnemy()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 ramdomPos = new Vector3(Random.Range(-49, 50), 0, Random.Range(-49, 50));
            GameObject newEnemy = Instantiate(enemyPrefab, ramdomPos, Quaternion.identity, enemyParent);
            newEnemy.GetComponent<Enemy>().OnDeath += OnEnemyDeath;
            newEnemy.GetComponent<Enemy>().pant.GetComponent<SkinnedMeshRenderer>().material = pantMaterials[Random.Range(0,pantMaterials.Count)];
            newEnemy.GetComponent<Enemy>().body.GetComponent<SkinnedMeshRenderer>().material = bodyMaterials[Random.Range(0, bodyMaterials.Count)];
        }
    }

    void OnEnemyDeath()
    {
        //
        enemyCount--;
        DisplayEnemyAlive();
    }

    private void DisplayEnemyAlive()
    {
        enemiesAliveTxt.text = enemyCount.ToString();

    }
}
