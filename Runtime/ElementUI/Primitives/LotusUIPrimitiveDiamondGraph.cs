//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitiveDiamondGraph.cs
*		Компонент векторного примитива огранки драгоценного камня.
*		Реализация компонента обеспечивающего генерацию векторного примитива типа огранки драгоценного камня.
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
		/// Компонент векторного примитива огранки драгоценного камня
		/// </summary>
		/// <remarks>
		/// Реализация компонента обеспечивающего генерацию векторного примитива типа огранки драгоценного камня
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathPrimitive + "DiamondGraph")]
		public class LotusUIPrimitiveDiamondGraph : LotusUIPrimitiveBase
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание векторного примитива огранки драгоценного камня
			/// </summary>
			/// <param name="left">Позиция по X примитива</param>
			/// <param name="top">Позиция по Y примитива</param>
			/// <param name="width">Ширина примитива</param>
			/// <param name="height">Высота примитива</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный примитив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusUIPrimitiveDiamondGraph CreateDiamondGraph(Single left, Single top, Single width, Single height, RectTransform parent = null)
			{
				// 1) Создание объекта
				LotusUIPrimitiveDiamondGraph circle = LotusElementUIDispatcher.CreateElement<LotusUIPrimitiveDiamondGraph>("DiamondGraph ",
					left, top, width, height);

				// 2) Определение в иерархии
				circle.SetParent(parent);

				return circle;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal Single mLeftFace = 1;
			[SerializeField]
			internal Single mTopFace = 1;
			[SerializeField]
			internal Single mRightFace = 1;
			[SerializeField]
			internal Single mBottomFace = 1;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Левая грань
			/// </summary>
			public Single LeftFace
			{
				get { return mLeftFace; }
				set
				{
					if(!Mathf.Approximately(mLeftFace, value))
					{
						mLeftFace = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Верхняя грань
			/// </summary>
			public Single TopFace
			{
				get { return mTopFace; }
				set
				{
					if (!Mathf.Approximately(mTopFace, value))
					{
						mTopFace = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Правая грань
			/// </summary>
			public Single RightFace
			{
				get { return mRightFace; }
				set
				{
					if (!Mathf.Approximately(mRightFace, value))
					{
						mRightFace = value;
						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Нижняя грань
			/// </summary>
			public Single BottomFace
			{
				get { return mBottomFace; }
				set
				{
					if (!Mathf.Approximately(mBottomFace, value))
					{
						mBottomFace = value;
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
				Single width_half = rectTransform.rect.width / 2;
				Single height_half = rectTransform.rect.height / 2;
				mLeftFace = Math.Min(1, Math.Max(0, mLeftFace));
				mTopFace = Math.Min(1, Math.Max(0, mTopFace));
				mRightFace = Math.Min(1, Math.Max(0, mRightFace));
				mBottomFace = Math.Min(1, Math.Max(0, mBottomFace));

				Color32 color32 = color;
				vertex_helper.AddVert(new Vector3(-width_half * mLeftFace, 0), color32, Vector2.zero);
				vertex_helper.AddVert(new Vector3(0, height_half * mTopFace), color32, Vector2.up);
				vertex_helper.AddVert(new Vector3(width_half * mRightFace, 0), color32, Vector2.one);
				vertex_helper.AddVert(new Vector3(0, -height_half * mBottomFace), color32, Vector2.right);

				vertex_helper.AddTriangle(0, 1, 2);
				vertex_helper.AddTriangle(2, 3, 0);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================