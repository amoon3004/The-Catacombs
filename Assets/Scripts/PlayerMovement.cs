using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f; // tiles per second
    public float stepInterval = 0.35f; // seconds between step sounds
    float stepTimer;

    Rigidbody2D rb;
    Vector2 moveDir;
    SpriteRenderer sprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Read the keyboard each frame
    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float x = 0f, y = 0f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) y += 1f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) x -= 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) y -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) x += 1f;

        // make diagonal movement the same speed
        moveDir = new Vector2(x, y).normalized;

        // Footstep sounds while moving
        if (moveDir != Vector2.zero)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                SoundManager.PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // first step plays as soon as player moves
        }

        // Face left or right based on movement. Keep last direction when moving up/down
        if (x != 0f) sprite.flipX = x < 0f;
    }

    //Apply move speed to physics so wall collisions stay solid
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}
