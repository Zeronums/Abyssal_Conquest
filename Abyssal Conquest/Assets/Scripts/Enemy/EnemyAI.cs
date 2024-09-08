using System.Collections;
using UnityEngine;

public enum State
{
    Wander,
    Follow,
    Dead
};

public class EnemyChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distance;
    public State currentState = State.Wander;
    private Vector3 wanderDirection;
    private bool chooseDir = false;
    public Rigidbody2D rb;
    public bool notPresent = false;
    private Room currentRoom;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentRoom = GetComponentInParent<Room>();
        StartCoroutine(WanderRoutine());
    }


    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Wander:
                Wander();
                break;
            case State.Follow:
                Follow();
                break;
            case State.Dead:
                Dead();
                break;
        }

        if (InRange(distance) && currentState != State.Dead)
        {
            currentState = State.Follow;
        }
        else if (!InRange(distance) && currentState != State.Dead)
        {
            currentState = State.Wander;
        }
    }

   
    private IEnumerator WanderRoutine()
    {
        chooseDir = true;
        while (currentState == State.Wander)
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
        /*RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);*/
        if (currentRoom != null)
        {
            currentRoom.OnEnemyDefeated(this);
        }
        
        Destroy(gameObject);
    }

    private bool InRange(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= distance;
    }
}
