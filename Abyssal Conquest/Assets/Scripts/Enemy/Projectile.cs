using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float duration = 5f;
    public float damage;
    private Vector2 direction;
    private GameObject player;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        //Vector3 direction = player.transform.position - transform.position;
        rb.velocity = (Vector2)direction.normalized * speed;

        float angle = Mathf.Atan2(rb.velocity.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(speed * Time.deltaTime * direction);
    }

    public void SetDir(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDmg(damage);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        
    }
}
