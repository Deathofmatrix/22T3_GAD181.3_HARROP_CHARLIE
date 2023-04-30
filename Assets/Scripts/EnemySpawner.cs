using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class EnemySpawner : MonoBehaviour
    {

        public GameObject[] enemyPrefabs;
        public BoxCollider2D spawnArea;
        public float enemiesToSpawn;
        [SerializeField] private float minDistanceFromPlayer;

        public float timeBetweenWaves;
        private float nextBossRoomWave;

        private List<Vector2> listOfTemporarySpawnPositions = new List<Vector2>();


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Golem"))
            {
                collision.GetComponent<GolemController>().currentRoom = transform.parent.gameObject;
                if (transform.parent.gameObject.GetComponent<Room>().isBossRoom == false)
                {
                    Debug.Log("Golem Entered Room");
                    SpawnWave();
                    enemiesToSpawn = 0;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Golem"))
            {
                if (transform.parent.gameObject.GetComponent<Room>().isBossRoom == true)
                {
                    gameObject.transform.parent.Find("Boss(Clone)").GetComponent<Boss>().isActive = true;

                    if (Time.time >= nextBossRoomWave)
                    {
                        nextBossRoomWave = Time.time + timeBetweenWaves;
                        SpawnWave();
                    }
                }
            }
        }

        private void SpawnWave()
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition();
                GameObject newEnemy = Instantiate(RandomiseEnemy(), spawnPosition, Quaternion.identity);
                newEnemy.transform.parent = this.transform;
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector2 center = spawnArea.bounds.center;
            Vector2 size = spawnArea.bounds.size;
            Vector2 spawnPosition;

            float boxSize = 1f;
            bool canSpawn = false;
            do
            {
                float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
                float y = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);
                spawnPosition = new Vector2(x, y);

                if (!spawnArea.bounds.Contains(spawnPosition)) continue;

                canSpawn = true;

                Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnPosition, new Vector2(boxSize, boxSize), 0f, LayerMask.GetMask("Wall"));
                Debug.Log(colliders.Length);
                if (colliders.Length > 0)
                {
                    canSpawn = false;
                    print("Tried to find a suitable spawn position, but we hit a collider. Also, \"canSpawn\" has been set to false.");
                }

                if(Vector2.Distance(spawnPosition, PlayerCharacterManager.golem.transform.position) < minDistanceFromPlayer)
                {
                    print("Tried to find a suitable spawn position, but we are too close to the golem's position.");
                }

                listOfTemporarySpawnPositions.Add(spawnPosition);

            } while (Vector2.Distance(spawnPosition, PlayerCharacterManager.golem.transform.position) < minDistanceFromPlayer || !canSpawn);

            return spawnPosition;
        }

        //private Vector3 GetRandomSpawnPosition()
        //{
        //    Vector2 center = spawnArea.bounds.center;
        //    Vector2 size = spawnArea.bounds.size;
        //    Vector2 spawnPosition;

        //    float boxSize = 10f;

        //    do
        //    {
        //        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        //        float y = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);
        //        spawnPosition = new Vector2(x, y);

        //        Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnPosition, new Vector2(boxSize, boxSize), 0f);
        //        if (colliders.Length == 0)
        //        {
        //            continue;
        //        }

        //        bool foundWall = false;
        //        foreach (Collider2D collider in colliders)
        //        {
        //            if (collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        //            {
        //                foundWall = true;
        //                break;
        //            }
        //        }

        //        if (foundWall)
        //        {
        //            continue;
        //        }

        //    } while (Vector2.Distance(spawnPosition, PlayerCharacterManager.golem.transform.position) < minDistanceFromPlayer);
        //    return spawnPosition;
        //}


        private GameObject RandomiseEnemy()
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[randomIndex];
            return enemyToSpawn;
        }

        private void OnDrawGizmos()
        {
            if (listOfTemporarySpawnPositions.Count == 0) return;

            foreach (Vector2 spawnPosition in listOfTemporarySpawnPositions)
            {
                Gizmos.color = new Color(Random.Range(0.6f, 1f), Random.Range(0.6f, 1f), Random.Range(0.6f, 1f));
                Gizmos.DrawWireCube(spawnPosition, Vector3.one * 1f);
            }
        }
    }
}

