using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "WanderAction", menuName = "RTS/AI/Actions/Wander")]
public class WanderAction : Action
{
    public float wanderRadius = 10f;

    public override void Do(Unit unit)
    {
        unit.currentAction = "Wandering";
        //if the unit is idle, go somewhere random
        if(unit.Movement.Idle)
        {
            unit.Movement.Move(RandomNavSphere(unit.transform.position, wanderRadius));
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;
    }
}
