using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class Room : MonoBehaviour
    {
        public int enemiesSpawned;
        public int enemiesToSpawn;
        public GameObject[] spawners;
        public List<GameObject> allDoorInRoom;
        //public List<GameObject> adjacentRooms;

        private void Start()
        {
            FindDoorsInRoom();
        }

        private void Update()
        {
            if (enemiesSpawned >= enemiesToSpawn)
            {
                for (int i = 0; i < spawners.Length; i++)
                {
                    spawners[i].SetActive(false);
                }
            }
        }

        private void FindDoorsInRoom()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("Door"))
                {
                    allDoorInRoom.Add(child.gameObject);
                }
            }
        }
    }
}

