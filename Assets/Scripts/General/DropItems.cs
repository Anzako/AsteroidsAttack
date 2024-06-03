using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{
    [SerializeField] private List<ItemDropChance> dropableItems;
    private int totalWeight = 100;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GetDropItem();
        }
    }

    public GameObject GetDropItem()
    {
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var obj in dropableItems)
        {
            cumulativeWeight += obj.droppingChance;
            if (randomValue <= cumulativeWeight)
            {
                Debug.Log(randomValue + " wylosowano " + obj.dropableObject.name);
                return obj.dropableObject;
            }
        }
        Debug.Log(randomValue + " wylosowano nic");
        return null;
    }

}

[System.Serializable]
public class ItemDropChance
{
    public GameObject dropableObject;
    [Range(0, 100)] public int droppingChance;
}
