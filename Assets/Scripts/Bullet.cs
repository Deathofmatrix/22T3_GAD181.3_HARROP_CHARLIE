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
    }
}

