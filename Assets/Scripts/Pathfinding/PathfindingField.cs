using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class PathfindingField : MonoBehaviour {
    public Tilemap tilemap;

    public Bounds tileBounds {
        get;
        private set;
    }

    public Field<DOTSPathNode> field {
        get;
        private set;
    }

    public int2 fieldSize {
        get;
        private set;
    }

    void Start() {
        this.tileBounds = this.tilemap.localBounds;
        this.fieldSize = new int2(Mathf.FloorToInt(this.tileBounds.size.x), Mathf.FloorToInt(this.tileBounds.size.y / Utils.TILE_RATIO));
        
        // Bounds.size returns the size in game units.
        this.field = new Field<DOTSPathNode>(Mathf.FloorToInt(this.tileBounds.size.x),
                                             Mathf.FloorToInt(this.tileBounds.size.y / Utils.TILE_RATIO),
                                             1,
                                             Utils.TILE_RATIO,
                                             this.tileBounds.min,
                                             (Field<DOTSPathNode> field, int x, int y) => new DOTSPathNode());
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.gray;

        for (int i = 0; i < this.tileBounds.size.x; i++) {
            Gizmos.DrawLine(new Vector2(this.tileBounds.min.x + i, this.tileBounds.min.y),
                            new Vector2(this.tileBounds.min.x + i, this.tileBounds.max.y));
        }

        for (int i = 0; i < this.tileBounds.size.y / Utils.TILE_RATIO; i++) {
            Gizmos.DrawLine(new Vector2(this.tileBounds.min.x, this.tileBounds.min.y + i * Utils.TILE_RATIO),
                            new Vector2(this.tileBounds.max.x, this.tileBounds.min.y + i * Utils.TILE_RATIO));
        }

        // Left border.
        Gizmos.DrawLine(new Vector2(this.tileBounds.min.x, this.tileBounds.min.y),
                        new Vector2(this.tileBounds.min.x, this.tileBounds.max.y));

        // Bottom border.
        Gizmos.DrawLine(new Vector2(this.tileBounds.min.x, this.tileBounds.max.y),
                        new Vector2(this.tileBounds.max.x, this.tileBounds.max.y));

        // Right border.
        Gizmos.DrawLine(new Vector2(this.tileBounds.max.x, this.tileBounds.max.y),
                        new Vector2(this.tileBounds.max.x, this.tileBounds.min.y));

        // Top border.
        Gizmos.DrawLine(new Vector2(this.tileBounds.max.x, this.tileBounds.min.y),
                        new Vector2(this.tileBounds.min.x, this.tileBounds.min.y));
    }
}
