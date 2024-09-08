using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generates the dungeon layout using random walker (crawler) algorithm

public enum Direction{
    top = 0, left = 1, bot = 2, right = 3
    
};

public class CrawlerController : MonoBehaviour
{
    public static List<Vector2Int> posVisited = new();
    private static readonly Dictionary<Direction, Vector2Int> mapDirMove = new()
    {
        {Direction.top, Vector2Int.up},
        {Direction.left, Vector2Int.left},
        {Direction.bot, Vector2Int.down},
        {Direction.right, Vector2Int.right},
    };

    public static List<Vector2Int> GenDungeon(DunGenData dungeonData){
        posVisited.Clear();
        List<DunCrawler> dCrws= new();
        for (int i=0; i < dungeonData.numCrawl; i++){
            dCrws.Add(new DunCrawler(Vector2Int.zero));
        }
        int iter = Random.Range(dungeonData.minIter, dungeonData.maxIter);
        for (int i=0; i< iter; i++){
            foreach(DunCrawler dunCrawler in dCrws){
                Vector2Int newPos = dunCrawler.CrawlerMove(mapDirMove);
                if(!posVisited.Contains(newPos)){
                    posVisited.Add(newPos);
                }
            }
        }

        return posVisited;
    }
}
