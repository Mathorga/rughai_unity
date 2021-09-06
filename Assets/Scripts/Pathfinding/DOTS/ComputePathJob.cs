using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

// Defines a pathfinding job to be scheduled for a single pathfinding actor.
[BurstCompile]
public struct ComputePathJob : IJob {
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

    // Size of the pathfinding field.
    public int2 fieldSize;

    // Starting point for the pathfinding algorithm.
    public int2 start;

    // Destination point for the pathfinding algorithm.
    public int2 end;

    public NativeArray<DOTSPathNode> pathNodes;

    public NativeList<int2> result;

    // Runs the pathfinding algorithm on the given start and end points.
    public void Execute() {

        NativeArray<int2> neighborOffsets = new NativeArray<int2>(8, Allocator.Temp);
        neighborOffsets[0] = new int2(-1, 0);
        neighborOffsets[1] = new int2(1, 0);
        neighborOffsets[2] = new int2(-1, -1);
        neighborOffsets[3] = new int2(1, -1);
        neighborOffsets[4] = new int2(-1, 1);
        neighborOffsets[5] = new int2(1, 1);
        neighborOffsets[6] = new int2(0, -1);
        neighborOffsets[7] = new int2(0, 1);

        int endNodeIndex = this.ComputeIndex(this.end.x, this.end.y, this.fieldSize.x);

        DOTSPathNode startNode = this.pathNodes[this.ComputeIndex(this.start.x, this.start.y, this.fieldSize.x)];
        startNode.gCost = 0;
        startNode.ComputeFCost();
        this.pathNodes[startNode.index] = startNode;

        NativeList<int> openList = new NativeList<int>(Allocator.Temp);
        NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

        openList.Add(startNode.index);

        int count = 0;

        while (openList.Length > 0) {
            int currentNodeIndex = this.GetFastestNodeIndex(openList, this.pathNodes);
            DOTSPathNode currentNode = this.pathNodes[currentNodeIndex];

            count++;
            if (count > 1000) {
                break;
            }

            if (currentNodeIndex != endNodeIndex) {
                for (int i = 0; i < openList.Length; i++) {
                    if (openList[i] == currentNodeIndex) {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighborOffsets.Length; i++) {
                    int2 neighborOffset = neighborOffsets[i];
                    int2 neighborPosition = new int2(currentNode.x + neighborOffset.x, currentNode.y + neighborOffset.y);

                    if (neighborPosition.x > 0 &&
                        neighborPosition.y > 0 &&
                        neighborPosition.x < this.fieldSize.x &&
                        neighborPosition.y < this.fieldSize.y) {
                        int neighborIndex = this.ComputeIndex(neighborPosition.x, neighborPosition.y, this.fieldSize.x);

                        if (!closedList.Contains(neighborIndex)) {
                            DOTSPathNode neighborNode = this.pathNodes[neighborIndex];

                            if (neighborNode.walkable) {
                                int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                                int tentativeGCost = currentNode.gCost + this.ComputeHCost(currentNodePosition, neighborPosition);
                                if (tentativeGCost < neighborNode.gCost) {
                                    neighborNode.previous = currentNodeIndex;
                                    neighborNode.gCost = tentativeGCost;
                                    neighborNode.ComputeFCost();
                                    this.pathNodes[neighborIndex] = neighborNode;

                                    if (!openList.Contains(neighborNode.index)) {
                                        openList.Add(neighborNode.index);
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                break;
            }
        }

        DOTSPathNode endNode = this.pathNodes[endNodeIndex];
        this.RetrievePath(this.pathNodes, endNode, this.result);

        neighborOffsets.Dispose();
        openList.Dispose();
        closedList.Dispose();
    }
    
    private void RetrievePath(NativeArray<DOTSPathNode> pathNodes, DOTSPathNode endNode, NativeList<int2> result) {
        if (endNode.previous != -1) {
            // Start by adding the last node to the empty list.
            result.Add(new int2(endNode.x, endNode.y));

            DOTSPathNode currentNode = endNode;
            while (currentNode.previous != -1) {
                DOTSPathNode previousNode = pathNodes[currentNode.previous];
                result.Add(new int2(previousNode.x, previousNode.y));
                currentNode = previousNode;
            }
        }
    }

    private int GetFastestNodeIndex(NativeList<int> indexes, NativeArray<DOTSPathNode> nodes) {
        DOTSPathNode result = nodes[indexes[0]];

        for (int i = 0; i < indexes.Length; i++) {
            DOTSPathNode candidate = nodes[indexes[i]];
            if (candidate.fCost < result.fCost) {
                result = candidate;
            }
        }

        return result.index;
    }

    private int ComputeHCost(int2 start, int2 end) {
        int xDistance = math.abs(start.x - end.x);
        int yDistance = math.abs(start.y - end.y);
        int remaining = math.abs(xDistance - yDistance);

        return DIAGONAL_COST * math.min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }

    private int ComputeIndex(int x, int y, int width) {
        return x + y * width;
    }
}
