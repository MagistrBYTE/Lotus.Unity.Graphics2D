//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseHeaderDrawer.cs
*		Редакторы для рисования параметров заголовочных элементов.
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
/// Редактор для рисования элемента BaseHeader
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIBaseHeader))]
public class LotusBaseHeaderDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountBaseHeaderProperties = 5;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentHeaderText = new GUIContent("Text");
	protected static GUIContent mContentHeaderIcon = new GUIContent("Icon");
	protected static GUIContent mContentHeaderStyle = new GUIContent("Style");
	protected static GUIContent mContentHeaderSize = new GUIContent("Size");
	protected static GUIContent mContentHeaderLocation = new GUIContent("Location");
	protected Int32 mSelectedHeaderStyleIndex;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (property.isExpanded)
		{
			return (GetPropertyHeightBaseHeader(property) + XInspectorViewParams.SPACE);
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
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров базового заголовочного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawBaseHeaderParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIBaseHeader element_header = property.GetValue<CGUIBaseHeader>();
		if (element_header != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Header settings", EditorStyles.boldLabel);

				// 2) - 2/1
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				SerializedProperty field_localize = property.FindPropertyRelative(nameof(CGUIBaseHeader.mHeaderText));
				position.height = EditorGUI.GetPropertyHeight(field_localize);
				EditorGUI.PropertyField(position, field_localize, mContentHeaderText);

				// 3)
				position.y += position.height;
				position.height = XInspectorViewParams.CONTROL_HEIGHT;
				element_header.HeaderIcon = EditorGUI.ObjectField(position, mContentHeaderIcon, element_header.HeaderIcon,
					typeof(Texture2D), false) as Texture2D;

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(element_header.StyleHeaderName);
				mSelectedHeaderStyleIndex = EditorGUI.Popup(position, mContentHeaderStyle.text, index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedHeaderStyleIndex)
				{
					element_header.StyleHeaderName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedHeaderStyleIndex);
				}

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element_header.HeaderSize = EditorGUI.FloatField(position, mContentHeaderSize, element_header.HeaderSize);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element_header.HeaderLocation = (THeaderLocation)EditorGUI.EnumPopup(position, mContentHeaderLocation,
					element_header.HeaderLocation);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}


	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств базового заголовочного элемента
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightBaseHeader(SerializedProperty property)
	{
		Single base_height = GetPropertyHeightElement();
		Single local_height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(CGUIBaseHeader.mHeaderText)));
		Single base_header_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * (CountBaseHeaderProperties);

		return (base_height + local_height + base_header_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента PanelHeader
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIPanelHeader))]
public class LotusPanelHeaderDrawer : LotusBaseHeaderDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountHeaderBoxProperties = 2;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentText = new GUIContent("ContentText");
	protected static GUIContent mContentContentStyle = new GUIContent("ContentStyle");
	protected Int32 mSelectedContentStyleIndex;
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
			return (GetPropertyHeightHeaderBox(property) + XInspectorViewParams.SPACE);
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
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров заголовочного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawPanelHeaderParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIPanelHeader header_box = property.GetValue<CGUIPanelHeader>();
		if (header_box != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Content settings", EditorStyles.boldLabel);

				// 2) -2/1
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				SerializedProperty field_localize = property.FindPropertyRelative(nameof(CGUIPanelHeader.mContentText));
				position.height = EditorGUI.GetPropertyHeight(field_localize);
				EditorGUI.PropertyField(position, field_localize, mContentText);


				// 3)
				position.y += (position.height);
				position.height = XInspectorViewParams.CONTROL_HEIGHT;
				Int32 index = LotusGUIDispatcher.GetStyleIndex(header_box.StyleContentName);
				mSelectedContentStyleIndex = EditorGUI.Popup(position, mContentContentStyle.text, index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedContentStyleIndex)
				{
					header_box.StyleContentName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedContentStyleIndex);
				}
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств заголовочного элемента
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightHeaderBox(SerializedProperty property)
	{
		Single base_height = GetPropertyHeightBaseHeader(property);
		Single local_height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(CGUIPanelHeader.mContentText)));
		Single header_box_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountHeaderBoxProperties;
		return (base_height + local_height + header_box_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента PanelSpoiler
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIPanelSpoiler))]
public class LotusPanelSpoilerDrawer : LotusBaseHeaderDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountPanelSpoilerProperties = 4;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentIsOpened = new GUIContent("IsOpened");
	protected static GUIContent mContentDuration = new GUIContent("Duration");
	protected static GUIContent mContentSizeView = new GUIContent("SizeView");
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
			return (GetPropertyHeightPanelSpoiler(property) + XInspectorViewParams.SPACE);
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

		// Отображаем свойства сворачиваемой панели
		OnDrawPanelSpoilerParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров сворачиваемой панели
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawPanelSpoilerParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIPanelSpoiler panel_spoiler = property.GetValue<CGUIPanelSpoiler>();
		if (panel_spoiler != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Spoler settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				panel_spoiler.IsOpened = EditorGUI.Toggle(position, mContentIsOpened, panel_spoiler.IsOpened);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				panel_spoiler.Duration = EditorGUI.FloatField(position, mContentDuration, panel_spoiler.Duration);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				panel_spoiler.SizeView = EditorGUI.FloatField(position, mContentSizeView, panel_spoiler.SizeView);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств сворачиваемой панели
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightPanelSpoiler(SerializedProperty property)
	{
		Single base_height = GetPropertyHeightBaseHeader(property);
		Single panel_spoiler = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountPanelSpoilerProperties;
		return (base_height + panel_spoiler);
	}
	#endregion
}
//=====================================================================================================================