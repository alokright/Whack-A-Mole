using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private Queue<GameObject> objectPool;
    private GameObject molePrefab;

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
        objectPool = new Queue<GameObject>();
        molePrefab = Resources.Load<GameObject>("MolePrefab");
        InitializePool(3); // Initialize with 3 objects
    }

    void InitializePool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newObj = Instantiate(molePrefab);
            newObj.SetActive(false);
            objectPool.Enqueue(newObj);
        }
    }

    public GameObject GetObject()
    {
        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // Instantiate a new object if the pool is empty
            return Instantiate(molePrefab);
        }
    }
    Vector3 DeathPosition = new Vector3(100f,100f,100f);
    public void ReturnObject(GameObject obj)
    {
        obj.transform.localPosition = DeathPosition;
        objectPool.Enqueue(obj);
    }
}
