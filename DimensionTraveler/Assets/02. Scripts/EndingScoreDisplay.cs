using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            int score = gameManager.GetScore();
            scoreText.text = "Your Score : " + score.ToString();
        }
        else
        {
            Debug.LogError("GameManager를 찾을 수 없습니다.");
        }
    }
}
