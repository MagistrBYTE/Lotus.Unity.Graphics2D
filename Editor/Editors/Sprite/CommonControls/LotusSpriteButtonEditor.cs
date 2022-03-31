//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteButtonEditor.cs
*		Редактор компонента элемента Button.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента элемента Button
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusSpriteButton))]
public class LotusSpriteButtonEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static GUIContent mContentEventClickSender = new GUIContent("On ClickSender ()");
	#endregion

	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента Button с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XSpriteEditorSettings.MenuPathCommon + "Create Button", false, XSpriteEditorSettings.MenuOrderCommon + 2)]
	public static void Create()
	{
		LotusSpriteButton button = LotusSpriteButton.Create(0, 0, 200, 50, Selection.activeTransform);
		Undo.RegisterCreatedObjectUndo(button.gameObject, "Button");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusSpriteButton mButton;
	private SerializedProperty mEventClickExProperty;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mButton = this.target as LotusSpriteButton;
		mEventClickExProperty = serializedObject.FindProperty("mOnClickEx");
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Параметры базового элемента (местоположение и интерактивность)
		mButton.mExpandedSize = XEditorInspector.DrawGroupFoldout("Parameters and size", mButton.mExpandedSize);
		if (mButton.mExpandedSize)
		{
			LotusSpriteElementEditor.DrawElementParam(mButton);
		}

		EditorGUI.BeginChangeCheck();
		{
			// Основные параметры элемента
			GUILayout.Space(4.0f);
			mButton.mExpandedParam = XEditorInspector.DrawGroupFoldout("Settings button", mButton.mExpandedParam);
			if (mButton.mExpandedParam)
			{
				DrawButtonParams(mButton);

				GUILayout.Space(4.0f);
				XEditorInspector.DrawGroup("Events");
				EditorGUI.BeginChangeCheck();
				{
					EditorGUILayout.PropertyField(mEventClickExProperty, mContentEventClickSender);
				}
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mButton.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров кнопки
	/// </summary>
	/// <param name="button">Элемент</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawButtonParams(LotusSpriteButton button)
	{
		GUILayout.Space(4.0f);
		XEditorInspector.DrawGroup("Control params");
		LotusSpriteControlEditor.DrawControlParams(button);

		GUILayout.Space(4.0f);
		XEditorInspector.DrawGroup("Button params");
		if (button.InteractiveMode == TInteractiveMode.Toogle)
		{
			button.ButtonGroup = XEditorInspector.PropertyComponent(nameof(button.ButtonGroup), button.ButtonGroup);
		}
		else
		{
			button.IsImmediate = XEditorInspector.PropertyBoolean(nameof(button.IsImmediate), button.IsImmediate);

			if (button.IsImmediate)
			{
				GUILayout.Space(2.0f);
				EditorGUI.indentLevel++;
				button.IntervalImmediate = XEditorInspector.PropertyFloatSlider(nameof(button.IntervalImmediate), button.IntervalImmediate, 0.01f, 1.0f);
				EditorGUI.indentLevel--;
			}
		}
	}
	#endregion
}
//=====================================================================================================================