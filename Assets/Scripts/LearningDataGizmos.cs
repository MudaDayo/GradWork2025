using UnityEngine;
using System.Collections.Generic;

public class LearningDataGizmos : MonoBehaviour
{
    [Header("World Position Visualization")]
    public Color positionPointColor = Color.cyan;
    public Color positionLineColor = Color.blue;
    public float positionPointRadius = 0.25f;

    [Header("Attack Distance Visualization")]
    public Color minAtkColor = Color.green;
    public Color avgAtkColor = Color.yellow;
    public Color maxAtkColor = Color.red;

    public Transform referencePoint; // player spawn or world origin

    void OnDrawGizmos()
    {
        DrawWorldPositionPath();
        DrawAttackDistanceRanges();
    }

    // ─────────── WORLD POSITION WINDOWS ───────────
    void DrawWorldPositionPath()
    {
        List<Vector3> positions = LearningData.avgWorldPositionsPerWindow;
        if (positions == null || positions.Count == 0)
            return;

        // Draw points
        Gizmos.color = positionPointColor;
        foreach (Vector3 pos in positions)
        {
            Gizmos.DrawSphere(pos, positionPointRadius);
        }

        // Draw lines
        Gizmos.color = positionLineColor;
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
    }

    // ─────────── ATTACK DISTANCE RANGES ───────────
    void DrawAttackDistanceRanges()
    {
        // Debug.Log("min " +LearningData.minAtkDistance);
        // Debug.Log("max " + LearningData.maxAtkDistance);
        // Debug.Log("avg " + LearningData.avgAtkDistance);

        if (referencePoint == null)
            return;

        Vector3 center = referencePoint.position;


            Gizmos.color = minAtkColor;
            Gizmos.DrawWireSphere(center, LearningData.minAtkDistance);

            Gizmos.color = avgAtkColor;
            Gizmos.DrawWireSphere(center, LearningData.avgAtkDistance);

            Gizmos.color = maxAtkColor;
            Gizmos.DrawWireSphere(center, LearningData.maxAtkDistance);
        

    }
}
