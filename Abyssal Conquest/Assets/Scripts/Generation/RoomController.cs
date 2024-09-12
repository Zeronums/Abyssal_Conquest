using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

//Manages the overall dungeon; loading, updating rooms and special rooms

public class RoomInfo{
    public string name;
    public int X;
    public int Y;

}
public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    Queue<RoomInfo> loadRoomQ = new();
    public List<Room> loadedRooms = new();
    string wName = "Cavern";
    public Room currentRoom;
    RoomInfo loadRoomData;
    bool isLoading = false;
    bool spawnBossRoom = false;
    bool updatedRooms = false;
    bool spawnItemRoom = false;

    private Rigidbody2D playerRb;

    void Start(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate(){
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if(isLoading){
            return;
        }

        if(loadRoomQ.Count == 0){
            if(!spawnBossRoom){
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnBossRoom && !spawnItemRoom){
                StartCoroutine(SpawnItemRoom());
            }
            else if (spawnBossRoom && spawnItemRoom && !updatedRooms){
                    foreach(Room room in loadedRooms){
                        room.UselessDoors();
                    }
                    UpdateRooms();
                    updatedRooms = true;
                }
            
            return;
        }
        loadRoomData = loadRoomQ.Dequeue();
        isLoading = true;

        StartCoroutine(LoadRoomRoutine(loadRoomData));
    }

    IEnumerator SpawnBossRoom(){
        spawnBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQ.Count == 0){
            Room bossRoom = loadedRooms[^1];
            Vector2Int tempRoom = new(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var toRemove = loadedRooms.Single(r => r.X == tempRoom.x && r.Y == tempRoom.y);
            loadedRooms.Remove(toRemove);
            LoadRoom("Boss", tempRoom.x, tempRoom.y);
        }
    }

    IEnumerator SpawnItemRoom(){
        spawnItemRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQ.Count == 0){
            Room itemRoom = loadedRooms[UnityEngine.Random.Range(0, loadedRooms.Count)];
            Vector2Int tempRoom = new(itemRoom.X, itemRoom.Y);
            Destroy(itemRoom.gameObject);
            var toRemove = loadedRooms.Single(r => r.X == tempRoom.x && r.Y == tempRoom.y);
            loadedRooms.Remove(toRemove);
            LoadRoom("Item", tempRoom.x, tempRoom.y);
        }
    }

    public Room FindRoom(int x, int y){
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info){
        string roomName = wName + info.name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        while(loadRoom.isDone == false){
           yield return null;
        }
    }

    public void LoadRoom(string name, int x, int y){
        
        if(Exists(x,y)){
            return;
        }

        RoomInfo nRoomData = new()
        {
            name = name,
            X = x,
            Y = y
        };
        loadRoomQ.Enqueue(nRoomData);

    }

    public IEnumerator RoomCoroutine(){
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void EnteringRoom(Room room){
        currentRoom = room;
        UpdateCameraTarget();
        StartCoroutine(RoomCoroutine());

        SetPlayerDynamic();
    }

    public void SetPlayerDynamic(){
        if(playerRb != null){
            playerRb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void SetPlayerKinematic(){
        if(playerRb != null){
            playerRb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void RegRoom(Room room){
        if(!Exists(loadRoomData.X, loadRoomData.Y)){

            room.transform.position = new Vector3(loadRoomData.X * room.width, loadRoomData.Y * room.height, 0);
            room.X = loadRoomData.X;
            room.Y = loadRoomData.Y;
            room.name = wName + " " + loadRoomData.name + " " + room.X + " , " + room.Y;
            room.transform.parent = transform;

            isLoading = false;
            loadedRooms.Add(room);
        }
        else{
            Destroy(room.gameObject);
            isLoading = false;
        }
    }

    

    public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            if (currentRoom != room)
            {
                EnemyAI[] enemies = room.GetComponentsInChildren<EnemyAI>();
                if (enemies != null)
                {
                    foreach (EnemyAI enemy in enemies)
                    {
                        enemy.notPresent = true;
                    }
                }
                
            }
            else
            {
                EnemyAI[] enemies = room.GetComponentsInChildren<EnemyAI>();
                BossAI[] bosses = room.GetComponentsInChildren<BossAI>();
                bool enemiesPresent = enemies.Length > 0;
                bool bossesPresent = bosses.Length > 0;
                
                if (enemiesPresent || bossesPresent)
                {
                    foreach (EnemyAI enemy in enemies)
                    {
                        enemy.notPresent = false;
                    }

                    foreach (BossAI boss in bosses)
                    {
                        boss.notPresent = false;
                    }
                }
                
                
                
            }
        }
    }

    void Awake(){
        instance = this;
    }
    
    public bool Exists(int x, int y){
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public string GetRandRoomName(){
        string[] possibleRooms = new string[]{
            "Empty",
            "Normal"
        };
        return possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Length)];
    }

    private void UpdateCameraTarget()
    {
        Cinemachine.CinemachineVirtualCamera virtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (virtualCamera != null && currentRoom != null)
        {
            Transform roomCenter = currentRoom.transform;
            virtualCamera.Follow = roomCenter;

            // Adjust the orthographic size of the camera to fit the room
            float roomWidth = currentRoom.width;
            float roomHeight = currentRoom.height;
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = roomWidth / roomHeight;

            if (screenRatio >= targetRatio)
            {
                virtualCamera.m_Lens.OrthographicSize = roomHeight / 2;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                virtualCamera.m_Lens.OrthographicSize = roomHeight / 2 * differenceInSize;
            }
        }
    }
  
}
