using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.Events;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float lookSpeed;
    public float velocity;

    public bool executingCommand = false;

    public bool Idle { get => !agent.hasPath; }
    public float StoppingDistance { get => agent.stoppingDistance; set => agent.stoppingDistance = value; }

    public UnityEvent OnMoveCommandGiven;
    // Start is called before the first frame update
    void Start()
    {
        
    }




    #region Server
    [ServerCallback]
    void Update()
    {
        UpdateMovementParameters();

        //need a way to turn arriving off for during pursuits and such
        
        Arrive();
        
        

    }
    /// <summary>
    /// Client tells the server to perform the actual move instructions after passing in the input value
    /// </summary>
    /// <param name="targetPos"></param>
    [Command]
    public void CmdMove(Vector3 targetPos)
    {
        executingCommand = true;
        Move(targetPos);
    }

    [Server]
    public void Stop()
    {
        agent.ResetPath();
    }

    [Server]
    public void LookAt(Transform target)
    {
        Vector3 targetVector = target.position - transform.position;
        targetVector.y = 0; //remove vertical difference
        if(targetVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        }
        
    }

    [Server]
    public void Move(Vector3 targetPos)
    {
        OnMoveCommandGiven.Invoke();
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);

        }
    }

    [Server]
    private void Arrive()
    {
        if(agent.remainingDistance > agent.stoppingDistance)
        {
            return;
        }

        if(!agent.hasPath)
        {
            return;
        }

        agent.ResetPath();
        executingCommand = false;
    }

    [Server]
    public void UpdateMovementParameters()
    {
        animator.SetFloat("Velocity", agent.velocity.magnitude);
    }

    #endregion


    #region Client
    

    #endregion
}
