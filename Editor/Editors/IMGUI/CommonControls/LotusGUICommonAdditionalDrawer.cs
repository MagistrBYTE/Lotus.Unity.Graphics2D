//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса пользователя
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonAdditionalDrawer.cs
*		едакторы для рисования параметров дополнительных управляющих элементов.
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
/// Редактор для рисования элемента SliderValue
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUISliderValue))]
public class LotusSliderValueDrawer : LotusLabelDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountSliderValueProperties = 10;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentNumberValue = new GUIContent("NumberValue");
	protected static GUIContent mContentMaxValue = new GUIContent("MaxValue");
	protected static GUIContent mContentMinValue = new GUIContent("MinValue");
	protected static GUIContent mContentFormatValue = new GUIContent("FormatValue");
	protected static GUIContent mContentSuffixValue = new GUIContent("SuffixValue");
	protected static GUIContent mContentValueStyle = new GUIContent("Style");
	protected static GUIContent mContentValueSize = new GUIContent("Size");
	protected static GUIContent mContentValueLocation = new GUIContent("Location");
	protected Int32 mSelectedValueStyleIndex;
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
			return (GetPropertyHeightSlider() + EditorGUIUtility.standardVerticalSpacing);
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
		OnDrawSliderValueParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров ползунка
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawSliderValueParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUISliderValue slider = property.GetValue<CGUISliderValue>();
		if (slider != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "SliderValue settings", EditorStyles.boldLabel);

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

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				slider.SuffixValue = EditorGUI.TextField(position, mContentSuffixValue, slider.SuffixValue);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				slider.FormatValue = EditorGUI.TextField(position, mContentFormatValue, slider.FormatValue);

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Value settings", EditorStyles.boldLabel);

				// 8)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(slider.StyleValueName);
				mSelectedValueStyleIndex = EditorGUI.Popup(position, mContentValueStyle.text, index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedValueStyleIndex)
				{
					slider.StyleValueName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedValueStyleIndex);
				}

				// 9)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				slider.ValueSize = EditorGUI.FloatField(position, mContentValueSize, slider.ValueSize);

				// 10)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				slider.ValueLocation = (TValueLocation)EditorGUI.EnumPopup(position, mContentValueLocation,
					slider.ValueLocation);
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
		return (GetPropertyHeightElement() + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountSliderValueProperties));
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента Joystick
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIJoystick))]
public class LotusJoystickDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountJoystickProperties = 9;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentImageHandle = new GUIContent("ImageHandle");
	protected static GUIContent mContentHandleSize = new GUIContent("HandleSize");
	protected static GUIContent mContentMaxOffsetX = new GUIContent("MaxOffsetX");
	protected static GUIContent mContentMaxOffsetY = new GUIContent("MaxOffsetY");
	protected static GUIContent mContentIsInverseY = new GUIContent("IsInverseY");
	protected static GUIContent mContentIsNormalize = new GUIContent("IsNormalize");
	protected static GUIContent mContentSpring = new GUIContent("Spring");
	protected static GUIContent mContentDeadZone = new GUIContent("DeadZone");
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
			return (GetPropertyHeightJoystick() + EditorGUIUtility.standardVerticalSpacing);
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

		// Отображаем свойства джойстика
		OnDrawJoystickParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров джойстика
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawJoystickParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIJoystick joystick = property.GetValue<CGUIJoystick>();
		if (joystick != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Joystick settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				joystick.ImageHandle = EditorGUI.ObjectField(position, mContentImageHandle, joystick.ImageHandle,
					typeof(Texture2D), false) as Texture2D;

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				joystick.HandleSize = EditorGUI.Vector2Field(position, mContentHandleSize, joystick.HandleSize);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				joystick.MaxOffsetX = EditorGUI.FloatField(position, mContentMaxOffsetX, joystick.MaxOffsetX);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				joystick.MaxOffsetY = EditorGUI.FloatField(position, mContentMaxOffsetY, joystick.MaxOffsetY);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				joystick.IsInverseY = EditorGUI.Toggle(position, mContentIsInverseY, joystick.IsInverseY);

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				joystick.IsNormalize = EditorGUI.Toggle(position, mContentIsNormalize, joystick.IsNormalize);

				// 8)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				joystick.Spring = EditorGUI.FloatField(position, mContentSpring, joystick.Spring);

				// 9)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				joystick.DeadZone = EditorGUI.FloatField(position, mContentDeadZone, joystick.DeadZone);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств джойстика
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightJoystick()
	{
		return (GetPropertyHeightElement() + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountJoystickProperties));
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ScrollViewSnap
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIScrollViewSnap))]
public class LotusScrollViewSnapDrawer : LotusScrollViewDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountScrollViewSnapProperties = 2;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentCountFixed = new GUIContent("CountFixed");
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
			return (base.GetPropertyHeight(property, label) + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountScrollViewSnapProperties));
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

		// Отображаем свойства области c привязкой
		OnDrawScrollViewSnapParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров области просмотра c привязкой
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawScrollViewSnapParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIScrollViewSnap scroll_view = property.GetValue<CGUIScrollViewSnap>();
		if (scroll_view != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Snap settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				scroll_view.CountFixedOffset = EditorGUI.IntField(position, mContentCountFixed, scroll_view.CountFixedOffset);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств области просмотра с привязкой
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightScrollViewSnap()
	{
		return (GetPropertyHeightScrollView() + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountScrollViewSnapProperties));
	}
	#endregion
}
//=====================================================================================================================