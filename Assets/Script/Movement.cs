using UnityEngine;
using UnityEngine.UI;

public class JoystickMove : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed, jumpForce;
    public Joystick movementJoystick;
    public Button jumpButton;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    private Animator anim;
    public bool isWalled;
    public Transform gunHand;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
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

    void Update()
    {
        anim.SetBool("Jump", !isGrounded());
    }

    void FixedUpdate()
    {
        float movementInputX = movementJoystick.Direction.x;

        // Check if there's a wall on the left or right
        bool isWallOnLeft = IsWallOnDirection(Vector2.left);
        bool isWallOnRight = IsWallOnDirection(Vector2.right);

        // Adjust movement direction based on wall presence
        if (isWallOnLeft && movementInputX < 0)
        {
            movementInputX = 0; // Disable left movement
        }
        else if (isWallOnRight && movementInputX > 0)
        {
            movementInputX = 0; // Disable right movement
        }

        // Find the nearest enemy
        GameObject nearestEnemy = FindNearestEnemy();
        Vector3 enemyDirection = Vector3.zero;

        if (nearestEnemy != null)
        {
            // Calculate the direction to the nearest enemy, ignoring the vertical component
            enemyDirection = nearestEnemy.transform.position - transform.position;
            enemyDirection.y = 0; // Ignore the vertical component
            enemyDirection.Normalize();

            // Flip the character to face the nearest enemy
            if (enemyDirection.x > 0)
            {
                transform.localScale = new Vector3(5, 5, 1);
                gunHand.localScale = new Vector3(1, 1, 1);
            }
            else if (enemyDirection.x < 0)
            {
                transform.localScale = new Vector3(-5, 5, 1);
                gunHand.localScale = new Vector3(-1, 1, 1);
            }
        }

        // Determine movement direction based on user input
        Vector3 movementDirection = new Vector3(movementInputX, 0, 0).normalized;

        // Flip character based on movement direction
        if (movementInputX < -0.01f && nearestEnemy == null)
        {
            transform.localScale = new Vector3(-5, 5, 1);
        }
        else if (movementInputX > 0.01f && nearestEnemy == null)
        {
            transform.localScale = new Vector3(5, 5, 1);
        }

        // If moving opposite to the enemy, move backward
        bool isOppositeDirection = Vector3.Dot(movementDirection, enemyDirection) < 0;
        if (isOppositeDirection)
        {
            movementDirection *= -1;
        }

        // Set animation for running
        if (Mathf.Abs(movementDirection.x) > 0 && isGrounded())
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        // Prevent movement when not grounded and walled
        if (!isGrounded() && isWalled)
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }
        else
        {
            body.velocity = new Vector2(movementInputX * speed * Time.deltaTime, body.velocity.y);
        }
    }

    void OnJumpButtonClick()
    {
        Debug.Log("Jump button clicked!");
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }

    public bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if colliding with the ground layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("groundLayer"))
        {
            isWalled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if no longer colliding with the ground layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("groundLayer"))
        {
            isWalled = false;
        }
    }

    bool IsWallOnDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, castDistance, groundLayer);
        return hit.collider != null;
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
