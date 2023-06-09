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

        private bool isInvincible = false;

        public GameObject currentRoom;

        [SerializeField] private bool golemRecharging = false;

        public Rigidbody2D golemRigidbody;
        private Vector2 golemMovement;
        [SerializeField] private Vector2 golemShootingDirection;
        public float timeBetweenShots = 1f;
        [SerializeField] private float timeSinceLastShot = 0f;
        private bool canShoot = true;

        [SerializeField] private bool canDash = true;
        [SerializeField] private bool isDashing;
        [SerializeField] private float dashPower = 100f;
        [SerializeField] private float dashTime = 0.2f;
        [SerializeField] private float dashCooldown = 1f;

        [SerializeField] private Collider2D entryCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite golemActiveSprite;
        [SerializeField] private Sprite golemInactiveSprite;
        [SerializeField] private TrailRenderer dashTrail;

        [SerializeField] private GameObject bullet;
        [SerializeField] private Slider fuelBar;

        [SerializeField] private GameObject golemLight;

        [SerializeField] private GameObject targetArrow;

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
            dashPower = moveSpeed * 2;
            bulletSpeed = dashPower + 1;

            if (PlayerCharacterManager.player2 != null)
            {
                if (PlayerCharacterManager.player2.GetComponent<CharacterController>().inGolem == false)
                {
                    targetArrow.SetActive(false);
                }
            }

            if (currentRoom.GetComponent<Room>().enemiesInRoom == 0)
            {
                golemFuel = golemMaxFuel;
                currentRoom.GetComponent<Room>().roomComplete = true;
            }

            RechargeGolem();

            if (golemFuel <= 0 && golemRecharging == false)
            {
                DeactivateGolem();
            }

            if (golemFuel == golemMaxFuel)
            {
                if (golemRecharging == true)
                {
                    FindObjectOfType<SoundManager>().Play("GolemRecharge");
                }
                ActivateGolem();
            }

            if (Time.time > nextFuelReduction)
            {
                nextFuelReduction = Time.time + timeBetweenFuelReduction;
                if (currentRoom.GetComponent<Room>().enemiesInRoom > 0)
                {
                    Debug.LogWarning("fuel reduction");
                    ReduceFuel(1);
                }
            }

            ChangeFuelLevelSlider();

            if (PlayerCharacterManager.player2 != null)
            {
                GolemShoot();
                GolemAim();
            }

            if (isDashing)
            {
                return;
            }

            if (PlayerCharacterManager.player1 != null && PlayerCharacterManager.player1.GetComponent<CharacterController>().inGolem)
            {
                GolemMove();
            }
        }

        private void FixedUpdate()
        {
            golemRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            //if (isDashing)
            //{
            //    return;
            //}

            if (golemActivated)
            {
                if (PlayerCharacterManager.player1.GetComponent<CharacterController>().inGolem)
                {
                    golemRigidbody.constraints &= ~RigidbodyConstraints2D.FreezePosition;
                    if (!isDashing)
                    {
                        golemRigidbody.MovePosition(golemRigidbody.position + golemMovement.normalized * moveSpeed * Time.fixedDeltaTime);
                    }
                    else if (isDashing)
                    {
                        golemRigidbody.MovePosition(golemRigidbody.position + golemMovement.normalized * dashPower * Time.fixedDeltaTime);
                    }
                }
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
                    GameObject newbullet = Instantiate(bullet, golemRigidbody.position + golemShootingDirection.normalized * bulletDistanceFromGolem, transform.rotation);
                    newbullet.GetComponent<Rigidbody2D>().velocity = golemShootingDirection.normalized * bulletSpeed;

                    FindObjectOfType<SoundManager>().Play("Shoot");

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

        public void GolemAim()
        {
            if (PlayerCharacterManager.player2.GetComponent<CharacterController>().inGolem)
            {
                golemShootingDirection = PlayerCharacterManager.player2.GetComponent<CharacterController>().playerMovement;
                Vector3 direction = new Vector3(golemShootingDirection.x, golemShootingDirection.y, 0).normalized;
                targetArrow.transform.position = transform.position + direction;

                float angle = Mathf.Atan2(targetArrow.transform.position.y - transform.position.y, targetArrow.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

                targetArrow.transform.rotation = Quaternion.Euler(new Vector3(0,0, angle - 90));

                if (golemShootingDirection == new Vector2(0, 0))
                {
                    targetArrow.SetActive(false);
                } 
                else
                {
                    targetArrow.SetActive(true);
                }
                //targetArrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.position - targetArrow.transform.position);
            }
        }

        public void GolemDash()
        {
            if (canDash)
            {
                StartCoroutine(Dash());
            }
        }

        public void StartShooting()
        {
            canShoot = true;
            timeSinceLastShot = 0f;
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
            if (golemActivated && !isInvincible)
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
            golemLight.SetActive(false);
            FindObjectOfType<SoundManager>().Play("GolemDead");
        }

        private void ActivateGolem()
        {
            entryCollider.enabled = true;
            spriteRenderer.sprite = golemActiveSprite;
            golemRecharging = false;
            golemLight.SetActive(true);
        }

        private void RechargeGolem()
        {
            if (Time.time > nextFuelRecharge && golemRecharging)
            {
                nextFuelRecharge = Time.time + timeBetweenFuelRecharges;
                IncreaseFuel(2);
                Debug.Log("recharging");
            }
        }

        private IEnumerator Dash()
        {
            Debug.Log("started corountine");
            Debug.Log("Is Invincible");
            isInvincible = true;
            for (int i = 0; i < 32; i++)
            {
                if (i != LayerMask.NameToLayer("Wall") && i != LayerMask.NameToLayer("Player Bullet"))
                {
                    Debug.LogWarning("wall layer");
                    Physics2D.IgnoreLayerCollision(gameObject.layer, i, true);
                }
            }
            FindObjectOfType<SoundManager>().Play("Dash");
            canDash = false;
            isDashing = true;
            dashTrail.emitting = true;
            yield return new WaitForSeconds(dashTime);
            for (int i = 0; i < 32; i++)
            {
                if (i != LayerMask.NameToLayer("Wall") && i != LayerMask.NameToLayer("Player Bullet"))
                {
                    Physics2D.IgnoreLayerCollision(gameObject.layer, i, false);
                }
            }
            isInvincible = false;
            Debug.Log("Is not Invincible");
            dashTrail.emitting = false;
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.gameObject.CompareTag("Room"))
        //    {
        //        currentRoom = collision.transform.parent.gameObject;
        //    }
        //}


    }
}

