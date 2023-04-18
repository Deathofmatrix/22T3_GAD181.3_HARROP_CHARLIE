using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class RoomTemplates : MonoBehaviour
    {
        public GameObject[] bottomRooms;
        public GameObject[] topRooms;
        public GameObject[] leftRooms;
        public GameObject[] rightRooms;

        public GameObject closedRoom;

        public List<GameObject>  rooms;

        public float waitTime;
        [SerializeField] private bool spawnedBoss;
        public GameObject boss;

        public int roomsToSpawn;

        private void Update()
        {
            if (waitTime <= 0 && spawnedBoss == false)
            {
                Debug.Log("start spawning boss");
                for (int i = 0; i < rooms.Count; i++)
                {
                    Debug.Log(i);
                    if (i == rooms.Count - 1)
                    {
                        Debug.Log("spawn boss");
                        Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                        spawnedBoss = true;
                        rooms[i].gameObject.GetComponent<Room>().isBossRoom = true;
                        rooms[i].GetComponentInChildren<EnemySpawner>().enemiesToSpawn = 7;
                    }

                    if (i > 0 && i < rooms.Count - 1)
                    {
                        rooms[i].GetComponentInChildren<EnemySpawner>().enemiesToSpawn = Random.Range(i, i + 1);
                    }

                }
            }

            else
            {
                waitTime -= Time.deltaTime;
                if (spawnedBoss == true)
                {
                    waitTime = 0;
                }
            }
        }
    }
}

