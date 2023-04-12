using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class EnemyManager : MonoBehaviour
    {
        public static float enemySpeed;

        private void Start()
        {
            enemySpeed = 0;
        }
        private void Update()
        {
            enemySpeed += 0.1f * Time.deltaTime;
        }
    }
}

