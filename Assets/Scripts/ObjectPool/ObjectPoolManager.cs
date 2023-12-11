using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField]
    private List<PoolConfig> poolConfig;

    Dictionary<PoolObjectType, Queue<GameObject>> objectPool;
    public static ObjectPoolManager Instance { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize object pool
        objectPool = new Dictionary<PoolObjectType, Queue<GameObject>>();
        InitializePool();
    }

    void InitializePool()
    {
        GameObject newObj = null;
        for (int i = 0; i < poolConfig.Count; i++)
        {
            newObj = Instantiate(poolConfig[i].Prefab);
            newObj.SetActive(false);
            if (!objectPool.ContainsKey(poolConfig[i].Type))
                objectPool.Add(poolConfig[i].Type, new Queue<GameObject>());
            objectPool[poolConfig[i].Type].Enqueue(newObj);
        }
    }

    public GameObject GetObject(PoolObjectType type)
    {
        GameObject obj = null;
        if (!objectPool.ContainsKey(type) || objectPool[type].Count <= 0)
        {
            for (int i = 0; i < poolConfig.Count; i++)
            {
                if (poolConfig[i].Type == type)
                {
                    obj = Instantiate(poolConfig[i].Prefab);
                    obj.SetActive(false);
                    if (!objectPool.ContainsKey(poolConfig[i].Type))
                        objectPool.Add(poolConfig[i].Type, new Queue<GameObject>());
                    objectPool[poolConfig[i].Type].Enqueue(obj);
                    break;
                }

            }
        }
        obj = objectPool[type].Dequeue();
        obj.SetActive(true);
        return obj;
    }
    
    private Vector3 DeathPosition = new Vector3(100f,100f,100f);
    public void ReturnObject(GameObject obj)
    {
        obj.transform.localPosition = DeathPosition;
        objectPool[obj.GetComponent<PoolObject>().ObjectType].Enqueue(obj);
    }

}
