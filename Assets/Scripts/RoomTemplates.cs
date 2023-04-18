using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        public int enemiesInBossWaves;

        private void Update()
        {
            if (waitTime <= 0 && spawnedBoss == false)
            {
                Debug.Log("start spawning boss");
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (i == rooms.Count - 1)
                    {
                        Debug.Log("spawn boss");
                        GameObject newBoss = Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                        newBoss.transform.parent = rooms[i].transform;
                        spawnedBoss = true;
                        rooms[i].gameObject.GetComponent<Room>().isBossRoom = true;
                        rooms[i].GetComponentInChildren<EnemySpawner>().enemiesToSpawn = enemiesInBossWaves;
                    }

                    //if(i == 4 && rooms.Count < 5)
                    //{
                    //    //Instantiate(bottomRooms[rand], transform.position, roomTemplates.bottomRooms[rand].transform.rotation);
                    //    //Instantiate( rooms[i].gameObject.GetComponent<RoomSpawner>().SpawnRoom()
                    //}

                    //i want the minimum rooms to be lerget than 5

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

