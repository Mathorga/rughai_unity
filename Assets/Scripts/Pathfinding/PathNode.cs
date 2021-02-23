using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
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
        set;
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
        List<PathNode> neighbors = new List<PathNode>();

        if (this.x - 1 >= 0) {
            // Left.
            neighbors.Add(this.field.GetElement(this.x - 1, this.y));

            if (this.y - 1 >= 0) {
                // Top-left.
                neighbors.Add(this.field.GetElement(this.x - 1, this.y - 1));
            }
            if (this.y + 1 < this.field.height) {
                // Bottom-left.
                neighbors.Add(this.field.GetElement(this.x - 1, this.y + 1));
            }
        }

        if (this.x + 1 < this.field.width) {
            // Right.
            neighbors.Add(this.field.GetElement(this.x + 1, this.y));

            if (this.y - 1 >= 0) {
                // Top-right.
                neighbors.Add(this.field.GetElement(this.x + 1, this.y - 1));
            }
            if (this.y + 1 < this.field.height) {
                // Bottom-right.
                neighbors.Add(this.field.GetElement(this.x + 1, this.y + 1));
            }
        }

        if (this.y - 1 >= 0) {
            // Top.
            neighbors.Add(this.field.GetElement(this.x, this.y - 1));
        }

        if (this.y + 1 < this.field.height) {
            // Bottom.
            neighbors.Add(this.field.GetElement(this.x, this.y + 1));
        }

        return neighbors;
    }

    public void ComputeFCost() {
        this.fCost = this.gCost + this.hCost;
    }

    public override string ToString() {
        return this.x + " | " + this.y;
    }
}
