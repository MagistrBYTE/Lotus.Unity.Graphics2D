//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitiveGrid.cs
*		Компонент векторного примитива сетки.
*		Реализация компонент обеспечивающего генерацию векторного примитива сетки.
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
		/// Компонент векторного примитива сетки
		/// </summary>
		/// <remarks>
		/// Реализация компонент обеспечивающего генерацию векторного примитива сетки
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathPrimitive + "Grid")]
		public class LotusUIPrimitiveGrid : LotusUIPrimitiveBase
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание векторного примитива сетки
			/// </summary>
			/// <param name="left">Позиция по X примитива</param>
			/// <param name="top">Позиция по Y примитива</param>
			/// <param name="width">Ширина примитива</param>
			/// <param name="height">Высота примитива</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный примитив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusUIPrimitiveGrid CreateGrid(Single left, Single top, Single width, Single height, RectTransform parent = null)
			{
				// 1) Создание объекта
				LotusUIPrimitiveGrid Grid = LotusElementUIDispatcher.CreateElement<LotusUIPrimitiveGrid>("Grid", left, top, width, height);

				// 2) Определение в иерархии
				Grid.SetParent(parent);

				return Grid;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal Vector4 mPadding;
			[SerializeField]
			internal Single mCellWidth = 20;
			[SerializeField]
			internal Single mCellHeight = 20;
			[SerializeField]
			internal Single mLineThickness = 2;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Внутренние отступы сетки от краев элемента
			/// </summary>
			public Vector4 Padding
			{
				get { return mPadding; }
				set
				{
					if (mPadding != value)
					{
						mPadding = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Внутренний отступ сетки от левого края элемента
			/// </summary>
			public Single PaddingLeft
			{
				get { return mPadding.x; }
				set
				{
					if (!Mathf.Approximately(mPadding.x, value))
					{
						mPadding.x = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Внутренний отступ сетки от верхнего края элемента
			/// </summary>
			public Single PaddingTop
			{
				get { return mPadding.y; }
				set
				{
					if (!Mathf.Approximately(mPadding.y, value))
					{
						mPadding.y = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Внутренний отступ сетки от правого края элемента
			/// </summary>
			public Single PaddingRight
			{
				get { return mPadding.z; }
				set
				{
					if (!Mathf.Approximately(mPadding.z, value))
					{
						mPadding.z = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Внутренний отступ сетки от нижнего края элемента
			/// </summary>
			public Single PaddingBottom
			{
				get { return mPadding.w; }
				set
				{
					if (!Mathf.Approximately(mPadding.w, value))
					{
						mPadding.w = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Ширина ячейки сетки
			/// </summary>
			public Single CellWidth
			{
				get { return mCellWidth; }
				set
				{
					if (!Mathf.Approximately(mCellWidth, value))
					{
						mCellWidth = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Высота ячейки сетки
			/// </summary>
			public Single CellHeight
			{
				get { return mCellHeight; }
				set
				{
					if (!Mathf.Approximately(mCellHeight, value))
					{
						mCellHeight = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Толщина линий сетки
			/// </summary>
			public Single LineThickness
			{
				get { return mLineThickness; }
				set
				{
					if (!Mathf.Approximately(mLineThickness, value))
					{
						mLineThickness = value;

						UpdateGeometryForced();
						SetAllDirty();
					}
				}
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
				// Очищаем набор вершин
				vertex_helper.Clear();

				Single offset_x = -rectTransform.pivot.x * rectTransform.rect.width;
				Single offset_y = -rectTransform.pivot.y * rectTransform.rect.height;
				Single width_grid = rectTransform.rect.width - (PaddingLeft + PaddingRight);
				Single heigth_grid = rectTransform.rect.height - (PaddingTop + PaddingBottom);

				// Кол-во вертикальных линий
				Int32 vert_count = (Int32)(width_grid / mCellWidth);
				Int32 horiz_count = (Int32)(heigth_grid / mCellHeight);
				Single sx = PaddingLeft + offset_x;
				Single sy = PaddingBottom + offset_y; // Здесь делаем так потому что оси по у обратима

				for (Int32 iy = 0; iy < vert_count + 1; iy++)
				{
					Single v = sx + iy * mCellWidth;
					vertex_helper.AddLine(new Vector2(v, sy), new Vector2(v, sy + heigth_grid), mLineThickness, color);
				}

				for (Int32 ix = 0; ix < horiz_count + 1; ix++)
				{
					Single h = sy + ix * mCellHeight;
					vertex_helper.AddLine(new Vector2(sx, h), new Vector2(sx + width_grid, h), mLineThickness, color);
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