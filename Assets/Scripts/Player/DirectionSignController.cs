using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSignController : MonoBehaviour {
    public float extent;
    public float elevation;

    private PlayerController parentController;

    // Start is called before the first frame update
    void Start() {
        this.parentController = this.transform.parent.gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update(){
        // Calculate the application point of the attack.
        float dir = this.parentController.faceDir;
        float len = this.extent;

        Vector3 pos2D = this.transform.parent.position + (Vector3) Utils.PolarToCartesian(dir, len);
        pos2D[1] += this.elevation;
        this.transform.position = pos2D;
    }
}
