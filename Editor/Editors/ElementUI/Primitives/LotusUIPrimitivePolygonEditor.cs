//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitivePolygonEditor.cs
*		Редактор компонента векторного примитива многоугольника.
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
/// Редактор компонента векторного примитива многоугольника
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIPrimitivePolygon), true)]
public class LotusUIPrimitivePolygonEditor : LotusUIPrimitiveBaseEditor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание векторного примитива многоугольника с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPathPrimitive + "Create Polygon", false, XElementUIEditorSettings.MenuOrderPrimitive + 5)]
	public static void CreatePolygon()
	{
		LotusUIPrimitivePolygon polygon = LotusUIPrimitivePolygon.CreatePolygon(30, 30, 60, 60, Selection.activeTransform as RectTransform);
		Undo.RegisterCreatedObjectUndo(polygon.gameObject, "Polygon");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIPrimitivePolygon mPrimitivePolygon;
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
		mPrimitivePolygon = this.target as LotusUIPrimitivePolygon;
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
			mPrimitivePolygon.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location", mPrimitivePolygon.mExpandedSize);
			if (mPrimitivePolygon.mExpandedSize)
			{
				GUILayout.Space(4.0f);
				LotusUIPlaceable2DEditor.DrawPlaceable(mPrimitivePolygon, 2000, 2000);
			}

			mPrimitivePolygon.mExpandedBase = XEditorInspector.DrawGroupFoldout("Main settings", mPrimitivePolygon.mExpandedBase);
			if (mPrimitivePolygon.mExpandedBase)
			{
				GUILayout.Space(4.0f);
				LotusUIPrimitiveBaseEditor.DrawPrimitiveBase(mPrimitivePolygon);
			}

			mPrimitivePolygon.mExpandedMainParam = XEditorInspector.DrawGroupFoldout("Settings poligon", mPrimitivePolygon.mExpandedMainParam);
			if (mPrimitivePolygon.mExpandedMainParam)
			{
				GUILayout.Space(4.0f);
				mPrimitivePolygon.Fill = XEditorInspector.PropertyBoolean("Fill", mPrimitivePolygon.Fill);

				GUILayout.Space(2.0f);
				mPrimitivePolygon.Thickness = XEditorInspector.PropertyFloatSlider("Thickness", mPrimitivePolygon.Thickness, 1, 10);

				GUILayout.Space(2.0f);
				mPrimitivePolygon.Sides = XEditorInspector.PropertyIntSlider("Sides", mPrimitivePolygon.Sides, 3, 360);

				GUILayout.Space(2.0f);
				mPrimitivePolygon.Rotation = XEditorInspector.PropertyFloatSlider("Rotation", mPrimitivePolygon.Rotation, 0, 359);
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