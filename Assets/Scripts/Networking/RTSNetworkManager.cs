using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField]
    private GameObject unitSpawnerPrefab;

    #region Server
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        //instantiate the object on the server at the connection identity (current player prefab) object transform
        GameObject newUnitSpawner = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        //spawn across all clients while passing the current player as the owner
        NetworkServer.Spawn(newUnitSpawner, conn);
    }
    #endregion
}
