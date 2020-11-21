using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttacker : NetworkBehaviour
{
    [SerializeField] protected Targeter targeter;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackRate;
    [SerializeField] protected NetworkAnimator animator;
    [SerializeField] protected int damage;

    protected float lastAttack;

    

    [Server]
    public virtual void Attack()
    {
        if(targeter.Target == null)
        {
            return;
        }

        if(Time.time > (1 / attackRate) + lastAttack && TargetInRange())
        {
            //attack
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            lastAttack = Time.time;
        }

        
    }

    [Server]
    public bool TargetInRange()
    {
        return (targeter.Target.transform.position - transform.position).sqrMagnitude < (attackRange - .25f) * (attackRange - .25f);
    }

    [ServerCallback]
    public virtual void Hit()
    {
        if (targeter.Target == null)
        {
            return;
        }

        if (TargetInRange())
        {
            if (targeter.Target.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(damage);
            }
        }

    }
}
