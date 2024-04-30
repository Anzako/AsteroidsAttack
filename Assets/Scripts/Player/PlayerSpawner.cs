using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private int spawnMetaballID = 0;
    public float distanceFromGround;

    private void Awake()
    {
        GameManager.OnStateChanged += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= GameManagerOnStateChanged;
    }

    public void GameManagerOnStateChanged(GameState state)
    {
        player.GetComponent<UIController>().SetActive(state == GameState.Game);
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = Spawner.RandomPositionOnMetaball(spawnMetaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.instance.Position(spawnMetaballID));

        Transform playerTransform = player.transform;
        playerTransform.position = spawnPosition;
        playerTransform.rotation = rotation;

        player.GetComponent<PlayerController>().EnablePlayer();
    }

}
