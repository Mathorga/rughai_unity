using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildStats : MonoBehaviour {
    public WildStatsBounds statsBounds;

    private float healthMod;
    private float attackMod;
    private float defenseMod;
    private float energyMod;
    private float critDamageMod;
    private float critRateMod;
    private float failDamageMod;
    private float failRateMod;
    private float speedMod;

    public float fullMod;
    public float avgMod;

    [HideInInspector]
    public float health;
    [HideInInspector]
    public float attack;
    [HideInInspector]
    public float defense;
    [HideInInspector]
    public float energy;
    [HideInInspector]
    public float critDamage;
    [HideInInspector]
    public float critRate;
    [HideInInspector]
    public float failDamage;
    [HideInInspector]
    public float failRate;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float walkSpeed;

    void Start() {
        this.GenerateMods();
        this.ComputeStats();
    }

    // Defines a new modifier for each stat.
    private void GenerateMods() {
        this.healthMod = Random.Range(0.1f, 1.0f);
        this.attackMod = Random.Range(0.1f, 1.0f);
        this.defenseMod = Random.Range(0.1f, 1.0f);
        this.energyMod = Random.Range(0.1f, 1.0f);
        this.critDamageMod = Random.Range(0.1f, 1.0f);
        this.critRateMod = Random.Range(0.1f, 1.0f);
        this.failDamageMod = Random.Range(0.1f, 1.0f);
        this.failRateMod = Random.Range(0.1f, 1.0f);
        this.speedMod = Random.Range(0.1f, 1.0f);

        // Compute the mean crit/fail mod.
        float critFailMod = (this.critDamageMod + this.critRateMod + this.failDamageMod + this.failRateMod) / 4;

        this.fullMod = this.healthMod *
                       this.attackMod *
                       this.defenseMod *
                       critFailMod *
                       this.speedMod;
        this.avgMod = (this.healthMod +
                       this.attackMod +
                       this.defenseMod +
                       this.critRateMod +
                       this.failRateMod +
                       this.speedMod) / 6;
    }

    private void ComputeStats() {
        this.health = this.statsBounds.minHealth + ((this.statsBounds.maxHealth - this.statsBounds.minHealth) * this.healthMod);
        this.attack = this.statsBounds.minAttack + ((this.statsBounds.maxAttack - this.statsBounds.minAttack) * this.attackMod);
        this.defense = this.statsBounds.minDefense + ((this.statsBounds.maxDefense - this.statsBounds.minDefense) * this.defenseMod);
        this.energy = this.statsBounds.minEnergy + ((this.statsBounds.maxEnergy - this.statsBounds.minEnergy) * this.energyMod);
        this.critDamage = this.statsBounds.minCritDamage + ((this.statsBounds.maxCritDamage - this.statsBounds.minCritDamage) * this.critDamageMod);
        this.critRate = this.statsBounds.minCritRate + ((this.statsBounds.maxCritRate - this.statsBounds.maxCritRate) * this.critRateMod);
        this.failDamage = this.statsBounds.minFailDamage + ((this.statsBounds.maxFailDamage - this.statsBounds.minFailDamage) * this.failDamageMod);
        this.failRate = this.statsBounds.minFailRate + ((this.statsBounds.maxFailRate - this.statsBounds.minFailRate) * this.failRateMod);
        this.speed = this.statsBounds.minSpeed + ((this.statsBounds.maxSpeed - this.statsBounds.minSpeed) * this.speedMod);
        this.walkSpeed = this.statsBounds.minSpeed + ((this.statsBounds.maxSpeed - this.statsBounds.minSpeed) * this.speedMod * this.speedMod);
    }
}
