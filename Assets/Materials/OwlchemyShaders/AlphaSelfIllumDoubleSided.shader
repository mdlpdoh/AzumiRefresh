Shader "Unlit/AlphaSelfIllumDoubleSided" {
    Properties {
        _Color ("Color Tint", Color) = (1,1,1,1)
        _MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white"
    }
    Category {
       Lighting On
       ZWrite Off
       Cull Off
       Blend SrcAlpha OneMinusSrcAlpha
       Tags { "Queue" = "transparent" }
       SubShader {
            Material {
               Emission [_Color]
            }
            Pass {
               SetTexture [_MainTex] {
                      Combine Texture * Primary, Texture * Primary
                }
            }
        }
    }
}