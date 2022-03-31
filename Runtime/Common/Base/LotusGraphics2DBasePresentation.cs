//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DBasePresentation.cs
*		Определение структур данных и интерфейсов для визуальных аспектов представления элемента.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using TMPro;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DCommonBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для переднего цвета элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusPresentationForecolor
		{
			/// <summary>
			/// Передний цвет элемента
			/// </summary>
			Color ForegroundColor { get; set; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для фонового цвета элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusPresentationBackcolor
		{
			/// <summary>
			/// Фоновый цвет элемента
			/// </summary>
			Color BackgroundColor { get; set; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для интерфейса <see cref="ILotusPresentationForecolor"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XPresentationForecolorExtension
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения цвета переднего цвета элемента
			/// </summary>
			/// <param name="this">Интерфейс для переднего цвета элемента</param>
			/// <param name="duration">Время изменения</param>
			/// <param name="target_color">Целевой цвет</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ForecolorColorLinearIteration(this ILotusPresentationForecolor @this, Single duration, 
				Color target_color)
			{
				Single time = 0;
				Single start_time = 0;
				Color start_color = @this.ForegroundColor;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.ForegroundColor = Color.Lerp(start_color, target_color, time);
					yield return null;
				}

				@this.ForegroundColor = target_color;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения цвета переднего цвета элемента
			/// </summary>
			/// <param name="this">Интерфейс для переднего цвета элемента</param>
			/// <param name="duration">Время изменения</param>
			/// <param name="target_color">Целевой цвет</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ForecolorColorLinearIteration(this TextMeshProUGUI @this, Single duration,
				Color target_color)
			{
				Single time = 0;
				Single start_time = 0;
				Color start_color = @this.color;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.color = Color.Lerp(start_color, target_color, time);
					yield return null;
				}

				@this.color = target_color;
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для интерфейса <see cref="ILotusPresentationBackcolor"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XPresentationBackcolorExtension
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения цвета фонового цвета элемента
			/// </summary>
			/// <param name="this">Интерфейс для фонового цвета элемента</param>
			/// <param name="duration">Время изменения</param>
			/// <param name="target_color">Целевой цвет</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator BackcolorColorLinearIteration(this ILotusPresentationBackcolor @this, Single duration,
				Color target_color)
			{
				Single time = 0;
				Single start_time = 0;
				Color start_color = @this.BackgroundColor;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.BackgroundColor = Color.Lerp(start_color, target_color, time);
					yield return null;
				}

				@this.BackgroundColor = target_color;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================