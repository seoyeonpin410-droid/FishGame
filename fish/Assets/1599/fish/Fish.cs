using UnityEngine;

public class Fish : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float changeDirTime = 2f;

    private Vector2 moveDirection;
    private float timer;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        GetNewDirection();
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= changeDirTime)
        {
            GetNewDirection();
            timer = 0f;
        }
    }

    void GetNewDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        if (moveDirection.x > 0)
            sr.flipX = true;
        else if (moveDirection.x < 0)
            sr.flipX = false;
    }

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name + " 클릭 성공! 물고기 획득.");

       
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

       
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(10);
        }

   
        if (FishManager.Instance != null)
        {
            FishManager.Instance.OnFishDestroyed();
        }

        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (FishManager.Instance != null && collision == FishManager.Instance.waterCollider)
        {
            moveDirection = -moveDirection;

            if (moveDirection.x > 0)
                sr.flipX = true;
            else if (moveDirection.x < 0)
                sr.flipX = false;
        }
    }
}