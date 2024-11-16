using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI scoreText;

    private int currentScore = 0;

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
            Debug.LogError("score Text is not assigned!");
            return;
        }

        scoreText.text = $"Score: {currentScore}";
    }
}
