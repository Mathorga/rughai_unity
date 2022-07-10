using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildSpawner : MonoBehaviour {
    public GameObject spawn(GameObject wildType, float xPos, float yPos) {
        GameObject wild = Instantiate(wildType,
                                      new Vector2(xPos, yPos),
                                      Quaternion.identity);

        return wild;
    }
}
