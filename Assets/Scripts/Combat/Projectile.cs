using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public Targetable target;
    public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float duration;


    // Start is called before the first frame update
    void Start()
    {

    }

    [ServerCallback]
    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            DestroySelf();
        }

        transform.position = Vector3.MoveTowards(transform.position, target.TargetPoint.transform.position, speed * Time.deltaTime);

        if((target.TargetPoint.transform.position - transform.position).sqrMagnitude < .25f * .25f) //if the projectile has arrived at the target
        {
            if(target.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(damage);
            }
            
            DestroySelf();
            
        }
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), duration);
    }

    

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
