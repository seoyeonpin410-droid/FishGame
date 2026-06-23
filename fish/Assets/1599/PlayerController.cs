using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;
    public float frameTime = 0.15f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;
    public int playerHP = 0;
    public int playerAttack = 0;

    private void Awake()
    {
        // 내 오브젝트에 붙은 컴포넌트를 가져오는 필수 작업만 Awake에 남겨둡니다.
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        currentSprites = spriteDown;
        if (currentSprites != null && currentSprites.Length > 0)
        {
            sr.sprite = currentSprites[0];
        }
    }

    void Start()
    {
        // 1. 모든 Awake가 끝난 후, 안전하게 데이터 매니저에서 값을 가져옵니다.
        if (GameDataManager.Instance != null)
        {
            moveSpeed = GameDataManager.Instance.GetPlayerMoveSpeed();
            playerHP = GameDataManager.Instance.GetPlayerHp();
            playerAttack = GameDataManager.Instance.GetPlayerAttack();

            // 2. 튜토리얼 체크 로직 진행
            if (GameDataManager.Instance.isTutorialFinished == 0)
            {
                // 튜토리얼 안 했을 경우 튜토리얼 오픈
                Debug.Log("튜토리얼 오픈!");
                GameDataManager.Instance.isTutorialFinished = 1;
            }
        }
        else
        {
            Debug.LogError("하이어라키 창에 GameDataManager 오브젝트가 배치되어 있는지 확인해주세요!");
        }

        // 입력 속도 동기화 재설정
        velocity = input.normalized * moveSpeed;
    }

    private void Update()
    {
        if (input.sqrMagnitude <= 0.01f)
        {
            frameIndex = 0;
            if (currentSprites != null && currentSprites.Length > 0)
            {
                sr.sprite = currentSprites[frameIndex];
            }
            return;
        }

        timer += Time.deltaTime;

        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;

            if (currentSprites != null && frameIndex >= currentSprites.Length)
            {
                frameIndex = 0;
            }

            if (currentSprites != null && currentSprites.Length > 0)
            {
                sr.sprite = currentSprites[frameIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void ChangeSprites(Sprite[] newSprites)
    {
        if (currentSprites == newSprites)
        {
            return;
        }

        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        if (currentSprites != null && currentSprites.Length > 0)
        {
            sr.sprite = currentSprites[frameIndex];
        }
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                {
                    ChangeSprites(spriteRight);
                }
                else
                {
                    ChangeSprites(spriteLeft);
                }
            }
            else
            {
                if (input.y > 0)
                {
                    ChangeSprites(spriteUp);
                }
                else
                {
                    ChangeSprites(spriteDown);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}