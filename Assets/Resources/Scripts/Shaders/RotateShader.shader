Shader "RotatingTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
		Tags { "RenderType"="Opaque" }
		LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
					#include "UnityCG.cginc"
			
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
		
            };
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

            float4x4 _TextureRotation;
         
		       	float4 _MainTex_ST;
            sampler2D _MainTex;
            v2f vert (float4 pos : POSITION, float2 uv : TEXCOORD0, appdata v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, pos);
                
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) * mul(_TextureRotation, float4(uv,0,1)).xy;
                //o.uv = mul(_TextureRotation, float4(uv,0,1)).xy;
          
                return o;
            }
     
            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}