using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathTest : MonoBehaviour {

    void Start() {
        Pathfinding pf = new Pathfinding(this.transform.position, 50, 50);
        float startTime = Time.realtimeSinceStartup;

        for (int i = 0; i < 50; i++) {
            pf.ComputePath(0, 0, 49, 49);
        }

        Debug.Log("Time: " + ((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }
}
