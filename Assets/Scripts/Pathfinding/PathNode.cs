using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
    private Field<PathNode> field;
    private int x;
    private int y;

    private int gCost;
    private int hCost;
    private int fCost;

    private PathNode previous;

    public PathNode(Field<PathNode> field, int x, int y) {
        this.field = field;
        this.x = x;
        this.y = y;
    }
}
