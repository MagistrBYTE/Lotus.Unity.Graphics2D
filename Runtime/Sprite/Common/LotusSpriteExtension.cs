//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteExtension.cs
*		Методы расширения для модуля спрайтов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DSpriteCommon Общая подсистема
		//! Общая подсистема работы со спрайтами реализуется базовый функционал модуля работы со спрайтами
		//! \ingroup Unity2DSprite
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений функциональности компонента Transform 
		/// применительно к модулю спрайтов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XSpriteTransformExtension
		{
			#region ======================================= World Screen coordinate ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение экранных координат
			/// </summary>
			/// <param name="transform">Компонент трансформации</param>
			/// <returns>Экранные координаты</returns>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Vector2 GetSpriteWorldScreen(this Transform transform)
			{
				Single w = LotusSpriteDispatcher.ScreenWidth;
				Single h = LotusSpriteDispatcher.ScreenHeight;

				Single pos_x = transform.position.x;
				Single pos_y = -transform.position.y;

				Single left = (((pos_x / LotusSpriteDispatcher.CameraOrthoWidth) + 1) / 2) * w;
				Single top = (((pos_y / LotusSpriteDispatcher.CameraOrthoHeight) + 1) / 2) * h;

				return (new Vector2(left, top));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции по X в экранных координатах
			/// </summary>
			/// <param name="transform">Компонент трансформации</param>
			/// <returns>Позиция по X в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Single GetSpriteWorldScreenX(this Transform transform)
			{
				Single w = LotusSpriteDispatcher.ScreenWidth;
				Single left = (((transform.position.x / LotusSpriteDispatcher.CameraOrthoWidth) + 1) / 2) * w;
				return (left);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции по Y в экранных координатах
			/// </summary>
			/// <param name="transform">Компонент трансформации</param>
			/// <returns>Позиция по Y в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Single GetSpriteWorldScreenY(this Transform transform)
			{
				Single h = LotusSpriteDispatcher.ScreenHeight;
				Single pos_y = -transform.position.y;
				Single top = (((pos_y / LotusSpriteDispatcher.CameraOrthoHeight) + 1) / 2) * h;
				return (top);
			}
			#endregion

			#region ======================================= World Transform coordinate ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка мировых координат трансформации посредством указанных экранных координат
			/// </summary>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void SetSpriteWorldTransformFromWorldScreen(this Transform transform, Single left, Single top)
			{
				Single w = LotusSpriteDispatcher.ScreenWidth;
				Single h = LotusSpriteDispatcher.ScreenHeight;
				Single pos_x = LotusSpriteDispatcher.CameraOrthoWidth * ((((left / w) * 2) - 1));
				Single pos_y = LotusSpriteDispatcher.CameraOrthoHeight * ((((top / h) * 2) - 1));

				transform.position = new Vector3(pos_x, -pos_y, transform.position.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка мировых координат трансформации посредством указанных экранных координат
			/// </summary>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void SetSpriteWorldTransformFromWorldScreenX(this Transform transform, Single left)
			{
				Single w = LotusSpriteDispatcher.ScreenWidth;
				Single pos_x = LotusSpriteDispatcher.CameraOrthoWidth * ((((left / w) * 2) - 1));
				transform.position = new Vector3(pos_x, transform.position.y, transform.position.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка мировых координат трансформации посредством указанных экранных координат
			/// </summary>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="top">Позиция по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void SetSpriteWorldTransformFromWorldScreenY(this Transform transform, Single top)
			{
				Single h = LotusSpriteDispatcher.ScreenHeight;
				Single pos_y = LotusSpriteDispatcher.CameraOrthoHeight * ((((top / h) * 2) - 1));
				transform.position = new Vector3(transform.position.x, -pos_y, transform.position.z);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================