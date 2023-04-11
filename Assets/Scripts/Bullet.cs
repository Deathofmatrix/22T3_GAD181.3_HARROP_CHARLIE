using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_HarropCharlie
{
    public class Bullet : MonoBehaviour
    {
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}

