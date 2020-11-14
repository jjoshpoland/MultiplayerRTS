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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ServerCallback]
    void Update()
    {
        if(Time.time > lastEvaluation + evaluationInterval)
        {
            actions.Evaluate(unit);
            lastEvaluation = Time.time;
        }

        actions.Do(unit);
    }
}

[System.Serializable]
public class ConsiderationSlot
{
    public float weight;
    public Consideration consideration;
}
