using UnityEngine;
using UnityEngine.UI;

public class JoystickMove : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed, jumpForce;
    public Joystick movementJoystick;
    public Button jumpButton;
    private bool isGrounded;
    private Animator anim;
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

    void FixedUpdate()
    {
        float movementInputX = movementJoystick.Direction.x;

        // Find the nearest enemy
        GameObject nearestEnemy = FindNearestEnemy();
        Vector3 enemyDirection = Vector3.zero;

        if (nearestEnemy != null && isGrounded)
        {
            // Calculate the direction to the nearest enemy, ignoring the vertical component
            enemyDirection = nearestEnemy.transform.position - transform.position;
            enemyDirection.y = 0; // Ignore the vertical component
            enemyDirection.Normalize();

            // Rotate the character to face the nearest enemy
            float angle = Mathf.Atan2(enemyDirection.z, enemyDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            if (enemyDirection.x > 0)
            {
                gunHand.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (enemyDirection.x < 0) {
                gunHand.transform.localScale = new Vector3(1, -1, 1);
            }
        }

        // Determine movement direction based on user input
        Vector3 movementDirection = new Vector3(movementInputX, 0, 0).normalized;

        if (movementInputX < -0.01f && nearestEnemy == null)
        {

            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.localScale = new Vector3(5, 5, -1);
            

        }
        else if (movementInputX > 0.01f && nearestEnemy == null)
        {

            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(5, 5, 1);


        }
            // If moving opposite to the enemy, move backward
        bool isOppositeDirection = Vector3.Dot(movementDirection, enemyDirection) < 0;
        if (isOppositeDirection)
        {
            movementDirection *= -1;
        }

        if (Mathf.Abs(movementDirection.x) > 0 && isGrounded)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
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
        if (collision.gameObject != gameObject && collision.gameObject.layer != LayerMask.NameToLayer("playerZone")&& collision.gameObject.layer != LayerMask.NameToLayer("Enemy") ) // Ensure the player doesn't collide with itself
        {
            isGrounded = true;
            anim.SetBool("Jump", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != gameObject && collision.gameObject.layer != LayerMask.NameToLayer("playerZone")&& collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) // Ensure the player doesn't collide with itself
        {
            isGrounded = false;
            anim.SetBool("Jump", true);
        }
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
}
    