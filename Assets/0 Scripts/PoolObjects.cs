using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string objectName;
    public GameObject prefab;
    public int size;
}
public class PoolObjects : MonoBehaviour
{

    public List<Pool> pools;
    private Dictionary<string, List<GameObject>> poolDictionary;

    private void Awake()
    {


        poolDictionary = new Dictionary<string, List<GameObject>>();


        foreach (Pool pool in pools)
        {
            if (poolDictionary.ContainsKey(pool.objectName))
            {
                Debug.LogWarning(pool.objectName + " already  exist.");
                continue;
            }

            InitializePool(pool.objectName, pool.prefab, pool.size);
        }
    }

    private void InitializePool(string poolName, GameObject prefab, int size)
    {

        List<GameObject> objectPool = new List<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab,transform);
            obj.SetActive(false);
            objectPool.Add(obj);
        }

        poolDictionary.Add(poolName, objectPool);
    }

    public GameObject GetObject(string _objectName)
    {
        if (!poolDictionary.ContainsKey(_objectName))
        {
            Debug.LogWarning(_objectName + " doesn't exist.");
            return null;
        }

        foreach (GameObject obj in poolDictionary[_objectName])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // tao them object khi het
        Pool pool = pools.Find(p => p.objectName == _objectName);

        GameObject newObj = Instantiate(pool.prefab,transform);
        newObj.SetActive(true);
        poolDictionary[_objectName].Add(newObj);
        return newObj;

    }

    // tao pool moi hoac thay the pool da co cung ten
    public void AddPool(string _newObjectName, GameObject newPrefab, int size = 1)
    {
        if (poolDictionary.ContainsKey(_newObjectName))
        {
            ReplacePoolPrefab(_newObjectName, newPrefab, size);
        }
        else
        {
            InitializePool(_newObjectName, newPrefab, size);
        }
    }

    private void ReplacePoolPrefab(string poolName, GameObject newPrefab, int newSize)
    {
        List<GameObject> objectPool = poolDictionary[poolName];

        // Destroy old objects
        foreach (GameObject obj in objectPool)
        {
            Destroy(obj);
        }

        objectPool.Clear();

        for (int i = 0; i < newSize; i++)
        {
            GameObject obj = Instantiate(newPrefab, transform);
            obj.SetActive(false);
            objectPool.Add(obj);
        }

        poolDictionary[poolName] = objectPool;
    }
}
