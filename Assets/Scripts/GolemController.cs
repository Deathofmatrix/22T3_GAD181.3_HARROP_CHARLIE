using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler_Chaniel
{
    public class GolemController : MonoBehaviour
    {
        public static int charactersInGolem;

        public float moveSpeed = 5f;
        public float bulletSpeed = 5f;
        public float bulletDistanceFromGolem = 0.5f;
        //public float bulletCooldown = 1f;
        public int golemFuel = 100;
        public int golemMaxFuel = 100;
        public int killFuelGain = 10;

        [SerializeField] private bool golemRecharging = false;

        public Rigidbody2D golemRigidbody;
        private Vector2 golemMovement;
        [SerializeField] private Vector2 golemShootingDirection;
        public bool isShooting;
        public float timeBetweenShots = 1f;
        [SerializeField] private float timeSinceLastShot = 0f;
        [SerializeField] private bool canShoot = true;

        [SerializeField] private Collider2D entryCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite golemActiveSprite;
        [SerializeField] private Sprite golemInactiveSprite;

        [SerializeField] private GameObject bullet;
        [SerializeField] private Slider fuelBar;

        private float nextFuelReduction;
        [SerializeField] private float timeBetweenFuelReduction;
        
        private float nextFuelRecharge;
        [SerializeField] private float timeBetweenFuelRecharges;

        public bool golemActivated {get; set;}

        private void Start()
        {
            golemActivated = false;
            charactersInGolem = 0;
        }

        private void Update()
        {
            RechargeGolem();

            if (golemFuel <= 0 && golemRecharging == false)
            {
                DeactivateGolem();
            }

            if (golemFuel == golemMaxFuel)
            {
                ActivateGolem();
            }

            if (Time.time > nextFuelReduction)
            {
                nextFuelReduction = Time.time + timeBetweenFuelReduction;
                ReduceFuel(1);
            }

            ChangeFuelLevelSlider();

            if (PlayerCharacterManager.player1 != null && PlayerCharacterManager.player1.GetComponent<CharacterController>().inGolem)
            {
                GolemMove();
            }
            //else if (PlayerCharacterManager.player2 != null && PlayerCharacterManager.player2.GetComponent<CharacterController>().inGolem)
            //{
            //    //GolemShoot();
            //}

            if (isShooting)
            {
                GolemShoot();
            }
        }

        private void GolemMove()
        {
            golemMovement = PlayerCharacterManager.player1.GetComponent<CharacterController>().playerMovement;
            //Debug.Log(golemMovement);
        }

        public void GolemShoot()
        {
            if (canShoot)
            {
                golemShootingDirection = PlayerCharacterManager.player2.GetComponent<CharacterController>().playerMovement;

                if (PlayerCharacterManager.player2.GetComponent<CharacterController>().inGolem && golemShootingDirection != new Vector2(0, 0))
                {
                    GameObject newbullet = Instantiate(bullet, golemRigidbody.position + golemShootingDirection * bulletDistanceFromGolem, transform.rotation);
                    newbullet.GetComponent<Rigidbody2D>().velocity = golemShootingDirection.normalized * bulletSpeed;

                    canShoot = false;
                    timeSinceLastShot = 0f;
                }
            }
            
            timeSinceLastShot += Time.deltaTime;

            if (timeSinceLastShot >= timeBetweenShots)
            {
                canShoot = true;
            }
            
        }

        public void StartShooting()
        {
            canShoot = true;
            timeSinceLastShot = 0f;
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
        public void ReduceFuel(int damage)
        {
            if (golemActivated)
            {
                golemFuel -= damage;
            }
        }
        public void IncreaseFuel(int damage)
        {
            golemFuel += damage;
        }

        private void ChangeFuelLevelSlider()
        {
            fuelBar.value = golemFuel;
            if (golemFuel > golemMaxFuel)
            {
                golemFuel = golemMaxFuel;
            }
        }

        public void OnEnemyKill()
        {
            golemFuel += killFuelGain;
        }

        private void DeactivateGolem()
        {
            foreach (GameObject player in PlayerCharacterManager.allPlayers)
            {
                player.GetComponent<CharacterController>().GetOutOfGolem();
            }
            entryCollider.enabled = false;
            golemActivated = false;
            spriteRenderer.sprite = golemInactiveSprite;
            golemRecharging = true;
        }

        private void ActivateGolem()
        {
            entryCollider.enabled = true;
            spriteRenderer.sprite = golemActiveSprite;
            golemRecharging = false;
        }

        private void RechargeGolem()
        {
            if (Time.time > nextFuelRecharge && golemRecharging)
            {
                nextFuelRecharge = Time.time + timeBetweenFuelRecharges;
                IncreaseFuel(1);
                Debug.Log("recharging");
            }
        }
    }
}

