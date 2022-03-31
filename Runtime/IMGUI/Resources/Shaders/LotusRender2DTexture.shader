//---------------------------------------------------------------------------------------------------------------------
// Проект: LotusPlatform
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
// Описание: Системный шейдер для рисования примитивов цветом текстуры.
// Версия: 1.0.0.0
//---------------------------------------------------------------------------------------------------------------------
Shader "Lotus/Render2D/Texture"
{
	Properties
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Lighting Off
		Cull Off
		ZTest Always
		ZWrite Off
		Fog { Mode Off }

		Pass
		{
			Color [_Color]
			SetTexture [_MainTex] {combine texture * primary }
		}
	}

	Fallback "Sprites/Default"
}
//---------------------------------------------------------------------------------------------------------------------