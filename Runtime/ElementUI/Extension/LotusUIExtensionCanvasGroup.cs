//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIExtensionCanvasGroup.cs
*		Методы расширения компонента CanvasGroup.
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
		/// Статический класс реализующий методы расширений компонента <see cref="CanvasGroup"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionCanvasGroup
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма затенение канвы
			/// </summary>
			/// <param name="this">Канва</param>
			/// <param name="duration">Время затенения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator FadeIteration(this CanvasGroup @this, Single duration)
			{
				Single time = 0;
				Single start_time = 0;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					@this.alpha = 1.0f - time;
					yield return null;
				}

				@this.alpha = 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма затенение канвы
			/// </summary>
			/// <param name="this">Канва</param>
			/// <param name="duration">Время затенения</param>
			/// <param name="on_completed">Обработчик события окончания затенение канвы</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator FadeIteration(this CanvasGroup @this, Single duration, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					@this.alpha = 1.0f - time;
					yield return null;
				}

				@this.alpha = 0;
				if (on_completed != null) on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма показа канвы
			/// </summary>
			/// <param name="this">Канва</param>
			/// <param name="duration">Время затенения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator UnFadeIteration(this CanvasGroup @this, Single duration)
			{
				Single time = 0;
				Single start_time = 0;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					@this.alpha = time;
					yield return null;
				}

				@this.alpha = 1;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма показа канвы
			/// </summary>
			/// <param name="this">Канва</param>
			/// <param name="duration">Время затенения</param>
			/// <param name="on_completed">Обработчик события окончания показа канвы</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator UnFadeIteration(this CanvasGroup @this, Single duration, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					@this.alpha = time;
					yield return null;
				}

				@this.alpha = 1;
				if (on_completed != null) on_completed();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================