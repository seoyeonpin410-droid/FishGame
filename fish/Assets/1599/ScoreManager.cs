using UnityEngine;
using TMPro; // TextMeshPro를 사용할 경우 필요 (일반 Text라면 using UnityEngine.UI;)

public class ScoreManager : MonoBehaviour
{
    // 싱글톤 패턴을 적용하여 어디서든 쉽게 점수를 올릴 수 있도록 합니다.
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText; // UI 텍스트 연결용
    private int currentScore = 0;

    private void Awake()
    {
        // 싱글톤 초기화
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

    // 점수를 추가하는 함수
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    // UI 텍스트를 갱신하는 함수
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}