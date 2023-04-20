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

        [SerializeField] private bool isStartRoom;

        public GameObject minimapTexture;

        private void Start()
        {
            isBossRoom = false;
            isBossDead = false;
            FindDoorsInRoom();
            minimapTexture = transform.Find("Minimap texture").gameObject;
            if (!isStartRoom)
            {
                minimapTexture.SetActive(false);
            }
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

        public void ActivateRoomOnMinimap()
        {
            minimapTexture.SetActive(true);
        }
    }
}

