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
        public Room room;

        public float speed;
        public Transform target;
        public float minimumDistance;
        [SerializeField] private Rigidbody2D rigidBody2D;
        [SerializeField] Vector3 direction;

        private void Start()
        {
            room = GameObject.Find("Room1").GetComponent<Room>();
            room.enemiesSpawned++;
            sceneLoader = GameObject.Find("Scene Manager").GetComponent<SceneLoader>();

        }
        void FixedUpdate()
        {
            direction = (target.transform.position - rigidBody2D.transform.position).normalized;
            rigidBody2D.MovePosition(rigidBody2D.transform.position + direction * speed * Time.fixedDeltaTime);
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

