using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class PathfindingTest : MonoBehaviour {
    private Pathfinding pf;
    private List<PathNode> path;
    public Tilemap tilemap;

    public Bounds tileBounds {
        get;
        private set;
    }

    public Transform target;

    void Start() {
        this.tileBounds = this.tilemap.localBounds;
        this.pf = new Pathfinding(this.tileBounds.min, Mathf.FloorToInt(this.tileBounds.size.x), Mathf.FloorToInt(this.tileBounds.size.y / Utils.TILE_RATIO));

        // this.path = this.pf.ComputePath(0, 0, 45, 45);
        // this.UpdateVisuals();
    }

    void FixedUpdate() {
        Vector2Int startIndex = this.pf.field.PositionToIndex(this.transform.position);
        Vector2Int targetIndex = this.pf.field.PositionToIndex(this.target.position);
        this.path = this.pf.ComputePath(startIndex.x, startIndex.y, targetIndex.x, targetIndex.y);
        this.UpdateVisuals();
    }

    private void UpdateVisuals() {
        // for (int i = 0; i < this.pf.field.width; i++) {
        //     for (int j = 0; j < this.pf.field.height; j++) {
        //         Debug.DrawLine(new Vector2(this.pf.field.position.x + i * this.pf.field.cellWidth, this.pf.field.position.y + j * this.pf.field.cellHeight),
        //                        new Vector2(this.pf.field.position.x + (i + 1) * this.pf.field.cellWidth, this.pf.field.position.y + j * this.pf.field.cellHeight));
        //         Debug.DrawLine(new Vector2(this.pf.field.position.x + i * this.pf.field.cellWidth, this.pf.field.position.y + j * this.pf.field.cellHeight),
        //                        new Vector2(this.pf.field.position.x + i * this.pf.field.cellWidth, this.pf.field.position.y + (j + 1) * this.pf.field.cellHeight));
        //     }
        // }
        // Debug.DrawLine(new Vector2(this.pf.field.position.x, this.pf.field.position.y + this.pf.field.height * this.pf.field.cellHeight),
        //                new Vector2(this.pf.field.position.x + this.pf.field.width * this.pf.field.cellWidth, this.pf.field.position.y + this.pf.field.height * this.pf.field.cellHeight));
        // Debug.DrawLine(new Vector2(this.pf.field.position.x + this.pf.field.width * this.pf.field.cellWidth, this.pf.field.position.y),
        //                new Vector2(this.pf.field.position.x + this.pf.field.width * this.pf.field.cellWidth, this.pf.field.position.y + this.pf.field.height * this.pf.field.cellHeight));

        for (int i = 0; i < this.path.Count - 1; i++) {
            Vector2 currentPosition = this.pf.field.IndexToPosition(this.path[i].x, this.path[i].y);// + new Vector2(0.5f, 0.34375f);
            Vector2 nextPosition = this.pf.field.IndexToPosition(this.path[i + 1].x, this.path[i + 1].y);// + new Vector2(0.5f, 0.34375f);
            Debug.DrawLine(currentPosition, nextPosition, Color.cyan);
        }
    }
}
