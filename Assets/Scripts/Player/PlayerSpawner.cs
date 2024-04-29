using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Button restartButton;
    [SerializeField] private Camera deadCamera;

    private int spawnMetaballID = 0;
    public float distanceFromGround;

    public void SpawnPlayer()
    {
        Vector3 spawnPosition = Spawner.RandomPositionOnMetaball(spawnMetaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.instance.Position(spawnMetaballID));

        player.transform.position = spawnPosition;
        player.transform.rotation = rotation;

        EnablePlayer();
    }

    private void EnablePlayer()
    {
        // Turn off camera
        deadCamera.gameObject.SetActive(false);

        // UI things
        restartButton.gameObject.SetActive(false);

        player.gameObject.SetActive(true);
        player.GetComponent<UIController>().SetHealthToMax();
        player.GetComponent<UIController>().SetActiveUI(true);

        // Locking cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void DisablePlayer()
    {
        // Turn off camera
        deadCamera.gameObject.SetActive(true);

        // UI things
        restartButton.gameObject.SetActive(true);
        player.GetComponent<UIController>().SetActiveUI(false);
        
        // Locking cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnPlayerDead()
    {
        DisablePlayer();
        ScoreManager.instance.ResetScore();
    }

}
