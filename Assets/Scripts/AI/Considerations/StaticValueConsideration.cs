using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticValueConsideration", menuName = "RTS/AI/Considerations/StaticValue")]
public class StaticValueConsideration : Consideration
{
    [Range(0f,1f)]
    public float value;

    public override float Consider(Unit unit, GameObject target)
    {
        return value;
    }

}
