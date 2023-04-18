using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class EnemySpawner : MonoBehaviour
    {
        //private float currentTime = 0;
        //[SerializeField] private float timeToSpawnEnemy;

        //[SerializeField] private GameObject enemyPrefab;

        //public bool stopSpawning { get; set; }

        //// Update is called once per frame
        //void Update()
        //{
        //    if (!stopSpawning)
        //    {
        //        SpawnEnemyOnTimer();
        //    }
        //}

        //public void SpawnEnemyOnTimer()
        //{
        //    currentTime += Time.deltaTime;

        //    if (currentTime > timeToSpawnEnemy)
        //    {
        //        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        //        currentTime = 0;
        //    }
        //}

        public GameObject[] enemyPrefabs;
        public BoxCollider2D spawnArea;
        public float enemiesToSpawn;
        [SerializeField] private float minDistanceFromPlayer;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Golem"))
            {
                Debug.Log("Golem Entered Room");
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    Vector2 spawnPosition = GetRandomSpawnPosition();
                    Instantiate(RandomiseEnemy(), spawnPosition, Quaternion.identity);
                }
                enemiesToSpawn = 0;
            }
        }
        //public void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Golem"))
        //    {
        //        Debug.Log("Golem Entered Room");
        //        for (int i = 0; i < enemiesToSpawn; i++)
        //        {
        //            Vector3 spawnPosition = GetRandomSpawnPosition();
        //            Instantiate(RandomiseEnemy(), spawnPosition, Quaternion.identity);
        //        }
        //        enemiesToSpawn = 0;
        //    }
        //}

        private Vector3 GetRandomSpawnPosition()
        {
            Vector2 center = spawnArea.bounds.center;
            Vector2 size = spawnArea.bounds.size;
            Vector2 spawnPosition;

            do
            {
                float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
                float y = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);
                spawnPosition = new Vector2(x, y);
            } while (Vector2.Distance(spawnPosition, PlayerCharacterManager.golem.transform.position) < minDistanceFromPlayer);
            return spawnPosition;
        }

        private GameObject RandomiseEnemy()
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[randomIndex];
            return enemyToSpawn;
        }
    }
}

