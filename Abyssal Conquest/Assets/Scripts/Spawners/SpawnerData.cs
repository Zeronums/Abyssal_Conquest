using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner.asset", menuName ="Spawners/Spawner")]
public class SpawnerData : ScriptableObject
{
    public GameObject itemSpawn;
    public int minSpawn;
    public int maxSpawn;

}



