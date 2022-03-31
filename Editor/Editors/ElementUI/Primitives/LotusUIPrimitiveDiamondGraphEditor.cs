//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitiveDiamondGraphEditor.cs
*		Редактор компонента векторного примитива огранки драгоценного камня.
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
using Lotus.Editor;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента векторного примитива огранки драгоценного камня
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIPrimitiveDiamondGraph), true)]
public class LotusUIPrimitiveDiamondGraphEditor : LotusUIPrimitiveBaseEditor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание векторного примитива огранки драгоценного камня с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPathPrimitive + "Create DiamondGraph", false, XElementUIEditorSettings.MenuOrderPrimitive + 4)]
	public static void CreateDiamondGraph()
	{
		LotusUIPrimitiveDiamondGraph diamond_graph = LotusUIPrimitiveDiamondGraph.CreateDiamondGraph(30, 30, 60, 60, Selection.activeTransform as RectTransform);
		Undo.RegisterCreatedObjectUndo(diamond_graph.gameObject, "DiamondGraph");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIPrimitiveDiamondGraph mPrimitiveDiamondGraph;
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
		mPrimitiveDiamondGraph = this.target as LotusUIPrimitiveDiamondGraph;
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
			mPrimitiveDiamondGraph.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location", mPrimitiveDiamondGraph.mExpandedSize);
			if (mPrimitiveDiamondGraph.mExpandedSize)
			{
				GUILayout.Space(4.0f);
				LotusUIPlaceable2DEditor.DrawPlaceable(mPrimitiveDiamondGraph, 2000, 2000);
			}

			mPrimitiveDiamondGraph.mExpandedBase = XEditorInspector.DrawGroupFoldout("Main settings", mPrimitiveDiamondGraph.mExpandedBase);
			if (mPrimitiveDiamondGraph.mExpandedBase)
			{
				GUILayout.Space(4.0f);
				LotusUIPrimitiveBaseEditor.DrawPrimitiveBase(mPrimitiveDiamondGraph);
			}

			mPrimitiveDiamondGraph.mExpandedMainParam = XEditorInspector.DrawGroupFoldout("Settings diamond", mPrimitiveDiamondGraph.mExpandedMainParam);
			if (mPrimitiveDiamondGraph.mExpandedMainParam)
			{
				GUILayout.Space(4.0f);
				mPrimitiveDiamondGraph.TopFace = XEditorInspector.PropertyFloatSlider("TopFace", mPrimitiveDiamondGraph.TopFace, 0, 1);

				GUILayout.Space(4.0f);
				mPrimitiveDiamondGraph.LeftFace = XEditorInspector.PropertyFloatSlider("LeftFace", mPrimitiveDiamondGraph.LeftFace, 0, 1);

				GUILayout.Space(4.0f);
				mPrimitiveDiamondGraph.RightFace = XEditorInspector.PropertyFloatSlider("RightFace", mPrimitiveDiamondGraph.RightFace, 0, 1);

				GUILayout.Space(4.0f);
				mPrimitiveDiamondGraph.BottomFace = XEditorInspector.PropertyFloatSlider("BottomFace", mPrimitiveDiamondGraph.BottomFace, 0, 1);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================