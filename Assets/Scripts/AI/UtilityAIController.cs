using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UtilityAIController : MonoBehaviour
{
    public ActionGroup actions;
    public float evaluationInterval;
    public Unit unit;


    float lastEvaluation;
    [SerializeField]
    private UnitMovement movement;

    [ServerCallback]
    void Start()
    {
        //MissionManager.OnGameOverServer += DisableAI;
    }
    [ServerCallback]
    private void OnDestroy()
    {
        //MissionManager.OnGameOverServer -= DisableAI;
    }

    [ServerCallback]
    void Update()
    {
        if(movement.executingCommand)
        {
            return;
        }

        if(Time.time > lastEvaluation + evaluationInterval)
        {
            actions.Evaluate(unit);
            lastEvaluation = Time.time;
        }

        actions.Do(unit);
    }

    private void DisableAI()
    {
        enabled = false;
    }
}

[System.Serializable]
public class ConsiderationSlot
{
    public float weight;
    public Consideration consideration;
}
