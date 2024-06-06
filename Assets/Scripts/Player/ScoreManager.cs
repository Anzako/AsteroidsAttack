using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private UIController UIController;

    private int Score;

    public float comboDuration = 4f;
    public float comboDecayRate = 1f;

    private int currentCombo = 1;
    public float comboTimer = 0f;
    private int enemiesKilled = 0;

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
        Score += score * currentCombo;
        UIController.SetScore(Score);
        
        enemiesKilled++;
        comboTimer = comboDuration;

        if (enemiesKilled >= currentCombo + 1)
        {
            IncreaseCombo();
        }
    }

    private void ResetCombo()
    {
        currentCombo = 1;
        comboTimer = 0.0f;
        enemiesKilled = 0;
        comboDecayRate = 1f;
        UIController.SetCombo(currentCombo);
    }

    private void IncreaseCombo()
    {
        currentCombo += 1;
        comboDecayRate = 1f + (currentCombo - 2) / 10f;
        enemiesKilled = 0;
        UIController.SetCombo(currentCombo);
    }

    public void ResetScore()
    {
        Score = 0;
        UIController.SetScore(Score);
    }

    public int GetScore()
    {
        return Score;
    }

}
