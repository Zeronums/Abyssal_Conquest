using System.Collections;
using UnityEngine;

public enum State
{
    Wander,
    Follow,
    Attack,
    Dead
};

public enum EnemyType{
    Melee,
    Ranged

};

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public GameObject projectile1;
    public float speed;
    [Tooltip("This is the range for switching between states.")]
    public float distance;      // range for switching between states
    public float attRange;
    public float fireRate;
    public State currentState = State.Wander;
    public EnemyType enemyType = EnemyType.Melee;
    private Vector3 wanderDirection;
    private bool chooseDir = false;
    public Rigidbody2D rb;
    public bool notPresent = false;
    public bool canAttack = true;
    private Room currentRoom;
    private EnemyStats enemyStats;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyStats = GetComponent<EnemyStats>();
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
            case State.Attack:
                Attack();
                break;
            case State.Dead:
                Dead();
                break;
        }

        if (InRange(distance) && !InRange(attRange) && currentState != State.Dead)
        {
            currentState = State.Follow;
        }
        else if (InRange(distance) && InRange(attRange) && currentState != State.Dead)
        {
            currentState = State.Attack;       
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

    void Attack()
    {
        if (enemyType == EnemyType.Melee)
        {
            MeleeAttack();
        }
        else if (enemyType == EnemyType.Ranged && canAttack)
        {
            StartCoroutine(Shoot());
        }
    }

    public void Dead()
    {
        
        if (currentRoom != null)
        {
            currentRoom.OnEnemyDefeated(this);
        }
        
        Destroy(gameObject);
    }

    private void MeleeAttack()
    {
        //will use this for animation
    }


    private IEnumerator Shoot()
    {
        canAttack = false;
        GameObject projectile = Instantiate(projectile1, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        projectile.GetComponent<Projectile>().SetDir(direction);

        yield return new WaitForSeconds(fireRate);
        canAttack = true;
    }

    private bool InRange(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= distance;
    }


}
