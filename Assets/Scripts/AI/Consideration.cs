using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consideration : ScriptableObject
{
    public AnimationCurve curve;

    /// <summary>
    /// Should return a normalized value from 0-1 based on the evaluated state of the unit
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public abstract float Consider(Unit unit, GameObject other = null);
}
