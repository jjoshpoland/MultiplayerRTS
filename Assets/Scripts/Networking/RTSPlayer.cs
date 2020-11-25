﻿using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField]
    private List<Unit> myUnits;

    public List<Unit> MyUnits { get => myUnits; }

    

    #region Server
    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
    }

    /// <summary>
    /// Adds a unit to the player's unit list on the server side if the player has authority over that unit. Should be called whenever a unit is spawned on the server.
    /// </summary>
    /// <param name="unit"></param>
    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUnits.Add(unit);
    }
    /// <summary>
    /// Removes a unit from the player's unit list on the server side if the player has authority over that unit. Should be called whenever a unit is destroyed on the server.
    /// </summary>
    /// <param name="unit"></param>
    private void ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUnits.Remove(unit);
    }
    #endregion

    #region Client
    public override void OnStartAuthority()
    {
        if (NetworkServer.active) return;

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) return;

        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
    }

    /// <summary>
    /// Removes a unit from the player's unit list on the client side if the player has authority over that unit. Should be called whenever a unit is destroyed on the server.
    /// </summary>
    /// <param name="obj"></param>
    private void AuthorityHandleUnitDespawned(Unit obj)
    {
        myUnits.Remove(obj);
    }

    /// <summary>
    /// Adds a unit to the player's unit list on the client side if the player has authority over that unit. Should be called whenever a unit is spawned on the server.
    /// </summary>
    /// <param name="obj"></param>
    private void AuthorityHandleUnitSpawned(Unit obj)
    {
        myUnits.Add(obj);
    }

    

    #endregion
}





