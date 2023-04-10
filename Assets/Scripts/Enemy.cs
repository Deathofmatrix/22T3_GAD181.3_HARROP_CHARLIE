using DungeonCrawler_HarropCharlie;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DungeonCrawler_HarropCharlie
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float enemySpeed;
        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerCharacterManager.golem.transform.position, enemySpeed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("PlayerBullet"))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}

