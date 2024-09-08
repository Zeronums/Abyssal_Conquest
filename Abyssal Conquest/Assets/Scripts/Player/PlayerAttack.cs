using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private new Animator animation;
    public float attackSpeed;
    public float damage;
    public float betweenAttacks;
    public float knockback;


    void Start(){
        EnableAttack();
    }

    private void OnEnable(){
        PlayerHealth.OnPlayerDeath += DisableAttack;
    }

    private void OnDisable(){
        PlayerHealth.OnPlayerDeath -= DisableAttack;
    }

    private void Update(){
        if(betweenAttacks <= 0f){
            if(Input.GetMouseButtonDown(0)){
                animation.SetTrigger("Attack");
                betweenAttacks = attackSpeed;
            }
        }
        else{
            betweenAttacks -= Time.deltaTime;
        }
    }

    private void DisableAttack(){
        animation.enabled = false;
    }

    private void EnableAttack(){
        animation.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy") || other.CompareTag("Boss")){
            other.GetComponent<EnemyStats>().TakeDmg(damage);

            if (other.TryGetComponent<Rigidbody2D>(out var enemyRb))
            {
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDirection * knockback, ForceMode2D.Impulse);
            }
        }
    }
}
