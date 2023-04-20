using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class RoomSwitcher : MonoBehaviour
    {
        public PolygonCollider2D otherCameraBounds;
        public Collider2D otherDoor;
        public Collider2D roomDetector;
        public GameObject roomToMoveTo { get; set; }
        public CinemachineConfiner2D cameraConfiner;
        public CinemachineVirtualCamera virtualCamera;
        public int xPlayerSpawn;
        public int yPlayerSpawn;

        public Room otherRoom;

        [SerializeField] private SpriteRenderer spriteRenderer;

        public enum DoorDirection { UNDEFINED, NORTH, SOUTH, EAST, WEST }
        [SerializeField] private DoorDirection doorDirection = DoorDirection.UNDEFINED;

        private void Start()
        {
            cameraConfiner = GameObject.Find("CM vcam1").GetComponent<CinemachineConfiner2D>();
            virtualCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

            gameObject.tag = "Door";
            if (name == "North Door")
            {
                doorDirection = DoorDirection.NORTH;
                xPlayerSpawn = 0;
                yPlayerSpawn = 2;
            }
            else if (name == "South Door")
            {
                doorDirection = DoorDirection.SOUTH;
                xPlayerSpawn = 0;
                yPlayerSpawn = -2;
            }
            else if (name == "East Door")
            {
                doorDirection = DoorDirection.EAST;
                xPlayerSpawn = 2;
                yPlayerSpawn = 0;
            }
            else if (name == "West Door")
            {
                doorDirection = DoorDirection.WEST;
                xPlayerSpawn = -2;
                yPlayerSpawn = 0;
            }
            else
            {
                doorDirection = DoorDirection.UNDEFINED;    
            }
        }

        private void Update()
        {
            if (otherDoor == null)
            {
                spriteRenderer.enabled = true;
            }
            else if (otherDoor)
            {
                spriteRenderer.enabled = false;
            }
        }

        public void ActivateDeactivateDoors(bool activate)
        {
            if (activate)
            {
                roomDetector.enabled = true;
                spriteRenderer.enabled = false;
            }
            else if (activate == false)
            {
                roomDetector.enabled = false;
                otherDoor = null;
                spriteRenderer.enabled = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Golem"))
            {
                if (PlayerCharacterManager.player1.GetComponent<CharacterController>().inGolem && PlayerCharacterManager.player2.GetComponent<CharacterController>().inGolem && otherDoor != null)
                {
                    //roomToMoveTo.SetActive(true);
                    collision.gameObject.transform.position = new Vector2(otherDoor.transform.position.x + xPlayerSpawn, otherDoor.transform.position.y + yPlayerSpawn);
                    cameraConfiner.m_BoundingShape2D = otherCameraBounds;
                    virtualCamera.Follow = otherCameraBounds.transform.parent.transform;
                    otherRoom.ActivateRoomOnMinimap();
                }
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Door"))
            {
                Debug.Log("detection triggerd" + gameObject.tag + collision.gameObject.tag);
                otherDoor = collision.gameObject.GetComponent<Collider2D>();
                otherCameraBounds = collision.transform.parent.gameObject.GetComponentInChildren<PolygonCollider2D>();
                otherRoom = otherDoor.transform.parent.gameObject.GetComponent<Room>();
            }
        }
    }

}
