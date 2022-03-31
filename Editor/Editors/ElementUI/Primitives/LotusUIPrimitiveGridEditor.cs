//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitiveGridEditor.cs
*		Редактор компонента векторного примитива сетки.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента векторного примитива сетки
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIPrimitiveGrid), true)]
public class LotusUIPrimitiveGridEditor : LotusUIPrimitiveBaseEditor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание векторного примитива сетки с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPathPrimitive + "Create Grid", false, XElementUIEditorSettings.MenuOrderPrimitive + 2)]
	public static void CreateGrid()
	{
		LotusUIPrimitiveGrid polyline = LotusUIPrimitiveGrid.CreateGrid(30, 30, 60, 60, Selection.activeTransform as RectTransform);
		Undo.RegisterCreatedObjectUndo(polyline.gameObject, "Grid");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIPrimitiveGrid mPrimitiveGrid;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void OnEnable()
	{
		base.OnEnable();
		mPrimitiveGrid = this.target as LotusUIPrimitiveGrid;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		{
			mPrimitiveGrid.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location", mPrimitiveGrid.mExpandedSize);
			if (mPrimitiveGrid.mExpandedSize)
			{
				GUILayout.Space(4.0f);
				LotusUIPlaceable2DEditor.DrawPlaceable(mPrimitiveGrid, 2000, 2000);
			}

			mPrimitiveGrid.mExpandedBase = XEditorInspector.DrawGroupFoldout("Main settings", mPrimitiveGrid.mExpandedBase);
			if (mPrimitiveGrid.mExpandedBase)
			{
				GUILayout.Space(4.0f);
				DrawPrimitiveBase(mPrimitiveGrid);
			}

			mPrimitiveGrid.mExpandedMainParam = XEditorInspector.DrawGroupFoldout("Settings grid", mPrimitiveGrid.mExpandedMainParam);
			if (mPrimitiveGrid.mExpandedMainParam)
			{
				GUILayout.Space(4.0f);
				mPrimitiveGrid.Padding =  XEditorInspector.PropertyBorderNormal("Padding", mPrimitiveGrid.Padding);

				GUILayout.Space(4.0f);
				mPrimitiveGrid.CellWidth = XEditorInspector.PropertyFloatSlider("CellWidth", mPrimitiveGrid.CellWidth, 2, mPrimitiveGrid.Width);

				GUILayout.Space(4.0f);
				mPrimitiveGrid.CellHeight = XEditorInspector.PropertyFloatSlider("CellHeight", mPrimitiveGrid.CellHeight, 2, mPrimitiveGrid.Height);

				GUILayout.Space(4.0f);
				mPrimitiveGrid.LineThickness = XEditorInspector.PropertyFloatSlider("Thickness", mPrimitiveGrid.LineThickness, 1, 10);

				Int32 xc = (Int32)((mPrimitiveGrid.Width - (mPrimitiveGrid.PaddingLeft + mPrimitiveGrid.PaddingRight)) / mPrimitiveGrid.CellWidth);
				Int32 yc = (Int32)((mPrimitiveGrid.Height - (mPrimitiveGrid.PaddingTop + mPrimitiveGrid.PaddingBottom)) / mPrimitiveGrid.CellHeight);
				Single dw = xc * mPrimitiveGrid.CellWidth + (mPrimitiveGrid.PaddingLeft + mPrimitiveGrid.PaddingRight);
				Single dh = yc * mPrimitiveGrid.CellHeight + (mPrimitiveGrid.PaddingTop + mPrimitiveGrid.PaddingBottom);

				GUILayout.Space(4.0f);
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel("Count W[" + xc.ToString() + "], width = " + dw.ToString(), EditorStyles.label);

					if(GUILayout.Button("Set", EditorStyles.miniButtonLeft))
					{
						mPrimitiveGrid.Width = dw;
					}

					if (GUILayout.Button("Set parent", EditorStyles.miniButtonRight))
					{
						RectTransform parent_rect = mPrimitiveGrid.GetComponentInParent<RectTransform>();
						if(parent_rect != null)
						{
							parent_rect.SetWidth(dw);
						}
					}
				}
				EditorGUILayout.EndHorizontal();


				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel("Count H[" + yc.ToString() + "], height = " + dh.ToString());

					if (GUILayout.Button("Set", EditorStyles.miniButtonLeft))
					{
						mPrimitiveGrid.Height = dh;
					}

					if (GUILayout.Button("Set parent", EditorStyles.miniButtonRight))
					{
						RectTransform parent_rect = mPrimitiveGrid.GetComponentInParent<RectTransform>();
						if (parent_rect != null)
						{
							parent_rect.SetHeight(dh);
						}
					}
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mPrimitiveGrid.UpdateGeometryForced();
			this.serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================