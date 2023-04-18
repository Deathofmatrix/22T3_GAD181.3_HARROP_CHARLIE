using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class Room : MonoBehaviour
    {
        public int enemiesInRoom;
        public GameObject[] spawners;
        public List<GameObject> allDoorInRoom;
        //public List<GameObject> adjacentRooms;
        public bool isBossRoom;
        public bool isBossDead;

        private void Start()
        {
            isBossRoom = false;
            isBossDead = false;
            FindDoorsInRoom();
        }

        private void Update()
        {
            if (enemiesInRoom > 0)
            {
                foreach (GameObject door in allDoorInRoom)
                {
                    door.GetComponent<RoomSwitcher>().ActivateDeactivateDoors(false);
                }
            }
            else if (enemiesInRoom == 0)
            {
                foreach (GameObject door in allDoorInRoom)
                {
                    door.GetComponent<RoomSwitcher>().ActivateDeactivateDoors(true);
                }
            }
        }


        private void FindDoorsInRoom()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("Door"))
                {
                    allDoorInRoom.Add(child.gameObject);
                }
            }
        }
    }
}

