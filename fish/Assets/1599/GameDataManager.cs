using System.IO;
using UnityEngine;

// 1. 에러 해결: JSON 변환이 가능하도록 반드시 [System.Serializable]을 붙여야 합니다.
[System.Serializable]
public class SaveData
{
    // 'deathCount'에 대한 정의가 없다는 에러를 이 변수로 해결합니다.
    public int deathCount = 0;
    public int score = 0;
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public GameSettingData gameSettingData;
    public SaveData saveData;

    // 2. 에러 해결: 이미지 속 대소문자와 선언을 맞춰 'isTutorialFinished'를 정확히 선언합니다.
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

    public void LoadPlayerPrefs()
    {
        isTutorialFinished = PlayerPrefs.GetInt("TUTORIAL", 0);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("TUTORIAL", isTutorialFinished);
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs 저장 완료");
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteKey("TUTORIAL");
        LoadPlayerPrefs();

        Debug.Log("PlayerPrefs 삭제 완료");
    }
} // 3. 에러 해결: 파일 끝(EOF) 괄호 쌍을 정확히 닫아주었습니다.
