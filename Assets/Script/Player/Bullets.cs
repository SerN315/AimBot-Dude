using UnityEngine;

public class Bullets : MonoBehaviour
{
    private bool hit;
    public float speed = 0;
    public int damage = 20; // Base damage
    private float timer;
    public Rigidbody2D rb;

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
        // Ignore collision with objects tagged as "OneWayPlatform"
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
                enemy.TakeDamage(damage);
            }
        }

        // Destroy the bullet in any case
        Destroy(gameObject);
    }

    public void SetDamage(int additionalDamage)
    {
        damage += additionalDamage;
    }

    public void DamageUp(int DamageAmount)
    {
        damage += DamageAmount;
    }

    public void ResetDamage()
    {
        damage = 20;
    }

    // Example methods for additional effects
    public void MakeExplosive()
    {
        // Implement explosive effect
    }

    public void MakePiercing()
    {
        // Implement piercing effect
    }
}
