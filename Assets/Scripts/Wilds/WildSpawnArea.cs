using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildSpawnArea : MonoBehaviour {
    public WildSpawner wildSpawner;

    // The kind of wild to spawn.
    public GameObject wildType;

    // The amount of wild to spawn.
    [Range(0, 1000)]
    public int amount;

    // Width over which to extend the spawn.
    [Range(0.0f, 1000.0f)]
    public float width;

    // Height over which to extend the spawn.
    [Range(0.0f, 1000.0f)]
    public float height;

    void Start() {
        // Dummily instantiate amount Gameobjects
        for (int i = 0; i < this.amount; i++) {
            // Randomly place the current wild inside the given area.
            float wildPosX = this.transform.position.x + Random.Range(-(this.width / 2), this.width / 2);
            float wildPosY = this.transform.position.y + Random.Range(-(this.height / 2), this.height / 2);

            // Create the wild in the computed position.
            this.wildSpawner.spawn(this.wildType, wildPosX, wildPosY);
        }
    }

    void Update() {
        // TODO.
    }

    void OnDrawGizmos() {
        // Draw a solid transparent rectangle (0 thickness) to represent the spawn area.
        Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
        Gizmos.DrawCube(this.transform.position, new Vector3(this.width, this.height, 0.0f));
    }
}
