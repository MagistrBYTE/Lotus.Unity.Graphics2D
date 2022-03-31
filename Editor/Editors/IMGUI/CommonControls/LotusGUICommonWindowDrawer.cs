//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса пользователя
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonWindowDrawer.cs
*		Редакторы для рисования параметров окна.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента BaseWindow
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIBaseWindow))]
public class LotusBaseWindowDrawer : LotusPanelHeaderDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountBaseWindowProperties = 4;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentIsModal = new GUIContent("IsModal");
	protected static GUIContent mContentIsDragg = new GUIContent("IsDraggable");
	protected static GUIContent mContentIsDragHeader = new GUIContent("IsDragHeader");
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="element_header">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent element_header)
	{
		if (property.isExpanded)
		{
			return (GetPropertyHeightBaseWindow(property) + XInspectorViewParams.SPACE);
		}
		else
		{
			return (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование данных конкретного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnDrawParamemtrs(ref Rect position, SerializedProperty property)
	{
		// Отображаем базовые свойства
		OnDrawElementParamemtrs(ref position, property);

		// Отображаем свойства базового заголовочного элемента
		OnDrawBaseHeaderParamemtrs(ref position, property);

		// Отображаем свойства заголовочного элемента
		OnDrawPanelHeaderParamemtrs(ref position, property);

		// Отображаем свойства базового окна
		OnDrawBaseWindowParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров базового окна
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawBaseWindowParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIBaseWindow base_window = property.GetValue<CGUIBaseWindow>();
		if (base_window != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Window settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				base_window.IsDraggable = EditorGUI.Toggle(position, mContentIsDragg, base_window.IsDraggable);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				base_window.IsDragHeader = EditorGUI.Toggle(position, mContentIsDragHeader, base_window.IsDragHeader);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				base_window.IsModalState = EditorGUI.Toggle(position, mContentIsModal, base_window.IsModalState);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств базового окна
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightBaseWindow(SerializedProperty property)
	{
		Single base_height = GetPropertyHeightHeaderBox(property);
		Single base_window = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountBaseWindowProperties;
		return (base_height + base_window);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента DialogWindow
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIDialogWindow))]
public class LotusDialogWindowDrawer : LotusBaseWindowDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountDialogWindowProperties = 5;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentOKText = new GUIContent("Text OK");
	protected static GUIContent mContentOKIcon = new GUIContent("Icon OK");
	protected static GUIContent mContentCancelText = new GUIContent("Text Cancel");
	protected static GUIContent mContentCancelIcon = new GUIContent("Icon Cancel");
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="element_header">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent element_header)
	{
		if (property.isExpanded)
		{
			return (GetPropertyHeightDialogWindow(property) + XInspectorViewParams.SPACE);
		}
		else
		{
			return (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование данных конкретного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnDrawParamemtrs(ref Rect position, SerializedProperty property)
	{
		// Отображаем базовые свойства
		OnDrawElementParamemtrs(ref position, property);

		// Отображаем свойства базового заголовочного элемента
		OnDrawBaseHeaderParamemtrs(ref position, property);

		// Отображаем свойства заголовочного элемента
		OnDrawPanelHeaderParamemtrs(ref position, property);

		// Отображаем свойства базового окна
		OnDrawBaseWindowParamemtrs(ref position, property);

		// Отображаем свойства диалогового окна
		OnDrawDialogWindowParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров диалогового окна
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawDialogWindowParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIDialogWindow dialog_window = property.GetValue<CGUIDialogWindow>();
		if (dialog_window != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Dialog settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dialog_window.OkText = EditorGUI.TextField(position, mContentOKText, dialog_window.OkText);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dialog_window.OkIcon = EditorGUI.ObjectField(position, mContentOKIcon, dialog_window.OkIcon,
					typeof(Texture2D), false) as Texture2D;

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dialog_window.CancelText = EditorGUI.TextField(position, mContentCancelText, dialog_window.CancelText);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dialog_window.CancelIcon = EditorGUI.ObjectField(position, mContentCancelIcon, dialog_window.CancelIcon,
					typeof(Texture2D), false) as Texture2D;
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств диалогового окна
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightDialogWindow(SerializedProperty property)
	{
		Single base_window = GetPropertyHeightBaseWindow(property);
		Single dialog_window = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountDialogWindowProperties;
		return (base_window + dialog_window);
	}
	#endregion
}
//=====================================================================================================================