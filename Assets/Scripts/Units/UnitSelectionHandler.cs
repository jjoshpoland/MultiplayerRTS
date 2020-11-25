using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    private Camera mainCamera;

    private List<Unit> selectedUnits;
    private RTSPlayer player;
    private Vector2 dragStartPos;


    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private RectTransform unitSelectionBox;

    public List<Unit> SelectedUnits { get => selectedUnits;  }

    private void Start()
    {
        mainCamera = Camera.main;
        selectedUnits = new List<Unit>();

        Unit.AuthorityOnUnitDespawned += HandleUnitDespawned;
        MissionManager.OnGameOverClient += DisableControls;
    }

    

    private void OnDestroy()
    {
        Unit.AuthorityOnUnitDespawned -= HandleUnitDespawned;
        MissionManager.OnGameOverClient -= DisableControls;
    }

    

    private void Update()
    {
        if(player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if(Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionBox();
        }
    }

    private void StartSelectionArea()
    {
        if(!Keyboard.current.leftCtrlKey.isPressed)
        {
            foreach (Unit u in selectedUnits)
            {
                u.Deselect();
            }

            selectedUnits.Clear();
        }
        

        
        unitSelectionBox.gameObject.SetActive(true);

        dragStartPos = Mouse.current.position.ReadValue();

        UpdateSelectionBox();
    }

    private void UpdateSelectionBox()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - dragStartPos.x;
        float areaHeight = mousePosition.y - dragStartPos.y;

        unitSelectionBox.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        unitSelectionBox.anchoredPosition = dragStartPos + new Vector2(areaWidth / 2f, areaHeight / 2f);
    }

    private void ClearSelectionArea()
    {
        unitSelectionBox.gameObject.SetActive(false);

        if(unitSelectionBox.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                return;
            }

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit))
            {

                return;
            }

            if (!unit.hasAuthority) return;

            selectedUnits.Add(unit);

            foreach (Unit u in selectedUnits)
            {
                u.Select();
            }

            return;
        }
        else
        {
            Vector2 min = unitSelectionBox.anchoredPosition - (unitSelectionBox.sizeDelta / 2f);
            Vector2 max = unitSelectionBox.anchoredPosition + (unitSelectionBox.sizeDelta / 2f);

            foreach(Unit u in player.MyUnits)
            {
                if(selectedUnits.Contains(u))
                {
                    continue;
                }

                Vector3 screenPosition = mainCamera.WorldToScreenPoint(u.transform.position);
                if (screenPosition.x < max.x && 
                    screenPosition.x > min.x && 
                    screenPosition.y < max.y && 
                    screenPosition.y > min.y)
                {
                    selectedUnits.Add(u);
                    u.Select();
                }
            }
        }
        

        
    }

    private void HandleUnitDespawned(Unit obj)
    {
        selectedUnits.Remove(obj);
    }

    private void DisableControls(int obj)
    {
        enabled = false;
    }
}
