using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_HarropCharlie
{
    public class GolemController : MonoBehaviour
    {
        public float moveSpeed = 5f;

        public Rigidbody2D golemRigidbody;

        Vector2 movement;


        //private void Update()
        //{
        //    movement.x = Input.GetAxisRaw("Horizontal P1");
        //    movement.y = Input.GetAxisRaw("Vertical P1");
        //}

        //private void FixedUpdate()
        //{
        //    golemRigidbody.MovePosition(golemRigidbody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        //}

        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    Debug.Log(collision.name);
        //    Debug.Log("entered");
        //}
    }
}

