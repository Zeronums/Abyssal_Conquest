using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField]
    private float health;
    public float damage;


    public void TakeDmg(float damage){
        health -= damage;
        if (health <= 0){
            Dead();
            Destroy(gameObject, 0.2f);
        }
    }
    
    private void Dead(){
        if (TryGetComponent<EnemyChase>(out var enemyChase))
        {
            enemyChase.currentState = State.Dead;
        }

        if (TryGetComponent<BossAI>(out var bossAI))
        {
            bossAI.currentState = BossAI.BossState.Dead;
        }
    }
    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerHealth>().TakeDmg(damage);

        }
    }
    

}
