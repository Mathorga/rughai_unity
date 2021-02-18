using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

    private Field<PathNode> field;
    public int x {
        get;
        private set;
    }
    public int y {
        get;
        private set;
    }

    public PathNode previous {
        get;
        set;
    }

    public int gCost {
        get;
        set;
    }
    public int hCost {
        get;
        private set;
    }
    public int fCost {
        get;
        private set;
    }

    public PathNode(Field<PathNode> field, int x, int y) {
        this.field = field;
        this.x = x;
        this.y = y;
    }

    public List<PathNode> GetNeighbors() {
        //TODO
        return null;
    }

    public void ComputeHCost(PathNode goal) {
        int xDistance = Mathf.Abs(this.x - goal.x);
        int yDistance = Mathf.Abs(this.y - goal.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        this.hCost = STRAIGHT_COST * Mathf.Min(xDistance, yDistance) + DIAGONAL_COST * remaining;
    }

    public void ComputeFCost() {
        this.fCost = this.gCost + this.hCost;
    }

    public override string ToString() {
        return this.x + " | " + this.y;
    }
}
