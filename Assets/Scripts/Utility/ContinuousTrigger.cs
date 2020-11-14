using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ContinuousTrigger : MonoBehaviour
{
    public Collider trigger;
    public List<Collider> currentColliders;

    private void Start()
    {
        trigger = GetComponent<Collider>();
        currentColliders = new List<Collider>();
    }

    public void CheckCollisions()
    {
        List<Collider> deadColliders = new List<Collider>();
        for (int i = 0; i < currentColliders.Count; i++)
        {
            if(currentColliders[i] == null || !currentColliders[i].enabled)
            {
                deadColliders.Add(currentColliders[i]);
            }
        }

        if(deadColliders.Count > 0)
        {
            for (int i = 0; i < deadColliders.Count; i++)
            {
                currentColliders.Remove(deadColliders[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!currentColliders.Contains(other))
        {
            currentColliders.Add(other);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(currentColliders.Contains(other))
        {
            currentColliders.Remove(other);
        }
    }
}
