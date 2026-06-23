using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string titleSceneName = "TitleScene";
    public string gameSceneName = "GameScene";

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

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void GameOver()
    {
        GameDataManager.Instance.SaveGameResult();
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
        }
    }
}