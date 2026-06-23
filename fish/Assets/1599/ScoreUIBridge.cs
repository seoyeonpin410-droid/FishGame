using UnityEngine;
using TMPro;

public class ScoreUIBridge : MonoBehaviour
{
    // 인스펙터 창에서 드래그 앤 드롭으로 연결하는 구멍
    public TextMeshProUGUI scoreTextUGUI;

    void Start()
    {
        // 게임이 시작되자마자 문이 열린 GameManager.Instance.scoreText에 다이렉트로 주소 배달
        if (GameManager.Instance != null && scoreTextUGUI != null)
        {
            GameManager.Instance.scoreText = scoreTextUGUI;

            // 연결 완료되었으니 00으로 멈춰있던 화면을 현재 세이브 점수로 갱신!
            GameManager.Instance.UpdateScoreUI();
        }
    }
}