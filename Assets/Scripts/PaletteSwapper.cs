using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;
    public float probability;

    void Start() {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (renderer != null && Random.Range(0.0f, 1.0f) < this.probability) {
            // Pass the palette to the swapper shader.
            renderer.material.SetTexture("_Palette", this.palette);
        }
    }
}
