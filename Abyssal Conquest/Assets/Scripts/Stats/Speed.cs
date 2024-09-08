using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatEffects/Speed")]
public class Speed : StatEffects
{
   public float value;
   public override void Apply(GameObject Player)
    {
        Player.GetComponent<PlayerInfo>().speed += value;
    }

    
}
