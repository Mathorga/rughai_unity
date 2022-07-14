using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;

    // TODO Remove.
    public float mutationRate;

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
            this.mutationRate = this.stats.fullMod;
            if (mutationRate > 0.8f) Debug.Log("MUTATION_RATE " + mutationRate.ToString());

            renderer.material.SetFloat("_MutationRate", mutationRate);
        }
    }
}
