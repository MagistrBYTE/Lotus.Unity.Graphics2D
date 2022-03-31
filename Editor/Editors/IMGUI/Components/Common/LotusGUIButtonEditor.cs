//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Компоненты IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIButtonEditor.cs
*		Редактор компонента представляющего кнопку модуля IMGUI Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента представляющего кнопку модуля IMGUI Unity
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusGUIButton))]
public class LotusGUIButtonEditor : LotusGUILabelEditor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента Button
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGUIEditorSettings.MenuPathComponent + "Create Button", false, XGUIEditorSettings.MenuOrderComponent + 51)]
	public static void CreateButton()
	{
		LotusGUIButton.CreateElement();
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusGUIButton mButton;
	private SerializedProperty mEventClickSenderProperty;
	private SerializedProperty mEventClickProperty;
	private static GUIContent mEventClickSenderContent = new GUIContent("On ClickSender()");
	private static GUIContent mEventClickContent = new GUIContent("On Click()");
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public new void OnEnable()
	{
		mButton = this.target as LotusGUIButton;
		mEventClickProperty = serializedObject.FindProperty("mOnClick");
		mEventClickSenderProperty = serializedObject.FindProperty("mOnClickSender");
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Рисование свойств основного элемента
		DrawElementParamemtrs(mButton);

		// Рисование свойств надписи
		DrawLabelParamemtrs(mButton);

		// Рисование свойств кнопки
		DrawButtonParamemtrs(mButton);

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров компонента Button
	/// </summary>
	/// <param name="button">Компонент Button</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawButtonParamemtrs(LotusGUIButton button)
	{
		GUILayout.Space(4.0f);
		button.mExpandedButton = XEditorInspector.DrawGroupFoldout("Button settings", button.mExpandedButton);
		if (button.mExpandedButton)
		{
			this.serializedObject.Update();

			EditorGUI.BeginChangeCheck();
			{
				GUILayout.Space(4.0f);
				EditorGUILayout.PropertyField(mEventClickProperty, mEventClickContent);

				GUILayout.Space(4.0f);
				EditorGUILayout.PropertyField(mEventClickSenderProperty, mEventClickSenderContent);
			}
			if (EditorGUI.EndChangeCheck())
			{
				this.serializedObject.ApplyModifiedProperties();
				this.serializedObject.Save();
			}
		}
	}
	#endregion
}
//=====================================================================================================================