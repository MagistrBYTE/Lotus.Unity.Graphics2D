//---------------------------------------------------------------------------------------------------------------------
// Проект: LotusPlatform
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
// Описание: Индивидуальный шейдер для прокрутки текстурных координат по времени.
// Версия: 1.0.0.0
//---------------------------------------------------------------------------------------------------------------------
Shader "Lotus/UI/ScrollTexture" 
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite", 2D) = "white" {}
		[PerRendererData] _Color("Sprite Tint", Color) = (1,1,1,1)

		_ScrollColor("Scroll Tint", Color) = (1,1,1,1)
		_ScrollTex("Scroll Texture", 2D) = "black" {}
		_ScrollOpacity("Scroll Opacity", Range(0, 1)) = 1
		_ScrollX("Scroll X", Range(-1, 1)) = 0
		_ScrollY("Scroll Y", Range(-1, 1)) = 0
	}

	SubShader
	{
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite Off


			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex; 
			uniform float4 _MainTex_ST;
			uniform sampler2D _ScrollTex; 
			uniform float4 _ScrollTex_ST;
			uniform half _ScrollOpacity;
			uniform half4 _ScrollColor;
			uniform half _ScrollX;
			uniform half _ScrollY;
			uniform half4 _Color;

			struct VertexInput 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord0 : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			struct VertexOutput 
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				float4 vertexColor : COLOR;
			};

			half2 Scroll(float2 uv, half x, half y) 
			{
				return half2(uv.r + _Time.y * x, uv.g + _Time.y * y);
			}

			float4 AdditiveMix(float4 in_b, float4 in_t) 
			{
				float4 result = float4(in_b.rgb * (1.0 - in_t.a) + in_t.rgb * in_t.a, 0.0);
				result.a = in_t.a + in_b.a * (1.0 - in_t.a);
				return result;
			}

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;

				o.vertexColor = v.vertexColor * _Color;
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(VertexOutput i) : COLOR
			{
				half _time = _Time.w;
				half2 scrollUV = Scroll(i.uv0, _ScrollX, _ScrollY);
				half4 mainTexture = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
				half4 canvas = mainTexture * i.vertexColor;

				half4 scrollTex = tex2D(_ScrollTex, TRANSFORM_TEX(scrollUV, _ScrollTex));
				scrollTex = AdditiveMix(canvas, scrollTex);

				canvas = lerp(canvas, scrollTex, _ScrollOpacity);

				half3 finalColor = canvas;
				return fixed4(finalColor, (mainTexture.a * i.vertexColor.a));
			}
		ENDCG
		}
	}
	FallBack "Sprites/Diffuse"
}
//---------------------------------------------------------------------------------------------------------------------