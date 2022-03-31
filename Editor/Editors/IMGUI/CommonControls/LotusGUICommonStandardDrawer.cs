//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса пользователя
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonStandardDrawer.cs
*		Редакторы для рисования параметров стандартных управляющих элементов.
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
/// Редактор для рисования элемента Button
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIButton))]
public class LotusButtonDrawer : LotusLabelDrawer
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
		return (base.GetPropertyHeight(property, label));
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ToogleButton
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIToogleButton))]
public class LotusToogleButtonDrawer : LotusLabelDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountToogleButtonProperties = 6;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentIsSelected = new GUIContent("IsSelected");
	protected static GUIContent mContentOrientation = new GUIContent("Orientation");
	protected static GUIContent mContentOffText = new GUIContent("OffText");
	protected static GUIContent mContentButtonStyle = new GUIContent("ButtonStyle");
	protected static GUIContent mContentButtonSize = new GUIContent("ButtonSize");
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
			Single base_height = GetPropertyHeightElement();
			Single local_height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(CGUILabel.mCaptionText)));
			Single label_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountLabelProperties;
			Single toogle_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountToogleButtonProperties;
			return (base_height + local_height + label_height + toogle_height +
				XInspectorViewParams.BUTTON_MINI_HEIGHT + XInspectorViewParams.SPACE);
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

		// Отображаем свойства надписи
		OnDrawLabelParamemtrs(ref position, property);

		// Отображаем свойства переключателя
		OnDrawToogleButtonParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров переключателя
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawToogleButtonParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIToogleButton toogle = property.GetValue<CGUIToogleButton>();
		if (toogle != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "ToogleButton settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				toogle.IsSelected = EditorGUI.Toggle(position, mContentIsSelected, toogle.IsSelected);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				toogle.OffText = EditorGUI.TextField(position, mContentOffText, toogle.OffText);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				toogle.Orientation = (TOrientation)EditorGUI.EnumPopup(position, mContentOrientation, toogle.Orientation);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(toogle.StyleButtonName);
				mSelectedButtonStyleIndex = EditorGUI.Popup(position, mContentButtonStyle.text, index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedButtonStyleIndex)
				{
					toogle.StyleButtonName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedButtonStyleIndex);
				}

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				toogle.ButtonSize = EditorGUI.FloatField(position, mContentButtonSize, toogle.ButtonSize);

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
/// Редактор для рисования элемента CheckBox
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUICheckBox))]
public class LotusCheckBoxPropertyDrawer : LotusLabelDrawer
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
		return (base.GetPropertyHeight(property, label));
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента Slider
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUISlider))]
public class LotusSliderDrawer : LotusLabelDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountSliderProperties = 4;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentNumberValue = new GUIContent("NumberValue");
	protected static GUIContent mContentMaxValue = new GUIContent("MaxValue");
	protected static GUIContent mContentMinValue = new GUIContent("MinValue");
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
			return (GetPropertyHeightSlider() + XInspectorViewParams.SPACE);
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
		
		// Отображаем свойства ползунка
		OnDrawSliderParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров ползунка
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawSliderParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUISlider slider = property.GetValue<CGUISlider>();
		if (slider != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Slider settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				slider.NumberValue = EditorGUI.Slider(position, mContentNumberValue, slider.NumberValue,
					slider.MinValue, slider.MaxValue);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				slider.MinValue = EditorGUI.FloatField(position, mContentMinValue, slider.MinValue);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				slider.MaxValue = EditorGUI.FloatField(position, mContentMaxValue, slider.MaxValue);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств ползунка
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightSlider()
	{
		Single base_height = GetPropertyHeightElement();
		Single slider_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountSliderProperties;

		return (base_height + slider_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ScrollView
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIScrollView))]
public class LotusScrollViewDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountScrollViewProperties = 9;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentContentScrollDirection = new GUIContent("ScrollDirection");
	protected static GUIContent mContentContentOffsetX = new GUIContent("OffsetX");
	protected static GUIContent mContentContentOffsetY = new GUIContent("OffsetY");
	protected static GUIContent mContentContentWidth = new GUIContent("ContentWidth");
	protected static GUIContent mContentContentHeight = new GUIContent("ContentHeight");
	protected static GUIContent mContentContentIsInertia = new GUIContent("IsInertia");
	protected static GUIContent mContentContentInertiaForce = new GUIContent("InertiaForce");
	protected static GUIContent mContentContentInertiaRange = new GUIContent("InertiaRange");
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
			Single scroll_view_height = GetPropertyHeightScrollView() + EditorGUIUtility.standardVerticalSpacing;
			CGUIScrollView scroll_view = property.GetValue<CGUIScrollView>();
			if (scroll_view != null)
			{
				if (!scroll_view.IsInertia)
				{
					return (scroll_view_height - (XInspectorViewParams.CONTROL_HEIGHT_SPACE * 2));
				}
			}

			return (scroll_view_height);
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

		// Отображаем свойства области просмотра
		OnDrawScrollViewParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров области просмотра
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawScrollViewParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIScrollView scroll_view = property.GetValue<CGUIScrollView>();
		if (scroll_view != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "ScrollView settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				scroll_view.ScrollDirection = (TScrollDirection)EditorGUI.EnumPopup(position, mContentContentScrollDirection,
					scroll_view.ScrollDirection);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				scroll_view.ContentOffsetX = EditorGUI.FloatField(position, mContentContentOffsetX, scroll_view.ContentOffsetX);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				scroll_view.ContentOffsetY = EditorGUI.FloatField(position, mContentContentOffsetY, scroll_view.ContentOffsetY);

				// 5)
				EditorGUI.BeginDisabledGroup(scroll_view.ScrollDirection == TScrollDirection.Vertical);
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					scroll_view.ContentWidth = EditorGUI.FloatField(position, mContentWidth, scroll_view.ContentWidth);
				}
				EditorGUI.EndDisabledGroup();

				// 6)
				EditorGUI.BeginDisabledGroup(scroll_view.ScrollDirection == TScrollDirection.Horizontal);
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					scroll_view.ContentHeight = EditorGUI.FloatField(position, mContentHeight, scroll_view.ContentHeight);
				}
				EditorGUI.EndDisabledGroup();

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				scroll_view.IsInertia = EditorGUI.Toggle(position, mContentContentIsInertia, scroll_view.IsInertia);

				if(scroll_view.IsInertia)
				{
					// 8)
					EditorGUI.indentLevel++;
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					scroll_view.InertiaForce = EditorGUI.FloatField(position, mContentContentInertiaForce, scroll_view.InertiaForce);

					// 9)
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					scroll_view.InertiaRange = EditorGUI.FloatField(position, mContentContentInertiaRange, scroll_view.InertiaRange);
					EditorGUI.indentLevel--;
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
	/// Получение совокупной высоты свойств области просмотра
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightScrollView()
	{
		Single base_height = GetPropertyHeightElement();
		Single scroll_view_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountScrollViewProperties;

		return (base_height + scroll_view_height);
	}
	#endregion
}
//=====================================================================================================================