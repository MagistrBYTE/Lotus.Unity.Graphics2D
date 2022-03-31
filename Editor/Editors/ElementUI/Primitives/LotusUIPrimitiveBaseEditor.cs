//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitiveBaseEditor.cs
*		Редактор базового компонента векторного примитива.
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
/// Редактор базового компонента векторного примитива
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIPrimitiveBase), false)]
public class LotusUIPrimitiveBaseEditor : GraphicEditor
{
	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIPrimitiveBase mPrimitiveBase;
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
		mPrimitiveBase = this.target as LotusUIPrimitiveBase;
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
			mPrimitiveBase.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location", mPrimitiveBase.mExpandedSize);
			if (mPrimitiveBase.mExpandedSize)
			{
				GUILayout.Space(4.0f);
				LotusUIPlaceable2DEditor.DrawPlaceable(mPrimitiveBase, 2000, 2000);
			}

			mPrimitiveBase.mExpandedBase = XEditorInspector.DrawGroupFoldout("Main settings", mPrimitiveBase.mExpandedBase);
			if (mPrimitiveBase.mExpandedBase)
			{
				GUILayout.Space(4.0f);
				LotusUIPrimitiveBaseEditor.DrawPrimitiveBase(mPrimitiveBase);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование базовых параметров векторного примитива
	/// </summary>
	/// <param name="primitive_base">Базовый векторный примитив</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawPrimitiveBase(LotusUIPrimitiveBase primitive_base)
	{
		GUILayout.Space(4.0f);
		primitive_base.sprite = XEditorInspector.PropertyResource("Sprite", primitive_base.sprite);

		GUILayout.Space(4.0f);
		primitive_base.color = XEditorInspector.PropertyColor("Color", primitive_base.color);

		GUILayout.Space(4.0f);
		primitive_base.material = XEditorInspector.PropertyResource("Material", primitive_base.material);

		GUILayout.Space(4.0f);
		primitive_base.raycastTarget = XEditorInspector.PropertyBoolean("RaycastTarget", primitive_base.raycastTarget);
	}
	#endregion
}
//=====================================================================================================================