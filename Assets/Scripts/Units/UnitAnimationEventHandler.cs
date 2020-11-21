using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    public void Hit()
    {
        UnitAttacker attacker = GetComponentInParent<UnitAttacker>();

        if(attacker != null)
        {
            attacker.Hit();
        }
    }
}
