using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;

    private float mutationRate = 0.8f;

    [Range(0.0f, 100.0f)]
    public float distributionRate = 2.0f;

    void Start() {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (renderer != null) {
            // Pass the palette to the swapper shader.
            renderer.material.SetTexture("_Palette", this.palette);

            // Get a random mutation rate and send it to the shader.
            // The mutation rate is adjusted by elevating it to the power of distributionRate.
            this.mutationRate = Mathf.Pow(Random.value, this.distributionRate);
            renderer.material.SetFloat("_MutationRate", this.mutationRate);
        }
    }

    private void FixedUpdate() {
        GetComponent<SpriteRenderer>().material.SetFloat("_MutationRate", this.mutationRate);
    }
}
