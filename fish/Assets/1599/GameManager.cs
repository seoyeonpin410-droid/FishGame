using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindAnyObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    public string titleSceneName = "TitleScene";
    public string gameSceneName = "GameScene";

    private TextMeshProUGUI tmpScoreText;
    private Text legacyScoreText;

    [Header("--- Level System ---")]
    public int currentLevel = 1;
    public int currentScore = 0;
    private int scorePerClick = 10;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetupComponents();
        UpdateScoreUI();
    }

    private void SetupComponents()
    {
        if (tmpScoreText == null)
        {
            var tmpTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Exclude);
            foreach (var t in tmpTexts)
            {
                if (t.name.ToLower().Contains("score") || t.text.ToLower().Contains("score"))
                {
                    tmpScoreText = t;
                    break;
                }
            }
        }

        if (legacyScoreText == null)
        {
            var legacyTexts = Object.FindObjectsByType<Text>(FindObjectsInactive.Exclude);
            foreach (var t in legacyTexts)
            {
                if (t.name.ToLower().Contains("score") || t.text.ToLower().Contains("score"))
                {
                    legacyScoreText = t;
                    break;
                }
            }
        }
    }

    public void AddScore(int amount)
    {
        SetupComponents();

        int dynamicAmount = scorePerClick;
        currentScore += dynamicAmount;

        if (GameDataManager.Instance != null && GameDataManager.Instance.saveData != null)
        {
            GameDataManager.Instance.saveData.score = currentScore;
            // 실시간 낚시 중에는 임시 데이터만 저장
            GameDataManager.Instance.SaveJsonData();
        }

        Debug.Log($"[점수 획득] +{dynamicAmount}점! | 현재 총 점수: {currentScore} (레벨: {currentLevel})");

        CheckLevelUp();
        UpdateScoreUI();
    }

    private void CheckLevelUp()
    {
        if (currentScore >= 500 && currentLevel < 3)
        {
            currentLevel = 3;
            scorePerClick = 100;
        }
        else if (currentScore >= 100 && currentLevel < 2)
        {
            currentLevel = 2;
            scorePerClick = 50;
        }
    }

    public void UpdateScoreUI()
    {
        int displayScore = currentScore;

        if (GameDataManager.Instance != null && GameDataManager.Instance.saveData != null)
        {
            displayScore = GameDataManager.Instance.saveData.score;
            currentScore = displayScore;
        }

        string scoreString = "Score: " + displayScore;

        if (tmpScoreText != null) legacyScoreText = null;

        if (tmpScoreText != null) tmpScoreText.text = scoreString;
        if (legacyScoreText != null) legacyScoreText.text = scoreString;
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupComponents();
        UpdateScoreUI();
    }

    public void StartGame()
    {
        
        currentScore = 0;
        SceneManager.LoadScene(gameSceneName);
    }

    public void GoTitle() { SceneManager.LoadScene(titleSceneName); }

    public void GameOver()
    {
       
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.SaveGameResult();
        }
        GoTitle();
    }
}