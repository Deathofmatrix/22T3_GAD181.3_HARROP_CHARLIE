using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonCrawler_Chaniel
{
    public class PlayerCharacterManager : MonoBehaviour
    {
        public static int totalPlayersInGame;
        public int publicTotalPlayersInGame;

        public List<GameObject> allPlayers = new List<GameObject>(2);

        public static GameObject player1;
        public static GameObject player2;

        public static GameObject golem;

        private void Awake()
        {
            totalPlayersInGame = 0;
            golem = GameObject.FindGameObjectWithTag("Golem");
        }
        private void Update()
        {
            publicTotalPlayersInGame = totalPlayersInGame;
            FindCharacterController();
        }

        public void OnPlayerJoin(PlayerInput playerInput)
        {
            allPlayers.Add(playerInput.gameObject);
            totalPlayersInGame++;
        }
        public void OnPlayerLeave(PlayerInput playerInput)
        {
            allPlayers.Remove(playerInput.gameObject);
            totalPlayersInGame--;
        }

        public void FindCharacterController()
        {
            foreach (GameObject player in allPlayers)
            {
                CharacterController playerCharacterControllerScript = player.GetComponent<CharacterController>();
                if (playerCharacterControllerScript.GetPlayerNumberEnum() == CharacterController.PlayerNumberEnum.Player1)
                {
                    player1 = player;
                }
                else if (playerCharacterControllerScript.GetPlayerNumberEnum() == CharacterController.PlayerNumberEnum.Player2)
                {
                    player2 = player;
                }
            }
        }
    }
}

