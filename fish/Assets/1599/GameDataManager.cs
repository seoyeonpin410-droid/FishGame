using System.IO;
using System.Collections.Generic; // ◀ 리스트 사용을 위해 추가
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int deathCount = 0;
    public int score = 0;
    // --- [의도 반영] 역대 기록을 누적할 리스트 추가 ---
    public List<int> highScores = new List<int>();
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public GameSettingData gameSettingData;
    public SaveData saveData;

    public int isTutorialFinished;
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/saveData.json";
            LoadJsonData();
            LoadPlayerPrefs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetPlayerHp()
    {
        int baseHp = gameSettingData.startHp;
        int bonusHp = gameSettingData.hpBonusPerDeath;
        return baseHp + bonusHp * saveData.deathCount;
    }

    public int GetPlayerAttack()
    {
        int baseAttack = gameSettingData.startAttack;
        int bonusAttack = gameSettingData.atkBonusPerDeath;
        return baseAttack + bonusAttack * saveData.deathCount;
    }

    public float GetPlayerMoveSpeed()
    {
        return gameSettingData.playerMoveSpeed;
    }

    // --- [의도 반영] 게임이 끝났을 때 점수 기록 연동 확장 ---
    public void SaveGameResult()
    {
        saveData.deathCount++;

        // 1. 현재 GameManager에 쌓인 진짜 점수를 데이터 리스트에 추가
        if (GameManager.Instance != null)
        {
            saveData.highScores.Add(GameManager.Instance.currentScore);
        }

        // 2. 리스트를 높은 순서(내림차순)대로 정렬
        saveData.highScores.Sort((a, b) => b.CompareTo(a));

        // 3. 기록이 너무 무한대로 쌓이지 않게 상위 5개만 컷트
        if (saveData.highScores.Count > 5)
        {
            saveData.highScores.RemoveRange(5, saveData.highScores.Count - 5);
        }

        SaveJsonData();
    }

    public void SaveJsonData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("JSON 저장 완료: " + savePath);
    }

    public void LoadJsonData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);

            // 파일 리스트가 비어있다면 새로 할당 방지
            if (saveData.highScores == null) saveData.highScores = new List<int>();
        }
        else
        {
            saveData = new SaveData();
            SaveJsonData();
        }
    }

    public void DeleteJsonData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        saveData = new SaveData();
        SaveJsonData();
        Debug.Log("JSON 저장 데이터 삭제");
    }

    public void LoadPlayerPrefs() { isTutorialFinished = PlayerPrefs.GetInt("TUTORIAL", 0); }
    public void SavePlayerPrefs() { PlayerPrefs.SetInt("TUTORIAL", isTutorialFinished); PlayerPrefs.Save(); }
    public void DeletePlayerPrefs() { PlayerPrefs.DeleteKey("TUTORIAL"); LoadPlayerPrefs(); }
}