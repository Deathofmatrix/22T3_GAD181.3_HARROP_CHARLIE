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
        private bool spawnedBoss;
        public GameObject boss;

        private void Update()
        {
            if (waitTime <= 0 && spawnedBoss == false)
            {
                for (int i = 0; i < rooms.Count; i++)
                {

                    if (i == rooms.Count - 1)
                    {
                        Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                        spawnedBoss = true;
                    }

                    if (i > 0 && i < rooms.Count - 1)
                    {
                        rooms[i].GetComponentInChildren<EnemySpawner>().enemiesToSpawn = Random.Range(i, i++);
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

