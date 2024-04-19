using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerSpawner pSpawner;
    [SerializeField] private AsteroidsSpawner aSpawner;
    [SerializeField] private PlayerHealth pHealth;
    [SerializeField] private Button restartButton;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        pSpawner.SpawnPlayer(0);
        pHealth.Killed += GameOver;
        aSpawner.SpawnAsteroids(5);
        restartButton.onClick.AddListener(aSpawner.ResetGame);
    }


    public void GameOver()
    {
        pSpawner.OnPlayerDead();
    }
}
