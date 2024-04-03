using UnityEngine;

public interface IPooledObject
{
    string Tag
    {
        get; set;
    }

    void OnObjectSpawn();
}
