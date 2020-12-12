using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Stats : ScriptableObject {
    // Health stat.
    public float health;

    // Speed stat.
    public float speed;

    // Walk speed stat (depends on speed).
    public float walkSpeed;
}
