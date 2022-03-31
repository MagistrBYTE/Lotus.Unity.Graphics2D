//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseIndicationDrawer.cs
*		Редакторы для рисования параметров элементов индикации.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ProgressBar
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIProgressBar))]
public class LotusProgressBarDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountProgressBarProperties = 6;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentOrientation = new GUIContent("Orientation");
	protected static GUIContent mContentPercent = new GUIContent("Percent");
	protected static GUIContent mContentPaddingFill = new GUIContent("PaddingFill");
	protected static GUIContent mContentIsTextureCoord = new GUIContent("IsTextureCoord");
	protected static GUIContent mContentFill = new GUIContent("TextureFill");
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
			return (GetPropertyHeightProgress() + EditorGUIUtility.standardVerticalSpacing);
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

		// Отображаем свойства индикатора прогресса
		OnDrawProgressParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров индикатора прогресса
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawProgressParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIProgressBar progress = property.GetValue<CGUIProgressBar>();
		if (progress != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Progress settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.Orientation = (TOrientation)EditorGUI.EnumPopup(position, mContentOrientation, progress.Orientation);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.Percent = EditorGUI.Slider(position, mContentPercent, progress.Percent, 0, 1);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.mPaddingFill = EditorGUI.Vector4Field(position, mContentPaddingFill, progress.mPaddingFill);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.TextureFill = EditorGUI.ObjectField(position, mContentFill, progress.TextureFill, typeof(Texture2D), false) as Texture2D;

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.IsTextureCoord = EditorGUI.Toggle(position, mContentIsTextureCoord, progress.IsTextureCoord);

			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств индикатора прогресса
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightProgress()
	{
		Single base_height = GetPropertyHeightElement();
		Single progress_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountProgressBarProperties;

		return (base_height + progress_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ProgressBarValue
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIProgressBarValue))]
public class LotusProgressBarValueDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountProgressBarProperties = 15;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentOrientation = new GUIContent("Orientation");
	protected static GUIContent mContentNumberValue = new GUIContent("NumberValue");
	protected static GUIContent mContentMaxValue = new GUIContent("MaxValue");
	protected static GUIContent mContentMinValue = new GUIContent("MinValue");
	protected static GUIContent mContentFormatValue = new GUIContent("FormatValue");
	protected static GUIContent mContentSuffixValue = new GUIContent("SuffixValue");
	protected static GUIContent mContentPercent = new GUIContent("Percent");
	protected static GUIContent mContentPaddingFill = new GUIContent("PaddingFill");
	protected static GUIContent mContentIsTextureCoord = new GUIContent("IsTextureCoord");
	protected static GUIContent mContentFill = new GUIContent("TextureFill");
	protected static GUIContent mContentIsInfoValue = new GUIContent("IsInfoValue");
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
			return (GetPropertyHeightProgress() + XInspectorViewParams.SPACE);
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

		// Отображаем свойства индикатора прогресса
		OnDrawProgressParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров индикатора прогресса
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawProgressParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIProgressBarValue progress = property.GetValue<CGUIProgressBarValue>();
		if (progress != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Progress settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.Orientation = (TOrientation)EditorGUI.EnumPopup(position, mContentOrientation, progress.Orientation);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.NumberValue = EditorGUI.Slider(position, mContentNumberValue, progress.NumberValue,
					progress.MinValue, progress.MaxValue);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.MinValue = EditorGUI.FloatField(position, mContentMinValue, progress.MinValue);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.MaxValue = EditorGUI.FloatField(position, mContentMaxValue, progress.MaxValue);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.FormatValue = EditorGUI.TextField(position, mContentFormatValue, progress.FormatValue);

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.SuffixValue = EditorGUI.TextField(position, mContentSuffixValue, progress.SuffixValue);

				// 8)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.mPaddingFill = EditorGUI.Vector4Field(position, mContentPaddingFill, progress.mPaddingFill);

				// 9)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.TextureFill = EditorGUI.ObjectField(position, mContentFill, progress.TextureFill, typeof(Texture2D), false) as Texture2D;

				// 10)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.IsTextureCoord = EditorGUI.Toggle(position, mContentIsTextureCoord, progress.IsTextureCoord);

				// 11)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Value settings", EditorStyles.boldLabel);

				// 12)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.IsInfoValue = EditorGUI.Toggle(position, mContentIsInfoValue, progress.IsInfoValue);

				// 13)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(progress.StyleValueName);
				mSelectedValueStyleIndex = EditorGUI.Popup(position, mContentValueStyle.text, index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedValueStyleIndex)
				{
					progress.StyleValueName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedValueStyleIndex);
				}

				// 14)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.ValueSize = EditorGUI.FloatField(position, mContentValueSize, progress.ValueSize);

				// 15)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				progress.ValueLocation = (TProgressBarValueLocation)EditorGUI.EnumPopup(position, mContentValueLocation, progress.ValueLocation);

			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств индикатора прогресса
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightProgress()
	{
		Single base_height = GetPropertyHeightElement();
		Single progress_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountProgressBarProperties;

		return (base_height + progress_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента Rating
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIRating))]
public class LotusRatingPropertyDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountRatingProperties = 1;
	#endregion

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
			return (GetPropertyHeightRating() + XInspectorViewParams.SPACE);
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

		// Отображаем свойства рейтинга
		OnDrawRatingParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров рейтинга
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawRatingParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIRating rating = property.GetValue<CGUIRating>();
		if (rating != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Rating settings", EditorStyles.boldLabel);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.serializedObject.ApplyModifiedProperties();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств рейтинга
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightRating()
	{
		Single base_height = GetPropertyHeightElement();
		Single rating_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountRatingProperties;

		return (base_height + rating_height);
	}
	#endregion
}
//=====================================================================================================================