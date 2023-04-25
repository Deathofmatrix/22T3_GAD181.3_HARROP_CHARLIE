using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class Room : MonoBehaviour
    {
        public int enemiesInRoom;
        public GameObject[] spawners;
        public List<GameObject> allDoorInRoom;
        public bool isBossRoom;
        public bool isBossDead;
        private GameObject currentObstacleLayout;

        public bool roomComplete = false;
        public bool upgradeGiven = false;

        [SerializeField] private bool isStartRoom;
        [SerializeField] private RoomTemplates roomTemplates;

        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private GameObject upgradePanel;

        public GameObject minimapTexture;

        private void Start()
        {
            mainCanvas = GameObject.Find("Canvas - Overlay");
            upgradePanel = mainCanvas.transform.Find("Upgrade Panel").gameObject;
            roomTemplates = FindObjectOfType<RoomTemplates>();
            isBossRoom = false;
            isBossDead = false;
            FindDoorsInRoom();
            minimapTexture = transform.Find("Minimap texture").gameObject;
            if (!isStartRoom)
            {
                minimapTexture.SetActive(false);
                SpawnObstacles();
            }
        }

        private void Update()
        {
            GiveUpgrade();

            if (isBossRoom)
            {
                Destroy(currentObstacleLayout);
            }
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

        public void SpawnObstacles()
        {
            int rand = Random.Range(0, roomTemplates.obstacleLayouts.Length + 1);
            Debug.Log(rand);
            if (rand < roomTemplates.obstacleLayouts.Length)
            {
                GameObject newObstacleLayout = Instantiate(roomTemplates.obstacleLayouts[rand], transform.position, Quaternion.identity);
                newObstacleLayout.transform.parent = transform;
                currentObstacleLayout = newObstacleLayout;
            }
        }

        public void GiveUpgrade()
        {
            if (roomComplete == true && upgradeGiven == false && !isStartRoom && !isBossRoom)
            {
                upgradePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}

