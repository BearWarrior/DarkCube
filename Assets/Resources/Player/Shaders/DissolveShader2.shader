﻿//TextureChangingSmooth
Shader "Custom/DissolveShader" {
	Properties{
		_MainTex("Texture1 (RGB)", 2D) = "white" {}
	_Dissolver("Slice Guide (RGB)", 2D) = "white" {}
	_SliceAmount("Slice Amount", Range(0.0, 1.0)) = 0.5
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull Off

		CGPROGRAM
		//if you're not planning on using shadows, remove "addshadow" for better performance
#pragma surface surf Lambert 
	struct Input {
		float2 uv_MainTex;
		float2 uv_Dissolver;
		float _SliceAmount;
	};
	sampler2D _MainTex;
	sampler2D _Dissolver;
	float _SliceAmount;
	void surf(Input IN, inout SurfaceOutput o)
	{
		clip(tex2D(_Dissolver, IN.uv_Dissolver).rgb - _SliceAmount);
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
	}
	ENDCG
	}
		Fallback "Diffuse"
}

