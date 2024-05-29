using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private UIController UIController;

    private int Score;

    public void AddScore(int score)
    {
        Score += score;
        UIController.SetScore(Score);
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
