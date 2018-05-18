Shader "Skybox/BlendableSkybox" {
	Properties{
		_Tint("Tint Color", Color) = (.5, .5, .5, .5)
		[Gamma] _Exposure("Exposure", Range(0, 8)) = 1.0
		_Rotation("Rotation", Range(0, 360)) = 0
		_Blend("Blend", Range(0.0,1.0)) = 0.5
		[NoScaleOffset] _FrontTex("Front (+Z)", 2D) = "white" {}
		[NoScaleOffset] _BackTex("Back (-Z)", 2D) = "white" {}
		[NoScaleOffset] _LeftTex("Left (+X)", 2D) = "white" {}
		[NoScaleOffset] _RightTex("Right (-X)", 2D) = "white" {}
		[NoScaleOffset] _UpTex("Up (+Y)", 2D) = "white" {}
		[NoScaleOffset] _DownTex("Down (-Y)", 2D) = "white" {}
		[NoScaleOffset] _FrontTex2("2 Front (+Z)", 2D) = "white" {}
		[NoScaleOffset] _BackTex2("2 Back (-Z)", 2D) = "white" {}
		[NoScaleOffset] _LeftTex2("2 Left (+X)", 2D) = "white" {}
		[NoScaleOffset] _RightTex2("2 Right (-X)", 2D) = "white" {}
		[NoScaleOffset] _UpTex2("2 Up (+Y)", 2D) = "white" {}
		[NoScaleOffset] _DownTex2("2 Down (-Y)", 2D) = "white" {}
		}

		SubShader{
		Tags{ "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
		Cull Off ZWrite Off

		CGINCLUDE
#include "UnityCG.cginc"

		half4 _Tint;
	half _Exposure;
	float _Rotation;
	half _Blend;

	float4 RotateAroundYInDegrees(float4 vertex, float degrees)
	{
		float alpha = degrees * UNITY_PI / 180.0;
		float sina, cosa;
		sincos(alpha, sina, cosa);
		float2x2 m = float2x2(cosa, -sina, sina, cosa);
		return float4(mul(m, vertex.xz), vertex.yw).xzyw;
	}

	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};
	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
	};
	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, RotateAroundYInDegrees(v.vertex, _Rotation));
		o.texcoord = v.texcoord;
		return o;
	}
	half4 skybox_frag(v2f i, sampler2D smp1, sampler2D smp2)
	{
		half4 tex1 = tex2D(smp1, i.texcoord);
		half4 tex2 = tex2D(smp2, i.texcoord);

		half3 c1 = tex1.xyz * _Tint.rgb * unity_ColorSpaceDouble.rgb;
		half3 c2 = tex2.xyz * _Tint.rgb * unity_ColorSpaceDouble.rgb;
		half3 c = (1.0 - _Blend)*c1 + _Blend*c2;
		c *= _Exposure;

		return half4(c, 1);
	}

	ENDCG

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		sampler2D _FrontTex;
	sampler2D _FrontTex2;
	half4 frag(v2f i) : SV_Target{ return skybox_frag(i,_FrontTex, _FrontTex2); }
		ENDCG
	}
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		sampler2D _BackTex;
	sampler2D _BackTex2;
	half4 frag(v2f i) : SV_Target{ return skybox_frag(i,_BackTex, _BackTex2); }
		ENDCG
	}
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		sampler2D _LeftTex;
	sampler2D _LeftTex2;
	half4 frag(v2f i) : SV_Target{ return skybox_frag(i,_LeftTex, _LeftTex2); }
		ENDCG
	}
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		sampler2D _RightTex;
	sampler2D _RightTex2;
	half4 frag(v2f i) : SV_Target{ return skybox_frag(i,_RightTex, _RightTex2); }
		ENDCG
	}
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		sampler2D _UpTex;
	sampler2D _UpTex2;
	half4 frag(v2f i) : SV_Target{ return skybox_frag(i,_UpTex, _UpTex2); }
		ENDCG
	}
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		sampler2D _DownTex;
	sampler2D _DownTex2;
	half4 frag(v2f i) : SV_Target{ return skybox_frag(i,_DownTex, _DownTex2); }
		ENDCG
	}
	}
}
