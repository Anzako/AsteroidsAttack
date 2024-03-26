using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private MetaBalls metaballs;
    [SerializeField] private GameObject player;
    [SerializeField] private Button restartButton;
    [SerializeField] private Camera deadCamera;

    public float distanceFromGround;

    // Start is called before the first frame update
    void Start()
    {
        // Turn off camera
        deadCamera.gameObject.SetActive(false);

        // Restart button
        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(OnRestartButton);
        player.GetComponent<PlayerHealth>().Killed += OnPlayerDead;

        // Spawn player
        SpawnPlayer(0);
    }

    private void SpawnPlayer(int metaballID)
    {
        Vector3 pos = metaballs.Position(metaballID);
        float radius = metaballs.Radius(metaballID);

        Vector3 directionFromCenter = CalculateRandomVector3();

        Vector3 spawnPosition = pos + directionFromCenter.normalized * (radius / 2);

        while (metaballs.CalculateScalarFieldValue(spawnPosition) > 0.5f)
        {
            Debug.Log("Zle2");
            directionFromCenter = CalculateRandomVector3();
            spawnPosition = pos + directionFromCenter.normalized * (radius / 2);
        }
        
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - pos);

        player.transform.position = spawnPosition;
        player.transform.rotation = rotation;
        player.GetComponent<UIController>().SetHealthToMax();
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
        deadCamera.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        UIController uicontroller = player.GetComponent<UIController>();
        uicontroller.SetActiveUI(false);
        
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.ResetScore();
    }

    private void OnRestartButton()
    {
        restartButton.gameObject.SetActive(false);
        deadCamera.gameObject.SetActive(false);

        UIController uicontroller = player.GetComponent<UIController>();
        uicontroller.SetActiveUI(true);

        SpawnPlayer(0);
    }
}
