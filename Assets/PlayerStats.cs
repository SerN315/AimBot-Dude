using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player takes damage: " + damage); // Log damage taken
        health -= damage;
        Debug.Log("Player health: " + health); // Log current health

        if (health <= 0)
        {
            Debug.Log("Player health reached zero or below. Destroying player.");
            Destroy(gameObject);
        }
    }
}
