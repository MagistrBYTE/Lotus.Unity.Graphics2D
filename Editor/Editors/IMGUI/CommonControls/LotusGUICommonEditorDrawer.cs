//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса пользователя
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonEditorDrawer.cs
*		Редакторы для рисования параметров элементов для редактирования и ввода данных.
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
/// Редактор для рисования элемента TextField
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUITextField))]
public class LotusTextFieldDrawer : LotusContentDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountTextFieldProperties = 4;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentIsReadOnly = new GUIContent("IsReadOnly");
	protected static GUIContent mContentMaxLength = new GUIContent("MaxLength");
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
			return (GetPropertyHeightTextField() + EditorGUIUtility.standardVerticalSpacing);
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

		// Отображаем свойства текстового поля
		OnDrawTextFieldParamemtrs(ref position, property);

		// Кнопка оптимального размера
		DrawButtonOptimalSize(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров текстового поля
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawTextFieldParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUITextField text_field = property.GetValue<CGUITextField>();
		if (text_field != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Text settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				text_field.IsReadOnly = EditorGUI.Toggle(position, mContentIsReadOnly, text_field.IsReadOnly);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				text_field.MaxLength = EditorGUI.IntField(position, mContentMaxLength, text_field.MaxLength);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				text_field.Text = EditorGUI.TextField(position, mContentText, text_field.Text);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств текстового поля
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightTextField()
	{
		Single base_height = GetPropertyHeightElement();
		Single text_field_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountTextFieldProperties;

		return (base_height + text_field_height + XInspectorViewParams.BUTTON_MINI_HEIGHT);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента Spinner
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUISpinner))]
public class LotusSpinnerDrawer : LotusTextFieldDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountSpinnerProperties = 10;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentNumberValue = new GUIContent("NumberValue");
	protected static GUIContent mContentMaxValue = new GUIContent("MaxValue");
	protected static GUIContent mContentMinValue = new GUIContent("MinValue");
	protected static GUIContent mContentStepValue = new GUIContent("StepValue");
	protected static GUIContent mContentFormatValue = new GUIContent("FormatValue");
	protected static GUIContent mContentSuffixValue = new GUIContent("SuffixValue");
	protected static GUIContent mContentIsUserInput = new GUIContent("IsUserInput");
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
			return (GetPropertyHeightSpinner() + EditorGUIUtility.standardVerticalSpacing);
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

		// Отображаем свойства текстового поля
		OnDrawTextFieldParamemtrs(ref position, property);

		// Отображаем свойства счетчика
		OnDrawSpinnerParamemtrs(ref position, property);

		// Кнопка оптимального размера
		DrawButtonOptimalSize(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров счетчика
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawSpinnerParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUISpinner spinner = property.GetValue<CGUISpinner>();
		if (spinner != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Spinner settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.NumberValue = EditorGUI.Slider(position, mContentNumberValue, spinner.NumberValue,
					spinner.MinValue, spinner.MaxValue);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.MinValue = EditorGUI.FloatField(position, mContentMinValue, spinner.MinValue);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.MaxValue = EditorGUI.FloatField(position, mContentMaxValue, spinner.MaxValue);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.StepValue = EditorGUI.FloatField(position, mContentStepValue, spinner.StepValue);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.FormatValue = EditorGUI.TextField(position, mContentFormatValue, spinner.FormatValue);

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.SuffixValue = EditorGUI.TextField(position, mContentSuffixValue, spinner.SuffixValue);

				// 8)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.IsUserInput = EditorGUI.Toggle(position, mContentIsUserInput, spinner.IsUserInput);

				// 9)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				spinner.ButtonSize = EditorGUI.FloatField(position, mContentButtonSize, spinner.ButtonSize);

				// 10)
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

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств счетчика
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightSpinner()
	{
		return (GetPropertyHeightTextField() + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountSpinnerProperties));
	}
	#endregion
}
//=====================================================================================================================