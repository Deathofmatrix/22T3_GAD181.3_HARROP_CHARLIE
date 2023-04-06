using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonCrawler_HarropCharlie
{
    public class CharacterController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public Rigidbody2D characterRigidbody;
        Vector2 movement;

        [SerializeField] private GameObject golem;

        public int playerNumber;


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

        private void Start()
        {
            playerNumber = PlayerCharacterManager.totalPlayersInGame;
            golem = GameObject.FindGameObjectWithTag("Golem");
        }

        public void OnMove(InputAction.CallbackContext callbackContext)
        {
            movement = callbackContext.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            characterRigidbody.MovePosition(characterRigidbody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }

        private void GetInGolem()
        {
            this.transform.parent = golem.transform;
            this.transform.position = golem.transform.position;
            characterRigidbody.simulated = false;

            this.GetComponent<SpriteRenderer>().enabled = false;
        }

        private void GetOutOfGolem()
        {
            this.transform.SetParent(null);
            characterRigidbody.simulated = true;

            this.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}