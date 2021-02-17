using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathfindingTest : MonoBehaviour {
    private Pathfinding pf;

    void Start() {
        this.pf = new Pathfinding(5, 5);
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
    }
}
