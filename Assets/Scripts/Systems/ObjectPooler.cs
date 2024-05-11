using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : Singleton<ObjectPooler>
{
    Transform parent;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        //public poolTags tager;
    }

    // Declares pool objects with tag
    public List<Pool> pools;
    // Store pooled objects
    public static Dictionary<string, List<GameObject>> poolObjects;
    public static Dictionary<string, List<GameObject>> activeObjects;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform;
        poolObjects = new Dictionary<string, List<GameObject>>();
        activeObjects = new Dictionary<string, List<GameObject>>();
    }

    public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        Pool pool = pools.Find(p => p.tag == tag);

        if (pool == null)
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist. There is no prefab to create new pool");
            return null;
        }

        if (!poolObjects.ContainsKey(tag))
        {
            //Debug.LogWarning("Pool with tag " + tag + " doesn't exist. Creating new one");
            poolObjects.Add(tag, new List<GameObject>());
            activeObjects.Add(tag, new List<GameObject>());
        }

        GameObject spawnableObj = poolObjects[tag].FirstOrDefault();

        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(pool.prefab, position, rotation, parent);
        }
        else
        {
            Transform obj = spawnableObj.transform;
            obj.SetPositionAndRotation(position, rotation);
            poolObjects[tag].Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        activeObjects[tag].Add(spawnableObj);
/*        Debug.Log("Tag pool " + tag + " have active objects: " + activeObjects[tag].Count +
            " and pool objects: " + poolObjects[tag].Count);*/
        spawnableObj.GetComponent<IPooledObject>().OnObjectSpawn();
        return spawnableObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj == null) 
        {
            Debug.LogWarning("Object don't have Pooled Object interface");
            return;
        }
        string tag = pooledObj.Tag;

        Pool pool = pools.Find(p => p.tag == tag);
        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled " + tag);
            return;
        }
        
        obj.SetActive(false);
        poolObjects[tag].Add(obj);
        activeObjects[tag].Remove(obj);
        /*Debug.Log("Tag pool " + tag + " have active objects: " + activeObjects[tag].Count +
            " and pool objects: " + poolObjects[tag].Count);*/
    }

    public void ReturnObjectsToPool(string tagName)
    {
        Pool pool = pools.Find(p => p.tag == tagName);
        if (pool == null)
        {
            //Debug.LogWarning("Trying to return objects from pool " + tag + " but there is no pool");
            return;
        } 

        if (!activeObjects.ContainsKey(tagName))
        {
            //Debug.Log("There is no created pool for objects " + tagName);
            return;
        }

        for (int i = activeObjects[tagName].Count - 1; i >= 0; i--)
        {
            GameObject obj = activeObjects[tagName][i];
            obj.SetActive(false);
            activeObjects[tagName].Remove(obj);
            poolObjects[tagName].Add(obj);
        }

    }

}


/*public enum poolTags
{
    projectile = 0,
    smallAsteroid = 1,
    mediumAsteroid = 2,
    bigAsteroid = 3,
}*/