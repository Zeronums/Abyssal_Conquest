using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjRoomSpawner : MonoBehaviour
{
    [Serializable]
    public struct RandSpawner{
        public string name;
        public SpawnerData spawnerData;

    }
    public GridController grid;
    public RandSpawner[] spawnerData;

    void Start(){
        grid = GetComponentInChildren<GridController>();
    }

    void SpawnObj(RandSpawner data){
        int randIter = UnityEngine.Random.Range(data.spawnerData.minSpawn, data.spawnerData.maxSpawn + 1);
        for(int i = 0; i < randIter; i++){
            int randPos = UnityEngine.Random.Range(0, grid.gridPos.Count-1);
            GameObject g = Instantiate(data.spawnerData.itemSpawn, grid.gridPos[randPos], Quaternion.identity, transform);
            grid.gridPos.RemoveAt(randPos);

            
        }
    }

    public void InitObjSpawn(){
        foreach(RandSpawner rndspawner in spawnerData){
            SpawnObj(rndspawner);
        }
    }
}


