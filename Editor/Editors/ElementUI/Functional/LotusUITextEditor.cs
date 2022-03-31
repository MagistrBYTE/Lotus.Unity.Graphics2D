//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUITextEditor.cs
*		Редактор визуального компонента расширяющего возможности базового компонента текста модуля компонентов Unity UI.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор визуального компонента расширяющего возможности базового компонента текста модуля компонентов Unity UI
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIText), true)]
public class LotusUITextEditor : UnityEditor.UI.TextEditor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static GUIContent mContentOptimalSize = new GUIContent("Optimal Size");
	protected static GUIContent mContentPreferredSize = new GUIContent("Preferred Size");
	protected static GUIContent mContentGenerate = new GUIContent("Generate", "Generate from hachcode string");
	protected static GUIContent mContentDisable = new GUIContent("Disable", "Disable from localization");
	protected static GUIContent mContentRemoveEffect = new GUIContent("X", "Remove this effect");
	protected static GUIContent mContentAddEffect = new GUIContent("Add effect for");
	#endregion

	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание компонента Text с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPathFunc + "Create Text", false, XElementUIEditorSettings.MenuOrderFunc + 2)]
	public static void CreateText()
	{
		LotusUIText text = LotusUIText.CreateText("New Text");
		Undo.RegisterCreatedObjectUndo(text.gameObject, "Text");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIText mUIText;
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
		mUIText = this.target as LotusUIText;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Отключение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void OnDisable()
	{
		base.OnDisable();
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
			GUILayout.Space(4.0f);
			mUIText.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location", mUIText.mExpandedSize);
			if (mUIText.mExpandedSize)
			{
				LotusUIPlaceable2DEditor.DrawPlaceable(mUIText, 1500, 1000);

				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				{
					if (GUILayout.Button(mContentOptimalSize, EditorStyles.miniButtonLeft))
					{
						mUIText.SetOptimalSize();
					}
					if (GUILayout.Button(mContentPreferredSize, EditorStyles.miniButtonRight))
					{
						mUIText.SetPreferredSize();
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			GUILayout.Space(4.0f);
			mUIText.mExpandedParam = XEditorInspector.DrawGroupFoldout("Main settings", mUIText.mExpandedParam);
			if (mUIText.mExpandedParam)
			{
				base.OnInspectorGUI();
			}

			DrawOtherText(mUIText);
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
	/// Рисование дополнительных параметров компонента тектса
	/// </summary>
	/// <param name="ui_text">Компонент текста</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawOtherText(LotusUIText ui_text)
	{
		GUILayout.Space(4.0f);
		ui_text.mExpandedOther = XEditorInspector.DrawGroupFoldout("Other settings", ui_text.mExpandedOther);
		if (ui_text.mExpandedOther)
		{
			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup("Use Localize");
			{
				EditorGUI.indentLevel++;

				GUILayout.Space(2.0f);
				ui_text.mIDKeyLocalize = XEditorInspector.PropertyInt("ID Key", ui_text.mIDKeyLocalize);

				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Space(XInspectorViewParams.OFFSET_INDENT);
					if (GUILayout.Button(mContentGenerate, EditorStyles.miniButtonLeft))
					{
						ui_text.mIDKeyLocalize = ui_text.text.GetHashCode();
					}
					if (GUILayout.Button(mContentDisable, EditorStyles.miniButtonRight))
					{
						ui_text.mIDKeyLocalize = -1;
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUI.indentLevel--;
			}

			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup("Additionally");
			{
				EditorGUI.indentLevel++;

				GUILayout.Space(2.0f);
				EditorGUI.BeginChangeCheck();
				{
					ui_text.mUseShadow = XEditorInspector.PropertyBoolean("UseShadow", ui_text.mUseShadow);
					if (EditorGUI.EndChangeCheck())
					{
						ui_text.AutoComponent<Shadow>(ui_text.mUseShadow);
					}
				}

				GUILayout.Space(2.0f);
				EditorGUI.BeginChangeCheck();
				{
					ui_text.mUseOutline = XEditorInspector.PropertyBoolean("UseOutline", ui_text.mUseOutline);
					if (EditorGUI.EndChangeCheck())
					{
						ui_text.AutoComponent<Outline>(ui_text.mUseOutline);
					}
				}
				EditorGUI.indentLevel--;
			}
		}
	}
	#endregion
}
//=====================================================================================================================