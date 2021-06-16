using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(fileName = "AttackBestTarget-Action", menuName = "RTS/AI/Actions/AttackBestTarget")]
public class PursueBestEnemy : Action
{
    public List<ConsiderationSlot> TargetConsiderations;

    public override void Do(Unit unit)
    {
        Targetable target = unit.Targeter.Target;

        if(target == null)
        {
            target = findBestTarget(unit);
            if(target != null)
            {
                unit.Targeter.SetTarget(target.gameObject);
            }
            else
            {
                return;
            }
        }

        if(target != null)
        {
            //unit.Movement.Move(target.transform.position);
            //unit.Movement.StoppingDistance = unit.AttackRange - .25f;

            unit.Movement.LookAt(target.TargetPoint.transform);
            //using the squared distance between the target and unit is more efficient than vector3.distance because that needs to calculate a square root
            if ((target.transform.position - unit.transform.position).sqrMagnitude > (unit.AttackRange - .25f) * (unit.AttackRange - .25f))
            {
                unit.Movement.Move(target.transform.position);
                
            }
            else if (!unit.Movement.Idle)
            {
                
                //Debug.Log("stopping unit - in range of target");
                unit.Movement.Stop();

                
            }
            else
            {
                if (unit.TryGetComponent<UnitAttacker>(out UnitAttacker attacker))
                {
                    attacker.Attack();
                }
            }

            

            
        }
    }

    public override float Evaluate(Unit unit)
    {
        float baseValue = base.Evaluate(unit) * .5f;
        float bestTargetValue = 0;
        Targetable bestTarget = findBestTarget(unit, ref bestTargetValue);

        if(bestTarget != null)
        {
            //Debug.Log($"best target value for {unit} is {bestTargetValue}");
            return bestTargetValue;
        }
        else
        {
            //Debug.Log($"no target found for {unit}");
            return 0;
        }
    }

    public Targetable findBestTarget(Unit unit)
    {
        float targetValue = 0f;
        return findBestTarget(unit, ref targetValue);
    }

    /// <summary>
    /// Returns a Targetable object with the highest value according to the target considerations assigned in this action.
    /// </summary>
    /// <param name="unit">The unit looking at targets</param>
    /// <param name="targetValue">Should be normalized and halved, so a value between 0 and .5</param>
    /// <returns></returns>
    public Targetable findBestTarget(Unit unit, ref float targetValue)
    {
        Targeter targeter = unit.Targeter;

        if(targeter == null)
        {
            targetValue = 0;
            return null;
        }


        float bestWeight = float.MinValue;
        Targetable bestTarget = null;

        Collider[] colliders =  Physics.OverlapSphere(unit.transform.position, targeter.targetRadius);
        
        

        foreach(Collider obj in colliders)
        {
            if(obj.gameObject == unit.gameObject)
            {
                continue;
            }

            if(obj.TryGetComponent<Targetable>(out Targetable potentialTarget))
            {
                float weight = 0;

                if(potentialTarget.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkID))
                {
                    if(networkID.connectionToClient == targeter.connectionToClient)
                    {
                        continue;
                    }
                }

                foreach(ConsiderationSlot consideration in TargetConsiderations)
                {
                    //need to create some considerations that consider targets instead of units before i make this function
                    //considerations that can consider health, strength, distance etc from targetable instead of from unit
                    //for now, just using a targetable 
                    weight += consideration.consideration.Consider(unit, potentialTarget.gameObject);
                }

                if(weight > bestWeight)
                {
                    bestWeight = weight;
                    bestTarget = potentialTarget;
                }
            }
        }

        if(bestTarget != null)
        {
            targetValue = bestWeight;
            return bestTarget;
        }
        else
        {
            targetValue = 0;
            return null;
        }
    }
}
