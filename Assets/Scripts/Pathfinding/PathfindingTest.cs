using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathfindingTest : MonoBehaviour {
    private Pathfinding pf;
    private List<PathNode> path;

    void Start() {
        this.pf = new Pathfinding(20, 20);
        this.path = this.pf.ComputePath(0, 0, 14, 18);
        Debug.Log(this.path.Count);
        for (int i = 0; i < this.path.Count - 1; i++) {
            Debug.Log("Current " + this.path[i].x + " - " +  this.path[i].y);
            Debug.Log("Next " + this.path[i + 1].x + " - " +  this.path[i + 1].y);
        }
        this.UpdateVisuals();
    }

    void FixedUpdate() {
        this.UpdateVisuals();
    }

    private void UpdateVisuals() {
        for (int i = 0; i < this.pf.field.width; i++) {
            for (int j = 0; j < this.pf.field.height; j++) {
                Debug.DrawLine(new Vector2(this.pf.field.position.x + i * this.pf.field.cellWidth, this.pf.field.position.y + j * this.pf.field.cellHeight),
                               new Vector2(this.pf.field.position.x + (i + 1) * this.pf.field.cellWidth, this.pf.field.position.y + j * this.pf.field.cellHeight));
                Debug.DrawLine(new Vector2(this.pf.field.position.x + i * this.pf.field.cellWidth, this.pf.field.position.y + j * this.pf.field.cellHeight),
                               new Vector2(this.pf.field.position.x + i * this.pf.field.cellWidth, this.pf.field.position.y + (j + 1) * this.pf.field.cellHeight));
            }
        }
        Debug.DrawLine(new Vector2(this.pf.field.position.x, this.pf.field.position.y + this.pf.field.height * this.pf.field.cellHeight),
                       new Vector2(this.pf.field.position.x + this.pf.field.width * this.pf.field.cellWidth, this.pf.field.position.y + this.pf.field.height * this.pf.field.cellHeight));
        Debug.DrawLine(new Vector2(this.pf.field.position.x + this.pf.field.width * this.pf.field.cellWidth, this.pf.field.position.y),
                       new Vector2(this.pf.field.position.x + this.pf.field.width * this.pf.field.cellWidth, this.pf.field.position.y + this.pf.field.height * this.pf.field.cellHeight));
        
        for (int i = 0; i < this.path.Count - 1; i++) {
            Vector2 currentPosition = this.pf.field.IndexToPosition(this.path[i].x, this.path[i].y) + new Vector2(0.5f, 0.34375f);
            Vector2 nextPosition = this.pf.field.IndexToPosition(this.path[i + 1].x, this.path[i + 1].y) + new Vector2(0.5f, 0.34375f);
            Debug.DrawLine(currentPosition, nextPosition, Color.cyan);
        }
    }
}
