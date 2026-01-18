using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "EnemyAttackTimer", story: "enemy attack timer is [operator] [number] via [valuechecker]", category: "Conditions", id: "f9c01f91e941c60e4d239ec6d5fc7216")]
public partial class EnemyAttackTimerCondition : Condition
{
    [Comparison(comparisonType: ComparisonType.All)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Operator;
    [SerializeReference] public BlackboardVariable<float> Number;
    [SerializeReference] public BlackboardVariable<BehaviourValueChecker> Valuechecker;

    public override bool IsTrue()
    {
        if (Valuechecker == null || Valuechecker.Value == null)
            return false;

        float timer = Valuechecker.Value.enemyCooldownTimer;
        float compareValue = Number.Value;

        switch (Operator.Value)
        {
            case ConditionOperator.Lower:
                return timer < compareValue;

            case ConditionOperator.LowerOrEqual:
                return timer <= compareValue;

            case ConditionOperator.Greater:
                return timer > compareValue;

            case ConditionOperator.GreaterOrEqual:
                return timer >= compareValue;

            case ConditionOperator.Equal:
                return Mathf.Approximately(timer, compareValue);

            case ConditionOperator.NotEqual:
                return !Mathf.Approximately(timer, compareValue);

            default:
                return false;
        }
    }


    public override void OnStart() { }
    public override void OnEnd() { }
}
