//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIExtensionGraphics.cs
*		Методы расширения компонента Graphics.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DUIExtension
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений компонента <see cref="Graphic"/> 
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionGraphic
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции цвета
			/// </summary>
			/// <param name="this">Графический элемент</param>
			/// <param name="duration">Время затенения</param>
			/// <param name="target_color">Целевой цвет</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator LerpColorIteration(this Graphic @this, Single duration, Color target_color)
			{
				Single time = 0;
				Single start_time = 0;
				Color current_color = @this.color;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					Color color = Color.Lerp(current_color, target_color, time);
					@this.color = color;
					yield return null;
				}

				@this.color = target_color;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции цвета
			/// </summary>
			/// <param name="this">Графический элемент</param>
			/// <param name="duration">Время затенения</param>
			/// <param name="target_color">Целевой цвет</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator LerpColorIteration(this Graphic @this, Single duration, Color target_color, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Color current_color = @this.color;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					Color color = Color.Lerp(current_color, target_color, time);
					@this.color = color;
					yield return null;
				}

				@this.color = target_color;
				if (on_completed != null) on_completed();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================