using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("--- High Score UI ---")]
    public TextMeshProUGUI highScoreText; // 스코어보드용 TMP 텍스트 창 연결
    public GameObject highScorePanel;    // 팝업 패널 오브젝트 전체 연결

    void Start()
    {
        if (highScorePanel != null) highScorePanel.SetActive(false);
    }

    public void OpenHighScore()
    {
        if (highScorePanel != null)
        {
            highScorePanel.SetActive(true);
            ShowHighScoreBoard(); // 화면이 열리는 타이밍에 진짜 리스트 데이터를 바인딩합니다.
        }
    }

    public void CloseHighScore()
    {
        if (highScorePanel != null) highScorePanel.SetActive(false);
    }

    // --- [의도 반영] 저장소 리스트 데이터를 긁어와 등수별로 가공하는 함수 ---
    public void ShowHighScoreBoard()
    {
        if (highScoreText == null) return;

        if (GameDataManager.Instance != null && GameDataManager.Instance.saveData != null)
        {
            var scoreList = GameDataManager.Instance.saveData.highScores;

            // 기록이 아무것도 없을 때 처리
            if (scoreList == null || scoreList.Count == 0)
            {
                highScoreText.text = "NO RECORD";
                return;
            }

            // [폰트 안전] 영어와 숫자로 높은 등수 데이터를 한 줄씩 개행하며 추가
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
        // 1. GameManager에 남아있는 현재 판 점수와 레벨만 태초의 상태로 리셋
        GameManager.Instance.currentScore = 0;
        GameManager.Instance.currentLevel = 1;

        // 화면 UI 텍스트도 즉시 0점(Lv.1)으로 안전하게 갱신
        GameManager.Instance.UpdateScoreUI();

        // 2. 세이브 데이터(GameDataManager)가 존재한다면 현재 판 점수 데이터만 0으로 원상복구
        if (GameDataManager.Instance != null && GameDataManager.Instance.saveData != null)
        {
            GameDataManager.Instance.saveData.score = 0;

            // [의도 반영] 역대 스코어보드 기록(highScores)은 절대 건드리지 않고 보존합니다!
            // GameDataManager.Instance.saveData.highScores.Clear(); ◀ 이 쓰레기 줄을 삭제했습니다.

            GameDataManager.Instance.SaveJsonData();
        }
    }

    public void GameExitButton()
    {
#if UNITY_EDITOR
        // 유니티 에디터에서 플레이 중일 때는 재생이 멈추도록 처리
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 실제 빌드된 PC/모바일 게임 환경에서 게임이 완벽히 꺼지도록 처리
        Application.Quit();
#endif

        Debug.Log("[GAME EXIT] Game has been terminated.");
    }

}