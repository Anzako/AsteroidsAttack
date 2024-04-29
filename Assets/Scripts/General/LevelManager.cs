using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerSpawner pSpawner;
    [SerializeField] private AsteroidsSpawner aSpawner;
    [SerializeField] private PlayerHealth pHealth;
    [SerializeField] private Button restartButton;
    public int amount;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        pHealth.Killed += GameOver;
        restartButton.onClick.AddListener(RestartGame);

        StartGame();
    }

    private void StartGame()
    {
        pSpawner.SpawnPlayer();
        aSpawner.SpawnAsteroids(amount);
    }

    public void GameOver()
    {
        pSpawner.OnPlayerDead();
    }

    public void RestartGame()
    {
        aSpawner.ReturnAsteroidsToPool();
        StartGame();
    }
}
