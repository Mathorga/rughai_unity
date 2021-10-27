using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;
    public float rareProbability;
    public float darkProbability;

    void Start() {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (renderer != null) {
            // Pass the palette to the swapper shader.
            renderer.material.SetTexture("_Palette", this.palette);

            float random = Random.Range(0.0f, 1.0f);

            if (random < this.darkProbability) {
                renderer.material.SetInt("_PaletteIndex", 2);
            } else if (random < this.rareProbability) {
                renderer.material.SetInt("_PaletteIndex", 1);
            } else {
                renderer.material.SetInt("_PaletteIndex", 0);
            }
        }
    }
}
