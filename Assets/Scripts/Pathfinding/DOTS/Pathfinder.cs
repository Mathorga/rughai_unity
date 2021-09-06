using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {
    public PathfindingField pfField;
    public Transform target;

    public Vector2 nextStep {
        get;
        private set;
    }

    public void Start() {
        this.nextStep = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public void FixedUpdate() {
        int2 fieldSize = this.pfField.fieldSize;
        Vector2Int end = this.pfField.field.PositionToIndex(this.target.position);
        Vector2Int start = this.pfField.field.PositionToIndex(this.transform.position);

        // Allocate all the nodes in the current field.
        NativeArray<DOTSPathNode> pathNodes = new NativeArray<DOTSPathNode>(fieldSize.x * fieldSize.y, Allocator.TempJob);
        
        // Populate nodes data.
        for (int i = 0; i < fieldSize.x; i++) {
            for (int j = 0; j < fieldSize.y; j++) {
                DOTSPathNode node = new DOTSPathNode();
                node.x = i;
                node.y = j;
                node.index = Utils.ComputeIndex(i, j, fieldSize.x);

                node.gCost = int.MaxValue;
                node.hCost = Utils.ComputeHCost(new int2(i, j), new int2(end.x, end.y));
                node.ComputeFCost();

                // Collider2D hitCollider = Physics2D.OverlapPoint(this.pfField.field.IndexToPosition(i, j));

                // node.walkable = hitCollider != null ? false : true;
                node.walkable = this.pfField.field.data[i, j].walkable;
                // node.walkable = true;

                node.previous = -1;

                pathNodes[node.index] = node;
            }
        }

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

        float endTime = Time.realtimeSinceStartup;

        // Debug.Log("DOTSTime: " + ((endTime - startTime) * 1000f) + "ms");

        // Schedule and complete the pathfinding job.
        JobHandle handle = computePathJob.Schedule();
        handle.Complete();

        // for (int i = 0; i < result.Length - 1; i++) {
        //     Debug.DrawLine(this.pfField.field.IndexToPosition(result[i].x, result[i].y),
        //                    this.pfField.field.IndexToPosition(result[i + 1].x, result[i + 1].y),
        //                    new Color(((float) i) / result.Length, ((float) i) / result.Length, 0.0f));
        // }

        if (result.Length > 1) {
            this.nextStep = this.pfField.field.IndexToPosition(result[result.Length - 2].x, result[result.Length - 2].y);
        }

        pathNodes.Dispose();
        result.Dispose();
    }
}
