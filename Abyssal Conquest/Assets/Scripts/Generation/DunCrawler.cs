using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//Crawlers that move randomly from the starting position

public class DunCrawler : MonoBehaviour
{
    public Vector2Int Pos{
        get;
        set;
    }

    public DunCrawler(Vector2Int startPos){
        Pos = startPos;
    }

    public Vector2Int CrawlerMove(Dictionary<Direction, Vector2Int> mapDirMove){
        Direction toMove = (Direction)Random.Range(0, mapDirMove.Count);
        Pos += mapDirMove[toMove];
        return Pos;
    }
}




