using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public string enemyTag = "Player";
    public Transform gunHand;
    public Transform firePoint;

    public abstract void Attack(GameObject target);

    protected GameObject FindNearestEnemy()
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
}
