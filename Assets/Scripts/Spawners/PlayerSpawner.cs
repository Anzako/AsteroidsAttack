using UnityEngine;

public class PlayerSpawner : Singleton<PlayerSpawner>
{
    [SerializeField] private GameObject player;

    private int spawnMetaballID = 0;

    private void Start()
    {
        player.GetComponent<UIController>().SetActive(false);
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = Spawner.RandomPositionOnMetaball(spawnMetaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.Instance.Position(spawnMetaballID));

        Transform playerTransform = player.transform;
        playerTransform.position = spawnPosition;
        playerTransform.rotation = rotation;

        player.GetComponent<PlayerController>().EnablePlayer();
    }

}
