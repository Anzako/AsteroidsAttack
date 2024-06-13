using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private UIController playerHUD;

    [SerializeField] private TMP_Text scoreText;
    public int score;
    private int scoreOnCombo;
    private int endGameScore;

    public float comboDuration = 4f;
    public float comboDecayRate = 1f;

    private int currentCombo = 1;
    public float comboTimer = 0f;
    private int enemiesKilled = 0;

    protected override void Awake()
    {
        base.Awake();
        GameManager.OnStateChanged += GameManagerOnStateChanged;
    }

    private void Start()
    {
        playerHUD = GameManager.GetPlayerController().GetComponent<UIController>();
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState state)
    {
        if (state == GameState.EndGame)
        {
            ResetCombo();
            SetEndGameScore();
        }
    }

    private void Update()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime * comboDecayRate;
            if ( comboTimer <= 0 )
            {
                ResetCombo();
            }
        }
    }

    public void AddScore(int score)
    {
        scoreOnCombo += score * currentCombo;
        playerHUD.SetScoreOnCombo(scoreOnCombo);
        
        enemiesKilled++;
        comboTimer = comboDuration;

        if (enemiesKilled >= currentCombo + 1)
        {
            IncreaseCombo();
        }
    }

    public void ResetCombo()
    {
        // Reseting combo variables
        currentCombo = 1;
        comboTimer = 0.0f;
        enemiesKilled = 0;
        comboDecayRate = 1f;

        // Adding combo score to overall score
        score += scoreOnCombo;
        scoreOnCombo = 0;

        // Update UI
        playerHUD.SetScore(score);
        playerHUD.SetScoreOnCombo(scoreOnCombo);
        playerHUD.SetCombo(currentCombo);
    }

    private void IncreaseCombo()
    {
        currentCombo += 1;
        comboDecayRate = 1f + (currentCombo - 2) / 10f;
        enemiesKilled = 0;
        playerHUD.SetCombo(currentCombo);
    }

    public void ResetScore()
    {
        ResetCombo();

        score = 0;
        playerHUD.SetScore(score);
    }

    public int GetEndGameScore()
    {
        return endGameScore;
    }

    public void SetEndGameScore()
    {
        endGameScore = score;
        scoreText.text = "Your Score: " + endGameScore;
    }

}
