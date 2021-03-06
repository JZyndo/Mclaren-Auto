﻿Shader "RenderFX/SkySphereBlended" {
	Properties{
		_Tint("Tint Color", Color) = (.5, .5, .5, .5)
		[Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
		_Rotation ("Rotation", Range(0, 360)) = 0
		_Blend("Blend", Range(0.0,1.0)) = 0.5
		[NoScaleOffset] _SkyTex("Old Sky", CUBE) = "white" {}
		[NoScaleOffset] _SkyTex2("New Sky", CUBE) = "grey" {}
	}

		SubShader{
			Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
			Cull Off ZWrite Off

			Pass {
		
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				samplerCUBE _SkyTex;
				samplerCUBE _SkyTex2;
				half4 _Tex_HDR;
				half4 _Tex_HDR_1;
				half4 _Tint;
				half _Exposure;
				half _Blend;
				float _Rotation;

				float4 RotateAroundYInDegrees (float4 vertex, float degrees)
				{
					float alpha = degrees * UNITY_PI / 180.0;
					float sina, cosa;
					sincos(alpha, sina, cosa);
					float2x2 m = float2x2(cosa, -sina, sina, cosa);
					return float4(mul(m, vertex.xz), vertex.yw).xzyw;
				}
				
				struct appdata_t {
					float4 vertex : POSITION;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float3 texcoord : TEXCOORD0;
				};

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, RotateAroundYInDegrees(v.vertex, _Rotation));
					o.texcoord = v.vertex.xyz;
					return o;
				}

				fixed4 frag (v2f i) : SV_Target
				{
					half4 tex = texCUBE (_SkyTex, i.texcoord);
					half4 tex2 = texCUBE (_SkyTex2, i.texcoord);

					half3 c1 = tex.xyz * _Tint.rgb * unity_ColorSpaceDouble.rgb;
					half3 c2 = tex2.xyz * _Tint.rgb * unity_ColorSpaceDouble.rgb;
					//c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
					half3 c = (1.0 - _Blend)*c1 + _Blend*c2;
					c *= _Exposure;
					
					return half4(c, 1);
				}
				ENDCG 
			}

	}

		Fallback "RenderFX/Skybox", 1
}
