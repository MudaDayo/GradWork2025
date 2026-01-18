using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Move Away From Enemy (Timed)",
    story: "move away from [enemy] for a short time using [valuechecker] via [playercontroller]",
    category: "Action",
    id: "c8b1d2e77a9b4f59a6f94a1c2d0e9b41")] // NEW unique ID
public partial class MoveAwayFromEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<PlayerController3D> Playercontroller;
    [SerializeReference] public BlackboardVariable<BehaviourValueChecker> Valuechecker;

    public float moveDuration = 0.25f;

    private float elapsed;

    protected override Status OnStart()
    {
        if (Enemy.Value == null ||
            Playercontroller.Value == null ||
            Valuechecker.Value == null)
            return Status.Failure;

        elapsed = 0f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        elapsed += Time.deltaTime;

        Playercontroller.Value.SetExternalMovement(
            Valuechecker.Value.directionAwayFromEnemy
        );

        if (elapsed >= moveDuration)
            return Status.Success;

        return Status.Running;
    }

    protected override void OnEnd()
    {
        Playercontroller.Value.ClearExternalMovement();
    }
}
