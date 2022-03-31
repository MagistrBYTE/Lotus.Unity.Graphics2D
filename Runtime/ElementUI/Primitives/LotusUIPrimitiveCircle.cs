//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitiveCircle.cs
*		Компонент векторного примитива окружности.
*		Реализация компонент обеспечивающего генерацию векторного примитива окружности.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DUIPrimitive
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент векторного примитива окружности
		/// </summary>
		/// <remarks>
		/// Реализация компонент обеспечивающего генерацию векторного примитива окружности
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathPrimitive + "Circle")]
		public class LotusUIPrimitiveCircle : LotusUIPrimitiveBase
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание векторного примитива окружности
			/// </summary>
			/// <param name="center_x">Позиция центра по X</param>
			/// <param name="center_y">Позиция центра по Y</param>
			/// <param name="radius">Радиус окружности</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный примитив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusUIPrimitiveCircle CreateCircle(Single center_x, Single center_y, Single radius, RectTransform parent = null)
			{
				// 1) Создание объекта
				LotusUIPrimitiveCircle circle = LotusElementUIDispatcher.CreateElement<LotusUIPrimitiveCircle>("Circle", 
					center_x - radius, center_y - radius, radius * 2, radius * 2);

				// 2) Определение в иерархии
				circle.SetParent(parent);

				return circle;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal Int32 mFillPercent = 100;
			[SerializeField]
			internal Boolean mFixedToSegments = false;
			[SerializeField]
			internal Boolean mFill = true;
			[SerializeField]
			internal Single mThickness = 5;
			[SerializeField]
			internal Int32 mSegments = 360;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Процент заливки фигура по объему
			/// </summary>
			public Int32 FillPercent
			{
				get { return mFillPercent; }
				set
				{
					if (mFillPercent != value)
					{
						mFillPercent = value;

						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Использовать заливку для окружности
			/// </summary>
			public Boolean Fill
			{
				get { return mFill; }
				set
				{
					if (mFill != value)
					{
						mFill = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Толщина границы окружности
			/// </summary>
			public Single Thickness
			{
				get { return mThickness; }
				set
				{
					if (!Mathf.Approximately(mThickness, value))
					{
						mThickness = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Рисование границы окружности сегментами или гладкими линиями
			/// </summary>
			public Boolean FixedToSegments
			{
				get { return mFixedToSegments; }
				set
				{
					if (mFixedToSegments != value)
					{
						mFixedToSegments = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Количество сегментов для рисовании границы окружности сегментами
			/// </summary>
			public Int32 Segments
			{
				get { return mSegments; }
				set
				{
					if (mSegments != value)
					{
						mSegments = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void Reset()
			{
				base.Reset();
				mFillPercent = 100;
				mFixedToSegments = false;
				mFill = true;
				mThickness = 5;
				mSegments = 360;
			}
#endif
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация квада
			/// </summary>
			/// <param name="outer">Наружный радиус</param>
			/// <param name="inner">Внутренний радиус</param>
			/// <param name="prev_x">Предыдущая вершина по X</param>
			/// <param name="prev_y">Предыдущая вершина по Y</param>
			/// <param name="pos0">1 генерируемая вершина</param>
			/// <param name="pos1">2 генерируемая вершина</param>
			/// <param name="pos2">3 генерируемая вершина</param>
			/// <param name="pos3">4 генерируемая вершина</param>
			/// <param name="cos">Косинус</param>
			/// <param name="sin">Синус</param>
			//---------------------------------------------------------------------------------------------------------
			private void StepThroughPointsAndFill(Single outer, Single inner, ref Vector2 prev_x, ref Vector2 prev_y, 
				out Vector2 pos0, out Vector2 pos1, out Vector2 pos2, out Vector2 pos3, Single cos, Single sin)
			{
				pos0 = prev_x;
				pos1 = new Vector2(outer * cos, outer * sin);

				if (mFill)
				{
					pos2 = Vector2.zero;
					pos3 = Vector2.zero;
				}
				else
				{
					pos2 = new Vector2(inner * cos, inner * sin);
					pos3 = prev_y;
				}

				prev_x = pos1;
				prev_y = pos2;
			}
			#endregion

			#region ======================================= МЕТОДЫ Graphic ============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация вершин элементом
			/// </summary>
			/// <param name="vertex_helper">Набор вершин для модификации</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnPopulateMesh(VertexHelper vertex_helper)
			{
				Single outer = -rectTransform.pivot.x * rectTransform.rect.width;
				Single inner = -rectTransform.pivot.x * rectTransform.rect.width + this.mThickness;

				vertex_helper.Clear();

				Vector2 prevX = Vector2.zero;
				Vector2 prevY = Vector2.zero;
				Vector2 uv0 = new Vector2(0, 0);
				Vector2 uv1 = new Vector2(0, 1);
				Vector2 uv2 = new Vector2(1, 1);
				Vector2 uv3 = new Vector2(1, 0);
				Vector2 pos0;
				Vector2 pos1;
				Vector2 pos2;
				Vector2 pos3;

				if (mFixedToSegments)
				{
					Single f = this.mFillPercent / 100f;
					Single degrees = 360f / mSegments;
					Int32 fa = (Int32)((mSegments + 1) * f);

					for (Int32 i = 0; i < fa; i++)
					{
						Single rad = Mathf.Deg2Rad * (i * degrees);
						Single cos = Mathf.Cos(rad);
						Single sin = Mathf.Sin(rad);

						uv0 = new Vector2(0, 1);
						uv1 = new Vector2(1, 1);
						uv2 = new Vector2(1, 0);
						uv3 = new Vector2(0, 0);

						StepThroughPointsAndFill(outer, inner, ref prevX, ref prevY, out pos0, out pos1, out pos2, out pos3, cos, sin);

						vertex_helper.AddUIVertexQuad(SetVertexBufferQuad(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }, color));
					}
				}
				else
				{
					Single tw = rectTransform.rect.width;
					Single th = rectTransform.rect.height;

					// Угол за шаг
					Single angle_by_step = mFillPercent / 100f * (Mathf.PI * 2f) / mSegments;
					Single current_angle = 0f;

					for (Int32 i = 0; i < mSegments + 1; i++)
					{
						//Color vc = Color.Lerp(Color.red, Color.green, (Single)(i) / (Single)mSegments);

						Single cos = Mathf.Cos(current_angle);
						Single sin = Mathf.Sin(current_angle);

						StepThroughPointsAndFill(outer, inner, ref prevX, ref prevY, out pos0, out pos1, out pos2, out pos3, cos, sin);

						uv0 = new Vector2(pos0.x / tw + 0.5f, pos0.y / th + 0.5f);
						uv1 = new Vector2(pos1.x / tw + 0.5f, pos1.y / th + 0.5f);
						uv2 = new Vector2(pos2.x / tw + 0.5f, pos2.y / th + 0.5f);
						uv3 = new Vector2(pos3.x / tw + 0.5f, pos3.y / th + 0.5f);

						vertex_helper.AddUIVertexQuad(SetVertexBufferQuad(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }, color));

						current_angle += angle_by_step;
					}
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================