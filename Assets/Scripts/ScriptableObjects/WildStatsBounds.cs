using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WildStatsBounds : ScriptableObject {
    // Health stat.
    public float minHealth;
    public float maxHealth;

    // Attack stat.
    public float minAttack;
    public float maxAttack;

    // Defense stat.
    public float minDefense;
    public float maxDefense;

    // Energy stat.
    public float minEnergy;
    public float maxEnergy;

    // Critical damage stat.
    public float minCritDamage;
    public float maxCritDamage;

    // Critical rate stat.
    public float minCritRate;
    public float maxCritRate;

    // Fail damage stat.
    public float minFailDamage;
    public float maxFailDamage;

    // Fail rate stat.
    public float minFailRate;
    public float maxFailRate;

    // Speed stat.
    public float minSpeed;
    public float maxSpeed;
}
