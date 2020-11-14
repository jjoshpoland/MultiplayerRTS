using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class Unit : NetworkBehaviour
{
    public string currentAction;
    public float AttackRange = 1f;
    [SerializeField]
    private UnityEvent OnSelected;
    [SerializeField]
    private UnityEvent OnDeselected;
    [SerializeField]
    private UnitMovement movement;
    [SerializeField]
    private Targeter targeter;
    

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    public UnitMovement Movement { get => movement; }
    public Targeter Targeter { get => targeter; }

    #region Server
    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }
    #endregion

    #region Client
    public override void OnStartClient()
    {
        if (!isClientOnly || !hasAuthority) return;
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) return;
        AuthorityOnUnitDespawned?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority) return;

        OnSelected?.Invoke();
    }
    [Client]
    public void Deselect()
    {
        if (!hasAuthority) return;

        OnDeselected?.Invoke();
    }
    #endregion
}
