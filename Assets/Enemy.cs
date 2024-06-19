using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public Transform pointA;
    public Transform pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;
    public float detectionRadius = 1.0f;  // Radius of the gizmo circle around the enemy

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("run", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (currentPoint.position - transform.position).normalized;
        rb.velocity = direction * speed;

        float distance = Vector2.Distance(transform.position, currentPoint.position);

        // Debug logs to track the state
        Debug.Log("Current Point: " + currentPoint.name);
        Debug.Log("Distance to Current Point: " + distance);

        if (distance < 0.5f)
        {
            // Switch to the other point when close enough
            currentPoint = (currentPoint == pointB.transform) ? pointA.transform : pointB.transform;
            Debug.Log("Switched to: " + currentPoint.name);
        }
    }
void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.collider.CompareTag("Player"))
    {
        Debug.Log("Player touched the enemy");
        PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(30); 
        }
    }
}

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    // Draw gizmos to visualize points and the enemy
    void OnDrawGizmos()
    {
        // Draw a sphere at the enemy's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw lines between enemy and points
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, pointA.position);
            Gizmos.DrawLine(transform.position, pointB.position);
        }

        // Draw spheres at pointA and pointB positions
        if (pointA != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pointA.position, 0.3f);
        }

        if (pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pointB.position, 0.3f);
        }
    }
}
