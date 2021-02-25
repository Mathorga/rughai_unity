using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public class DOTSPathfinding : MonoBehaviour {
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

    private void ComputePath(int2 start, int2 end) {
        int2 fieldSize = new int2(4, 4);

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

        PathNode startNode = pathNodes[this.ComputeIndex(start.x, start.y, fieldSize.x)];
        startNode.gCost = 0;
        startNode.ComputeFCost();
        pathNodes[startNode.index] = startNode;

        pathNodes.Dispose();
    }

    private int ComputeIndex(int x, int y, int width) {
        return x + y * width;
    }

    private int ComputeHCost(int2 start, int2 end) {
        int xDistance = math.abs(start.x - end.x);
        int yDistance = math.abs(start.y - end.y);
        int remaining = math.abs(xDistance - yDistance);

        return DIAGONAL_COST * math.min(xDistance, yDistance) + STRAIGHT_COST * remaining;
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
