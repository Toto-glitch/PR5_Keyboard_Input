using TMPro;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 8f;

    private float currentSpeed;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI positionText;

    private bool isGrounded  = true;
    private bool isSprinting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentSpeed = normalSpeed;

        UpdateUI();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleSprint();
        HandleSpecialActions();
        UpdateUI();
    }

    void HandleMovement()
    {
        float moveX = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
            spriteRenderer.flipY = false;
        }

        Vector2 movement = new Vector2(moveX, 0f).normalized * currentSpeed;
        rb.linearVelocity = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    void HandleSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            currentSpeed = normalSpeed;
            isSprinting = false;
        }
    }

    void HandleSpecialActions()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Действие: Использовать предмет (E)");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Действие: Атака (Q)");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Действие: Перезарядка (R)");
        }
    }

    void UpdateUI()
    {
        if (speedText != null)
        {
            string speedMove = isSprinting ? "СПРИНТ" : "НОРМА";
            speedText.text = $"Скорость: {speedMove} ({currentSpeed:F1})";
        }

        if (jumpText != null)
        {
            string jumpState = isGrounded ? "ГОТОВ К ПРЫЖКУ" : "В ПОЛЕТЕ ";
            jumpText.text = $"прыжок: {jumpState}";
        }

        if (positionText != null)
        {
            positionText.text = $"Позиция: X {transform.position.x:F1} Y {transform.position.y:F1}";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Столкнулся с {collision.gameObject.name}, тег: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
