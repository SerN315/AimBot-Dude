using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUp", order = 1)]
public class PowerUpsEffect : ScriptableObject
{
    public string powerUpName;
    public Sprite icon;
    public string description;
      public int StackCount; 

    public virtual void ApplyEffect(GameObject player)
    {
    }
}
