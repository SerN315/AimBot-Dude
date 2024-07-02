using UnityEngine;

public class Bullets : MonoBehaviour
{
    private bool hit;
    public float speed = 0;
    private float timer;
    public Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb.velocity = transform.right * speed;
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Ignore collision with objects tagged as "Platform"
        if (hitInfo.CompareTag("OneWayPlatform"))
        {
            Physics2D.IgnoreCollision(hitInfo, GetComponent<Collider2D>());
            return;
        }

        // Handle collision with enemies
        if (hitInfo.CompareTag("Enemy"))
        {
            Enemy enemy = hitInfo.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(20);
            }
        }

        // Destroy the bullet in any case
        Destroy(gameObject);
    }
}
