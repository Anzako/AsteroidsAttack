using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }
    [SerializeField] private UIController UIController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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
