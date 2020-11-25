using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : NetworkBehaviour
{
    public static event Action<int> OnHomeBaseDeadServer;
    public static event Action<HomeBase> OnBaseSpawnedServer;
    public static event Action<HomeBase> OnBaseDeSpawnedServer;

    #region Server
    public override void OnStartServer()
    {
        OnBaseSpawnedServer?.Invoke(this);
    }

    public override void OnStopServer()
    {
        OnBaseDeSpawnedServer?.Invoke(this);
    }

    [Server]
    public void DestroySelf()
    {
        OnHomeBaseDeadServer?.Invoke(connectionToClient.connectionId);

        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region Client
    #endregion
}
