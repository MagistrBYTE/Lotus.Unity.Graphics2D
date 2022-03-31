//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DBasePlaceable.cs
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
		/// Определение базового интерфейса для элемента размещаемого в двухмерном пространстве
		/// </summary>
		/// <remarks>
		/// Это базовый интерфейс для непосредственного размещения элемента в двухмерном пространстве в экранных координатах
		/// не зависимо от конкретной реализации отображения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusBasePlaceable2D
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Позиция левого угла элемента по X в экранных координатах
			/// </summary>
			Single LeftScreen { get; set; }

			/// <summary>
			/// Позиция правого угла элемента по X в экранных координатах
			/// </summary>
			Single RightScreen { get; set; }

			/// <summary>
			/// Позиция верхнего угла элемента по Y в экранных координатах
			/// </summary>
			Single TopScreen { get; set; }

			/// <summary>
			/// Позиция нижнего угла элемента по Y в экранных координатах
			/// </summary>
			Single BottomScreen { get; set; }

			/// <summary>
			/// Позиция верхнего-левого угла элемента в экранных координатах
			/// </summary>
			Vector2 LocationScreen { get; set; }

			/// <summary>
			/// Ширина(размер по X) элемента
			/// </summary>
			Single WidthScreen { get; set; }

			/// <summary>
			/// Высота(размер по Y) элемента
			/// </summary>
			Single HeightScreen { get; set; }

			/// <summary>
			/// Размеры элемента в экранных координатах
			/// </summary>
			Vector2 SizeScreen { get; set; }

			/// <summary>
			/// Прямоугольника элемента в экранных координатах
			/// </summary>
			Rect RectScreen { get; }

			/// <summary>
			/// Глубина элемента интерфейса (влияет на последовательность прорисовки)
			/// </summary>
			/// <remarks>
			/// <para>
			/// По общему правилу элементы отображаются от наименьшего значения к наибольшему. 
			/// Т.е. элемент с самым большим значением глубины будет нарисован позже всех 
			/// </para>
			/// </remarks>
			Int32 Depth { get; set; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на вхождение точки в область элемента
			/// </summary>
			/// <param name="point">Точка в экранных координатах</param>
			/// <returns>Статус вхождения точки</returns>
			//---------------------------------------------------------------------------------------------------------
			Boolean ContainsScreen(Vector2 point);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров элемента
			/// </summary>
			/// <param name="left">Позиция по X левого угла элемента в экранных координатах</param>
			/// <param name="top">Позиция по Y верхнего угла элемента в экранных координатах</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			//---------------------------------------------------------------------------------------------------------
			void SetFromScreen(Single left, Single top, Single width, Single height);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Основной метод определяющий положение и размер элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void UpdatePlacement();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для интерфейса <see cref="ILotusBasePlaceable2D"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XBasePlaceable2DExtension
		{
			#region ======================================= МЕТОДЫ Move ===============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenLinearIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.Linear(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenLinearIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.Linear(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenQuadInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.QuadIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenQuadInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.QuadIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenQuadOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.QuadOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenQuadOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.QuadOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenQuadInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.QuadInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenQuadInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.QuadInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenCubeInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.CubeIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenCubeInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.CubeIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenCubeOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.CubeOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenCubeOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.CubeOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenCubeInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.CubeInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenCubeInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.CubeInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBackInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BackIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBackInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BackIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBackOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BackOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBackOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BackOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBackInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BackInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBackInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BackInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenExpoInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ExpoIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenExpoInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ExpoIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenExpoOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ExpoOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenExpoOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ExpoOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenExpoInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ExpoInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenExpoInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ExpoInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenSineInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.SineIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenSineInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.SineIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenSineOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.SineOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenSineOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.SineOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenSineInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.SineInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenSineInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.SineInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenElasticInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ElasticIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenElasticInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ElasticIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenElasticOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ElasticOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenElasticOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ElasticOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenElasticInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ElasticInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenElasticInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.ElasticInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBounceInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BounceIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBounceInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BounceIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBounceOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BounceOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBounceOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BounceOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBounceInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BounceInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToWorldScreenBounceInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_position = @this.LocationScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.LocationScreen = XEasing.BounceInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.LocationScreen = target_position;

				on_completed();
			}
			#endregion

			#region ======================================= МЕТОДЫ Resize =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenLinearIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.Linear(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenLinearIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.Linear(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenQuadInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.QuadIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenQuadInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.QuadIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenQuadOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.QuadOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenQuadOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.QuadOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenQuadInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.QuadInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenQuadInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.QuadInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenCubeInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.CubeIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenCubeInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.CubeIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenCubeOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.CubeOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenCubeOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.CubeOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenCubeInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.CubeInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenCubeInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.CubeInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBackInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BackIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBackInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BackIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBackOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BackOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBackOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BackOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBackInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BackInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBackInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BackInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenExpoInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ExpoIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenExpoInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ExpoIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenExpoOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ExpoOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenExpoOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ExpoOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenExpoInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ExpoInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenExpoInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ExpoInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenSineInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.SineIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenSineInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.SineIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenSineOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.SineOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenSineOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.SineOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenSineInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.SineInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenSineInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.SineInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenElasticInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ElasticIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenElasticInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ElasticIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenElasticOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ElasticOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenElasticOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ElasticOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenElasticInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ElasticInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenElasticInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.ElasticInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBounceInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BounceIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBounceInIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BounceIn(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBounceOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BounceOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBounceOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BounceOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBounceInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BounceInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размера базового интерфейса размещения в двухмерном пространстве
			/// </summary>
			/// <param name="this">Базовый интерфейс размещения в двухмерном пространстве</param>
			/// <param name="duration">Время изменения размера</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeToWorldScreenBounceInOutIteration(this ILotusBasePlaceable2D @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 start_size = @this.SizeScreen;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.SizeScreen = XEasing.BounceInOut(ref start_size, ref target_size, time);
					yield return null;
				}

				@this.SizeScreen = target_size;

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