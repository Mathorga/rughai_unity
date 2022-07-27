using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;

    private WildStats stats;

    void Start() {
        this.stats = this.GetComponent<WildStats>();
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (renderer != null) {
            // Pass the palette to the swapper shader.
            renderer.material.SetTexture("_Palette", this.palette);

            // Get a mutation rate based on stats and send it to the shader.
            renderer.material.SetFloat("_MutationRate", this.stats.fullMod);
        }
    }
}
