using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : NetworkBehaviour
{
    public static event System.Action OnGameOverServer;
    public static event Action<int> OnGameOverClient;

    private List<HomeBase> HomeBases;

    private void Awake()
    {
        HomeBases = new List<HomeBase>();
    }

    #region Server

    public override void OnStartServer()
    {
        HomeBase.OnBaseSpawnedServer += RegisterHomeBaseSpawnedServer;
        HomeBase.OnBaseDeSpawnedServer += RegisterHomeBaseDeSpawnedServer;

    }

    public override void OnStopServer()
    {
        HomeBase.OnBaseSpawnedServer -= RegisterHomeBaseSpawnedServer;
        HomeBase.OnBaseDeSpawnedServer -= RegisterHomeBaseDeSpawnedServer;
    }

    private void RegisterHomeBaseSpawnedServer(HomeBase homeBase)
    {
        HomeBases.Add(homeBase);
    }
    /// <summary>
    /// Each time a home base is despawned, call this function to check if any end game conditions were fulfilled by the base despawn
    /// </summary>
    /// <param name="homeBase">The home base being despawned</param>
    [Server]
    private void RegisterHomeBaseDeSpawnedServer(HomeBase homeBase)
    {
        HomeBases.Remove(homeBase);
        CheckEndGameConditions();
    }


    public void CheckEndGameConditions()
    {
        if(HomeBases.Count != 1) { return; }

        int winner = HomeBases[0].connectionToClient.connectionId;

        RPCEndGame(winner);
        OnGameOverServer?.Invoke();
    }
    #endregion

    #region Client
    [ClientRpc]
    private void RPCEndGame(int winner)
    {
        OnGameOverClient?.Invoke(winner);
    }
    #endregion
}
