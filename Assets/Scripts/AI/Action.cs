using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public List<ConsiderationSlot> considerations;

    public abstract void Do(Unit unit);

    public virtual float Evaluate(Unit unit)
    {
        if(considerations.Count == 0)
        {
            return 0;
        }

        float value = 0;
        foreach(ConsiderationSlot consideration in considerations)
        {
            value += consideration.consideration.Consider(unit) * consideration.weight;
        }

        if(value > 1)
        {
            value = 1f;
        }

        return value;
    }

}

