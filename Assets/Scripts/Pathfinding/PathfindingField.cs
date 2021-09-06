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
                                             1.0f,
                                             Utils.TILE_RATIO,
                                             this.tileBounds.min,
                                             (Field<DOTSPathNode> field, int x, int y) => new DOTSPathNode());

        for (int i = 0; i < this.field.data.GetLength(0); i++) {
            for (int j = 0; j < this.field.data.GetLength(1); j++) {
                Collider2D[] hitColliders = Physics2D.OverlapPointAll(this.field.IndexToPosition(i, j));

                this.field.data[i, j].walkable = hitColliders.Length > 0 ? false : true;
            }
        }
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

        for (int i = 0; i < this.field.data.GetLength(0); i++) {
            for (int j = 0; j < this.field.data.GetLength(1); j++) {
                Gizmos.color = this.field.data[i, j].walkable ? new Color(0f, 0f, 1f, 0.2f) : new Color(1f, 0f, 0f, 0.2f);

                Gizmos.DrawCube(new Vector3(this.field.IndexToPosition(i, j).x, this.field.IndexToPosition(i, j).y, 0f), new Vector3(1f, 0.6875f, 0.5f));
            }
        }
    }
}
