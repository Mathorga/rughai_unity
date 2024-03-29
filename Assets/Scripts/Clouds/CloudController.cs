﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {
    public float speed {
        get;
        set;
    }

    void FixedUpdate() {
        this.transform.position += new Vector3(this.speed, 0.0f, 0.0f);
        this.transform.position += new Vector3(0.0f, Mathf.Sin(this.transform.position.x * 0.5f) * 0.1f, 0.0f);
    }
}
