using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonCrawler_Chaniel
{
    public class CharacterController : MonoBehaviour
    {
        public enum PlayerNumberEnum { Undeclared, Player1, Player2 };
        [SerializeField] private PlayerNumberEnum playerNumberEnum = PlayerNumberEnum.Undeclared;

        private SceneLoader sceneLoader;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite player1Sprite;
        [SerializeField] private Sprite player2Sprite;

        private Cinemachine.CinemachineTargetGroup targetGroup1;
        [SerializeField] private Cinemachine.CinemachineTargetGroup.Target target;

        public float moveSpeed = 5f;
        public Rigidbody2D characterRigidbody;
        public bool isShooting;

        [SerializeField] private bool canDash = true;
        [SerializeField] private bool isDashing;
        [SerializeField] private float dashPower = 100f;
        [SerializeField] private float dashTime = 0.2f;
        [SerializeField] private float dashCooldown = 1f;
        [SerializeField] private TrailRenderer dashTrail;

        private bool isInvincible = false;
        [SerializeField] private float invincibilityDuration = 1f;
        public Vector2 playerMovement { get; set; }
        public bool isInGolemTrigger { get; set; }
        public bool inGolem { get; set; }

        private void OnEnable()
        {
            EventManager.SwitchedToGolem += GetInGolem;
            EventManager.SwitchedFromGolem += GetOutOfGolem;
        }

        private void OnDisable()
        {
            EventManager.SwitchedToGolem -= GetInGolem;
            EventManager.SwitchedFromGolem -= GetOutOfGolem;
        }
        private void Awake()
        {
            PlayerCharacterManager.golem = GameObject.FindGameObjectWithTag("Golem");
        }

        private void Start()
        {
            sceneLoader = GameObject.Find("Scene Manager").GetComponent<SceneLoader>();
            playerNumberEnum = (PlayerNumberEnum)PlayerCharacterManager.totalPlayersInGame;
            this.name = this.playerNumberEnum.ToString();

            if(playerNumberEnum == PlayerNumberEnum.Player1)
            {
                transform.position = Vector2.left * 2;
            }
            if (playerNumberEnum == PlayerNumberEnum.Player2)
            {
                transform.position = Vector2.right * 2;
            }

            AddPlayerToTargetGroup();
        }

        public void OnMove(InputAction.CallbackContext callbackContext)
        {
            playerMovement = callbackContext.ReadValue<Vector2>();
        }

        public void OnAccept(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                if (!inGolem)
                {
                    if (isInGolemTrigger)
                    {
                        GetInGolem();
                    }
                }
                else
                {
                    GetOutOfGolem();
                }
            }
        }

        public void OnDash(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                if (canDash)
                {
                    StartCoroutine(Dash());
                }

                if (playerNumberEnum == PlayerNumberEnum.Player1)
                PlayerCharacterManager.golem.GetComponent<GolemController>().GolemDash();
            }
            //if (callbackContext.performed)
            //{
            //    isShooting = true;
            //    Debug.Log("shoot");
            //}
            //else if (callbackContext.canceled)
            //{
            //    isShooting = false;
            //    //PlayerCharacterManager.golem.GetComponent<GolemController>().GolemShootStop();
            //}
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Enemy"))
            {
                if (isInvincible) return;

                sceneLoader.LoadThisScene("GameOver");
            }
        }

        private void Update()
        {
            switch (playerNumberEnum)
            {
                case PlayerNumberEnum.Player1:
                    spriteRenderer.sprite = player1Sprite;
                    break;
                case PlayerNumberEnum.Player2:
                    spriteRenderer.sprite = player2Sprite;
                    break;
                case PlayerNumberEnum.Undeclared:
                    Debug.LogError("No player number!!!");
                    break;
                default:
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (!isDashing)
            {
                characterRigidbody.MovePosition(characterRigidbody.position + playerMovement.normalized * moveSpeed * Time.fixedDeltaTime);
            }
            else if (isDashing)
            {
                characterRigidbody.MovePosition(characterRigidbody.position + playerMovement.normalized * dashPower * Time.fixedDeltaTime);
            }
        }


        public PlayerNumberEnum GetPlayerNumberEnum()
        {
            return playerNumberEnum;
        }

        public void GetInGolem()
        {
            GolemController.charactersInGolem++;
            inGolem = true;

            dashTrail.enabled = false;

            this.transform.parent = PlayerCharacterManager.golem.transform;
            this.transform.position = PlayerCharacterManager.golem.transform.position;
            characterRigidbody.simulated = false;

            spriteRenderer.enabled = false;

            PlayerCharacterManager.golem.GetComponent<GolemController>().golemActivated = true;
        }

        public void GetOutOfGolem()
        {
            GolemController.charactersInGolem--;
            inGolem = false;

            dashTrail.enabled = true;

            this.transform.SetParent(null);
            characterRigidbody.simulated = true;

            spriteRenderer.enabled = true;
            
            if (GolemController.charactersInGolem == 0)
            {
                PlayerCharacterManager.golem.GetComponent<GolemController>().golemActivated = false;
            }

            StartCoroutine(BecomeTemporarilyInvincible());
        }

        private void AddPlayerToTargetGroup()
        {
            targetGroup1 = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();

            for (int i = 0; i < targetGroup1.m_Targets.Length; i++)
            {
                if (targetGroup1.m_Targets[i].target == null)
                {
                    targetGroup1.m_Targets.SetValue(target, i);
                    return;
                }
            }
        }

        private IEnumerator BecomeTemporarilyInvincible()
        {
            Debug.Log("Is Invincible");
            isInvincible = true;
            for (int i = 0; i < 32; i++)
            {
                if (i != LayerMask.NameToLayer("Wall") || i != LayerMask.NameToLayer("Player Bullet"))
                {
                    Debug.LogWarning("wall layer");
                    Physics2D.IgnoreLayerCollision(gameObject.layer, i, true);
                }
            }
            StartCoroutine(Blinker());
            yield return new WaitForSeconds(invincibilityDuration);
            for (int i = 0; i < 32; i++)
            {
                if (i != LayerMask.NameToLayer("Wall") || i != LayerMask.NameToLayer("Player Bullet"))
                {
                    Physics2D.IgnoreLayerCollision(gameObject.layer, i, false);
                }
            }
            isInvincible = false;
            Debug.Log("Is not Invincible");
        }
        private IEnumerator Blinker()
        {
            Color tmp = spriteRenderer.color;
            tmp.a = 0.2f;
            spriteRenderer.color = tmp;

            yield return new WaitForSeconds(invincibilityDuration / 5);

            tmp.a = 1f;
            spriteRenderer.color = tmp;

            yield return new WaitForSeconds(invincibilityDuration / 5);

            tmp.a = 0.2f;
            spriteRenderer.color = tmp;

            yield return new WaitForSeconds(invincibilityDuration / 5);

            tmp.a = 1f;
            spriteRenderer.color = tmp;

            yield return new WaitForSeconds(invincibilityDuration / 5);

            tmp.a = 0.2f;
            spriteRenderer.color = tmp;

            yield return new WaitForSeconds(invincibilityDuration / 5);

            tmp.a = 1f;
            spriteRenderer.color = tmp;

        }

        private IEnumerator Dash()
        {
            Debug.Log("started corountine");
            canDash = false;
            isDashing = true;
            dashTrail.emitting = true;
            yield return new WaitForSeconds(dashTime);
            dashTrail.emitting = false;
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }
}