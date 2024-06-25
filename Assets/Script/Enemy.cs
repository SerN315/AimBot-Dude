
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public Transform pointA;
    public Transform pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public string boolParameterName = "run";
    public float decelerationRate = 2f;
    public float speed;
    private BoxCollider2D hitboxCollider;
    private ShieldEnemy shieldEnemy;
    



    // Start is called before the first frame update
   void Start()
    {
        shieldEnemy = GetComponent<ShieldEnemy>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB; // Start with pointB to move towards pointA first
        anim.SetBool("run", true);
        // Find the hitbox collider in children (assuming it's a child of the enemy)
        hitboxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        bool isRunning = anim.GetBool(boolParameterName);
        if(isRunning && !anim.GetBool("shield")){
        Vector2 direction = (currentPoint.position - transform.position).normalized;
        rb.velocity = direction * speed;

        float distance = Vector2.Distance(transform.position, currentPoint.position);

        if (distance < 0.5f)
        {
            // Switch to the other point when close enough
            currentPoint = (currentPoint == pointA) ? pointB : pointA;
        }
        }
        else {
                // Gradually slow down the rb.velocity to zero

                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * decelerationRate);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Projectile")){
            Debug.Log("Player touched the enemy");
            Bullets bullets= collision.collider.GetComponent<Bullets>();
            if (bullets != null){
                Destroy(bullets);
            }
        }
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player touched the enemy");
            PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(0); // Example: Player takes 10 damage on collision
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (anim.GetBool("shield"))
        {
            // If shield is active, take no damage
            Debug.Log("Shield is active. No damage taken.");
            return;
        }
        health -= damage;
        Debug.Log("Enemy took damage: " + damage + ", current health: " + health);
        if (health < 50 && health > 0 && !shieldEnemy.isShieldActive)
        {
            shieldEnemy.CreateShield();
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
    public void DetectPlayer(){

    }

    public void AttackPlayer(){

    }


    // Draw gizmos to visualize points and the enemy
    //void OnDrawGizmos()
    //{
    //    // Draw a sphere at the enemy's position
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);

    //    // Draw lines between enemy and points
    //    if (pointA != null && pointB != null)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawLine(transform.position, pointA.position);
    //        Gizmos.DrawLine(transform.position, pointB.position);
    //    }

    //    // Draw spheres at pointA and pointB positions
    //    if (pointA != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(pointA.position, 0.3f);
    //    }

    //    if (pointB != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(pointB.position, 0.3f);
    //    }
    //}
}
