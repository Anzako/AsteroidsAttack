using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{
    [SerializeField] private List<ItemDropChance> dropableItems;
    private int totalWeight = 100;

    public void DropItem()
    {
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var obj in dropableItems)
        {
            cumulativeWeight += obj.droppingChance;
            if (randomValue <= cumulativeWeight)
            {
                ObjectPooler.Instance.SpawnObject(obj.dropableObject, transform.position, transform.rotation);
                return;
            }
        }
    }

}

[System.Serializable]
public class ItemDropChance
{
    public poolTags dropableObject;
    [Range(0, 100)] public int droppingChance;
}
