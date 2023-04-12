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

        // Update is called once per frame
        void Update()
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

