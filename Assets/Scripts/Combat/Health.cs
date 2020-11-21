using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth;

    public UnityEvent OnDieServer;
    public UnityEvent<float> OnHealthUpdatedClient;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    private int currentHealth;

    #region Server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void TakeDamage(int damage)
    {
        if(currentHealth == 0)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        if(currentHealth != 0)
        {
            return;
        }

        //isdead
        OnDieServer.Invoke();
        Debug.Log($"{gameObject} has died");
    }
    #endregion

    #region Client
    
    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        OnHealthUpdatedClient.Invoke(((float)newHealth / (float)maxHealth));
        //Debug.Log($"updating health to {((float)newHealth / (float)maxHealth)}");
    }
    #endregion
}
