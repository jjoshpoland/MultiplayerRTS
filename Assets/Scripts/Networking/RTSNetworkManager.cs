using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField]
    private GameObject unitSpawnerPrefab; //need to change this so that player info can be passed to the home base, so other objects can reference player data through the home base
    [SerializeField] private MissionManager missionManagerPrefab;


    #region Server
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        //instantiate the object on the server at the connection identity (current player prefab) object transform
        GameObject newUnitSpawner = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        //spawn across all clients while passing the current player as the owner
        NetworkServer.Spawn(newUnitSpawner, conn);
    }

    /// <summary>
    /// Spawns in a mission manager if the scene is a "Map"
    /// </summary>
    /// <param name="sceneName"></param>
    public override void OnServerSceneChanged(string sceneName)
    {
        if(SceneManager.GetActiveScene().name.StartsWith("Map"))
        {
            MissionManager missionManager = Instantiate(missionManagerPrefab);

            NetworkServer.Spawn(missionManager.gameObject);
        }
    }
    #endregion
}
