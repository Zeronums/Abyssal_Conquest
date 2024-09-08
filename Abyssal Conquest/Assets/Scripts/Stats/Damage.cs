using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatEffects/Damage")]
public class Damage : StatEffects
{
   public float value;

   public override void Apply(GameObject Player)
    {
        Player.GetComponent<PlayerAttack>().damage += value;
    }
}
