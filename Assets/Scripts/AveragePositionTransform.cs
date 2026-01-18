using UnityEngine;
using System.Collections.Generic;

public class AveragePositionTransform : MonoBehaviour
{
    [Header("Timing")]
    public float windowDuration = 5f; // must match training window

    [Header("Playback")]
    public bool loop = true;
    public float timeOffset = 0f; // optional manual offset

    private float timer;

    void Update()
    {
        List<Vector3> positions = LearningData.avgWorldPositionsPerWindow;
        if (positions == null || positions.Count == 0)
            return;

        timer += Time.deltaTime;

        float playbackTime = timer + timeOffset;
        int index = Mathf.FloorToInt(playbackTime / windowDuration);

        if (loop)
        {
            index %= positions.Count;
        }
        else
        {
            index = Mathf.Clamp(index, 0, positions.Count - 1);
        }

        transform.position = positions[index];
    }

    public void ResetPlayback()
    {
        timer = 0f;
    }
}
