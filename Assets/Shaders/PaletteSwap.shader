// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PaletteSwap" {
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        // _Palette ("Palette", 2D) = "white" {}
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader {
        Tags { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _AlphaSplitEnabled;
            uniform sampler2D _Palette;
            float4 _Palette_TexelSize;

            v2f vert(appdata_t IN) {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 SampleSpriteTexture (float2 uv) {
                fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled) {
                    color.a = tex2D (_AlphaTex, uv).r;
                }
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target {
                fixed4 baseColor = SampleSpriteTexture (IN.texcoord) * IN.color;

                int paletteIndex = -1;

                // Loop through the first column of the palette texture.
                [loop]
                for (int i = 0; i < _Palette_TexelSize.w; i++) {
                    // Compare the current palette pixel color to the color being rendered.
                    if (all(baseColor == tex2D(_Palette, float2(0, i / _Palette_TexelSize.w)))) {
                        paletteIndex = i;
                        break;
                    }
                }

                if (paletteIndex != -1) {
                    baseColor.rgb = tex2D(_Palette, float2(0.5, paletteIndex / _Palette_TexelSize.w));
                }

                baseColor.rgb *= baseColor.a;
                return baseColor;
            }
        ENDCG
        }
    }
}