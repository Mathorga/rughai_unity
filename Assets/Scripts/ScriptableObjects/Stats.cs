using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Stats : ScriptableObject {
    // Health stat.
    public float health;

    public float attack;

    public float defense;

    public float energy;

    public float critDamage;

    public float critRate;

    public float failDamage;

    public float failRate;

    // Speed stat.
    public float speed;

    // Walk speed stat.
    public float walkSpeed;
}
