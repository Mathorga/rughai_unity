using UnityEngine;

public struct DOTSPathNode {
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
