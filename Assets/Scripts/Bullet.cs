using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private bool isPlayerBullet;
        //false --> enemy bullet
        Vector3 targetPosition;
        public float speed;

        private void Start()
        {
            targetPosition = FindObjectOfType<GolemController>().transform.position;
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
            }
        }
    }
}

