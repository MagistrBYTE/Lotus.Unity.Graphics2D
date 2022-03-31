//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseGraphicsDrawer.cs
*		Редакторы для рисования параметров элементов для отображения графического содержимого.
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
/// Редактор для рисования элемента Image
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIImage))]
public class LotusImageDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountImageProperties = 4;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentSprite = new GUIContent("Sprite");
	protected static GUIContent mContentUseBack = new GUIContent("UseBackground");
	protected static GUIContent mContentScaleMode = new GUIContent("ScaleMode");
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
		// Получаем статус раскрытия параметров
		if (property.isExpanded)
		{
			return (GetPropertyHeightSprite() + XInspectorViewParams.SPACE);
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

		// Отображаем свойства изображения
		OnDrawImageParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров изображения
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawImageParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIImage image = property.GetValue<CGUIImage>();
		if (image != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Image settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				image.Image = EditorGUI.ObjectField(position, mContentSprite, image.Image, typeof(Texture2D), false) as Texture2D;

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				image.UseBackground = EditorGUI.Toggle(position, mContentUseBack, image.UseBackground);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				image.ScaleMode = (ScaleMode)EditorGUI.EnumPopup(position, mContentScaleMode, image.ScaleMode);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств изображения
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightSprite()
	{
		Single base_height = GetPropertyHeightElement();
		Single image_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountImageProperties;

		return (base_height + image_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента Sprite
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUISprite))]
public class LotusSpriteDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountSpriteProperties = 3;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentSprite = new GUIContent("Sprite");
	protected static GUIContent mContentUseBack = new GUIContent("UseBackground");
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
		// Получаем статус раскрытия параметров
		if (property.isExpanded)
		{
			return (GetPropertyHeightSprite() + XInspectorViewParams.SPACE);
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

		// Отображаем свойства спрайта
		OnDrawSpriteParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров спрайта
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawSpriteParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUISprite sprite = property.GetValue<CGUISprite>();
		if (sprite != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Sprite settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				sprite.Sprite = EditorGUI.ObjectField(position, mContentSprite, sprite.Sprite,
					typeof(Sprite), false) as Sprite;

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				sprite.UseBackground = EditorGUI.Toggle(position, mContentUseBack, sprite.UseBackground);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств спрайта
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightSprite()
	{
		Single base_height = GetPropertyHeightElement();
		Single sprite_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountSpriteProperties;

		return (base_height + sprite_height);
	}
	#endregion
}
//=====================================================================================================================