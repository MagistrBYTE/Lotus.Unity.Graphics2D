//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Компоненты IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUILabelEditor.cs
*		Редактор компонента представляющего простую надпись модуля IMGUI Unity.
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
/// Редактор компонента представляющего простую надпись модуля IMGUI Unity
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusGUILabel))]
public class LotusGUILabelEditor : LotusGUIElementEditor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента Label
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGUIEditorSettings.MenuPathComponent + "Create Label", false, XGUIEditorSettings.MenuOrderComponent + 3)]
	public static void CreateLabel()
	{
		LotusGUILabel.CreateElement();
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusGUILabel mLabel;
	private static GUIContent mContentOpen = new GUIContent("+");
	private static GUIContent mContentClose = new GUIContent("-");
	private static GUIContent mContentGet = new GUIContent("Get");
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public new void OnEnable()
	{
		mLabel = this.target as LotusGUILabel;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Рисование свойств основного элемента
		DrawElementParamemtrs(mLabel);

		// Рисование свойств надписи
		DrawLabelParamemtrs(mLabel);

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров компонента Label
	/// </summary>
	/// <param name="label">Компонент Label</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawLabelParamemtrs(LotusGUILabel label)
	{
		GUILayout.Space(4.0f);
		label.mExpandedLabel = XEditorInspector.DrawGroupFoldout("Label settings", label.mExpandedLabel);
		if (label.mExpandedLabel)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				GUILayout.Space(2.0f);
				Boolean expanded = this.serializedObject.LoadEditorBool("Expanded", false);
				if (expanded)
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.PrefixLabel("CaptionText", EditorStyles.label);
						if (GUILayout.Button(expanded ? mContentClose : mContentOpen, EditorStyles.miniButtonRight,
							GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
						{
							expanded = !expanded;
							this.serializedObject.SaveBoolEditor("Expanded", expanded);
						}
					}
					EditorGUILayout.EndHorizontal();
					label.CaptionText = EditorGUILayout.TextArea(label.CaptionText);
				}
				else
				{
					EditorGUILayout.BeginHorizontal();
					{
						label.CaptionText = XEditorInspector.PropertyString("CaptionText", label.CaptionText);
						// Кнопка для раскрытия кода
						if (GUILayout.Button(expanded ? mContentClose : mContentOpen, EditorStyles.miniButtonRight,
							GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
						{
							expanded = !expanded;
							this.serializedObject.SaveBoolEditor("Expanded", expanded);
						}
					}
					EditorGUILayout.EndHorizontal();
				}

				// 2)
				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				{
					label.IDKeyLocalize = XEditorInspector.PropertyInt("IDKeyLocalize", label.IDKeyLocalize);

					if (GUILayout.Button(mContentGet, EditorStyles.miniButtonRight))
					{
						label.ComputeIDKeyLocalizeFromHash();
					}
				}
				EditorGUILayout.EndHorizontal();

				// 3)
				GUILayout.Space(2.0f);
				label.CaptionIcon = XEditorInspector.PropertyResource("CaptionIcon", label.CaptionIcon);
			}
			if (EditorGUI.EndChangeCheck())
			{
				this.serializedObject.Save();
			}
		}
	}
	#endregion
}
//=====================================================================================================================