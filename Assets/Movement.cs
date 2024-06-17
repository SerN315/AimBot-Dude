using UnityEngine;
using UnityEngine.UI;

public class JoystickMove : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed, jumpForce;
    public Joystick movementJoystick;
    public Button jumpButton;
    private bool isGrounded;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        // Ensure the button is found
        if (jumpButton != null)
        {
            // Add a listener to the button
            jumpButton.onClick.AddListener(OnJumpButtonClick);
        }
        else
        {
            Debug.LogError("Jump button not assigned!");
        }
    }

    void FixedUpdate()
    {
        float movementInputX = movementJoystick.Direction.x;

        if (movementInputX < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movementInputX > 0.01f)
        {
            transform.localScale = Vector3.one;
        }

        body.velocity = new Vector2(movementInputX * speed * Time.deltaTime, body.velocity.y);
    }

    void OnJumpButtonClick()
    {
        Debug.Log("Jump button clicked!");
        if (isGrounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != gameObject) // Ensure the player doesn't collide with itself
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != gameObject) // Ensure the player doesn't collide with itself
        {
            isGrounded = false;
        }
    }
}
    