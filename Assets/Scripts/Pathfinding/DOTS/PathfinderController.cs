using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class PathfinderController : MonoBehaviour {
    public PathfindingField pfField;

    private GameObject[] chasers;

    // Start is called before the first frame update
    void Start() {
        this.chasers = GameObject.FindGameObjectsWithTag("Chaser");
    }

    // Update is called once per frame
    void FixedUpdate() {
        int2 fieldSize = this.pfField.fieldSize;

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
                // node.hCost = Utils.ComputeHCost(new int2(i, j), new int2(end.x, end.y));
                node.ComputeFCost();

                node.walkable = this.pfField.field.data[i, j].walkable;

                node.previous = -1;

                pathNodes[node.index] = node;
            }
        }

        // The resulting path from start to end.
        NativeList<int2> result = new NativeList<int2>(Allocator.TempJob);

        // Allocate jobs.
        NativeArray<JobHandle> pathfindingJobs = new NativeArray<JobHandle>(this.chasers.Length, Allocator.TempJob);

        for (int i = 0; i < this.chasers.Length; i++) {
            ChaserController chaserController = this.chasers[i].GetComponent<ChaserController>();
            Vector2Int start = this.pfField.field.PositionToIndex(chaserController.transform.position);
            Vector2Int end = this.pfField.field.PositionToIndex(chaserController.target.position);

            ComputePathJob computePathJob = new ComputePathJob {
                fieldSize = fieldSize,
                start = new int2(start.x, start.y),
                end = new int2(end.x, end.y),
                pathNodes = pathNodes,
                result = result
            };
            pathfindingJobs[i] = computePathJob.Schedule();
        }

        // Schedule and complete the pathfinding job.
        JobHandle.CompleteAll(pathfindingJobs);

        // if (result.Length > 1) {
        //     this.nextStep = this.pfField.field.IndexToPosition(result[result.Length - 2].x, result[result.Length - 2].y);
        // }

        pathNodes.Dispose();
        result.Dispose();
        pathfindingJobs.Dispose();
    }
}
