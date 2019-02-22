// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


#include "UnityCG.cginc"
#include "UnityGBuffer.cginc"
#include "UnityStandardUtils.cginc"


struct appdata 
{
	float4 vertex : POSITION;
	fixed4 color: COLOR;
};

struct v2g
{
	float4 vertex : SV_POSITION;
	fixed4 color : COLOR;
	float4 up : TEXCOORD0;
	float4 right : TEXCOORD1;
};

struct g2f
{
	float4 vertex : SV_POSITION;
	fixed4 color : COLOR;
};

v2g Vertex(appdata input)
{
	v2g output;
	output.vertex = input.vertex;
	output.color = input.color;
	
	float3 view = normalize(UNITY_MATRIX_IT_MV[2].xyz);
	float3 upvec = normalize(UNITY_MATRIX_IT_MV[1].xyz);
	float3 R = normalize(cross(view, upvec));
	output.up = float4(upvec*1, 0);
	output.right = float4(R * 1, 0);
	return output;
}

[maxvertexcount(4)]
void Geometry(point v2g input[1], inout TriangleStream<g2f>outputStream) {
	g2f output;

	output.vertex = UnityObjectToClipPos(input[0].vertex + (-input[0].up + input[0].right));
	output.color = input[0].color;
	outputStream.Append(output);

	output.vertex = UnityObjectToClipPos(input[0].vertex + (-input[0].up - input[0].right));
	outputStream.Append(output);

	output.vertex = UnityObjectToClipPos(input[0].vertex + (+input[0].up + input[0].right));
	outputStream.Append(output);

	output.vertex = UnityObjectToClipPos(input[0].vertex + (+input[0].up - input[0].right));
	outputStream.Append(output);

	outputStream.RestartStrip();
}

fixed4 Fragment(g2f input) : SV_Target
{
	return fixed4(1,0,0,1);
}