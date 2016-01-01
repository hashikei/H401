﻿Shader "Custom/mapshader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_HighLightCol("HighLightCol",Color) = (1,1,1,1)
		_HightLidghtPos("HighLightPos",vector) = (0.0,0.0,0.0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _HLCol;
		float3 _HLPos;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;


		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			// HightLightPosからの座標を計算し、Rangeとの余りとの比率でテクスチャ色とカラーを合成


			o.Albedo = c.rgb;
			
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
