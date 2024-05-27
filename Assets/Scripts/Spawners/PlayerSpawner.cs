using UnityEngine;

public class PlayerSpawner : Singleton<PlayerSpawner>
{
    [SerializeField] private GameObject player;

    private int spawnMetaballID = 0;

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = Spawner.RandomPositionOnMetaball(spawnMetaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.Instance.Position(spawnMetaballID));

        player.transform.SetPositionAndRotation(spawnPosition, rotation);

        player.GetComponent<PlayerController>().EnablePlayer();
    }

}
