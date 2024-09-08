using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAI : MonoBehaviour{

public enum BossState
{
    Wander,
    Follow,
    Dead
};


    public GameObject player;
    public float speed;
    public float distance;
    public BossState currentState = BossState.Wander;
    private Vector3 wanderDirection;
    private bool chooseDir = false;
    public Rigidbody2D rb;
    public bool notPresent = false;
    //private Room currentRoom;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //currentRoom = GetComponentInParent<Room>();
        StartCoroutine(WanderRoutine());
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case BossState.Wander:
                Wander();
                break;
            case BossState.Follow:
                Follow();
                break;
            case BossState.Dead:
                Dead();
                break;
        }

        if (InRange(distance) && currentState != BossState.Dead)
        {
            currentState = BossState.Follow;
        }
        else if (!InRange(distance) && currentState != BossState.Dead)
        {
            currentState = BossState.Wander;
        }
    }

    private IEnumerator WanderRoutine()
    {
        chooseDir = true;
        while (currentState == BossState.Wander)
        {
            SetWanderDirection();
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 5f));
        }
        chooseDir = false;
    }

    private void SetWanderDirection()
    {
        wanderDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(WanderRoutine());
        }
        rb.MovePosition(rb.position + (Vector2)wanderDirection * (speed * Time.deltaTime));
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void Dead()
    {
        
        Destroy(gameObject);
        SceneManager.LoadScene("WinScene");
    }

    private bool InRange(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= distance;
    }
}


