using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class EnemySpawner : MonoBehaviour
    {
        private float currentTime = 0;
        [SerializeField] private float timeToSpawnEnemy;

        [SerializeField] private GameObject enemyPrefab;

        public bool stopSpawning { get; set; }

        // Update is called once per frame
        void Update()
        {
            if (!stopSpawning)
            {
                SpawnEnemyOnTimer();
            }
        }

        public void SpawnEnemyOnTimer()
        {
            currentTime += Time.deltaTime;

            if (currentTime > timeToSpawnEnemy)
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                currentTime = 0;
            }
        }
    }
}

