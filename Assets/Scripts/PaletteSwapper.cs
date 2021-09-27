using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;

    void Start() {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (renderer != null) {
            // Pass the palette to the swapper shader.
            renderer.material.SetTexture("_Palette", this.palette);
        }
    }
}
