using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreenDisplayParent;
    [SerializeField] private TMP_Text victoryText;

    // Start is called before the first frame update
    void Start()
    {
        MissionManager.OnGameOverClient += HandleGameOverClient;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        MissionManager.OnGameOverClient -= HandleGameOverClient;
    }

    public void LeaveGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

    private void HandleGameOverClient(int winner)
    {
        victoryText.text = $"Player {winner} Victory";

        victoryScreenDisplayParent.SetActive(true);
    }
}
