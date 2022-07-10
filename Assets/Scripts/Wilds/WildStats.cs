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
    private float walkSpeedMod;

    public float fullMod {
        get {
            return this.healthMod *
                   this.attackMod *
                   this.defenseMod *
                   this.energyMod *
                   this.critDamageMod *
                   this.critRateMod *
                   this.failDamageMod *
                   this.failRateMod *
                   this.speedMod *
                   this.walkSpeedMod;
        }
    }

    public float health {
        get {
            return this.statsBounds.minHealth + ((this.statsBounds.maxHealth - this.statsBounds.minHealth) * this.healthMod);
        }
    }

    public float attack {
        get {
            return this.statsBounds.minAttack + ((this.statsBounds.maxAttack - this.statsBounds.minAttack) * this.attackMod);
        }
    }

    public float defense {
        get {
            return this.statsBounds.minDefense + ((this.statsBounds.maxDefense - this.statsBounds.minDefense) * this.defenseMod);
        }
    }

    public float energy {
        get {
            return this.statsBounds.minEnergy + ((this.statsBounds.maxEnergy - this.statsBounds.minEnergy) * this.energyMod);
        }
    }

    public float critDamage {
        get {
            return this.statsBounds.minCritDamage + ((this.statsBounds.maxCritDamage - this.statsBounds.minCritDamage) * this.critDamageMod);
        }
    }

    public float critRate {
        get {
            return this.statsBounds.minCritRate + ((this.statsBounds.maxCritRate - this.statsBounds.maxCritRate) * this.critRateMod);
        }
    }

    public float failDamage {
        get {
            return this.statsBounds.minFailDamage + ((this.statsBounds.maxFailDamage - this.statsBounds.minFailDamage) * this.failDamageMod);
        }
    }

    public float failRate {
        get {
            return this.statsBounds.minFailRate + ((this.statsBounds.maxFailRate - this.statsBounds.minFailRate) * this.failRateMod);
        }
    }

    public float speed {
        get {
            return this.statsBounds.minSpeed + ((this.statsBounds.maxSpeed - this.statsBounds.minSpeed) * this.speedMod);
        }
    }

    public float walkSpeed {
        get {
            return this.statsBounds.minWalkSpeed + ((this.statsBounds.maxWalkSpeed - this.statsBounds.minWalkSpeed) * this.walkSpeedMod);
        }
    }

    void Start() {
        this.RandomizeMods();
    }

    // Defines a new modifier for each stat.
    public void RandomizeMods() {
        this.healthMod = Random.value;
        this.attackMod = Random.value;
        this.defenseMod = Random.value;
        this.energyMod = Random.value;
        this.critDamageMod = Random.value;
        this.critRateMod = Random.value;
        this.failDamageMod = Random.value;
        this.failRateMod = Random.value;
        this.speedMod = Random.value;
        this.walkSpeedMod = Random.value;
    }
}
