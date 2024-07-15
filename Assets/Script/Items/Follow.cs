using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float MinMod = 8;
    public float MaxMod = 12;

    Vector2 _velocity = Vector2.zero;
    bool _isFollowing = false;

    public void StartFollowing()
    {
        _isFollowing = true;
        transform.position = target.position; // Set initial position to target's current position
    }

    void Update()
    {
        if (_isFollowing)
        {
            transform.position = Vector2.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * Random.Range(MinMod, MaxMod));
        }
    }
}
