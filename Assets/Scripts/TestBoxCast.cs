using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class TestBoxCast : MonoBehaviour
    {
        public Collider2D[] colliders;
        public float boxSize = 5f;
        private void Update()
        {
            colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(boxSize, boxSize), 0f, LayerMask.GetMask("Wall"));
        }
    }
}

