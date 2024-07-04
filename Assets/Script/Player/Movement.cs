using UnityEngine;
using UnityEngine.UI;

public class JoystickMove : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed, jumpForce;
    public float baseSpeed = 50f;
    private float speedMultiplier = 10f;
    public Joystick movementJoystick;
    public Button jumpButton;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    private Animator anim;
    // public bool isWalled;
    public Transform gunHand;
    [SerializeField]
    private float offset = 1f; // Adjust this value as needed


    void Start()
    {
        speed = baseSpeed * speedMultiplier;
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
        speed = baseSpeed * speedMultiplier;
        anim.SetBool("Jump", !isGrounded());
    }

    void FixedUpdate()
    {
        float movementInputX = movementJoystick.Direction.x;

// Check if there's a wall on the left or right
bool isWallOnLeft = CheckWall(Vector2.left);
bool isWallOnRight = CheckWall(Vector2.right);

// Adjust movement direction based on wall presence
if (isWallOnLeft && movementInputX < 0)
{
    movementInputX = Mathf.Clamp(movementInputX, 0, 1); // Limit left movement
}
else if (isWallOnRight && movementInputX > 0)
{
    movementInputX = Mathf.Clamp(movementInputX, -1, 0); // Limit right movement
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
        // if (!isGrounded() && isWalled)
        // {
        //     body.velocity = new Vector2(0, body.velocity.y);
        // }
        // else
        // {
            body.velocity = new Vector2(movementInputX * speed * Time.deltaTime, body.velocity.y);
        // }
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
    // Calculate the raycast origin just below the character's feet
    Vector2 originPosition = new Vector2(transform.position.x, transform.position.y - GetComponent<Collider2D>().bounds.extents.y - 0.02f);
    RaycastHit2D hit = Physics2D.Raycast(originPosition, -transform.up, castDistance, groundLayer);
    return hit.collider != null;
}

private bool CheckWall(Vector2 direction)
{
    // Offset the raycast origin to the side of the character
    Vector2 origin = new Vector2(transform.position.x + direction.x * GetComponent<Collider2D>().bounds.extents.x * 1.1f,
                                 transform.position.y);

    // Perform an overlap check to see if there's a wall
    Collider2D hitCollider = Physics2D.OverlapBox(origin, new Vector2(0.2f, GetComponent<Collider2D>().bounds.size.y), 0, groundLayer);
    
    return hitCollider != null && !hitCollider.CompareTag("OneWayPlatform");
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


    public void ApplySpeedBoost(float multiplier)
    {
        speedMultiplier *= multiplier;
    }


    public void ResetPowerUps()
    {
        speedMultiplier = 1f;
        // Reset other power-up effects
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
