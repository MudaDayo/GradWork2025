using UnityEngine;
using System.Collections.Generic;
public class LearningData : MonoBehaviour
{
    // distances at which the player tends to attack
    public static float avgAtkDistance, maxAtkDistance = 0f;
    public static float minAtkDistance = float.MaxValue;

    public static float atkDistanceCount = 0f;

    public static float atkCount = 0f;

    public static float avgTimeBetweenAttacks = 0f;
    public static float minTimeBetweenAttacks = float.MaxValue;
    public static int atkTimingCount = 0;
    public static float lastAttackTime = -999f;


    //public static float minEnemyAtkDistance, avgEnemyAtkDistance, maxEnemyAtkDistance;


    // speed at which the player inputs things (for movement)
    public static float minInputDiff, avgInputDiff, maxInputDiff;

    // speed at which the player inputs the attack
    public static float minAtkDiff, avgAtkDiff, maxAtkDiff;


    // the enemy attack timings that the player decides to attack and get closer in
    public static float minAgressivnessTiming, avgAgressivnessTiming, maxAgressivnessTiming;


    // world position ( find average position of player each 5 seconds of game time )
    public static List<Vector3> avgWorldPositionsPerWindow = new List<Vector3>();
}
