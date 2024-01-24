using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private MetaBalls metaballs;
    [SerializeField] private GameObject player;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        // Restart button
        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(OnRestartButton);
        player.GetComponent<HealthController>().Killed += OnPlayerDead;

        // Spawn player
        SpawnPlayer(0);
    }

    private void SpawnPlayer(int metaballID)
    {
        Vector3 pos = metaballs.Position(metaballID);
        float radius = metaballs.Radius(metaballID);

        Vector3 directionFromCenter = CalculateRandomVector3();

        Vector3 spawnPosition = pos + directionFromCenter.normalized * (radius + 5);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - pos);

        player.transform.position = spawnPosition;
        player.transform.rotation = rotation;
        player.GetComponent<HealthBar>().SetHealthToMax();
        player.gameObject.SetActive(true);
    }

    private Vector3 CalculateRandomVector3()
    {
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);

        return new Vector3(randX, randY, randZ);
    }

    public void OnPlayerDead()
    {
        restartButton.gameObject.SetActive(true);
        healthBar.SetActive(false);
    }

    private void OnRestartButton()
    {
        SpawnPlayer(0);
        restartButton.gameObject.SetActive(false);
        healthBar.SetActive(true);
    }
}
