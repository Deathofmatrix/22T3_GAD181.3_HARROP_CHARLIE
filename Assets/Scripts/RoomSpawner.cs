using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class RoomSpawner : MonoBehaviour
    {
        //public int openingDirection;
        //1 / TOP --> need bottom door
        //2 / BOTTOM --> need top door
        //3 / RIGHT --> need left door
        //4 / LEFT --> need right door

        public enum OpeningDirection { UNDEFINED, TOP, BOTTOM, RIGHT, LEFT }
        [SerializeField] private OpeningDirection openingDirection = OpeningDirection.UNDEFINED;

        [SerializeField] private GameObject mainGrid;

        private RoomTemplates roomTemplates;
        private int rand;
        public bool spawnedRoom = false;

        public float waitTime = 4f;

        public GameObject parentRoom;

        private void Start()
        {
            Destroy(gameObject, waitTime);
            mainGrid = GameObject.Find("Grid - Tilemap");
            roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            Invoke("SpawnRoom", 0.1f);
        }
        private void SpawnRoom()
        {
            //if (openingDirection == 1)
            //{
            //    //need to spawn bottom door
            //}
            //else if (openingDirection == 2)
            //{
            //    //top
            //}
            //else if (openingDirection == 3)
            //{
            //    //left
            //}
            //else if (openingDirection == 4)
            //{
            //    //right
            //}

            //parentRoom = this.transform.parent.parent.gameObject;
            //Room parentRoomScript = parentRoom.GetComponent<Room>();
            if (roomTemplates.roomsToSpawn > roomTemplates.rooms.Count)
            {
                if (spawnedRoom == false)
                {
                    switch (openingDirection)
                    {
                        case OpeningDirection.TOP:
                            rand = Random.Range(0, roomTemplates.bottomRooms.Length);
                            GameObject newRoomT = Instantiate(roomTemplates.bottomRooms[rand], transform.position, roomTemplates.bottomRooms[rand].transform.rotation);
                            newRoomT.transform.parent = mainGrid.transform;
                            //parentRoomScript.adjacentRooms.Add(newRoomT);
                            break;
                        case OpeningDirection.BOTTOM:
                            rand = Random.Range(0, roomTemplates.topRooms.Length);
                            GameObject newRoomB = Instantiate(roomTemplates.topRooms[rand], transform.position, roomTemplates.topRooms[rand].transform.rotation);
                            newRoomB.transform.parent = mainGrid.transform;
                            //parentRoomScript.adjacentRooms.Add(newRoomB);
                            break;
                        case OpeningDirection.RIGHT:
                            rand = Random.Range(0, roomTemplates.leftRooms.Length);
                            GameObject newRoomR = Instantiate(roomTemplates.leftRooms[rand], transform.position, roomTemplates.leftRooms[rand].transform.rotation);
                            newRoomR.transform.parent = mainGrid.transform;
                            //parentRoomScript.adjacentRooms.Add(newRoomR);
                            break;
                        case OpeningDirection.LEFT:
                            rand = Random.Range(0, roomTemplates.rightRooms.Length);
                            GameObject newRoomL = Instantiate(roomTemplates.rightRooms[rand], transform.position, roomTemplates.rightRooms[rand].transform.rotation);
                            newRoomL.transform.parent = mainGrid.transform;
                            //parentRoomScript.adjacentRooms.Add(newRoomL);
                            break;
                        case OpeningDirection.UNDEFINED:
                            Debug.Log("OPENING DIRECTION NOT DEFINED!");
                            break;
                    }
                    spawnedRoom = true;
                }
            }
            
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("SpawnPoint"))
            {
                if (collision.GetComponent<RoomSpawner>().spawnedRoom == false && spawnedRoom == false)
                {
                    roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
                    mainGrid = GameObject.Find("Grid - Tilemap");
                    GameObject closedRoom = Instantiate(roomTemplates.closedRoom, transform.position, Quaternion.identity);
                    closedRoom.transform.parent = mainGrid.transform;
                    Destroy(gameObject);
                }
                spawnedRoom = true;
            }
        }
    }
}
