using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;
    public AnimationCurve mutationDistribution;

    [Range(0.0f, 1.0f)]
    public float mutationRate = 0.8f;

    void Start() {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (renderer != null) {
            // Pass the palette to the swapper shader.
            renderer.material.SetTexture("_Palette", this.palette);
            this.mutationRate = Mathf.Pow(Random.value, 6.0f);
            this.mutationRate = this.mutationDistribution.Evaluate(Random.value);

            renderer.material.SetFloat("_MutationRate", this.mutationRate);
        }
    }

    private void FixedUpdate() {
        GetComponent<SpriteRenderer>().material.SetFloat("_MutationRate", this.mutationRate);
    }
}
