using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "TryAttack from [playerController]", category: "Action", id: "bd2e47b4c9114aee27d62e09e5233836")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<PlayerController3D> PlayerController;
    protected override Status OnStart()
    {
        if (PlayerController.Value != null && PlayerController.Value.TryAttack())
        {
            return Status.Success;
        }
        else return Status.Failure;

    }
}

