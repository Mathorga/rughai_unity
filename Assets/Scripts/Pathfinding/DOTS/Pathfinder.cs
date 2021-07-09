using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;

public class Pathfinder : MonoBehaviour {
    public PathfindingField pfField;
    public Transform target;

    public void Start() {

    }

    public void FixedUpdate() {
        int2 fieldSize = this.pfField.fieldSize;
        Vector2Int end = this.pfField.field.PositionToIndex(this.target.position);
        Vector2Int start = this.pfField.field.PositionToIndex(this.transform.position);

        // Allocate all the nodes in the current field.
        NativeArray<DOTSPathNode> pathNodes = new NativeArray<DOTSPathNode>(fieldSize.x * fieldSize.y, Allocator.TempJob);

        // The resulting path from start to end.
        NativeList<int2> result = new NativeList<int2>(Allocator.TempJob);

        float startTime = Time.realtimeSinceStartup;

        // Create a new pathfinding job.
        ComputePathJob computePathJob = new ComputePathJob {
            fieldSize = fieldSize,
            start = new int2(start.x, start.y),
            end = new int2(end.x, end.y),
            pathNodes = pathNodes,
            result = result
        };

        // Debug.Log("DOTSTime: " + ((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");

        // Schedule and complete the pathfinding job.
        JobHandle handle = computePathJob.Schedule();
        handle.Complete();


        pathNodes.Dispose();
        result.Dispose();
    }
}
