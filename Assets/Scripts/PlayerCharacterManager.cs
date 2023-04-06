using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterManager : MonoBehaviour
{
    public static int totalPlayersInGame;
    public int publicTotalPlayersInGame;

    private void Awake()
    {
        totalPlayersInGame = 0;
    }
    private void Update()
    {
        publicTotalPlayersInGame = totalPlayersInGame;
    }

    public void OnPlayerJoin(PlayerInput playerInput)
    {
        PlayerCharacterManager.totalPlayersInGame++;
    }
}
