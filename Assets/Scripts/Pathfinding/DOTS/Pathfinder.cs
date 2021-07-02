using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;

public class Pathfinder : MonoBehaviour {
    public void FixedUpdate() {
        float startTime = Time.realtimeSinceStartup;
        // Create a new pathfinding job.
        ComputePathJob computePathJob = new ComputePathJob {
            fieldSize = new int2(50, 50),
            start = new int2(0, 0),
            end = new int2(19, 19),
        };

        // Schedule and complete the pathfinding job.
        JobHandle handle = computePathJob.Schedule();
        handle.Complete();

        Debug.Log("DOTSTime: " + ((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }
}
