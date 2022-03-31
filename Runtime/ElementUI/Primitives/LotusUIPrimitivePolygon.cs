//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitivePolygon.cs
*		Компонент векторного примитива многоугольника.
*		Реализация компонент обеспечивающего генерацию векторного примитива многоугольника.
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
		/// Компонент векторного примитива многоугольника
		/// </summary>
		/// <remarks>
		/// Реализация компонент обеспечивающего генерацию векторного примитива многоугольника
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathPrimitive + "Polygon")]
		public class LotusUIPrimitivePolygon : LotusUIPrimitiveBase
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание векторного примитива многоугольника
			/// </summary>
			/// <param name="left">Позиция по X примитива</param>
			/// <param name="top">Позиция по Y примитива</param>
			/// <param name="width">Ширина примитива</param>
			/// <param name="height">Высота примитива</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный примитив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusUIPrimitivePolygon CreatePolygon(Single left, Single top, Single width, Single height, RectTransform parent = null)
			{
				// 1) Создание объекта
				LotusUIPrimitivePolygon polygon = LotusElementUIDispatcher.CreateElement<LotusUIPrimitivePolygon>("Polygon",
					left, top, width, height);

				// 2) Определение в иерархии
				polygon.SetParent(parent);

				return polygon;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal Boolean mFill = true;
			[SerializeField]
			internal Single mThickness = 5;
			[SerializeField]
			internal Int32 mSides = 360;
			[SerializeField]
			internal Single mRotation = 0;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Использовать заливку для многоугольника
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
			/// Толщина границы многоугольника
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
			/// Количество сторон многоугольника
			/// </summary>
			public Int32 Sides
			{
				get { return mSides; }
				set
				{
					if (mSides != value)
					{
						mSides = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Угол поворота многоугольника
			/// </summary>
			public Single Rotation
			{
				get { return mRotation; }
				set
				{
					if (!Mathf.Approximately(mRotation, value))
					{
						mRotation = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
			protected override void Reset()
			{
				base.Reset();
			}
#endif
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
				Single size = rectTransform.rect.width;
				Single degrees = 360f / mSides;
				Int32 vertices = mSides + 1;

				for (Int32 i = 0; i < vertices; i++)
				{
					Single outer = -rectTransform.pivot.x * size;
					Single inner = -rectTransform.pivot.x * size + mThickness;
					Single rad = Mathf.Deg2Rad * (i * degrees + mRotation);
					Single c = Mathf.Cos(rad);
					Single s = Mathf.Sin(rad);
					uv0 = new Vector2(0, 1);
					uv1 = new Vector2(1, 1);
					uv2 = new Vector2(1, 0);
					uv3 = new Vector2(0, 0);
					pos0 = prevX;
					pos1 = new Vector2(outer * c, outer * s);
					if (mFill)
					{
						pos2 = Vector2.zero;
						pos3 = Vector2.zero;
					}
					else
					{
						pos2 = new Vector2(inner * c, inner * s);
						pos3 = prevY;
					}
					prevX = pos1;
					prevY = pos2;
					vertex_helper.AddUIVertexQuad(SetVertexBufferQuad(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));
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