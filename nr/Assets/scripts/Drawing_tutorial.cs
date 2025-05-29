using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class PlayerDrawingSystem : MonoBehaviour
{
    // Настройки движения
    public float moveSpeed = 10f;
    public float jumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded;

    // Настройки рисования
    public Camera playerCamera;
    public RectTransform drawingPanel;
    public GameObject brushPrefab;
    public float brushWidth = 0.1f;

    private LineRenderer currentLine;
    private bool isDrawing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор
    }

    void Update()
    {
        // Переключение между режимами
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleDrawingMode();
        }

        // Рисование
        if (isDrawing)
        {
            HandleDrawing();
        }
    }

    void FixedUpdate()
    {
        if (!isDrawing)
        {
            HandleMovement();
        }
    }

    void ToggleDrawingMode()
    {
        isDrawing = !isDrawing;
        Cursor.lockState = isDrawing ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isDrawing;
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.AddForce(move * moveSpeed);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void HandleDrawing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartNewLine();
        }

        if (Input.GetMouseButton(0) && currentLine != null)
        {
            AddPointToLine();
        }
    }

    void StartNewLine()
    {
        GameObject brush = Instantiate(brushPrefab, drawingPanel);
        currentLine = brush.GetComponent<LineRenderer>();
        currentLine.startWidth = brushWidth;
        currentLine.endWidth = brushWidth;
        currentLine.positionCount = 0;
    }

    void AddPointToLine()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            drawingPanel,
            Input.mousePosition,
            playerCamera,
            out localPoint);

        currentLine.positionCount++;
        currentLine.SetPosition(currentLine.positionCount - 1, localPoint);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}