//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseDrawer.cs
*		Редакторы для рисования параметров базовых элементов.
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
/// Редактор для рисования элемента BaseElement
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIBaseElement))]
public class LotusBaseElementDrawer : PropertyDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Смещения при окончании вывода свойств
	/// </summary>
	public const Single SpaceEndProperty = 8;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginChangeCheck();
		EditorGUI.BeginProperty(position, label, property);
		{
			LotusDisplayNameAttribute attribute_name = fieldInfo.GetAttribute<LotusDisplayNameAttribute>();
			if (attribute_name != null) label.text = attribute_name.Name;

			// Определяем высоту для отображения свойств
			position.height = XInspectorViewParams.CONTROL_HEIGHT;
			if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
			{
				EditorGUI.indentLevel++;

				// Рисуем данные элемента
				OnDrawParamemtrs(ref position, property);

				EditorGUI.indentLevel--;
			}
		}
		EditorGUI.EndProperty();
		if (EditorGUI.EndChangeCheck())
		{
			property.Save();
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
	public virtual void OnDrawParamemtrs(ref Rect position, SerializedProperty property)
	{
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента Element
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIElement))]
public class LotusElementDrawer : LotusBaseElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountElementProperties = 14+1;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentName = new GUIContent("Name");
	protected static GUIContent mContentVisible = new GUIContent("IsVisible");
	protected static GUIContent mContentEnabled = new GUIContent("IsEnabled");
	protected static GUIContent mContentOffsetLeft = new GUIContent("Left");
	protected static GUIContent mContentOffsetRight = new GUIContent("Right");
	protected static GUIContent mContentOffsetTop = new GUIContent("Top");
	protected static GUIContent mContentOffsetBottom = new GUIContent("Bottom");
	protected static GUIContent mContentPosition = new GUIContent("Position");
	protected static GUIContent mContentSize = new GUIContent("Size");
	protected static GUIContent mContentWidth = new GUIContent("Width");
	protected static GUIContent mContentHeight = new GUIContent("Height");
	protected static GUIContent mContentAspectMode = new GUIContent("AspectMode ");
	protected static GUIContent mContentHAlign = new GUIContent("HorizontalAlign");
	protected static GUIContent mContentVAlign = new GUIContent("VerticalAlign");
	protected static GUIContent mContentDepth = new GUIContent("Depth");
	protected static GUIContent mContentDepthUp = new GUIContent(XString.TriangleUp);
	protected static GUIContent mContentDepthDown = new GUIContent(XString.TriangleDown);
	protected static GUIContent mContentStyle = new GUIContent("Style");
	protected Int32 mSelectedStyleIndex;
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
			return (GetPropertyHeightElement() + XInspectorViewParams.SPACE);
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
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawElementParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIElement element = property.GetValue<CGUIElement>();
		if (element != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Base settings", EditorStyles.boldLabel);

				// 2)
				if (element.IParent != null)
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					GUI.backgroundColor = Color.green;
					EditorGUI.TextField(position, "Parent", element.IParent.Name);
					GUI.backgroundColor = Color.white;
				}
				else
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					EditorGUI.TextField(position, "Parent", "Screen");
				}

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element.Name = EditorGUI.TextField(position, mContentName, element.Name);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element.IsVisible = EditorGUI.Toggle(position, mContentVisible, element.IsVisible);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element.IsEnabled = EditorGUI.Toggle(position, mContentEnabled, element.IsEnabled);

				// 6)
				position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
				Rect rect_left = position;
				Rect rect_right = position;
				rect_left.width = position.width * 0.5f;
				rect_right.x = rect_left.xMax;
				rect_right.width = position.width * 0.5f;
				element.Left = EditorGUI.FloatField(rect_left, mContentOffsetLeft, element.Left);
				element.Right = EditorGUI.FloatField(rect_right, mContentOffsetRight, element.Right);

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Rect rect_top = rect_left;
				Rect rect_bottom = rect_right;
				rect_top.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				rect_bottom.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element.Top = EditorGUI.FloatField(rect_top, mContentOffsetTop, element.Top);
				element.Bottom = EditorGUI.FloatField(rect_bottom, mContentOffsetBottom, element.Bottom);

				// 8)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element.AspectMode = (TAspectMode)EditorGUI.EnumPopup(position, mContentAspectMode, element.AspectMode);

				// 9)
				EditorGUI.BeginDisabledGroup(element.HorizontalAlignment == THorizontalAlignment.Stretch);
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					element.Width = EditorGUI.Slider(position, mContentWidth, element.Width, 1, 2000);
				}
				EditorGUI.EndDisabledGroup();

				// 10)
				EditorGUI.BeginDisabledGroup(element.VerticalAlignment == TVerticalAlignment.Stretch);
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					element.Height = EditorGUI.Slider(position, mContentHeight, element.Height, 1, 2000);
				}
				EditorGUI.EndDisabledGroup();

				// 11)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element.HorizontalAlignment = (THorizontalAlignment)EditorGUI.EnumPopup(position, mContentHAlign, element.HorizontalAlignment);

				// 12)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				element.VerticalAlignment = (TVerticalAlignment)EditorGUI.EnumPopup(position, mContentVAlign, element.VerticalAlignment);

				// 13)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Rect rect_depth = position;
				rect_depth.width = position.width - XInspectorViewParams.BUTTON_MINI_WIDTH * 2;
				element.Depth = EditorGUI.IntField(rect_depth, mContentDepth, element.Depth);
				Rect rect_button_up = rect_depth;
				rect_button_up.x = rect_depth.xMax;
				rect_button_up.width = XInspectorViewParams.BUTTON_MINI_WIDTH;
				if (GUI.Button(rect_button_up, mContentDepthUp, EditorStyles.miniButtonLeft))
				{
					element.Depth++;
				}
				Rect rect_button_down = rect_button_up;
				rect_button_down.x = rect_button_up.xMax;
				rect_button_down.width = XInspectorViewParams.BUTTON_MINI_WIDTH;
				if (GUI.Button(rect_button_down, mContentDepthDown, EditorStyles.miniButtonRight))
				{
					element.Depth--;
				}

				// 14)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(element.StyleMainName);
				mSelectedStyleIndex = EditorGUI.Popup(position, mContentStyle.text, index, LotusGUIDispatcher.GetStyleNames());
				if(index != mSelectedStyleIndex)
				{
					element.StyleMainName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedStyleIndex);
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
	/// Рисование кнопки оптимального размера
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawButtonOptimalSize(ref Rect position, SerializedProperty property)
	{
		position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
		position.height = XInspectorViewParams.BUTTON_MINI_HEIGHT;
		var rect_optimal = position;
		if (GUI.Button(rect_optimal, "OptimalSize", EditorStyles.miniButton))
		{
			CGUIElement base_element = property.GetValue<CGUIElement>();
			if (base_element != null)
			{
				base_element.ComputeSizeFromContent();
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование кнопки получение параметров стиля
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawButtonSetStyle(ref Rect position, SerializedProperty property)
	{
		position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
		position.height = XInspectorViewParams.BUTTON_MINI_HEIGHT;
		var rect_optimal = position;
		if (GUI.Button(rect_optimal, "SetFromOriginalStyle", EditorStyles.miniButton))
		{
			CGUIElement base_element = property.GetValue<CGUIElement>();
			if (base_element != null)
			{
				base_element.SetFromOriginalStyle();
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств элемента
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightElement()
	{
		return (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountElementProperties);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ContentElement
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIContentElement))]
public class LotusContentDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountContentProperties = 3;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentText = new GUIContent("Text");
	protected static GUIContent mContentIcon = new GUIContent("Icon");
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
			return (GetPropertyHeightContent() + XInspectorViewParams.SPACE);
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

		// Отображаем свойства контента
		OnDrawContentParamemtrs(ref position, property);

		// Кнопка оптимального размера
		DrawButtonOptimalSize(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров контента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawContentParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIContentElement content_element = property.GetValue<CGUIContentElement>();
		if (content_element != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Content settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				content_element.Text = EditorGUI.TextField(position, mContentText, content_element.Text);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				content_element.Icon = EditorGUI.ObjectField(position, mContentIcon, content_element.Icon,
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
	/// Получение совокупной высоты свойств контента
	/// </summary>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightContent()
	{
		Single base_height = GetPropertyHeightElement();
		Single content_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountContentProperties;

		return (base_height + content_height + XInspectorViewParams.BUTTON_MINI_HEIGHT);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента Label
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUILabel))]
public class LotusLabelDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountLabelProperties = 2;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentText = new GUIContent("Caption");
	protected static GUIContent mContentIcon = new GUIContent("Icon");
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
			return (base_height + local_height + label_height + XInspectorViewParams.BUTTON_MINI_HEIGHT + XInspectorViewParams.SPACE);
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

		// Кнопка оптимального размера
		DrawButtonOptimalSize(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров надписи
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawLabelParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUILabel label = property.GetValue<CGUILabel>();
		if (label != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Label settings", EditorStyles.boldLabel);

				// 2) - 2/1
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				SerializedProperty field_localize = property.FindPropertyRelative(nameof(CGUILabel.mCaptionText));
				position.height = EditorGUI.GetPropertyHeight(field_localize, mContentText);
				EditorGUI.PropertyField(position, field_localize, mContentText);

				// 3)
				position.y += position.height;
				position.height = XInspectorViewParams.CONTROL_HEIGHT;
				label.CaptionIcon = EditorGUI.ObjectField(position, mContentIcon, label.CaptionIcon,
					typeof(Texture2D), false) as Texture2D;

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
/// Редактор для рисования элемента Panel
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIPanel))]
public class LotusPanelDrawer : LotusLabelDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
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

		// Кнопка оптимального размера
		DrawButtonOptimalSize(ref position, property);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента LabelValue
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUILabelValue))]
public class LotusLabelValueDrawer : LotusLabelDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountLabelValueProperties = 6 + 3;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentValueText = new GUIContent("Text");
	protected static GUIContent mContentValueIcon = new GUIContent("Icon");
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
			Single base_height = GetPropertyHeightElement();
			Single local_height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(CGUILabel.mCaptionText)));
			Single label_value_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountLabelValueProperties;

			return (base_height + label_value_height + XInspectorViewParams.BUTTON_MINI_HEIGHT + XInspectorViewParams.SPACE);
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
		OnDrawLabelValueParamemtrs(ref position, property);

		// Кнопка оптимального размера
		DrawButtonOptimalSize(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров надписи со значением
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawLabelValueParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUILabelValue label_value = property.GetValue<CGUILabelValue>();
		if (label_value != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Label settings", EditorStyles.boldLabel);

				// 2)-1
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				SerializedProperty field_localize = property.FindPropertyRelative(nameof(CGUILabel.mCaptionText));
				position.height = EditorGUI.GetPropertyHeight(field_localize);
				EditorGUI.PropertyField(position, field_localize, mContentText);

				// 3)
				position.y += (position.height);
				position.height = XInspectorViewParams.CONTROL_HEIGHT;
				label_value.CaptionIcon = EditorGUI.ObjectField(position, mContentIcon, label_value.CaptionIcon,
					typeof(Texture2D), false) as Texture2D;

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "Value settings", EditorStyles.boldLabel);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				label_value.ValueText = EditorGUI.TextField(position, mContentValueText, label_value.ValueText);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				label_value.ValueIcon = EditorGUI.ObjectField(position, mContentValueIcon, label_value.ValueIcon,
					typeof(Texture2D), false) as Texture2D;

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(label_value.StyleValueName);
				mSelectedValueStyleIndex = EditorGUI.Popup(position, mContentValueStyle.text, index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedValueStyleIndex)
				{
					label_value.StyleValueName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedValueStyleIndex);
				}

				// 8)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				label_value.ValueSize = EditorGUI.FloatField(position, mContentValueSize, label_value.ValueSize);

				// 9)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				label_value.ValueLocation = (TValueLocation)EditorGUI.EnumPopup(position, mContentValueLocation, 
					label_value.ValueLocation);

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