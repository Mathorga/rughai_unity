using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;

public class DataOrientedPathfinding : MonoBehaviour {
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

    void FixedUpdate() {
        float startTime = Time.realtimeSinceStartup;

        int jobCount = 20;
        NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(jobCount, Allocator.TempJob);
        for (int i = 0; i < jobCount; i++) {
            ComputePathJob computePathJob = new ComputePathJob {
                start = new int2(0, 0),
                end = new int2(49, 49)
            };
            jobHandles[i] = computePathJob.Schedule();
        }
        JobHandle.CompleteAll(jobHandles);
        jobHandles.Dispose();

        Debug.Log("DOTSTime: " + ((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    [BurstCompile]
    private struct ComputePathJob : IJob {

        public int2 start;
        public int2 end;
        public void Execute() {
            int2 fieldSize = new int2(50, 50);

            NativeArray<PathNode> pathNodes = new NativeArray<PathNode>(fieldSize.x * fieldSize.y, Allocator.Temp);

            for (int i = 0; i < fieldSize.x; i++) {
                for (int j = 0; j < fieldSize.y; j++) {
                    PathNode node = new PathNode();
                    node.x = i;
                    node.y = j;
                    node.index = this.ComputeIndex(i, j, fieldSize.x);

                    node.gCost = int.MaxValue;
                    node.hCost = this.ComputeHCost(new int2(i, j), end);
                    node.ComputeFCost();

                    node.walkable = true;
                    node.previous = -1;

                    pathNodes[node.index] = node;
                }
            }

            NativeArray<int2> neighborOffsets = new NativeArray<int2>(8, Allocator.Temp);
            neighborOffsets[0] = new int2(-1, 0);
            neighborOffsets[1] = new int2(1, 0);
            neighborOffsets[2] = new int2(-1, -1);
            neighborOffsets[3] = new int2(1, -1);
            neighborOffsets[4] = new int2(-1, 1);
            neighborOffsets[5] = new int2(1, 1);
            neighborOffsets[6] = new int2(0, -1);
            neighborOffsets[7] = new int2(0, 1);

            int endNodeIndex = this.ComputeIndex(end.x, end.y, fieldSize.x);

            PathNode startNode = pathNodes[this.ComputeIndex(start.x, start.y, fieldSize.x)];
            startNode.gCost = 0;
            startNode.ComputeFCost();
            pathNodes[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            int count = 0;

            while (openList.Length > 0) {
                int currentNodeIndex = this.GetFastestNodeIndex(openList, pathNodes);
                PathNode currentNode = pathNodes[currentNodeIndex];

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
                            neighborPosition.x < fieldSize.x &&
                            neighborPosition.y < fieldSize.y) {
                            int neighborIndex = this.ComputeIndex(neighborPosition.x, neighborPosition.y, fieldSize.x);

                            if (!closedList.Contains(neighborIndex)) {
                                PathNode neighborNode = pathNodes[neighborIndex];

                                if (neighborNode.walkable) {
                                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                                    int tentativeGCost = currentNode.gCost + this.ComputeHCost(currentNodePosition, neighborPosition);
                                    if (tentativeGCost < neighborNode.gCost) {
                                        neighborNode.previous = currentNodeIndex;
                                        neighborNode.gCost = tentativeGCost;
                                        neighborNode.ComputeFCost();
                                        pathNodes[neighborIndex] = neighborNode;

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

            PathNode endNode = pathNodes[endNodeIndex];
            if (endNode.previous != -1) {
                NativeList<int2> path = this.RetrievePath(pathNodes, endNode);
                path.Dispose();
            }

            pathNodes.Dispose();
            neighborOffsets.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }
        
        private NativeList<int2> RetrievePath(NativeArray<PathNode> pathNodes, PathNode endNode) {
            if (endNode.previous == -1) {
                return new NativeList<int2>(Allocator.Temp);
            } else {
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.x, endNode.y));

                PathNode currentNode = endNode;
                while (currentNode.previous != -1) {
                    PathNode previousNode = pathNodes[currentNode.previous];
                    path.Add(new int2(previousNode.x, previousNode.y));
                    currentNode = previousNode;
                }
                return path;
            }
        }

        private int GetFastestNodeIndex(NativeList<int> indexes, NativeArray<PathNode> nodes) {
            PathNode result = nodes[indexes[0]];

            for (int i = 0; i < indexes.Length; i++) {
                PathNode candidate = nodes[indexes[i]];
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

    private void ComputePath(int2 start, int2 end) {
    }





    private struct PathNode {
        public int x;
        public int y;

        public int index;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool walkable;
        public int previous;

        public void ComputeFCost() {
            this.fCost = this.gCost + this.hCost;
        }
    }
}
