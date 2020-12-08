using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Stats : ScriptableObject {
    // Speed stat.
    public float speed;

    // Walk speed stat (depends on speed).
    public float walkSpeed;
}
