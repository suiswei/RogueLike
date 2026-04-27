using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f);
    }

    void Update()
    {
        InputManagement();
    }

    void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        // Get the current keyboard instance
        Keyboard kb = Keyboard.current;

        // If no keyboard is connected, don't do anything
        if (kb == null) return;

        // Directly read the X and Y axes based on which keys are held
        float moveX = 0;
        float moveY = 0;

        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) moveY = 1;
        else if (kb.sKey.isPressed || kb.downArrowKey.isPressed) moveY = -1;

        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveX = 1;
        else if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveX = -1;

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);
        }
        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector);
        }

        if(moveDir.x != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}