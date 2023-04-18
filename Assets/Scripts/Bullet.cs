using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class Bullet : MonoBehaviour
    {
        private SceneLoader sceneLoader;
        [SerializeField] private bool isPlayerBullet;
        ////false --> enemy bullet
        
        //Vector3 targetPosition;
        //public float speed;
        public int bulletDamage;

        private void Start()
        {
            sceneLoader = GameObject.Find("Scene Manager").GetComponent<SceneLoader>();
            //targetPosition = FindObjectOfType<GolemController>().transform.position;
        }
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door"))
            {
                Destroy(gameObject);
            }

            if (!isPlayerBullet)
            {
                if (collision.gameObject.CompareTag("Golem"))
                {
                    GolemController golemScript = collision.gameObject.GetComponent<GolemController>();
                    golemScript.ReduceFuel(bulletDamage);
                    Destroy(gameObject);
                }
                else if (collision.gameObject.CompareTag("Player"))
                {
                    sceneLoader.LoadThisScene("GameOver");
                }
            }
            
            if (isPlayerBullet)
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                }
                if (collision.gameObject.CompareTag("Boss"))
                {
                    Destroy(gameObject);
                    //reduce boss health
                }
            }
        }
    }
}

