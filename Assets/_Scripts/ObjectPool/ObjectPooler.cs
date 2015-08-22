using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler
{
    //public static ObjectPooler current;
    public class PoolInfo
    {
        public GameObject pooledObject; // object we want to spawn
        public int pooledAmount = 20; // random default amount, will be reset on Init anyways

        public bool willGrow = true; // expand based on demand
    }

    public Dictionary<string, List<GameObject>> pool; // the holder of all the object pools, each key refers to a different pool
    private Dictionary<string, PoolInfo> poolInfoContainer; // the key will be the same as the 1 from above, this will hold the spawn info 

    public ObjectPooler()
    {
        pool = new Dictionary<string, List<GameObject>>();
        poolInfoContainer = new Dictionary<string, PoolInfo>();
    }

    public void InitPool(string poolName, GameObject poolPrefab, int amount, bool grow)
    {
        List<GameObject> objects = new List<GameObject>();
        PoolInfo pi = new PoolInfo();

        pi.pooledAmount = amount;
        pi.pooledObject = poolPrefab;
        pi.willGrow = grow;

        if (!pool.ContainsKey(poolName))
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = GameObject.Instantiate(poolPrefab);
                obj.SetActive(false);
                objects.Add(obj);
            }

            pool.Add(poolName, objects);

            poolInfoContainer.Add(poolName,pi);
        }

    }

    public GameObject GetPooledObj(string poolName)
    {

        if (pool.ContainsKey(poolName))
        {
            for (int i = 0; i < pool[poolName].Count; i++)
            {
                if (!pool[poolName][i].activeInHierarchy)
                {
                    return pool[poolName][i];
                }
            }

            if (poolInfoContainer[poolName].willGrow)
            {
                GameObject obj = GameObject.Instantiate(poolInfoContainer[poolName].pooledObject);
                pool[poolName].Add(obj);
                return obj;
            }

        }
        return null;
    }

    public void DebugInfo()
    {
        Debug.Log("pool info count: " + poolInfoContainer.Count);

    }

    public int GetCount(string k)
    {
        if (pool.ContainsKey(k))
        {
            return pool[k].Count;
        }

        return 0;
    }

    public int PoolCount()
    {
        return pool.Count;
    }
}
