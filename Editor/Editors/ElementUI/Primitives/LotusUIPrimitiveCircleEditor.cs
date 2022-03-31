//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitiveCircleEditor.cs
*		Редактор компонента векторного примитива окружности.
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
/// Редактор компонента векторного примитива окружности
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIPrimitiveCircle), true)]
public class LotusUIPrimitiveCircleEditor : LotusUIPrimitiveBaseEditor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание векторного примитива окружности с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPathPrimitive + "Create Circle", false, XElementUIEditorSettings.MenuOrderPrimitive + 3)]
	public static void CreateCircle()
	{
		LotusUIPrimitiveCircle circle = LotusUIPrimitiveCircle.CreateCircle(30, 30, 60, Selection.activeTransform as RectTransform);
		Undo.RegisterCreatedObjectUndo(circle.gameObject, "Circle");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIPrimitiveCircle mPrimitiveCircle;
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
		mPrimitiveCircle = this.target as LotusUIPrimitiveCircle;
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
			mPrimitiveCircle.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location", mPrimitiveCircle.mExpandedSize);
			if (mPrimitiveCircle.mExpandedSize)
			{
				GUILayout.Space(4.0f);
				LotusUIPlaceable2DEditor.DrawPlaceable(mPrimitiveCircle, 2000, 2000);
			}

			mPrimitiveCircle.mExpandedBase = XEditorInspector.DrawGroupFoldout("Main settings", mPrimitiveCircle.mExpandedBase);
			if (mPrimitiveCircle.mExpandedBase)
			{
				GUILayout.Space(4.0f);
				LotusUIPrimitiveBaseEditor.DrawPrimitiveBase(mPrimitiveCircle);
			}

			mPrimitiveCircle.mExpandedMainParam = XEditorInspector.DrawGroupFoldout("Settings circle", mPrimitiveCircle.mExpandedMainParam);
			if (mPrimitiveCircle.mExpandedMainParam)
			{
				GUILayout.Space(4.0f);
				mPrimitiveCircle.Fill = XEditorInspector.PropertyBoolean("Fill", mPrimitiveCircle.Fill);

				GUILayout.Space(2.0f);
				mPrimitiveCircle.FillPercent = XEditorInspector.PropertyIntSlider("FillPercent", mPrimitiveCircle.FillPercent, 1, 100);

				GUILayout.Space(2.0f);
				mPrimitiveCircle.Thickness = XEditorInspector.PropertyFloatSlider("Thickness", mPrimitiveCircle.Thickness, 1, 10);

				GUILayout.Space(2.0f);
				mPrimitiveCircle.FixedToSegments = XEditorInspector.PropertyBoolean("FixedToSegments", mPrimitiveCircle.FixedToSegments);

				GUILayout.Space(2.0f);
				mPrimitiveCircle.Segments = XEditorInspector.PropertyIntSlider("Segments", mPrimitiveCircle.Segments, 1, 360);
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