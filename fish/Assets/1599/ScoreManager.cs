using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
   
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText; 
    private int currentScore = 0;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

  
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }


    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}