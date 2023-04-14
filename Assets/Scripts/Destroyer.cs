using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class Destroyer : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 4f);
        }
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    //if (!collision.gameObject.CompareTag("CameraBounds"))
        //    //{
        //        Destroy(collision.gameObject);
        //    //}
        //}
    }
}

