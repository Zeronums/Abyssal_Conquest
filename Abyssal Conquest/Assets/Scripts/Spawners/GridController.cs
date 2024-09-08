using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generates grid tiles for placing objects within rooms

public class GridController : MonoBehaviour
{
    public Room room;
    [Serializable]
    public struct Grid{
        public int column;
        public int row;
        public float vOffset;
        public float hOffset;
    }
    public Grid grid;
    public GameObject gridTile;
    public List<Vector2> gridPos = new();


    void Awake(){
        room = GetComponentInParent<Room>();
        grid.column = room.width - 4;
        grid.row = room.height - 4;
        GenGrid();
    }

    private void GenGrid()
    {
        grid.vOffset += room.transform.localPosition.y;
        grid.hOffset += room.transform.localPosition.x;

        for(int y=0; y<grid.row; y++){
            for(int x=0; x<grid.column; x++){
                GameObject g = Instantiate(gridTile, transform);
                g.GetComponent<Transform>().position = new Vector2(x - (grid.column - grid.hOffset), y - (grid.row - grid.vOffset));
                g.name = "X: " + x + "Y: " + y;
                gridPos.Add(g.transform.localPosition);
                g.SetActive(false);
            }
        }
        GetComponentInParent<ObjRoomSpawner>().InitObjSpawn();
    }
}



