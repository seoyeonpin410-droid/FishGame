using UnityEngine;

public class FishManager : MonoBehaviour
{
    // 싱글톤 패턴 적용 (물고기들이 매니저를 쉽게 찾아서 보고할 수 있게 함)
    public static FishManager Instance { get; private set; }

    public GameObject[] fishPrefabs;
    public int maxFishCount = 10;     // 화면에 유지될 최대 물고기 수
    public Collider2D waterCollider;

    private int currentFishCount = 0; // 현재 화면에 있는 물고기 수

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (fishPrefabs == null || fishPrefabs.Length == 0 || waterCollider == null)
        {
            Debug.LogError("설정이 누락되었습니다! 인스펙터를 확인하세요.");
            return;
        }

        // 시작할 때 최대치만큼 꽉 채우기
        for (int i = 0; i < maxFishCount; i++)
        {
            SpawnFish();
        }
    }

    void SpawnFish()
    {
        Bounds bounds = waterCollider.bounds;
        Vector2 spawnPosition = Vector2.zero;
        bool validPosition = false;

        for (int i = 0; i < 10; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            spawnPosition = new Vector2(randomX, randomY);

            if (waterCollider.OverlapPoint(spawnPosition))
            {
                validPosition = true;
                break;
            }
        }

        if (validPosition)
        {
            int randomIndex = Random.Range(0, fishPrefabs.Length);
            GameObject fish = Instantiate(fishPrefabs[randomIndex], spawnPosition, Quaternion.identity);
            currentFishCount++;
        }
    }

    // ★ [새로 추가] 물고기가 죽을 때(잡힐 때) 매니저에게 알리는 함수
    public void OnFishDestroyed()
    {
        currentFishCount--;

      
        if (currentFishCount < maxFishCount)
        {
            SpawnFish();
        }
    }
}