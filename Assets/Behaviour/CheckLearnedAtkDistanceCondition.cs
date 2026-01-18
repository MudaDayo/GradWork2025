using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(
    name: "CheckLearnedAtkDistance",
    story: "distance between [self] and [enemy] is near [Tolerance] learned attack distance",
    category: "Conditions",
    id: "b76fe5c18d5a14b3b1353a951079d0ed")]
public partial class CheckLearnedAtkDistanceCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;

    // Tolerance around learned average (exposed in graph)
    [SerializeReference] public BlackboardVariable<float> Tolerance;

    public override bool IsTrue()
{
    if (Self?.Value == null || Enemy?.Value == null)
        return false;

    if (Tolerance == null)
        return false;

    float learnedAvg = LearningData.avgAtkDistance;
    if (learnedAvg <= 0f)
        return false;

    float toleranceValue = Tolerance.Value; // now safe

    float distance = Vector3.Distance(
        Self.Value.transform.position,
        Enemy.Value.transform.position
    );

    return Mathf.Abs(distance - learnedAvg) <= toleranceValue;
}


    public override void OnStart() { }
    public override void OnEnd() { }
}
