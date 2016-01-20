//Originally by Jaap Kreijkamp

Shader "Unlit/Unlit" {
    
Properties {_MainTex ("Texture", 2D) = ""}

SubShader {Pass {SetTexture[_MainTex]} }

}