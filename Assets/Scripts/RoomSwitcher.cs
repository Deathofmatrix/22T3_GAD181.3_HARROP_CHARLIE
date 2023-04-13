using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{
    public PolygonCollider2D cameraBounds;
    public Collider2D door;
    public GameObject roomUp;
    public CinemachineConfiner2D cameraConfiner;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Golem"))
        {
            roomUp.SetActive(true);
            collision.gameObject.transform.position = new Vector2(door.transform.position.x, door.transform.position.y + 1);
            cameraConfiner.m_BoundingShape2D = cameraBounds;
        }
    }
}
