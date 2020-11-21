using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShooter : UnitAttacker
{
    
    [SerializeField]
    private Projectile projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;

    [Server]
    public override void Attack()
    {
        if (targeter.Target == null)
        {
            return;
        }

        if (Time.time > (1 / attackRate) + lastAttack && TargetInRange())
        {
            //attack
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            lastAttack = Time.time;
        }
    }

    [ServerCallback]
    public override void Hit()
    {
        if(targeter.Target == null)
        {
            return;
        }

        if(TargetInRange())
        {
            Quaternion projectileRotation = Quaternion.LookRotation(targeter.Target.TargetPoint.position - projectileSpawnPoint.position);

            Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);
            projectile.target = targeter.Target;
            projectile.damage = damage;


            //give projectile ownership to the owner of the unit
            NetworkServer.Spawn(projectile.gameObject, connectionToClient);
        }
        
    }
}
