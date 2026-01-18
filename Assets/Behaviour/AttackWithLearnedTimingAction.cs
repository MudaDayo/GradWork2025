using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "AttackWithLearnedTiming",
    story: "TryAttack from [playerController] respecting learned attack timing",
    category: "Action",
    id: "494781b9d32ae5de30cacee524bb56d6"
)]
public partial class AttackWithLearnedTimingAction : Action
{
    [SerializeReference] public BlackboardVariable<PlayerController3D> PlayerController;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
{
    if (PlayerController.Value == null)
        return Status.Failure;

    float minCooldown = LearningData.minTimeBetweenAttacks;

    // Safety fallback
    if (minCooldown <= 0f || minCooldown == float.MaxValue)
        minCooldown = 0.25f;

    float timeSinceLastAttack = Time.time - LearningData.lastAttackTime;

    // ❌ Too soon — attack not available
    if (timeSinceLastAttack < minCooldown)
        return Status.Failure;

    // ✅ Allowed to attack
    if (PlayerController.Value.TryAttack())
    {
        LearningData.lastAttackTime = Time.time;
        return Status.Success;
    }

    return Status.Failure;
}

    protected override void OnEnd() { }
}
