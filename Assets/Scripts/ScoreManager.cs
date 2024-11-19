using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI scoreText;

    private int currentScore;

    void Start()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    public void AddScore(int score)
    {
        currentScore += score;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text is not assigned!");
            return;
        }

        scoreText.text = $"Points: {currentScore}";
    }
}
