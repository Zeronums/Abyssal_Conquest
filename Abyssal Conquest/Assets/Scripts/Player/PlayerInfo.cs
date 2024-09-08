using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInfo : MonoBehaviour
{
    public float speed;
    [SerializeField]
    Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 mousePos;
    public Camera sceneCamera;

    public float dashSpeed;
    public float dashDur;
    public float dashCd;
    public bool isDash;
    public bool possibleDash;



    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        EnableMovement();
        possibleDash = true;
    }

    void OnEnable(){
        PlayerHealth.OnPlayerDeath += DisableMovement;
    }

    void OnDisable(){
        PlayerHealth.OnPlayerDeath -= DisableMovement;
    }

    void Update()
    {
        if(isDash){
            return;
        }
        ProcInput();
        
    }

    void FixedUpdate()
    {
        if(isDash){
            return;
        }
        Movement();
        
    }

    void ProcInput()
    {
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        direction = new Vector2(moveX, moveY).normalized;
        mousePos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetKeyDown(KeyCode.Space) && possibleDash){
            StartCoroutine(Dash());
        }
    }

    void Movement()
    {
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
        Vector2 aim_dir = mousePos - rb.position;
        aim_dir.Normalize();
        float aim_angle = Mathf.Atan2(aim_dir.y, aim_dir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aim_angle;
    }

    private IEnumerator Dash(){
        possibleDash = false;
        isDash = true;
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
        yield return new WaitForSeconds(dashDur);
        isDash = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCd);
        possibleDash = true;
    }

    private void DisableMovement(){
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void EnableMovement(){
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    
    
}
