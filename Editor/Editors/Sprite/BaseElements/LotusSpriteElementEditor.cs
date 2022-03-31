//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteElementEditor.cs
*		Редактор компонента представляющего основной элемент интерфейса модуля спрайтов.
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
/// Редактор компонента представляющего основной элемент интерфейса модуля спрайтов
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusSpriteElement))]
public class LotusSpriteElementEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента Element с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XSpriteEditorSettings.MenuPathBase + "Create Element", false, XSpriteEditorSettings.MenuOrderBase + 2)]
	public static void CreateElement()
	{
		LotusSpriteElement element = LotusSpriteElement.Create(0, 0, 300, 600, Selection.activeTransform);
		Undo.RegisterCreatedObjectUndo(element.gameObject, "Element");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusSpriteElement mElement;
	protected static GUIContent mContentDepthUp = new GUIContent(XString.TriangleUp);
	protected static GUIContent mContentDepthDown = new GUIContent(XString.TriangleDown);
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mElement = this.target as LotusSpriteElement;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		mElement.mExpandedSize = XEditorInspector.DrawGroupFoldout("Parameters and size", mElement.mExpandedSize);
		if (mElement.mExpandedSize)
		{
			DrawElementParam(mElement);
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров компонента представляющего основной элемент интерфейса модуля спрайтов
	/// </summary>
	/// <param name="element">Базовый элемент</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawElementParam(LotusSpriteElement element)
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(4.0f);
		element.IsVisible = XEditorInspector.PropertyBoolean("IsVisible", element.IsVisible);

		GUILayout.Space(2.0f);
		element.IsEnabled = XEditorInspector.PropertyBoolean("IsEnabled", element.IsEnabled);

		GUILayout.Space(2.0f);
		element.UserTag = XEditorInspector.PropertyInt("UserTag", element.UserTag);

		LotusSpritePlaceable2DEditor.DrawBasePlaced2D(element);

		if (EditorGUI.EndChangeCheck())
		{
			element.SaveInEditor();
		}
	}
	#endregion
}
//=====================================================================================================================