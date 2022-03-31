//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIExtendedDynamicDrawer.cs
*		Редакторы для рисования параметров динамических элементов.
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
/// Редактор для рисования элемента BaseDynamic
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIBaseDynamic))]
public class LotusBaseDynamicDrawer : LotusBaseElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountBaseDynamicProperties = 5 + 1;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentChangeParam = new GUIContent("ChangeParam");
	protected static GUIContent mContentDuration = new GUIContent("Duration");
	protected static GUIContent mContentEasing = new GUIContent("Easing");
	protected static GUIContent mContentLocationStart = new GUIContent("LocationStart");
	protected static GUIContent mContentLocationTarget = new GUIContent("LocationTarget");
	protected static GUIContent mContentSizeStart = new GUIContent("SizeStart");
	protected static GUIContent mContentSizeTarget = new GUIContent("SizeTarget");
	protected static GUIContent mContentIsUnRegister = new GUIContent("IsUnRegister");
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
			return (GetPropertyHeightBaseDynamic(property) + XInspectorViewParams.SPACE);
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
		// Отображаем свойства динамического элемента
		OnDrawBaseDynamicParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров динамического элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawBaseDynamicParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIBaseDynamic dynamic_element = property.GetValue<CGUIBaseDynamic>();
		if (dynamic_element != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Dynamic settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);

				dynamic_element.ChangeParam = (TDynamicParam)EditorGUI.EnumFlagsField(position, mContentChangeParam, dynamic_element.ChangeParam);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_element.Duration = EditorGUI.FloatField(position, mContentDuration, dynamic_element.Duration);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_element.Easing = (TEasingType)EditorGUI.EnumPopup(position, mContentEasing, dynamic_element.Easing);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_element.IsUnRegister = EditorGUI.Toggle(position, mContentIsUnRegister, dynamic_element.IsUnRegister);

				// 6)
				if (dynamic_element.ChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_element.LocationStarting = EditorGUI.Vector2Field(position, mContentLocationStart, dynamic_element.LocationStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_element.LocationTarget = EditorGUI.Vector2Field(position, mContentLocationTarget, dynamic_element.LocationTarget);
				}

				// 7)
				if (dynamic_element.ChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_element.SizeStarting = EditorGUI.Vector2Field(position, mContentSizeStart, dynamic_element.SizeStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_element.SizeTarget = EditorGUI.Vector2Field(position, mContentSizeTarget, dynamic_element.SizeTarget);
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
	/// Получение совокупной высоты свойств базового динамического элемента
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightBaseDynamic(SerializedProperty property)
	{
		Single base_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountBaseDynamicProperties;

		CGUIBaseDynamic dynamic_element = property.GetValue<CGUIBaseDynamic>();
		if (dynamic_element != null)
		{
			// 1)
			if (dynamic_element.ChangeParam.IsFlagSet(TDynamicParam.Location))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}

			// 2)
			if (dynamic_element.ChangeParam.IsFlagSet(TDynamicParam.Size))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}
		}

		return (base_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента DynamicText
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIDynamicText))]
public class LotusDynamicTextDrawer : LotusBaseDynamicDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountDynamicTextProperties = 5 + 1;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentColorStart = new GUIContent("ColorStart");
	protected static GUIContent mContentColorTarget = new GUIContent("ColorTarget");
	protected static GUIContent mContentFontSizeStart = new GUIContent("FontSizeStart");
	protected static GUIContent mContentFontSizeTarget = new GUIContent("FontSizeTarget");
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
			return (GetPropertyHeightDynamicText(property) + XInspectorViewParams.SPACE);
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
		// Отображаем свойства динамического элемента c содержимым
		OnDrawDynamicTextParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров динамического элемента c содержимым
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawDynamicTextParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIDynamicText dynamic_text = property.GetValue<CGUIDynamicText>();
		if (dynamic_text != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Dynamic settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_text.ChangeParam = (TDynamicParam)EditorGUI.EnumFlagsField(position, mContentChangeParam, dynamic_text.ChangeParam);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_text.Duration = EditorGUI.FloatField(position, mContentDuration, dynamic_text.Duration);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_text.Easing = (TEasingType)EditorGUI.EnumPopup(position, mContentEasing, dynamic_text.Easing);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_text.IsUnRegister = EditorGUI.Toggle(position, mContentIsUnRegister, dynamic_text.IsUnRegister);

				// 6)
				if (dynamic_text.ChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_text.LocationStarting = EditorGUI.Vector2Field(position, mContentLocationStart, dynamic_text.LocationStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_text.LocationTarget = EditorGUI.Vector2Field(position, mContentLocationTarget, dynamic_text.LocationTarget);

				}

				// 7)
				if (dynamic_text.ChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_text.SizeStarting = EditorGUI.Vector2Field(position, mContentSizeStart, dynamic_text.SizeStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_text.SizeTarget = EditorGUI.Vector2Field(position, mContentSizeTarget, dynamic_text.SizeTarget);
				}

				// 8)
				if (dynamic_text.ChangeParam.IsFlagSet(TDynamicParam.ColorText))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_text.ColorStarting = EditorGUI.ColorField(position, mContentColorStart, dynamic_text.ColorStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_text.ColorTarget = EditorGUI.ColorField(position, mContentColorTarget, dynamic_text.ColorTarget);
				}

				// 9)
				if (dynamic_text.ChangeParam.IsFlagSet(TDynamicParam.FontSize))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_text.FontSizeStarting = EditorGUI.IntField(position, mContentFontSizeStart, dynamic_text.FontSizeStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_text.FontSizeTarget = EditorGUI.IntField(position, mContentFontSizeTarget, dynamic_text.FontSizeTarget);
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
	/// Получение совокупной высоты свойств динамического элемента с текстом
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightDynamicText(SerializedProperty property)
	{
		Single base_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountDynamicTextProperties;

		CGUIDynamicText dynamic_text = property.GetValue<CGUIDynamicText>();
		if (dynamic_text != null)
		{
			// 1)
			if (dynamic_text.ChangeParam.IsFlagSet(TDynamicParam.Location))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}

			// 2)
			if (dynamic_text.ChangeParam.IsFlagSet(TDynamicParam.Size))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}

			// 3)
			if (dynamic_text.ChangeParam.IsFlagSet(TDynamicParam.ColorText))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}

			// 4)
			if (dynamic_text.ChangeParam.IsFlagSet(TDynamicParam.FontSize))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}
		}

		return (base_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента DynamicSprite
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIDynamicSprite))]
public class LotusDynamicSpriteDrawer : LotusBaseDynamicDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountDynamicSpriteProperties = 7 + 1;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentWidth = new GUIContent("Width");
	protected static GUIContent mContentHeight = new GUIContent("Height");
	protected static GUIContent mContentStorageIndex = new GUIContent("StorageIndex");
	protected static GUIContent mContentFrameStart = new GUIContent("FrameStart");
	protected static GUIContent mContentFrameTarget = new GUIContent("FrameTarget");
	protected static GUIContent mContentColorStart = new GUIContent("ColorStart");
	protected static GUIContent mContentColorTarget = new GUIContent("ColorTarget");
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
			return (GetPropertyHeightDynamicSprite(property) + XInspectorViewParams.SPACE);
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
		// Отображаем свойства динамического элемента для анимации спрайта
		OnDrawDynamicSpriteParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров динамического элемента для анимации спрайта
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawDynamicSpriteParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIDynamicSprite dynamic_sprite = property.GetValue<CGUIDynamicSprite>();
		if (dynamic_sprite != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Animation settings", EditorStyles.boldLabel);
				
				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_sprite.mWidthSprite = EditorGUI.FloatField(position, mContentWidth, dynamic_sprite.mWidthSprite);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_sprite.mHeightSprite = EditorGUI.FloatField(position, mContentHeight, dynamic_sprite.mHeightSprite);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_sprite.ChangeParam = (TDynamicParam)EditorGUI.EnumFlagsField(position, mContentChangeParam, dynamic_sprite.ChangeParam);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_sprite.Duration = EditorGUI.FloatField(position, mContentDuration, dynamic_sprite.Duration);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_sprite.Easing = (TEasingType)EditorGUI.EnumPopup(position, mContentEasing, dynamic_sprite.Easing);

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				dynamic_sprite.IsUnRegister = EditorGUI.Toggle(position, mContentIsUnRegister, dynamic_sprite.IsUnRegister);

				// 8)
				if (dynamic_sprite.ChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_sprite.LocationStarting = EditorGUI.Vector2Field(position, mContentLocationStart, dynamic_sprite.LocationStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_sprite.LocationTarget = EditorGUI.Vector2Field(position, mContentLocationTarget, dynamic_sprite.LocationTarget);
				}

				// 9)
				if (dynamic_sprite.ChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_sprite.SizeStarting = EditorGUI.Vector2Field(position, mContentSizeStart, dynamic_sprite.SizeStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_sprite.SizeTarget = EditorGUI.Vector2Field(position, mContentSizeTarget, dynamic_sprite.SizeTarget);
				}

				// 10)
				if (dynamic_sprite.ChangeParam.IsFlagSet(TDynamicParam.ColorBackground))
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_sprite.ColorStarting = EditorGUI.ColorField(position, mContentColorStart, dynamic_sprite.ColorStarting);

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					dynamic_sprite.ColorTarget = EditorGUI.ColorField(position, mContentColorTarget, dynamic_sprite.ColorTarget);
				}

				// 11)
				if (dynamic_sprite.ChangeParam.IsFlagSet(TDynamicParam.AnimationSprite))
				{
					if (LotusTweenDispatcher.SpriteStorage.GroupSprites != null && LotusTweenDispatcher.SpriteStorage.GroupCount != 0)
					{
						// 12)
						position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
						Int32 count_frame = LotusTweenDispatcher.SpriteStorage[dynamic_sprite.StorageSpriteIndex].Count - 1;
						dynamic_sprite.StartFrame = EditorGUI.IntSlider(position, "Start Frame", dynamic_sprite.StartFrame, 0, count_frame);

						// 13)
						position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
						String text_target_frame = "Target Frame (max " + count_frame.ToString() + ")";
						dynamic_sprite.TargetFrame = EditorGUI.IntSlider(position, text_target_frame, dynamic_sprite.TargetFrame,
							dynamic_sprite.StartFrame, count_frame);
					}

					else
					{
						// 12)
						position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
						dynamic_sprite.StartFrame = EditorGUI.IntField(position, "Start Frame", dynamic_sprite.StartFrame);

						// 13)
						position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
						dynamic_sprite.TargetFrame = EditorGUI.IntField(position, "Target Frame", dynamic_sprite.TargetFrame);
					}

					if (LotusTweenDispatcher.SpriteStorage.GroupSprites != null)
					{
						if (LotusTweenDispatcher.SpriteStorage.GroupCount == 0)
						{
							// 14)
							position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
							EditorGUI.LabelField(position, "Empty storages");
						}
						else
						{
							// 14)
							//position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
							//EditorGUI.ObjectField(position, LotusTweenDispatcher.SpriteStorage.Instance[dynamic_sprite.StorageSpriteIndex].mSprites[0].texture,
							//	typeof(Texture), false);

							// 15)
							position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
							dynamic_sprite.StorageSpriteIndex = EditorGUI.Popup(position, "Storage Index", dynamic_sprite.StorageSpriteIndex,
								LotusTweenDispatcher.SpriteStorage.ListNames);

							// 16)
							position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
							dynamic_sprite.Duration = EditorGUI.FloatField(position, "Duration", dynamic_sprite.Duration);
						}
					}
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
	/// Получение совокупной высоты свойств динамического спрайта
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightDynamicSprite(SerializedProperty property)
	{
		Single base_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountDynamicSpriteProperties;

		CGUIDynamicSprite dynamic_sprite = property.GetValue<CGUIDynamicSprite>();
		if (dynamic_sprite != null)
		{
			// 1)
			if (dynamic_sprite.ChangeParam.IsFlagSet(TDynamicParam.Location))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}

			// 2)
			if (dynamic_sprite.ChangeParam.IsFlagSet(TDynamicParam.Size))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}

			// 3)
			if (dynamic_sprite.ChangeParam.IsFlagSet(TDynamicParam.ColorBackground))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 2;
			}

			// 4)
			if (dynamic_sprite.ChangeParam.IsFlagSet(TDynamicParam.AnimationSprite))
			{
				base_height += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * 4;
			}
		}

		return (base_height);
	}
	#endregion
}
//=====================================================================================================================