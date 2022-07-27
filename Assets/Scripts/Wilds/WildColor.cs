using UnityEngine;

public class WildColor : MonoBehaviour {
    public Texture2D palette;

    private WildStats stats;
    private IWild controller;
    private HitController hitController;
    private SpriteRenderer spriteRenderer;

    void Start() {
        this.stats = this.GetComponent<WildStats>();
        this.controller = this.GetComponent<IWild>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.hitController = this.GetComponent<HitController>();

        if (this.spriteRenderer != null) {
            // Pass the palette to the swapper shader.
            this.spriteRenderer.material.SetTexture("_Palette", this.palette);

            // Get a mutation rate based on stats and send it to the shader.
            this.spriteRenderer.material.SetFloat("_MutationRate", this.stats.fullMod);

            this.spriteRenderer.material.SetFloat("_Alive", 1.0f);
            this.spriteRenderer.material.SetFloat("_Hit", -1.0f);
        }
    }

    void Update() {
        if (this.hitController.hit) {
            this.spriteRenderer.material.SetFloat("_Hit", 1.0f);
        } else {
            this.spriteRenderer.material.SetFloat("_Hit", -1.0f);
        }

        if (this.controller.mode == IWild.Mode.Dead) {
            this.spriteRenderer.material.SetFloat("_Alive", -1.0f);
        }
    }
}
