using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targetable : NetworkBehaviour
{
    [SerializeField]
    private Transform targetPoint;

    public Transform TargetPoint { get => targetPoint; }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
