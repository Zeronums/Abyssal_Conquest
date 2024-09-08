using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Individual rooms, handles door logic, and notifies RoomController when entered

public class Room : MonoBehaviour
{
    public int width, height, X, Y;
    public Door tDoor, bDoor, lDoor, rDoor;
  

    public List<Door> doorsList= new();
    private bool updatedDoors = false;
    private List<EnemyChase> enemiesInRoom = new();
    private bool hasLockedDoors = false;
    public Room(int x, int y){
        X = x;
        Y = y;
    }
    



    void Start()
    {
        RoomController.instance.RegRoom(this);

        if (RoomController.instance == null)
        {
            throw new System.Exception("RoomController instance is null");
        }

        Door[] drs = GetComponentsInChildren<Door>();
        foreach(Door d in drs){
            doorsList.Add(d);
            switch(d.doorType){
                case Door.DoorType.top:
                tDoor = d;
                break;
                case Door.DoorType.bot:
                bDoor = d;
                break;
                case Door.DoorType.left:
                lDoor = d;
                break;
                case Door.DoorType.right:
                rDoor = d;
                break;
            }
        }

        enemiesInRoom = new List<EnemyChase>();
        enemiesInRoom.AddRange(GetComponentsInChildren<EnemyChase>());  // ???? error
        //RoomController.instance.RegRoom(this);

        
    }

    void Update(){

        if (enemiesInRoom.Count == 0 && hasLockedDoors)
        {
            UnlockDoors();
            hasLockedDoors = false; // Prevent re-locking after they have been unlocked
        }


        if ((name.Contains("Boss") || name.Contains("Item")) && !updatedDoors){
            UselessDoors();
            updatedDoors = true;
        }


        //only lock doors on enemy rooms not all rooms + only locks once, when room is cleared no loger locks the room
        //keep getting nullreferenceexception: object reference not set to an instance of an object at line 70

        /*if (enemiesInRoom.Count == 0 && AreDoorsLocked())
        {
            UnlockDoors();
        }*/
    }

    public void OnEnemyDefeated(EnemyChase enemy)
    {
        if (enemiesInRoom.Contains(enemy))
        {
            enemiesInRoom.Remove(enemy);
        }

       if (enemiesInRoom.Count == 0 && hasLockedDoors)
        {
            UnlockDoors();
            hasLockedDoors = false;
        }
    }

    public Room GrabTopDoor(){
        if(RoomController.instance.Exists(X, Y+1)){
            return RoomController.instance.FindRoom(X,Y+1);
        }
        return null;
    }
    public Room GrabBotDoor(){
        if(RoomController.instance.Exists(X, Y-1)){
            return RoomController.instance.FindRoom(X,Y-1);
        }
        return null;
    }
    public Room GrabLeftDoor(){
        if(RoomController.instance.Exists(X-1, Y)){
            return RoomController.instance.FindRoom(X-1,Y);
        }
        return null;
    }
    public Room GrabRightDoor(){
        if(RoomController.instance.Exists(X+1, Y)){
            return RoomController.instance.FindRoom(X+1,Y);
        }
        return null;
    }


    public void UselessDoors(){
        foreach(Door door in doorsList){
            switch(door.doorType){
                case Door.DoorType.top:
                    if(GrabTopDoor()==null)
                        door.gameObject.SetActive(false);
                break;
                case Door.DoorType.bot:
                    if(GrabBotDoor()==null)
                        door.gameObject.SetActive(false);
                break;
                case Door.DoorType.left:
                    if(GrabLeftDoor()==null)
                        door.gameObject.SetActive(false);
                break;
                case Door.DoorType.right:
                    if(GrabRightDoor()==null)
                        door.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void LockDoors()
    {
        foreach (Door door in doorsList)
        {
            door.LockDoor();
        }
    }

    public void UnlockDoors()
    {
        foreach (Door door in doorsList)
        {
            door.UnlockDoor();
        }
    }

    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(X * width, Y * height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (name.Contains("Normal") && enemiesInRoom.Count > 0 && !hasLockedDoors)
            {
                LockDoors();
                hasLockedDoors = true;
            }

            RoomController.instance.EnteringRoom(this);
        }
    }
}
