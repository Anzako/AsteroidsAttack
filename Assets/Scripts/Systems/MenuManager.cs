using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private Camera menuCamera;

    private void Awake()
    {
        GameManager.OnStateChanged += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState state)
    {
        startButton.SetActive(state == GameState.Menu);
        menuCamera.gameObject.SetActive(state != GameState.Game);
    }

    public void StartPressed()
    {
        GameManager.Instance.ChangeState(GameState.Game);
    }

}
