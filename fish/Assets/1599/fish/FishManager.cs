using UnityEngine;

public class FishManager : MonoBehaviour
{
   
    public static FishManager Instance { get; private set; }

    public GameObject[] fishPrefabs;
    public int maxFishCount = 10;     
    public Collider2D waterCollider;

    private int currentFishCount = 0; 

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
            Debug.LogError("설정이 누락확인");
            return;
        }

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

    // 물고기 잡을때마다 알려줌
    public void OnFishDestroyed()
    {
        currentFishCount--;

      
        if (currentFishCount < maxFishCount)
        {
            SpawnFish();
        }
    }
}