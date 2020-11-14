using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandHandler : MonoBehaviour
{
    [SerializeField]
    private UnitSelectionHandler selectionHandler;
    [SerializeField]
    private LayerMask layerMask;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        if(hit.collider.TryGetComponent<Targetable>(out Targetable target))
        {
            if(target.hasAuthority)
            {
                TryMove(hit.point);
            }
            TryTarget(target);
            return;
            
        }
        TryMove(hit.point);
        
    }

    private void TryMove(Vector3 point)
    {
        foreach(Unit unit in selectionHandler.SelectedUnits)
        {
            unit.Movement.CmdMove(point);
        }
    }

    private void TryTarget(Targetable target)
    {
        foreach(Unit unit in selectionHandler.SelectedUnits)
        {
            unit.Targeter.CmdSetTarget(target.gameObject);
        }
    }
}
