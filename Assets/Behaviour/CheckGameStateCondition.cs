using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "checkGameState", story: "check if gamestate from [gamedata] is [operator] [integer]", category: "Conditions", id: "53beca9416e22fa88143e7b524ec4034")]
public partial class CheckGameStateCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameData> Gamedata;
    [Comparison(comparisonType: ComparisonType.All)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Operator;
    [SerializeReference] public BlackboardVariable<int> Integer;

    public override bool IsTrue()
    {
        int gameStateValue = GameData.gameState;
        int compareValue = Integer.Value;

        switch (Operator.Value)
        {
            case ConditionOperator.Equal:
                return gameStateValue == compareValue;
            case ConditionOperator.NotEqual:
                return gameStateValue != compareValue;
            case ConditionOperator.Greater:
                return gameStateValue > compareValue;
            case ConditionOperator.GreaterOrEqual:
                return gameStateValue >= compareValue;
            case ConditionOperator.Lower:
                return gameStateValue < compareValue;
            case ConditionOperator.LowerOrEqual:
                return gameStateValue <= compareValue;
            default:
                return false;
        }
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
