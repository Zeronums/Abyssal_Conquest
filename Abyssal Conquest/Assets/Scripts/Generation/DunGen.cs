using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Initiates dungeon generation and spawns rooms

public class DunGen : MonoBehaviour
{
    public DunGenData dungendata;
    private List<Vector2Int> dRooms;

    private void Start(){
        dRooms = CrawlerController.GenDungeon(dungendata);
        RoomSpawn(dRooms);
    }

    private void RoomSpawn(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0);
        foreach(Vector2Int roomLocation in rooms){
                RoomController.instance.LoadRoom(RoomController.instance.GetRandRoomName(), 
                roomLocation.x, roomLocation.y);
            
        }
    }
}
