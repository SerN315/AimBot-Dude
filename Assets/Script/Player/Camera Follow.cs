using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2.0f;
    public float yOffset;
    public float xOffset;
    public Transform target;

    private bool isFacingRight = true; // Assume player starts facing right
    private float targetXOffset;
    private float transitionTime = 0.3f; // Duration for the offset change

    void Start()
    {
        if (target == null)
        {
            //Debug.LogWarning("Target not set for CameraFollow.");
        }

        // Set initial target offset
        targetXOffset = xOffset;
    }

    void Update()
    {
        if (target == null) return;

        // Determine if the player has changed facing direction
        CheckFacingDirection();

        // Calculate the new camera position
        Vector3 newPos = new Vector3(target.position.x + targetXOffset, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }

    private void CheckFacingDirection()
    {
        if (target.localScale.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            StartCoroutine(ChangeOffset(xOffset));
        }
        else if (target.localScale.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            StartCoroutine(ChangeOffset(-xOffset));
        }
    }

    private IEnumerator ChangeOffset(float newOffset)
    {
        float currentOffset = targetXOffset;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            targetXOffset = Mathf.Lerp(currentOffset, newOffset, elapsedTime / transitionTime);
            yield return null;
        }

        targetXOffset = newOffset; // Ensure the final value is set
    }
}
