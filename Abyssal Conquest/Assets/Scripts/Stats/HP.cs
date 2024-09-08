using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatEffects/HP")]
public class HP : StatEffects
{
    public float value;
    public override void Apply(GameObject Player)
    {
        if (Player.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.HealDmg(value);
        }
    }
}
