using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManagerController : MonoBehaviour {
    // Offset for positioning secondary type clouds.
    private static Vector2 SECONDARY_OFFSET = new Vector2(16, 16);

    public int cloudsCount;
    public float cloudsSpeed;
    public GameObject primaryType;
    public GameObject secondaryType;
    
    public Transform target;

    public Bounds bounds {
        get;
        private set;
    }

    private List<GameObject> clouds;

    // Start is called before the first frame update
    void Start() {
        // Calculate scene bounds.
        Bounds tmpBounds = new Bounds(Vector3.zero, Vector3.zero);
        foreach (Renderer renderer in GameObject.FindObjectsOfType<Renderer>()) {
            tmpBounds.Encapsulate(renderer.bounds);
        }
        this.bounds = tmpBounds;

        // Create all clouds.
        this.clouds = new List<GameObject>();

        for (int i = 0; i < this.cloudsCount; i++) {
            Vector2 randomPosition = new Vector2(Random.Range(this.bounds.center.x - this.bounds.extents.x, this.bounds.center.x + this.bounds.extents.x),
                                                 Random.Range(this.bounds.center.y - this.bounds.extents.y, this.bounds.center.y + this.bounds.extents.y));

            // Create primary.
            GameObject primary = Instantiate(this.primaryType, randomPosition, Quaternion.identity);
            primary.transform.parent = this.transform;

            // Create secondaries around the primary.
            for (int j = 0; j < 3; j++) {
                Vector2 randomOffset =  Vector2.zero;

                int choice = Random.Range(0, 3);

                switch(choice) {
                    case 0:
                        randomOffset = new Vector2(SECONDARY_OFFSET.x * Random.Range(-1.0f, 1.0f), -SECONDARY_OFFSET.y);
                        break;
                    case 1:
                        randomOffset = new Vector2(SECONDARY_OFFSET.x * Random.Range(-1.0f, 1.0f), SECONDARY_OFFSET.y);
                        break;
                    case 2:
                        randomOffset = new Vector2(-SECONDARY_OFFSET.x, SECONDARY_OFFSET.y * Random.Range(-1.0f, 1.0f));
                        break;
                    case 3:
                        randomOffset = new Vector2(SECONDARY_OFFSET.x, SECONDARY_OFFSET.y * Random.Range(-1.0f, 1.0f));
                        break;
                }

                GameObject secondary = Instantiate(this.secondaryType, randomPosition + randomOffset, Quaternion.identity);
                secondary.transform.parent = primary.transform;
            }

            primary.GetComponent<CloudController>().speed = Random.Range(this.cloudsSpeed - 0.005f, this.cloudsSpeed + 0.005f);
            this.clouds.Add(primary);
        }
    }

    // Update is called once per frame
    // void FixedUpdate() {
    //     this.transform.position = Utils.offsetPosition(this.target.position, 0.15f);

    //     // Move clouds to the beginning if they go off bounds.
    //     foreach (GameObject cloud in this.clouds) {
    //         if (cloud.transform.position.x > this.bounds.center.x + this.bounds.extents.x) {
    //             cloud.transform.position = new Vector2(this.bounds.center.x - this.bounds.extents.x,
    //                                                    Random.Range(this.bounds.center.y - this.bounds.extents.y, this.bounds.center.y + this.bounds.extents.y));
    //         } else if (cloud.transform.position.x < this.bounds.center.x - this.bounds.extents.x) {
    //             cloud.transform.position = new Vector2(this.bounds.center.x + this.bounds.extents.x,
    //                                                    Random.Range(this.bounds.center.y - this.bounds.extents.y, this.bounds.center.y + this.bounds.extents.y));
    //         }

    //         if (cloud.transform.position.y > this.bounds.center.y + this.bounds.extents.y) {
    //             cloud.transform.position = new Vector2(Random.Range(this.bounds.center.x - this.bounds.extents.x, this.bounds.center.x + this.bounds.extents.x),
    //                                                    this.bounds.center.y - this.bounds.extents.y);
    //         } else if (cloud.transform.position.y < this.bounds.center.y - this.bounds.extents.y) {
    //             cloud.transform.position = new Vector2(Random.Range(this.bounds.center.x - this.bounds.extents.x, this.bounds.center.x + this.bounds.extents.x),
    //                                                    this.bounds.center.y + this.bounds.extents.y);
    //         }
    //     }
    // }
}
