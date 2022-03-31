//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DBasePlaceableUI.cs
*		Определение интерфейсов для элемента размещаемого в двухмерном пространстве.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
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
		/// Определение интерфейса для элемента адаптивного размещаемого в двухмерном пространстве
		/// </summary>
		/// <remarks>
		/// Это универсальный интерфейс для размещения адаптивного элемента (которые подстраивается под разрешение экрана)
		/// в двухмерном пространстве не зависимо от конкретной реализации отображения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusPlaceable2D : ILotusBasePlaceable2D
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Позиция левого угла элемента по X от уровня родительской области
			/// </summary>
			Single Left { get; set; }

			/// <summary>
			/// Позиция правого угла элемента по X от уровня родительской области
			/// </summary>
			Single Right { get; set; }

			/// <summary>
			/// Позиция верхнего угла элемента по Y от уровня родительской области
			/// </summary>
			Single Top { get; set; }

			/// <summary>
			/// Позиция нижнего угла элемента по Y от уровня родительской области
			/// </summary>
			Single Bottom { get; set; }

			/// <summary>
			/// Позиция верхнего-левого угла элемента от уровня родительской области
			/// </summary>
			Vector2 Location { get; set; }

			/// <summary>
			/// Ширина(размер по X) элемента
			/// </summary>
			Single Width { get; set; }

			/// <summary>
			/// Высота(размер по Y) элемента
			/// </summary>
			Single Height { get; set; }

			/// <summary>
			/// Размер элемента
			/// </summary>
			Vector2 Size { get; set; }

			/// <summary>
			/// Прямоугольника элемента от уровня родительской области
			/// </summary>
			Rect RectLocalDesign { get; }

			/// <summary>
			/// Горизонтальное выравнивание элемента
			/// </summary>
			THorizontalAlignment HorizontalAlignment { get; set; }

			/// <summary>
			/// Вертикальное выравнивание элемента
			/// </summary>
			TVerticalAlignment VerticalAlignment { get; set; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров элемента
			/// </summary>
			/// <param name="left">Позиция по X левого угла элемента от уровня родительской области</param>
			/// <param name="top">Позиция по Y верхнего угла элемента от уровня родительской области</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			//---------------------------------------------------------------------------------------------------------
			void SetFromLocalDesign(Single left, Single top, Single width, Single height);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка выравнивания элемента
			/// </summary>
			/// <param name="h_align">Горизонтальное выравнивание элемента</param>
			/// <param name="v_align">Вертикальное выравнивание элемента</param>
			//---------------------------------------------------------------------------------------------------------
			void SetAlignment(THorizontalAlignment h_align, TVerticalAlignment v_align);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вверх по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void ToFrontSibling();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вниз по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void ToBackSibling();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента первым в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void SetAsFirstSibling();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента последним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void SetAsLastSibling();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента предпоследним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void SetAsPreLastSibling();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для интерфейса <see cref="ILotusPlaceable2D"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XPlaceable2DExtension
		{
			#region ======================================= МЕТОДЫ Move ===============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignLinearIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.Linear(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignLinearIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.Linear(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignQuadInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.QuadIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignQuadInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.QuadIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignQuadOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.QuadOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignQuadOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.QuadOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignQuadInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.QuadInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignQuadInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.QuadInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignCubeInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.CubeIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignCubeInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.CubeIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignCubeOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.CubeOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignCubeOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.CubeOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignCubeInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.CubeInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignCubeInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.CubeInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBackInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BackIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBackInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BackIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBackOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BackOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBackOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BackOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBackInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BackInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBackInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BackInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignExpoInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ExpoIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignExpoInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ExpoIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignExpoOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ExpoOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignExpoOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ExpoOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignExpoInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ExpoInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignExpoInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ExpoInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignSineInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.SineIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignSineInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.SineIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignSineOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.SineOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignSineOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.SineOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignSineInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.SineInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignSineInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.SineInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignElasticInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ElasticIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignElasticInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ElasticIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignElasticOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ElasticOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignElasticOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ElasticOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignElasticInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ElasticInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignElasticInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.ElasticInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBounceInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BounceIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBounceInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BounceIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBounceOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BounceOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBounceOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BounceOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBounceInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BounceInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLocalDesignBounceInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.Location;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Location = XEasing.BounceInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.Location = target_position;

				on_completed();
			}
			#endregion

			#region ======================================= МЕТОДЫ Resize =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignLinearIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.Linear(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignLinearIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.Linear(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignQuadInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.QuadIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignQuadInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.QuadIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignQuadOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.QuadOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignQuadOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.QuadOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignQuadInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.QuadInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignQuadInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.QuadInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignCubeInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.CubeIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignCubeInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.CubeIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignCubeOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.CubeOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignCubeOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.CubeOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignCubeInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.CubeInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignCubeInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.CubeInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBackInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BackIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBackInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BackIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBackOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BackOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBackOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BackOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBackInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BackInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBackInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BackInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignExpoInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ExpoIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignExpoInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ExpoIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignExpoOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ExpoOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignExpoOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ExpoOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignExpoInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ExpoInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignExpoInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ExpoInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignSineInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.SineIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignSineInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.SineIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignSineOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.SineOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignSineOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.SineOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignSineInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.SineInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignSineInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.SineInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignElasticInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ElasticIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignElasticInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ElasticIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignElasticOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ElasticOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignElasticOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ElasticOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignElasticInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ElasticInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignElasticInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.ElasticInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBounceInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BounceIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBounceInIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BounceIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBounceOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BounceOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBounceOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BounceOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBounceInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BounceInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера интерфейса адаптивного размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Интерфейс адаптивного размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToLocalDesignBounceInOutIteration(this ILotusPlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.Size;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.Size = XEasing.BounceInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.Size = target_size;

				on_completed();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================