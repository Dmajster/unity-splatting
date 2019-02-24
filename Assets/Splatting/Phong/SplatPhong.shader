Shader "Splatting/Phong"
{
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1) //The color of our object
		_Shininess("Shininess", Float) = 10
		_SpecColor("Specular Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{

		Tags { "RenderType" = "Opaque" }

		Pass
		{
			Tags { "LightMode" = "ForwardBase" } //For the first light
			CGPROGRAM
			#pragma target 4.0
			#pragma vertex Vertex
			#pragma geometry Geometry
			#pragma fragment Fragment
			#include "SplatPhong.cginc"
			ENDCG
		}
	}
}