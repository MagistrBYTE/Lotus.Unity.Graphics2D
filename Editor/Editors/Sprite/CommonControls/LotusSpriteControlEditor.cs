//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteControlEditor.cs
*		Редактор компонента элемента Control.
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
/// Редактор компонента элемента Control
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusSpriteControl))]
public class LotusSpriteControlEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента Control с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XSpriteEditorSettings.MenuPathCommon + "Create Control", false, XSpriteEditorSettings.MenuOrderCommon + 1)]
	public static void Create()
	{
		LotusSpriteControl control = LotusSpriteControl.Create(0, 0, 50, 50, Selection.activeTransform);
		Undo.RegisterCreatedObjectUndo(control.gameObject, "Control");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusSpriteControl mControl;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mControl = this.target as LotusSpriteControl;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Параметры базового элемента (местоположение и интерактивность)
		mControl.mExpandedSize = XEditorInspector.DrawGroupFoldout("Parameters and size", mControl.mExpandedSize);
		if (mControl.mExpandedSize)
		{
			LotusSpriteElementEditor.DrawElementParam(mControl);
		}

		EditorGUI.BeginChangeCheck();
		{
			// Основные параметры элемента
			GUILayout.Space(4.0f);
			mControl.mExpandedParam = XEditorInspector.DrawGroupFoldout("Settings control", mControl.mExpandedParam);
			if (mControl.mExpandedParam)
			{
				DrawControlParams(mControl);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mControl.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование основных параметров базового управляющего элемента
	/// </summary>
	/// <param name="control">Элемент</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawControlParams(LotusSpriteControl control)
	{
		GUILayout.Space(4.0f);
		control.InteractiveMode = (TInteractiveMode)XEditorInspector.PropertyEnum(nameof(control.InteractiveMode), control.InteractiveMode);

		switch (control.InteractiveMode)
		{
			case TInteractiveMode.None:
				break;
			case TInteractiveMode.Button:
				{
				}
				break;
			case TInteractiveMode.Toogle:
				{
					GUILayout.Space(4.0f);
					control.IsVisualActive = XEditorInspector.PropertyBoolean("IsActived", control.IsVisualActive);
				}
				break;
			default:
				break;
		}

		GUILayout.Space(4.0f);
		control.InteractiveSource = (TInteractiveSource)XEditorInspector.PropertyEnum(nameof(control.InteractiveSource), control.InteractiveSource);

		GUILayout.Space(4.0f);
		control.EffectDuration = XEditorInspector.PropertyFloatSlider(nameof(control.EffectDuration), control.EffectDuration, 0.01f, 3.0f);
	}
	#endregion
}
//=====================================================================================================================