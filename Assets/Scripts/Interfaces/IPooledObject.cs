using UnityEngine;

public interface IPooledObject
{
    poolTags Tag
    {
        get;
    }

    void OnObjectSpawn();
}
