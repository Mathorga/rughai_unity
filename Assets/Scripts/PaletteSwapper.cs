using UnityEngine;

public class PaletteSwapper : MonoBehaviour {
    public Texture2D palette;

    void Start() {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (renderer != null) {
            Texture2D spriteTexture = renderer.sprite.texture;

            // Loop through sprite texture.
            for (int i = 0; i < spriteTexture.width; i++) {
                for (int j = 0; j < spriteTexture.height; j++) {
                    Color pixelColor = spriteTexture.GetPixel(i, j);

                    int colorIndex = -1;
                    // Loop through source palette.
                    for (int k = 0; k < palette.height; k++) {
                        if (palette.GetPixel(0, k) == pixelColor) {
                            colorIndex = k;
                            break;
                        }
                    }

                    spriteTexture.SetPixel(i, j, palette.GetPixel(1, colorIndex));
                }
            }
        }
    }
}
