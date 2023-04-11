using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonCrawler_HarropCharlie
{
    public class CharacterController : MonoBehaviour
    {
        public enum PlayerNumberEnum { Undeclared, Player1, Player2 };
        [SerializeField] private PlayerNumberEnum playerNumberEnum = PlayerNumberEnum.Undeclared;

        public float moveSpeed = 5f;
        public Rigidbody2D characterRigidbody;

        [SerializeField] private Color playerColour;
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
            if (callbackContext.performed)
            {
                PlayerCharacterManager.golem.GetComponent<GolemController>().GolemShoot();
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
                    this.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case PlayerNumberEnum.Player2:
                    this.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case PlayerNumberEnum.Undeclared:
                    Debug.LogError("No player number!!!");
                    break;
                default:
                    break;
            }

            //this.GetComponent<SpriteRenderer>().color = playerColour;
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