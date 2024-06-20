

using UnityEngine;

public class Bullets : MonoBehaviour
{
    private bool hit;
    public float speed = 0;
    private float timer;
    private CapsuleCollider2D capsuleCollider2;
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
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.tag);
        if (hitInfo.gameObject.layer == LayerMask.NameToLayer("Enemy")) 
        {
            Enemy enemy = hitInfo.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(20);
                Destroy(gameObject);
            }
        }
        else {
            Destroy(gameObject);
        }

}
}