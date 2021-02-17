using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {
    public Field<PathNode> field {
        get;
        private set;
    }

    public Pathfinding(int width, int height) {
        this.field = new Field<PathNode>(width,
                                         height,
                                         1,
                                         Utils.TILE_RATIO,
                                         Vector2.zero,
                                         (Field<PathNode> field, int x, int y) => new PathNode(field, x, y));
        

    }

}
