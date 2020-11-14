using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private Transform spawnLocation;

    #region Server

    [Command]
    public void CmdSpawnUnit()
    {
        GameObject newUnit = Instantiate(unitPrefab, spawnLocation.position, spawnLocation.rotation);

        //spawns on all clients and uses the client connection to assign ownership for each
        NetworkServer.Spawn(newUnit, connectionToClient);
    }



    #endregion

    #region Client
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if(!hasAuthority)
        {
            return;
        }

        CmdSpawnUnit();
    }
    #endregion
}
