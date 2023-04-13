using DungeonCrawler_Chaniel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class CameraBounds : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Golem"))
            {
                RoomManager.currentRoom = this.transform.parent.gameObject;
            }
        }
    }

}
