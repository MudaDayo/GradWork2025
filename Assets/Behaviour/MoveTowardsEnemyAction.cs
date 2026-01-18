using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Move Towards Enemy (Timed)",
    story: "move towards [enemy] for a [moveDuration] time using [valuechecker] via [playercontroller]",
    category: "Action",
    id: "5d808db1beb7ddf76f172f2541c2c82f")]
public partial class MoveTowardsEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<float> moveDuration;
    [SerializeReference] public BlackboardVariable<PlayerController3D> Playercontroller;
    [SerializeReference] public BlackboardVariable<BehaviourValueChecker> Valuechecker;

    // How long to move (seconds)
    //public float moveDuration = 0.25f;

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

    Vector3 playerPos = Playercontroller.Value.transform.position;
    Vector3 enemyPos  = Enemy.Value.transform.position;

    Vector3 vectorToEnemy = enemyPos - playerPos;
    float distanceToEnemy = vectorToEnemy.magnitude;

    Vector3 directionToEnemy =
        distanceToEnemy > 0.0001f
        ? vectorToEnemy / distanceToEnemy
        : Vector3.zero;

    Playercontroller.Value.SetExternalMovement(directionToEnemy);

    if (moveDuration.Value == 0){
        moveDuration.Value = LearningData.avgInputDiff;
    }

    if (elapsed >= moveDuration.Value)
        return Status.Success;

    return Status.Running;
}


    protected override void OnEnd()
    {
        Playercontroller.Value.ClearExternalMovement();
    }

}
