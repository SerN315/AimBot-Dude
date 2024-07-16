using UnityEngine;

public class Gun : MonoBehaviour
{
    // public Sprite gunSprite; // The sprite for the gun
    public GameObject bulletPrefab; // The bullet prefab for this gun
    public string name; // Name of the prefab
    public float fireRate; // The fire rate for this gun
    public string additionalEffect; // Additional effect (e.g., "explosive", "piercing")
    public int bulletDamage; // Damage value for bullets fired by this gun
    public Sprite sprite;
}
