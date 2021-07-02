using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

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

    // Runs the pathfinding algorithm on the given start and end points.
    public void Execute() {
        NativeArray<DOTSPathNode> pathNodes = new NativeArray<DOTSPathNode>(this.fieldSize.x * this.fieldSize.y, Allocator.Temp);

        for (int i = 0; i < this.fieldSize.x; i++) {
            for (int j = 0; j < this.fieldSize.y; j++) {
                DOTSPathNode node = new DOTSPathNode();
                node.x = i;
                node.y = j;
                node.index = this.ComputeIndex(i, j, this.fieldSize.x);

                node.gCost = int.MaxValue;
                node.hCost = this.ComputeHCost(new int2(i, j), this.end);
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

        int endNodeIndex = this.ComputeIndex(this.end.x, this.end.y, this.fieldSize.x);

        DOTSPathNode startNode = pathNodes[this.ComputeIndex(this.start.x, this.start.y, this.fieldSize.x)];
        startNode.gCost = 0;
        startNode.ComputeFCost();
        pathNodes[startNode.index] = startNode;

        NativeList<int> openList = new NativeList<int>(Allocator.Temp);
        NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

        openList.Add(startNode.index);

        int count = 0;

        while (openList.Length > 0) {
            int currentNodeIndex = this.GetFastestNodeIndex(openList, pathNodes);
            DOTSPathNode currentNode = pathNodes[currentNodeIndex];

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
                            DOTSPathNode neighborNode = pathNodes[neighborIndex];

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

        DOTSPathNode endNode = pathNodes[endNodeIndex];
        if (endNode.previous != -1) {
            NativeList<int2> path = this.RetrievePath(pathNodes, endNode);
            path.Dispose();
        }

        pathNodes.Dispose();
        neighborOffsets.Dispose();
        openList.Dispose();
        closedList.Dispose();
    }
    
    private NativeList<int2> RetrievePath(NativeArray<DOTSPathNode> pathNodes, DOTSPathNode endNode) {
        if (endNode.previous == -1) {
            return new NativeList<int2>(Allocator.Temp);
        } else {
            NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
            path.Add(new int2(endNode.x, endNode.y));

            DOTSPathNode currentNode = endNode;
            while (currentNode.previous != -1) {
                DOTSPathNode previousNode = pathNodes[currentNode.previous];
                path.Add(new int2(previousNode.x, previousNode.y));
                currentNode = previousNode;
            }
            return path;
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
