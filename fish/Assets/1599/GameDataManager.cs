using System.IO;
using System.Collections.Generic; 
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int deathCount = 0;
    public int score = 0;

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

    
    public void SaveGameResult()
    {
        saveData.deathCount++;

        
        if (GameManager.Instance != null)
        {
            saveData.highScores.Add(GameManager.Instance.currentScore);
        }

     
        saveData.highScores.Sort((a, b) => b.CompareTo(a));

    
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