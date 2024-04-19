using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
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
        Vector3 spawnPosition = Spawner.SpawnPosition(metaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.instance.Position(metaballID));

        player.transform.position = spawnPosition;
        player.transform.rotation = rotation;
        player.GetComponent<UIController>().SetHealthToMax();
        player.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void OnPlayerDead()
    {
        deadCamera.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        UIController uicontroller = player.GetComponent<UIController>();
        uicontroller.SetActiveUI(false);

        Cursor.lockState = CursorLockMode.None;
        ScoreManager.instance.ResetScore();
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
