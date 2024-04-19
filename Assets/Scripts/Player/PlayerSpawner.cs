using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Button restartButton;
    [SerializeField] private Camera deadCamera;

    public float distanceFromGround;

    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartButton);
    }

    public void SpawnPlayer(int metaballID)
    {
        Vector3 spawnPosition = Spawner.SpawnPosition(metaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.instance.Position(metaballID));

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
        
        player.GetComponent<UIController>().SetHealthToMax();
        player.gameObject.SetActive(true);

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

    private void OnRestartButton()
    {
        SpawnPlayer(0);
    }

}
