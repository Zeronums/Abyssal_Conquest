using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    public StatEffects statEffects;
    private void OnTriggerEnter2D(Collider2D collision){
        Destroy(gameObject);
        statEffects.Apply(collision.gameObject);
    }
}



