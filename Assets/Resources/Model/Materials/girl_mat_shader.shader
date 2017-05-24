// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/girl_mat_shader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_RampTex("Ramp Tex", 2D) = "white"{}
		_Specular("Specular", Color) = (1, 1, 1, 1)
		_Gloss("Gloss", Range(8.0, 256)) = 20
	}
		SubShader{

			//光照模型pass，渲染模型正面
	Pass{
		Tags{"LightMode" = "ForwardBase"}
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		//#pragma multi_compile_fwdbase
		#include "Lighting.cginc"
			//顶点着色器的输入输出结构体
			struct a2v {
			float4 vertex:POSITION;
			float3 normal:NORMAL;
			float4 texcoord:TEXCOORD0;
		};

		struct v2f {
			float4 pos:SV_POSITION;
			float3 worldNormal:TEXCOORG0;
			float3 worldPos:TEXCOORD1;
			float2 uv:TEXCOORD2;
		};

		fixed4 _Color;
		sampler2D _RampTex;
		float4 _RampTex_ST;
		fixed4 _Specular;
		float _Gloss;

		//顶点着色器
		v2f vert(a2v v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.worldNormal = UnityObjectToWorldNormal(v.normal);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.uv = TRANSFORM_TEX(v.texcoord, _RampTex);
			return o;
		}

		//片元着色器
		fixed4 frag(v2f i) :SV_Target{
			fixed3 worldNormal = normalize(i.worldNormal);
			fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
			fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

			//use the texture to sample the diffuse color
			fixed halfLambert = 0.5*dot(worldNormal, worldLightDir) + 0.5;
			fixed3 diffuseColor = tex2D(_RampTex, fixed2(halfLambert, halfLambert)).rgb*_Color.rgb;
			fixed3 diffuse = _LightColor0.rgb*diffuseColor;

			fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
			fixed3 halfDir = normalize(worldLightDir);
			fixed3 specular = _LightColor0.rgb*_Specular.rgb*pow(max(0, dot(worldNormal, halfDir)), _Gloss);
			return fixed4(ambient + diffuse + specular, 1.0);

		}
			ENDCG
		}
		
	}
	FallBack "Diffuse"
}
