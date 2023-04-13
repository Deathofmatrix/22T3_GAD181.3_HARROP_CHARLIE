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
    }
}

