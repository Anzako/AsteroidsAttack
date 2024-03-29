using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
    }

    #region Singleton

    public static ObjectPooler instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else 
        {
            instance = this;
        }
    }

    #endregion

    public List<Pool> pools;
    public static Dictionary<string, List<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
    }

    public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        Pool pool = pools.Find(p => p.tag == tag);

        if (pool == null)
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist. There is no prefab to create new pool");
            return null;
        }

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist. Creating new one");
            poolDictionary.Add(tag, new List<GameObject>());
        }

        GameObject spawnableObj = poolDictionary[tag].FirstOrDefault();

        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(pool.prefab, position, rotation);
        }
        else
        {
            spawnableObj.transform.position = position;
            spawnableObj.transform.rotation = rotation;
            poolDictionary[tag].Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj == null) 
        {
            Debug.Log("Cycki");
        }
        string tag = pooledObj.Tag;
        Pool pool = pools.Find(p => p.tag == tag);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled " + tag);
        }
        else
        {
            obj.SetActive(false);
            poolDictionary[tag].Add(obj);
        }
    }

}
