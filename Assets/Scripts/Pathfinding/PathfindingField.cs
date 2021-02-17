using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class PathfindingField : MonoBehaviour {
    public Tilemap tilemap;

    public Bounds tileBounds {
        get;
        private set;
    }

    void Start() {
        this.tileBounds = this.tilemap.localBounds;
        // Debug.Log("center " + this.tileBounds.center);
        // Debug.Log("xMin " + this.tileBounds.min.x);
        // Debug.Log("xMax " + this.tileBounds.max.x);
        // Debug.Log("yMin " + this.tileBounds.min.y);
        // Debug.Log("yMax " + this.tileBounds.max.y);
        // Debug.Log("xSize " + this.tileBounds.size.x);
        // Debug.Log("ySize " + this.tileBounds.size.y / Utils.TILE_RATIO);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;

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
