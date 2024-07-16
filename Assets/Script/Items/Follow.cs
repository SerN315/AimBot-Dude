using UnityEngine;

public class Follow : MonoBehaviour
{
    public float MinMod = 8;
    public float MaxMod = 12;

    Vector2 _velocity = Vector2.zero;
    bool _isFollowing = false;
    Transform target;

    public void StartFollowing()
    {
        // Find the player object by tag and set it as the target
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
            _isFollowing = true;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    void Update()
    {
        if (_isFollowing && target != null)
        {
            // Smoothly move towards the player's current position
            transform.position = Vector2.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * Random.Range(MinMod, MaxMod));
        }
    }
}
