using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManagerController : MonoBehaviour {

    public int cloudsCount;
    public float cloudsSpeed;
    public GameObject primaryType;
    public GameObject secondaryType;

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
            GameObject cloud = Instantiate(Random.Range(0, 10) > 2 ? this.primaryType : this.secondaryType,
                                           new Vector2(Random.Range(this.bounds.center.x - this.bounds.extents.x, this.bounds.center.x + this.bounds.extents.x),
                                                       Random.Range(this.bounds.center.y - this.bounds.extents.y, this.bounds.center.y + this.bounds.extents.y)),
                                           Quaternion.identity);
            cloud.transform.parent = this.transform;
            cloud.GetComponent<CloudController>().speed = Random.Range(this.cloudsSpeed - 0.005f, this.cloudsSpeed + 0.005f);
            this.clouds.Add(cloud);
        }
    }

    // Update is called once per frame
    void Update() {
        // TODO Update all clouds.
        // Move clouds to the beginning if they go off bounds.
        foreach (GameObject cloud in this.clouds) {
            if (cloud.transform.position.x > this.bounds.center.x + this.bounds.extents.x) {
                cloud.transform.position = new Vector2(this.bounds.center.x - this.bounds.extents.x - 1,
                                                       Random.Range(this.bounds.center.y - this.bounds.extents.y, this.bounds.center.y + this.bounds.extents.y));
            }
        }
    }
}
