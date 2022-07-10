using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;

    [Range(0.0f, 100.0f)]
    public float distributionRate = 2.0f;

    private WildStats stats;

    void Start() {
        this.stats = this.GetComponent<WildStats>();
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (renderer != null) {
            // Pass the palette to the swapper shader.
            renderer.material.SetTexture("_Palette", this.palette);

            // Get a random mutation rate and send it to the shader.
            // The mutation rate is adjusted by elevating it to the power of distributionRate.
            // float mutationRate = Mathf.Pow(Random.value, this.distributionRate);
            float mutationRate = this.stats.fullMod;

            renderer.material.SetFloat("_MutationRate", mutationRate);
        }
    }
}
