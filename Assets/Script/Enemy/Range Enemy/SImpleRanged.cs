using UnityEngine;
using System.Collections;

public class SimpleRangedEnemy : RangeEnemy
{

    protected override void Start()
    {
        base.Start();
        health = 200; // Higher health for tank
        speed = 3f; // Slower speed for tank
    }

    public override void HandleDetection()
    {
        gameObject.GetComponent<StandardProjectileAttack>().enabled = true;
    }

    public override void HandleDetectionExit()
    {
        gameObject.GetComponent<StandardProjectileAttack>().enabled = false;
    }

}

