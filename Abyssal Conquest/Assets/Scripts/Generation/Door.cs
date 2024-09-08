using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages player transition between rooms

public class Door : MonoBehaviour
{
    public enum DoorType{
        top, bot, left, right
    }
  
    private GameObject player;
    public DoorType doorType;
    private float widthOffset = 5f;
     private bool isLocked;
     private Collider2D doorCollider;
     private SpriteRenderer spriteRend;
     public Sprite DoorOpen;
     public Sprite DoorLocked;

    private void Start(){
        player = GameObject.FindGameObjectWithTag("Player");

        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.sprite = DoorOpen;
    }
    void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            playerRb.bodyType = RigidbodyType2D.Kinematic;
            playerRb.velocity = Vector2.zero;

            Vector2 newPosition = player.transform.position;
            switch (doorType)
            {
                case DoorType.top:
                    newPosition = new Vector2(transform.position.x, transform.position.y + widthOffset);
                    break;
                case DoorType.bot:
                    newPosition = new Vector2(transform.position.x, transform.position.y - widthOffset);
                    break;
                case DoorType.left:
                    newPosition = new Vector2(transform.position.x - widthOffset, transform.position.y);
                    break;
                case DoorType.right:
                    newPosition = new Vector2(transform.position.x + widthOffset, transform.position.y);
                    break;
            }
            player.transform.position = newPosition;

            playerRb.bodyType = RigidbodyType2D.Dynamic;
            playerRb.velocity = Vector2.zero;
            Room newRoom = RoomController.instance.FindRoom(Mathf.RoundToInt(newPosition.x / widthOffset), Mathf.RoundToInt(newPosition.y / widthOffset));
            if (newRoom != null)
            {
                RoomController.instance.EnteringRoom(newRoom);
            }
        }
    }

    public void LockDoor()
    {
        isLocked = true;
        doorCollider.isTrigger = false;
        spriteRend.sprite = DoorLocked;
        // You can add visual or audio feedback here to indicate that the door is locked
    }

    public void UnlockDoor()
    {
        isLocked = false;
        doorCollider.isTrigger = true;
        spriteRend.sprite = DoorOpen;
        // You can add visual or audio feedback here to indicate that the door is unlocked
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    
}
