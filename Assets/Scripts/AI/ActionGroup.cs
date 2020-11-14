using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewActionGroup", menuName = "RTS/AI/ActionGroup")]
public class ActionGroup : Action
{
    public List<Action> actions;
    public Action bestAction;

    public override void Do(Unit unit)
    {
        if(bestAction != null)
        {
            bestAction.Do(unit);
        }
    }

    public override float Evaluate(Unit unit)
    {
        float bestWeight = 0;
        Action bestAction = null;

        //weigh the considerations of each action and call Do() on that action
        foreach (Action action in actions)
        {
            float weight = action.Evaluate(unit);
            if (weight > bestWeight)
            {
                bestWeight = weight;
                bestAction = action;
            }
        }

        this.bestAction = bestAction;

        return bestWeight;
    }
}
