using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler_HarropCharlie
{
    public class GolemController : MonoBehaviour
    {
        public static int charactersInGolem;

        public float moveSpeed = 5f;
        public float bulletSpeed = 5f;
        public float bulletDistanceFromGolem = 0.5f;
        //public float bulletCooldown = 1f;


        public Rigidbody2D golemRigidbody;
        private Vector2 golemMovement;
        [SerializeField] private Vector2 golemShootingDirection;

        [SerializeField] private Collider2D entryCollider;
        [SerializeField] private GameObject bullet;

        public bool golemActivated {get; set;}

        private void Start()
        {
            golemActivated = false;
            charactersInGolem = 0;
        }

        private void Update()
        {
            if (PlayerCharacterManager.player1 != null && PlayerCharacterManager.player1.GetComponent<CharacterController>().inGolem)
            {
                GolemMove();
            }
            else if (PlayerCharacterManager.player2 != null && PlayerCharacterManager.player2.GetComponent<CharacterController>().inGolem)
            {
                //GolemShoot();
            }
        }

        private void GolemMove()
        {
            golemMovement = PlayerCharacterManager.player1.GetComponent<CharacterController>().playerMovement;
            //Debug.Log(golemMovement);
        }

        public void GolemShoot()
        {
            golemShootingDirection = PlayerCharacterManager.player2.GetComponent<CharacterController>().playerMovement;

            if (PlayerCharacterManager.player2.GetComponent<CharacterController>().inGolem && golemShootingDirection != new Vector2(0,0))
            {
                GameObject newbullet = Instantiate(bullet, golemRigidbody.position + golemShootingDirection * bulletDistanceFromGolem, transform.rotation);
                newbullet.GetComponent<Rigidbody2D>().velocity = golemShootingDirection.normalized * bulletSpeed;
            }
        }

        private void FixedUpdate()
        {
            golemRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            if (golemActivated)
            {
                if (PlayerCharacterManager.player1.GetComponent<CharacterController>().inGolem)
                {
                    golemRigidbody.constraints &= ~RigidbodyConstraints2D.FreezePosition;
                    golemRigidbody.MovePosition(golemRigidbody.position + golemMovement.normalized * moveSpeed * Time.fixedDeltaTime);
                }
            }
            else
            {
                golemRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<CharacterController>().isInGolemTrigger = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<CharacterController>().isInGolemTrigger = false;
            }
        }
    }
}

