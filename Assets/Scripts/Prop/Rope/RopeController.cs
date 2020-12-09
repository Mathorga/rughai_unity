using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RopeController : MonoBehaviour {
    public GameObject ropeSegmentType;
    public float particlesPerUnit;

    private List<GameObject> anchors;
    private List<Vector2> distances;
    private List<float> angles;

    void Start() {
        this.anchors = new List<GameObject>();
        this.distances = new List<Vector2>();
        this.angles = new List<float>();

        // Retrieve all anchors objects.
        foreach (Transform child in this.transform) {
            this.anchors.Add(child.gameObject);
        }

        // Loop through anchors, but the last one in order to compute distances and angles.
        for (int i = 0; i < this.anchors.Count - 1; i++) {
            // Compute distance.
            this.distances.Add(this.anchors[i + 1].transform.position - this.anchors[i].transform.position);
            Debug.Log("Distance " + this.distances[i]);

            // Compute angle.
            this.angles.Add(Utils.AngleBetween(this.anchors[i].transform.position, this.anchors[i + 1].transform.position));
        }

        // Loop through anchors in order to create segments in between.
        for (int i = 0; i < this.anchors.Count - 1; i++) {
            // Compute the amount of segments to fill the distance between anchors.
            int amount = (int) (this.distances[i].magnitude * this.particlesPerUnit);
            Debug.Log("Magnitude " + this.distances[i].magnitude);
            Debug.Log("Amount " + amount);
            float xStep = this.distances[i].x / amount;
            float yStep = this.distances[i].y / amount;

            // Create segments between anchors.
            List<GameObject> ropeSegments = new List<GameObject>();
            for (int j = 0; j < amount; j++) {
                GameObject ropeSegment = Instantiate(this.ropeSegmentType,
                                                    //  new Vector2(this.anchors[i].transform.position.x + (j + 1) / 2f, this.anchors[i].transform.position.y),
                                                     new Vector2(this.anchors[i].transform.position.x + (j + 1) * xStep, this.anchors[i].transform.position.y + j * yStep),
                                                     Quaternion.identity);
                ropeSegment.transform.parent = this.transform;
                ropeSegments.Add(ropeSegment);
            }

            // Add hinge component to the first anchor and attach it to the first segment.
            SpringJoint2D firstJoint = this.anchors[i].AddComponent<SpringJoint2D>() as SpringJoint2D;
            firstJoint.connectedBody = ropeSegments[0].GetComponent<Rigidbody2D>();
            firstJoint.autoConfigureDistance = false;
            firstJoint.distance = 0f;
            firstJoint.frequency = 10f;

            // Loop through rope segments in order to attach hinges to them.
            for (int j = 0; j < amount - 1; j++) {
                SpringJoint2D segmentJoint = ropeSegments[j].AddComponent<SpringJoint2D>() as SpringJoint2D;
                segmentJoint.connectedBody = ropeSegments[j + 1].GetComponent<Rigidbody2D>();
                segmentJoint.autoConfigureDistance = false;
                segmentJoint.distance = 0f;
                segmentJoint.frequency = 10f;
            }

            // Add hinge component to the last rope segment and attach it to the next anchor.
            // (ropeSegments[amount - 1].AddComponent<HingeJoint2D>() as HingeJoint2D).connectedBody = this.anchors[i + 1].GetComponent<Rigidbody2D>();
            SpringJoint2D lastJoint = ropeSegments[amount - 1].AddComponent<SpringJoint2D>() as SpringJoint2D;
            lastJoint.connectedBody = this.anchors[i + 1].GetComponent<Rigidbody2D>();
            lastJoint.autoConfigureDistance = false;
            lastJoint.distance = 0f;
            lastJoint.frequency = 10f;
        }
    }

    void OnDrawGizmos() {
        List<Transform> anchorTransforms = new List<Transform>();
        // Retrieve all anchors objects.
        foreach (Transform child in this.transform) {
            anchorTransforms.Add(child);
        }
        // Loop through anchors, but the last one in order to draw lines in between.
        for (int i = 0; i < anchorTransforms.Count - 1; i++) {
            // Draw line to the next one.
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(anchorTransforms[i].transform.position, anchorTransforms[i + 1].transform.position);
        }
    }
}
