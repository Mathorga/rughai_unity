using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildSpawnController : MonoBehaviour {
    // The kind of wild to spawn.
    public GameObject wild;

    // The amount of wild to spawn.
    public int amount;

    // Radius in which the spawn extends.
    public float radius;

    void Start() {
        // Dummily instantiate amount Gameobjects
        for (int i = 0; i < this.amount; i++) {
            GameObject ropeSegment = Instantiate(this.wild,
                                                 new Vector2(this.transform.position.x + Random.Range(-this.radius, this.radius), this.transform.position.y + Random.Range(-this.radius, this.radius)),
                                                 Quaternion.identity);
        }
    }

    void Update() {
        
    }
}
