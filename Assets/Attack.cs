using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public Transform gunHand;
    public float fireRate;
    public Transform firePoint;
    public GameObject BulletPrefab;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

        GameObject nearestEnemy = FindNearestEnemy();

        // If an enemy is found, point the gunHoldingHand towards it
        if (nearestEnemy != null)
        {
            Vector3 direction = nearestEnemy.transform.position - gunHand.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gunHand.rotation = Quaternion.Euler(0, 0, angle);
        }
        fireRate += Time.deltaTime;
        if (fireRate > 1.2 && nearestEnemy!= null)
        {
            Shoot();
            fireRate = 0;
        }

    }
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(gunHand.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void Shoot()
    {
        Instantiate(BulletPrefab, firePoint.position,firePoint.rotation);
    }


}
