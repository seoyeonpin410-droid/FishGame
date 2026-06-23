using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string titleSceneName = "TitleScene";
    public string gameSceneName = "GameScene";

    // ★ [이곳을 수정] 다른 스크립트에서 텍스트를 넣어줄 수 있도록 public으로 바꿉니다!
    // (현재 텍스트 타입에 맞춰서 툴이 자동 인식하도록 우선 기본 TMPUGUI로 지정합니다)
    [HideInInspector] public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void GameOver()
    {
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.SaveGameResult();
        }
        GoTitle();
    }

    public void GoTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void AddScore(int amount)
    {
        if (GameDataManager.Instance != null && GameDataManager.Instance.saveData != null)
        {
            GameDataManager.Instance.saveData.score += amount;
            Debug.Log("물고기 낚시 성공! + " + amount + "점 | 현재 총 점수: " + GameDataManager.Instance.saveData.score);

            GameDataManager.Instance.SaveJsonData();
            UpdateScoreUI();
        }
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null && GameDataManager.Instance != null && GameDataManager.Instance.saveData != null)
        {
            scoreText.text = "Score: " + GameDataManager.Instance.saveData.score;
        }
    }
}