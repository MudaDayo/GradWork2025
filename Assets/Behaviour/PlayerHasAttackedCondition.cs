using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "PlayerHasAttacked", story: "check if player atk count is bigger than 0", category: "Conditions", id: "7900fc8dd1843fcdc1f9d39c7ff4f2b1")]
public partial class PlayerHasAttackedCondition : Condition
{

    public override bool IsTrue()
    {
        if(LearningData.atkCount > 0){
            return true;
        }
        else return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
