using System.Collections;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    public Joystick movementJoystick;
    [SerializeField] private BoxCollider2D playerCollider;
    

    // Update is called once per frame
    void Update()
    {
            float movementInputY = movementJoystick.Direction.y;
        // Check if joystick is moved down
        if (movementInputY < -0.5f && currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        CompositeCollider2D platformCollider = currentOneWayPlatform.GetComponent<CompositeCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
