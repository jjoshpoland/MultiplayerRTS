using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    
    private Targetable target;
    public float targetRadius;

    public Targetable Target { get => target; }



    #region Server
    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        SetTarget(targetGameObject);
    }

    [Server]
    public void SetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget))
        {
            return;
        }
        else
        {
            target = newTarget;
        }
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }
    #endregion

}
