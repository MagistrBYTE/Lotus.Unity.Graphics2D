//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса пользователя
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonItemsContentDrawer.cs
*		Редакторы для рисования параметров элементов со списком однотипных данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ContainerContents
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIContainerContents))]
public class LotusContainerContentsDrawer : LotusElementDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentRemove = new GUIContent("X");
	protected static GUIContent mContentAdd = new GUIContent("Add");
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
			Single base_height = GetPropertyHeightElement();
			Single container_height = GetContainerHeight(property);

			return (base_height + container_height + EditorGUIUtility.standardVerticalSpacing);
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

		// Отображаем свойства базового контейнера
		OnDrawBaseContainerParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров базового контейнера
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawBaseContainerParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIContainerContents base_container = property.GetValue<CGUIContainerContents>();
		if (base_container != null)
		{
			EditorGUI.BeginChangeCheck();

			position.y += (EditorGUIUtility.singleLineHeight + 2);
			GUI.Label(position, "Items [" + base_container.ContentItems.Count.ToString() + "]", EditorStyles.boldLabel);

			for (Int32 i = 0; i < base_container.ContentItems.Count; i++)
			{
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);

				// Текст
				Rect rect_text = new Rect(position.x, position.y, position.width * 0.6f, XInspectorViewParams.CONTROL_HEIGHT);
				base_container.ContentItems[i].Text = EditorGUI.TextField(rect_text, base_container.ContentItems[i].Text);

				// Иконка
				Rect rect_icon = new Rect(rect_text.xMax, position.y, position.width * 0.4f - XInspectorViewParams.BUTTON_MINI_WIDTH, 
					XInspectorViewParams.CONTROL_HEIGHT);
				base_container.ContentItems[i].Icon = EditorGUI.ObjectField(rect_icon, base_container.ContentItems[i].Icon, typeof(Texture2D), false) as Texture2D;

				// Удалить
				Rect rect_remove = new Rect(rect_icon.xMax, position.y, XInspectorViewParams.BUTTON_MINI_WIDTH, XInspectorViewParams.CONTROL_HEIGHT);
				if (GUI.Button(rect_remove, mContentRemove, XEditorStyles.ButtonMiniDefaultRedRightStyle))
				{
					base_container.Remove(i);
					break;
				}
			}

			position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
			var rect_optimal = position;
			rect_optimal.height = XInspectorViewParams.CONTROL_HEIGHT_SPACE;
			if (GUI.Button(rect_optimal, mContentAdd))
			{
				base_container.Add("Item");
			}

			if (EditorGUI.EndChangeCheck())
			{
				base_container.ResetFromList();
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты контейнера со списком всех элементов
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота контейнера</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetContainerHeight(SerializedProperty property)
	{
		CGUIContainerContents base_container = property.GetValue<CGUIContainerContents>();
		if (base_container != null)
		{
			return ((XInspectorViewParams.CONTROL_HEIGHT_SPACE * (base_container.ContentItems.Count + 1)) + XInspectorViewParams.CONTROL_HEIGHT_SPACE);
		}

		return (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ToolbarContents
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIToolbarContents))]
public class LotusToolbarContentsDrawer : LotusContainerContentsDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
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
			return (base.GetPropertyHeight(property, label));
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

		// Отображаем свойства панели инструментов
		OnDrawBaseContainerParamemtrs(ref position, property);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента GridContents
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIGridContents))]
public class LotusGridContentsDrawer : LotusContainerContentsDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountGridContentsProperties = 3;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentCountColumn = new GUIContent("CountColumn");
	protected static GUIContent mContentIsSelectedEmpty = new GUIContent("IsSelectedEmpty");
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
			return (base.GetPropertyHeight(property, label) + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountGridContentsProperties));
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

		// Отображаем свойства сеточного элемента
		OnDrawGridParamemtrs(ref position, property);

		// Отображаем свойства базового контейнера
		OnDrawBaseContainerParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров сеточного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawGridParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIGridContents grid = property.GetValue<CGUIGridContents>();
		if (grid != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Grid settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				grid.CountColumn = EditorGUI.IntField(position, mContentCountColumn, grid.CountColumn);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				grid.IsSelectedEmpty = EditorGUI.Toggle(position, mContentIsSelectedEmpty, grid.IsSelectedEmpty);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента SpinnerContents
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUISpinnerContents))]
public class LotusSpinnerContentsDrawer : LotusContainerContentsDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountSpinnerContentsProperties = 3;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentButtonSize = new GUIContent("ButtonSize");
	protected static GUIContent mContentButtonLocation = new GUIContent("ButtonLocation");
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
			return (base.GetPropertyHeight(property, label) + XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountSpinnerContentsProperties);
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

		// Отображаем свойства счетчика контейнера
		OnDrawSpinnerContainerParamemtrs(ref position, property);

		// Отображаем свойства базового контейнера
		OnDrawBaseContainerParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров счетчика контейнера
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawSpinnerContainerParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUISpinnerContents spinner = property.GetValue<CGUISpinnerContents>();
		if (spinner != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Spinner settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.ButtonSize = EditorGUI.FloatField(position, mContentButtonSize, spinner.ButtonSize);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.ButtonLocation = (TSpinnerButtonLocation)EditorGUI.EnumPopup(position,
					mContentButtonLocation, spinner.ButtonLocation);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ContextMenuContents
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIContextMenuContents))]
public class LotusContextMenuContentsDrawer : LotusContainerContentsDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountContextMenuContentsProperties = 9;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentCountColumn = new GUIContent("CountColumn");
	protected static GUIContent mContentWidthItem = new GUIContent("WidthItem");
	protected static GUIContent mContentHeightItem = new GUIContent("HeightItem");
	protected static GUIContent mContentCountVisibleX = new GUIContent("CountVisibleX");
	protected static GUIContent mContentCountVisibleY = new GUIContent("CountVisibleY");
	protected static GUIContent mContentDuration = new GUIContent("Duration");
	protected static GUIContent mContentOpened = new GUIContent("IsOpened");
	protected static GUIContent mContentOpenLocation = new GUIContent("OpenLocation");
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
			return (base.GetPropertyHeight(property, label) + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountContextMenuContentsProperties));
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

		// Отображаем свойства контекстного меню
		OnDrawContextMenuParamemtrs(ref position, property);

		// Отображаем свойства базового контейнера
		OnDrawBaseContainerParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров контекстного меню
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawContextMenuParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIContextMenuContents context_menu = property.GetValue<CGUIContextMenuContents>();
		if (context_menu != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Expanded settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				context_menu.CountColumn = EditorGUI.IntField(position, mContentCountColumn, context_menu.CountColumn);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				context_menu.WidthItem = EditorGUI.FloatField(position, mContentWidthItem, context_menu.WidthItem);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				context_menu.HeightItem = EditorGUI.FloatField(position, mContentHeightItem, context_menu.HeightItem);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				context_menu.CountVisibleItemX = EditorGUI.IntField(position, mContentCountVisibleX, context_menu.CountVisibleItemX);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				context_menu.CountVisibleItemY = EditorGUI.IntField(position, mContentCountVisibleY, context_menu.CountVisibleItemY);

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				context_menu.Duration = EditorGUI.FloatField(position, mContentDuration, context_menu.Duration);

				// 8)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				context_menu.IsOpened = EditorGUI.Toggle(position, mContentOpened, context_menu.IsOpened);

				// 9)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				context_menu.OpenLocation = (TContextOpenLocation)EditorGUI.EnumPopup(position, mContentOpenLocation, context_menu.OpenLocation);

			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента DropDownContents
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIDropDownContents))]
public class LotusDropDownContentsDrawer : LotusContextMenuContentsDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountDropDownContentsProperties = 4;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentText = new GUIContent("Text");
	protected static GUIContent mContentIcon = new GUIContent("Icon");
	protected static GUIContent mContentButtonStyle = new GUIContent("ButtonStyle");
	protected Int32 mSelectedButtonStyleIndex;
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
			return (base.GetPropertyHeight(property, label) + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountDropDownContentsProperties));
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

		// Отображаем свойства контекстного меню
		OnDrawContextMenuParamemtrs(ref position, property);

		// Отображаем свойства выпадающего списка
		OnDrawDropDownParamemtrs(ref position, property);

		// Отображаем свойства базового контейнера
		OnDrawBaseContainerParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров выпадающего списка
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawDropDownParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIDropDownContents drop_down = property.GetValue<CGUIDropDownContents>();
		if (drop_down != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "DropDown settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				drop_down.CaptionText = EditorGUI.TextField(position, mContentText, drop_down.CaptionText);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				drop_down.CaptionIcon = EditorGUI.ObjectField(position, mContentIcon, drop_down.CaptionIcon,
					typeof(Texture2D), false) as Texture2D;

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(drop_down.StyleButtonName);
				mSelectedButtonStyleIndex = EditorGUI.Popup(position, mContentButtonStyle.text, index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedButtonStyleIndex)
				{
					drop_down.StyleButtonName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedButtonStyleIndex);
				}
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}
	#endregion
}
//=====================================================================================================================