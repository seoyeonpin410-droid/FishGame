using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("--- High Score UI ---")]
    public TextMeshProUGUI highScoreText; 
    public GameObject highScorePanel;    

    void Start()
    {
        if (highScorePanel != null) highScorePanel.SetActive(false);
    }

    public void OpenHighScore()
    {
        if (highScorePanel != null)
        {
            highScorePanel.SetActive(true);
            ShowHighScoreBoard(); 
        }
    }

    public void CloseHighScore()
    {
        if (highScorePanel != null) highScorePanel.SetActive(false);
    }

   
    public void ShowHighScoreBoard()
    {
        if (highScoreText == null) return;

        if (GameDataManager.Instance != null && GameDataManager.Instance.saveData != null)
        {
            var scoreList = GameDataManager.Instance.saveData.highScores;

          
            if (scoreList == null || scoreList.Count == 0)
            {
                highScoreText.text = "NO RECORD";
                return;
            }

            
            string boardText = "=== SCORE BOARD ===\n\n";
            for (int i = 0; i < scoreList.Count; i++)
            {
                boardText += $"{i + 1}ST : {scoreList[i]}\n";
            }

            highScoreText.text = boardText;
        }
    }

    public void GameStartButton()
    {
        GameManager.Instance.StartGame();
    }

    public void ResetGameData()
    {
       
        GameManager.Instance.currentScore = 0;
        GameManager.Instance.currentLevel = 1;

       
        GameManager.Instance.UpdateScoreUI();

        
        if (GameDataManager.Instance != null && GameDataManager.Instance.saveData != null)
        {
            GameDataManager.Instance.saveData.score = 0;

            

            GameDataManager.Instance.SaveJsonData();
        }
    }

    public void GameExitButton()
    {
#if UNITY_EDITOR
   
        UnityEditor.EditorApplication.isPlaying = false;
#else
        
#endif

        Debug.Log("[GAME EXIT] Game has been terminated.");
    }

}