using DungeonCrawler_Chaniel;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class Enemy : MonoBehaviour
    {
        //[SerializeField] private float enemySpeed;
        [SerializeField] private SceneLoader sceneLoader;

        private void Start()
        {
            sceneLoader = GameObject.Find("Scene Manager").GetComponent<SceneLoader>();
        }
        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerCharacterManager.golem.transform.position, EnemyManager.enemySpeed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("PlayerBullet"))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Golem"))
            {
                ScoreManager.finalScore = ScoreManager.score;
                sceneLoader.LoadThisScene("GameOver");
            }
        }

        private void OnDestroy()
        {
            ScoreManager.score++;
        }
    }
}

