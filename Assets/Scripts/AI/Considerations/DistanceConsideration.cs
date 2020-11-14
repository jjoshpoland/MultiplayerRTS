using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceConsideration", menuName = "RTS/AI/Considerations/Distance")]
public class DistanceConsideration : Consideration
{
    public override float Consider(Unit unit, GameObject target)
    {
        float maxDistance = unit.Targeter.targetRadius; //may not always be a sphere collider, but will assume so for the sake of this architecture
        float distance = (target.transform.position - unit.transform.position).sqrMagnitude / (maxDistance * maxDistance);

        //Debug.Log($"distance = {distance}");
        float nValue = distance;

        if (curve != null)
        {
            nValue = curve.Evaluate(distance);
        }

        return nValue;
    }

}
