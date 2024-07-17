using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum TagType
//{
//    Player,
//    Enemy,
//    Projectile,
//}

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
                Debug.LogError("Duplicate _objectName in pools: " + pool.objectName);
                continue;
            }

            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolDictionary.Add(pool.objectName, objectPool);
        }
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
                //obj.SetActive(true);
                return obj;
            }
        }

        // tao them object khi het
        foreach (Pool pool in pools)
        {
            if (pool.objectName == _objectName)
            {
                GameObject newObj = Instantiate(pool.prefab);
                newObj.SetActive(true);
                poolDictionary[_objectName].Add(newObj);
                return newObj;
            }
        }
        Debug.LogError(_objectName + " null");
        return null;

        ////tao them object khi het object trong pool
        //Pool pool = GetPoolByName(_objectName);
        //if (pool != null)
        //{
        //    GameObject newObj = Instantiate(pool.prefab);
        //    newObj.SetActive(false);
        //    poolDictionary[_objectName].Add(newObj);
        //    return newObj;
        //}

        //return null;
    }

    // lay object trong pool va xoa object do khoi pool
    public GameObject GetObjectOutPool(string _objectName)
    {
        GameObject tmp = GetObject(_objectName);
        poolDictionary[_objectName].Remove(tmp);

        return tmp;
    }

    public void ReturnObject(string _objectName, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(_objectName))
        {
            Debug.LogWarning("Pool with _objectName " + _objectName + " doesn't exist.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
    }
}

//public static class Tags
//{
//    public const string Player = "Player";
//    public const string Enemy = "Enemy";
//    public const string Projectile = "Projectile";

//}