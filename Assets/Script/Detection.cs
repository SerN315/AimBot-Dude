using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    private Animator anim;
    private Enemy enemy;
    public float delayTime = 0.3f;
   private CapsuleCollider2D hitboxCollider;
   private BoxCollider2D detectionCollider;
    private bool playerInRange = false;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        hitboxCollider = transform.Find("hitBox").GetComponent<CapsuleCollider2D>();
        detectionCollider = GetComponent<BoxCollider2D>();
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
    }

void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        anim.SetBool("run", false);
        anim.SetBool("attack", true);
        playerInRange = true;
        Debug.Log("Player entered detection range");
    }
}

void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        playerInRange = false;
        Debug.Log("Player leaving detection range");
        StartCoroutine(DisableAttackAnimationAfterDelay());
    }
}

IEnumerator DisableAttackAnimationAfterDelay()
{
    Debug.Log("Starting disable animation coroutine");
    yield return new WaitForSeconds(delayTime); // Adjust delay as needed

    if (!playerInRange)
    {
        anim.SetBool("attack", false);
        anim.SetBool("run", true);
        Debug.Log("Attack animation disabled");
    }
    else
    {  
        Debug.Log("Player returned before animation finished");
    }
}

}
