using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stores parameters for dungeon generation

[CreateAssetMenu(fileName = "DunGenData.asset", menuName = "DunGenData/ Dungeon_Data")]
public class DunGenData : ScriptableObject
{
    public int numCrawl;
    public int minIter;
    public int maxIter;


}

