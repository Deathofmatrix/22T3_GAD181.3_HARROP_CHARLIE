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

        [SerializeField] private Sprite player1Sprite;
        [SerializeField] private Sprite player2Sprite;

        public float moveSpeed = 5f;
        public Rigidbody2D characterRigidbody;
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

        public void OnFire(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                PlayerCharacterManager.golem.GetComponent<GolemController>().GolemShoot();
                Debug.Log("shoot");
            }
            else if (callbackContext.canceled)
            {
                //PlayerCharacterManager.golem.GetComponent<GolemController>().GolemShootStop();
            }
        }

        private void Update()
        {
            switch (playerNumberEnum)
            {
                case PlayerNumberEnum.Player1:
                    this.GetComponent<SpriteRenderer>().sprite = player1Sprite;
                    break;
                case PlayerNumberEnum.Player2:
                    this.GetComponent<SpriteRenderer>().sprite = player2Sprite;
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
            characterRigidbody.MovePosition(characterRigidbody.position + playerMovement.normalized * moveSpeed * Time.fixedDeltaTime);
        }


        public PlayerNumberEnum GetPlayerNumberEnum()
        {
            return playerNumberEnum;
        }

        public void GetInGolem()
        {
            GolemController.charactersInGolem++;
            inGolem = true;

            this.transform.parent = PlayerCharacterManager.golem.transform;
            this.transform.position = PlayerCharacterManager.golem.transform.position;
            characterRigidbody.simulated = false;

            this.GetComponent<SpriteRenderer>().enabled = false;

            PlayerCharacterManager.golem.GetComponent<GolemController>().golemActivated = true;
        }

        public void GetOutOfGolem()
        {
            GolemController.charactersInGolem--;
            inGolem = false;

            this.transform.SetParent(null);
            characterRigidbody.simulated = true;

            this.GetComponent<SpriteRenderer>().enabled = true;
            
            if (GolemController.charactersInGolem == 0)
            {
                PlayerCharacterManager.golem.GetComponent<GolemController>().golemActivated = false;
            }
        }
    }
}