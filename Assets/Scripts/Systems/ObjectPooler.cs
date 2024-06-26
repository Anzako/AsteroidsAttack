using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : Singleton<ObjectPooler>
{
    Transform parent;

    [System.Serializable]
    public class Pool
    {
        public poolTags poolTag;
        public GameObject prefab;
    }

    // Declares pool objects with tag
    public List<Pool> pools;
    // Store pooled objects
    public static Dictionary<poolTags, List<GameObject>> poolObjects;
    public static Dictionary<poolTags, List<GameObject>> activeObjects;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform;
        poolObjects = new Dictionary<poolTags, List<GameObject>>();
        activeObjects = new Dictionary<poolTags, List<GameObject>>();
    }

    public GameObject SpawnObject(poolTags objectTag, Vector3 position, Quaternion rotation)
    {
        Pool pool = pools.Find(p => p.poolTag == objectTag);

        if (pool == null)
        {
            Debug.LogWarning("Pool with tag " + objectTag.ToString() + " doesn't exist. There is no prefab to create new pool");
            return null;
        }

        if (!poolObjects.ContainsKey(objectTag))
        {
            poolObjects.Add(objectTag, new List<GameObject>());
            activeObjects.Add(objectTag, new List<GameObject>());
        }

        GameObject spawnableObj = poolObjects[objectTag].FirstOrDefault();

        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(pool.prefab, position, rotation, parent);
        }
        else
        {
            Transform obj = spawnableObj.transform;
            obj.SetPositionAndRotation(position, rotation);
            poolObjects[objectTag].Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        activeObjects[objectTag].Add(spawnableObj);
        spawnableObj.GetComponent<IPooledObject>().OnObjectSpawn();
        return spawnableObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
        if (pooledObject == null) 
        {
            Debug.LogWarning("Object don't have Pooled Object interface");
            return;
        }

        Pool pool = pools.Find(p => p.poolTag == pooledObject.Tag);
        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled " + pooledObject.Tag.ToString());
            return;
        }
        
        obj.SetActive(false);
        poolObjects[pool.poolTag].Add(obj);
        activeObjects[pool.poolTag].Remove(obj);
    }

    public void ReturnAllObjectsToPool()
    {
        foreach (Pool pool in pools)
        {
            ReturnObjectsToPool(pool.poolTag);
        }

    }

    public void ReturnObjectsToPool(poolTags objectTag)
    {
        Pool pool = pools.Find(p => p.poolTag == objectTag);
        if (pool == null)
        {
            //Debug.LogWarning("Trying to return objects from pool " + tag + " but there is no pool");
            return;
        } 

        if (!activeObjects.ContainsKey(objectTag))
        {
            //Debug.Log("There is no created pool for objects " + tagName);
            return;
        }

        for (int i = activeObjects[objectTag].Count - 1; i >= 0; i--)
        {
            GameObject obj = activeObjects[objectTag][i];
            obj.SetActive(false);
            activeObjects[objectTag].Remove(obj);
            poolObjects[objectTag].Add(obj);
        }

    }

}


public enum poolTags
{
    smallAsteroid_lvl1 = 0,
    mediumAsteroid_lvl1 = 1,
    bigAsteroid_lvl1 = 2,
    healUpAsteroid = 3,
    ufoLVL1 = 4,
    enemyProjectile = 5,
    playerHitParticle = 6,
    playerProjectile = 7,
    playerProjectileParticle = 8,
    rocket = 9,
    rocketParticle = 10,
    healUpItem = 11,
    rocketItem = 12,
    laserItem = 13,
    ufoLVL2 = 14,
    shieldItem = 15,
    smallAsteroid_lvl2 = 16,
    mediumAsteroid_lvl2 = 17,
    bigAsteroid_lvl2 = 18,
}