//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные стили
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualStyleCommon.cs
*		Общие типы и структуры данных подсистемы визуальных стилей.
*		Реализация концепции визуального стиля элемента. Позволяет быстро, из предопределенного набора свойств применить
*	к элементу или к его структурное части визуальное оформление в виде фонового спрайта, настроек текста и т.д.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DCommonVisualStyle Визуальные стили
		//! Концепция визуального стиля элемента позволяет быстро, из предопределенного набора свойств применить
		//! к элементу или к его структурное части визуальное оформление в виде фонового спрайта, настроек текста и т.д.
		//! \ingroup Unity2DCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый тип для визуальных стилей
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CVisualStyle
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mName;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedInfo;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Название визуального стиля
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных в формате XML
			/// </summary>
			/// <param name="writer">Средство записи данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void WriteToXml(XmlWriter writer)
			{
				writer.WriteAttributeString(nameof(Name), mName);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных в формате XML
			/// </summary>
			/// <param name="reader">Средство чтении данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ReadFromXml(XmlReader reader)
			{
				String value;
				if ((value = reader.GetAttribute(nameof(Name))) != null)
				{
					Name = value;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Визуальный стиль для компонента текста. Содержит:
		/// - Цвет и параметры текста
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CVisualStyleText : CVisualStyle
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Font mFontFamily;
			[SerializeField]
			internal Int32 mFontSize = 14;
			[SerializeField]
			internal FontStyle mFontStyle;
			[SerializeField]
			internal TextAnchor mTextAnchor = TextAnchor.MiddleCenter;
			[SerializeField]
			internal Color mTextColor = Color.white;
			[SerializeField]
			internal Color mTextColorActive = Color.red;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Шрифт текста
			/// </summary>
			public Font FontFamily
			{
				get { return mFontFamily; }
				set { mFontFamily = value; }
			}

			/// <summary>
			/// Размер шрифта текста
			/// </summary>
			public Int32 FontSize
			{
				get { return mFontSize; }
				set { mFontSize = value; }
			}

			/// <summary>
			/// Стиль шрифта текста
			/// </summary>
			public FontStyle FontStyle
			{
				get { return mFontStyle; }
				set { mFontStyle = value; }
			}

			/// <summary>
			/// Выравнивания текста
			/// </summary>
			public TextAnchor TextAnchor
			{
				get { return mTextAnchor; }
				set { mTextAnchor = value; }
			}

			/// <summary>
			/// Цвет текста
			/// </summary>
			public Color TextColor
			{
				get { return mTextColor; }
				set { mTextColor = value; }
			}

			/// <summary>
			/// Цвет текста в активном состоянии
			/// </summary>
			public Color TextColorActive
			{
				get { return mTextColorActive; }
				set { mTextColorActive = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyleText()
			{
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального стиля к компоненту текста
			/// </summary>
			/// <param name="text">Компонент текста</param>
			/// <param name="enabled">Статус доступности текста</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetText(Text text, Boolean enabled)
			{
				if (text != null)
				{
					if (mFontFamily != null)
					{
						text.font = mFontFamily;
					}

					text.alignment = mTextAnchor;
					text.fontStyle = mFontStyle;

					if (enabled)
					{
						text.color = mTextColor;
						text.material = null;
					}
					else
					{
						text.color = Color.white;
						text.material = LotusGraphics2DVisualStyleService.MaterialDisableText;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание полной копии визуального стиля текста
			/// </summary>
			/// <returns>Копия визуального стиля текста</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyleText CloneText()
			{
				CVisualStyleText style = new CVisualStyleText();
				CopyText(style);
				
				return style;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование визуального стиля текста
			/// </summary>
			/// <param name="style">Визуальный стиль для куда будут скопированы свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void CopyText(CVisualStyleText style)
			{
				style.mName = mName;
				style.mFontFamily = mFontFamily;
				style.mTextColor = mTextColor;
				style.mTextColorActive = mTextColorActive;
				style.mFontSize = mFontSize;
				style.mFontStyle = mFontStyle;
				style.mTextAnchor = mTextAnchor;
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных в формате атрибутов XML
			/// </summary>
			/// <param name="writer">Средство записи данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void WriteToXml(XmlWriter writer)
			{
				base.WriteToXml(writer);
				writer.WriteResourceToAttribute(nameof(FontFamily), mFontFamily);
				writer.WriteIntegerToAttribute(nameof(FontSize), mFontSize);
				writer.WriteEnumToAttribute(nameof(FontStyle), mFontStyle);
				writer.WriteEnumToAttribute(nameof(TextAnchor), mTextAnchor);
				writer.WriteColorToAttribute(nameof(TextColor), mTextColor);
				writer.WriteColorToAttribute(nameof(TextColorActive), mTextColorActive);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных в формате атрибутов XML
			/// </summary>
			/// <param name="reader">Средство чтении данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void ReadFromXml(XmlReader reader)
			{
				base.ReadFromXml(reader);

				TextColorActive = reader.ReadUnityColorFromAttribute(nameof(TextColorActive), TextColorActive);
				TextColor = reader.ReadUnityColorFromAttribute(nameof(TextColor), TextColor);
				TextAnchor = reader.ReadEnumFromAttribute(nameof(TextAnchor), TextAnchor);
				FontStyle = reader.ReadEnumFromAttribute(nameof(FontStyle), FontStyle);
				FontSize = reader.ReadIntegerFromAttribute(nameof(FontSize), FontSize);
				mFontFamily = reader.ReadUnityResourceFromAttribute(nameof(FontFamily), mFontFamily);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый визуальный стиль. Содержит:
		/// - Фоновый спрайт и цвет модуляции
		/// - Спрайт иконки контента и цвет модуляции
		/// - Цвет и параметры текста
		/// </summary>
		/// <remarks>
		/// Каждый элемент имеет базовый визуальный стиль который определяет общее фоновое изображение, параметры
		/// основного текста и изображения иконки
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CVisualStyleBase : CVisualStyleText
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Sprite mBackgroundImage;
			[SerializeField]
			internal Sprite mBackgroundImagePressed;
			[SerializeField]
			internal Sprite mBackgroundImageSelected;
			[SerializeField]
			internal Color mBackgroundColor = Color.white;
			[SerializeField]
			internal Color mBackgroundColorActive = Color.red;
			[SerializeField]
			internal Sprite mIconImage;
			[SerializeField]
			internal Sprite mIconImageActive;
			[SerializeField]
			internal Color mIconColor = Color.white;
			[SerializeField]
			internal Color mIconColorActive = Color.red;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Спрайт фонового изображения элемента в обычном состоянии
			/// </summary>
			public Sprite BackgroundImage
			{
				get { return mBackgroundImage; }
				set { mBackgroundImage = value; }
			}

			/// <summary>
			/// Спрайт фонового изображения элемента в активном состоянии (нажатие)
			/// </summary>
			public Sprite BackgroundImagePressed
			{
				get { return mBackgroundImagePressed; }
				set { mBackgroundImagePressed = value; }
			}

			/// <summary>
			/// Спрайт фонового изображения элемента в активном состоянии (выбор)
			/// </summary>
			public Sprite BackgroundImageSelected
			{
				get { return mBackgroundImageSelected; }
				set { mBackgroundImageSelected = value; }
			}

			/// <summary>
			/// Цвет фонового изображения элемента в обычном состоянии 
			/// </summary>
			public Color BackgroundColor
			{
				get { return mBackgroundColor; }
				set { mBackgroundColor = value; }
			}

			/// <summary>
			/// Цвет фонового изображения элемента в активном состоянии 
			/// </summary>
			public Color BackgroundColorActive
			{
				get { return mBackgroundColorActive; }
				set { mBackgroundColorActive = value; }
			}

			/// <summary>
			/// Спрайт изображения иконки элемента в обычном состоянии
			/// </summary>
			public Sprite IconImage
			{
				get { return mIconImage; }
				set { mIconImage = value; }
			}

			/// <summary>
			/// Спрайт изображения иконки элемента в активном состоянии
			/// </summary>
			public Sprite IconImageActive
			{
				get { return mIconImageActive; }
				set { mIconImageActive = value; }
			}

			/// <summary>
			/// Цвет изображения иконки элемента в обычном состоянии
			/// </summary>
			public Color IconColor
			{
				get { return mIconColor; }
				set { mIconColor = value; }
			}

			/// <summary>
			/// Цвет изображения иконки элемента в активном состоянии
			/// </summary>
			public Color IconColorActive
			{
				get { return mIconColorActive; }
				set { mIconColorActive = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyleBase()
			{
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального стиля фонового изображения к компоненту изображения
			/// </summary>
			/// <param name="image">Компонент изображения</param>
			/// <param name="enabled">Статус доступности изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetImageBackground(Image image, Boolean enabled)
			{
				if (image != null)
				{
					if (mBackgroundImage != null)
					{
						image.SetSprite(mBackgroundImage);
					}

					if (enabled)
					{
						image.color = mBackgroundColor;
						image.material = null;
					}
					else
					{
						image.color = Color.white;
						image.material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального стиля изображения иконки к компоненту изображения
			/// </summary>
			/// <param name="image">Компонент изображения</param>
			/// <param name="enabled">Статус доступности изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetImageIcon(Image image, Boolean enabled)
			{
				if (image != null)
				{
					if (mIconImage != null)
					{
						image.SetSprite(mIconImage);
					}

					if (enabled)
					{
						image.color = mBackgroundColor;
						image.material = null;
					}
					else
					{
						image.color = Color.white;
						image.material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание полной копии базового визуального стиля
			/// </summary>
			/// <returns>Копия базового визуального стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyleBase CloneBase()
			{
				CVisualStyleBase style = new CVisualStyleBase();
				CopyBase(style);
				
				return style;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование базового визуального стиля
			/// </summary>
			/// <param name="style">Визуальный стиль для куда будут скопированы свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void CopyBase(CVisualStyleBase style)
			{
				style.mName = mName;
				style.mBackgroundImage = mBackgroundImage;
				style.mBackgroundImagePressed = mBackgroundImagePressed;
				style.mBackgroundImageSelected = mBackgroundImageSelected;
				style.mBackgroundColor = mBackgroundColor;
				style.mBackgroundColorActive = mBackgroundColorActive;
				style.mIconImage = mIconImage;
				style.mIconImageActive = mIconImageActive;
				style.mIconColor = mIconColor;
				style.mIconColorActive = mIconColorActive;
				style.mFontFamily = mFontFamily;
				style.mTextColor = mTextColor;
				style.mTextColorActive = mTextColorActive;
				style.mFontSize = mFontSize;
				style.mFontStyle = mFontStyle;
				style.mTextAnchor = mTextAnchor;
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных в формате атрибутов XML
			/// </summary>
			/// <param name="writer">Средство записи данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void WriteToXml(XmlWriter writer)
			{
				base.WriteToXml(writer);
				
				writer.WriteResourceToAttribute(nameof(BackgroundImage), mBackgroundImage);
				writer.WriteColorToAttribute(nameof(BackgroundColor), mBackgroundColor);

				writer.WriteResourceToAttribute(nameof(BackgroundImagePressed), mBackgroundImagePressed);
				writer.WriteResourceToAttribute(nameof(BackgroundImageSelected), mBackgroundImageSelected);
				writer.WriteColorToAttribute(nameof(BackgroundColorActive), mBackgroundColorActive);

				writer.WriteResourceToAttribute(nameof(IconImage), mIconImage);
				writer.WriteColorToAttribute(nameof(IconColor), mIconColor);

				writer.WriteResourceToAttribute(nameof(IconImageActive), mIconImageActive);
				writer.WriteColorToAttribute(nameof(IconColorActive), mIconColorActive);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных в формате атрибутов XML
			/// </summary>
			/// <param name="reader">Средство чтении данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void ReadFromXml(XmlReader reader)
			{
				base.ReadFromXml(reader);

				mBackgroundImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(BackgroundImage), null);
				BackgroundColor = reader.ReadUnityColorFromAttribute(nameof(BackgroundColor), BackgroundColor);

				mBackgroundImagePressed = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(BackgroundImagePressed), null);
				mBackgroundImageSelected = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(BackgroundImageSelected), null);
				BackgroundColorActive = reader.ReadUnityColorFromAttribute(nameof(BackgroundColorActive), BackgroundColorActive);

				mIconImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(IconImage), null);
				mIconColor = reader.ReadUnityColorFromAttribute(nameof(IconColor), IconColor);

				mIconImageActive = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(IconImageActive), null);
				mIconColorActive = reader.ReadUnityColorFromAttribute(nameof(IconColorActive), IconColor);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Визуальный стиль для различных прогрессов, полос прокруток и ползунков. Содержит:
		/// - Спрайт фона заполняющей области
		/// - Спрайт заполняющей области
		/// - Спрайт управляющего элемента - ручки
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CVisualStyleScroll : CVisualStyle
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Sprite mFillBackgroundImage;
			[SerializeField]
			internal Sprite mFillImage;
			[SerializeField]
			internal Sprite mHandleImage;
			[SerializeField]
			internal Sprite mHandleImagePressed;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Спрайт изображения фона заполняющей области
			/// </summary>
			public Sprite FillBackgroundImage
			{
				get { return mFillBackgroundImage; }
				set { mFillBackgroundImage = value; }
			}

			/// <summary>
			/// Спрайт изображения заполняющей области
			/// </summary>
			public Sprite FillImage
			{
				get { return mFillImage; }
				set { mFillImage = value; }
			}

			/// <summary>
			/// Спрайт изображения управляющего элемента
			/// </summary>
			public Sprite HandleImage
			{
				get { return mHandleImage; }
				set { mHandleImage = value; }
			}

			/// <summary>
			/// Спрайт изображения управляющего элемента в нажатом состоянии
			/// </summary>
			public Sprite HandleImagePressed
			{
				get { return mHandleImagePressed; }
				set { mHandleImagePressed = value; }
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального стиля изображения фона заполняющей области к компоненту изображения
			/// </summary>
			/// <param name="image">Компонент изображения</param>
			/// <param name="enabled">Статус доступности изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetImageFillBackground(Image image, Boolean enabled)
			{
				if (image != null)
				{
					if (mFillBackgroundImage != null)
					{
						image.SetSprite(mFillBackgroundImage);
					}

					if (enabled)
					{
						image.material = null;
					}
					else
					{
						image.color = Color.white;
						image.material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального стиля изображения заполняющей области к компоненту изображения
			/// </summary>
			/// <param name="image">Компонент изображения</param>
			/// <param name="enabled">Статус доступности изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetImageFill(Image image, Boolean enabled)
			{
				if (image != null)
				{
					if (mFillImage != null)
					{
						image.SetSprite(mFillImage);
					}

					if (enabled)
					{
						image.material = null;
					}
					else
					{
						image.color = Color.white;
						image.material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального стиля изображения управляющего элемента компоненту изображения
			/// </summary>
			/// <param name="image">Компонент изображения</param>
			/// <param name="enabled">Статус доступности изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetImageHandle(Image image, Boolean enabled)
			{
				if (image != null)
				{
					if (mHandleImage != null)
					{
						image.SetSprite(mHandleImage);
					}

					if (enabled)
					{
						image.material = null;
					}
					else
					{
						image.color = Color.white;
						image.material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание полной копии визуального стиля
			/// </summary>
			/// <returns>Копия визуального стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyleScroll CloneScroll()
			{
				CVisualStyleScroll style = new CVisualStyleScroll();

				CopyScroll(style);

				return style;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование визуального стиля
			/// </summary>
			/// <param name="style">Визуальный стиль для куда будут скопированы свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void CopyScroll(CVisualStyleScroll style)
			{
				style.mName = mName;
				style.mFillBackgroundImage = mFillBackgroundImage;
				style.mFillImage = mFillImage;
				style.mHandleImagePressed = mHandleImagePressed;
				style.mHandleImage = mHandleImage;
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных в формате XML
			/// </summary>
			/// <param name="writer">Средство записи данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void WriteToXml(XmlWriter writer)
			{
				base.WriteToXml(writer);

				writer.WriteResourceToAttribute(nameof(FillBackgroundImage), mFillBackgroundImage);
				writer.WriteResourceToAttribute(nameof(FillImage), mFillImage);
				writer.WriteResourceToAttribute(nameof(HandleImage), mHandleImage);
				writer.WriteResourceToAttribute(nameof(HandleImagePressed), mHandleImagePressed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных в формате XML
			/// </summary>
			/// <param name="reader">Средство чтении данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void ReadFromXml(XmlReader reader)
			{
				base.ReadFromXml(reader);

				mFillBackgroundImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(FillBackgroundImage), null);
				mFillImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(FillImage), null);
				mHandleImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(HandleImage), null);
				mHandleImagePressed = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(HandleImagePressed), null);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Визуальный стиль для элементов имеющих область заголовка. Содержит:
		/// - Спрайт области контента
		/// - Спрайт области заголовочного элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CVisualStyleHeader : CVisualStyle
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Sprite mHeaderImage;
			[SerializeField]
			internal Color mHeaderColor = Color.white;
			[SerializeField]
			internal Color mHeaderColorActive = Color.red;
			[SerializeField]
			internal Sprite mContentImage;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Спрайт заголовочного элемента (при его наличии)
			/// </summary>
			public Sprite HeaderImage
			{
				get { return mHeaderImage; }
				set { mHeaderImage = value; }
			}

			/// <summary>
			/// Цвет заголовочного элемента в обычном состоянии
			/// </summary>
			public Color HeaderColor
			{
				get { return mHeaderColor; }
				set { mHeaderColor = value; }
			}

			/// <summary>
			/// Цвет заголовочного элемента в активном состоянии
			/// </summary>
			public Color HeaderColorActive
			{
				get { return mHeaderColorActive; }
				set { mHeaderColorActive = value; }
			}

			/// <summary>
			/// Спрайт области контента
			/// </summary>
			public Sprite ContentImage
			{
				get { return mContentImage; }
				set { mContentImage = value; }
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание полной копии визуального стиля
			/// </summary>
			/// <returns>Копия визуального стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyleHeader CloneHeader()
			{
				CVisualStyleHeader style = new CVisualStyleHeader();

				CopyHeader(style);

				return style;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование визуального стиля
			/// </summary>
			/// <param name="style">Визуальный стиль для куда будут скопированы свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void CopyHeader(CVisualStyleHeader style)
			{
				style.mName = mName;
				style.mHeaderImage = mHeaderImage;
				style.mHeaderColor = mHeaderColor;
				style.mHeaderColorActive = mHeaderColorActive;
				style.mContentImage = mContentImage;
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных в формате XML
			/// </summary>
			/// <param name="writer">Средство записи данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void WriteToXml(XmlWriter writer)
			{
				base.WriteToXml(writer);

				writer.WriteResourceToAttribute(nameof(ContentImage), mContentImage);
				writer.WriteResourceToAttribute(nameof(HeaderImage), mHeaderImage);
				writer.WriteColorToAttribute(nameof(HeaderColor), mHeaderColor);
				writer.WriteColorToAttribute(nameof(HeaderColorActive), mHeaderColorActive);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных в формате XML
			/// </summary>
			/// <param name="reader">Средство чтении данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void ReadFromXml(XmlReader reader)
			{
				base.ReadFromXml(reader);

				HeaderColorActive = reader.ReadUnityColorFromAttribute(nameof(HeaderColorActive), HeaderColorActive);
				HeaderColor = reader.ReadUnityColorFromAttribute(nameof(HeaderColor), HeaderColor);

				mContentImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(ContentImage), null);
				mHeaderImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(HeaderImage), null);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Визуальный стиль для элементов имеющих две управляющие кнопки. Содержит:
		/// - Спрайт первой управляющей кнопки
		/// - Спрайт второй управляющей кнопки
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CVisualStyleSpinner : CVisualStyle
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Sprite mButtonUpImage;
			[SerializeField]
			internal Sprite mButtonUpImageActive;
			[SerializeField]
			internal Sprite mButtonDownImage;
			[SerializeField]
			internal Sprite mButtonDownImageActive;
			[SerializeField]
			internal Color mButtonColor = Color.white;
			[SerializeField]
			internal Color mButtonColorActive = Color.red;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Спрайт первой управляющей кнопки (вперед, вверх)
			/// </summary>
			public Sprite ButtonUpImage
			{
				get { return mButtonUpImage; }
				set { mButtonUpImage = value; }
			}

			/// <summary>
			/// Спрайт первой управляющей кнопки (вперед, вверх) в активном состоянии
			/// </summary>
			public Sprite ButtonUpImageActive
			{
				get { return mButtonUpImageActive; }
				set { mButtonUpImageActive = value; }
			}

			/// <summary>
			/// Спрайт второй управляющей кнопки (назад, вниз)
			/// </summary>
			public Sprite ButtonDownImage
			{
				get { return mButtonDownImage; }
				set { mButtonDownImage = value; }
			}

			/// <summary>
			/// Спрайт второй управляющей кнопки (назад, вниз) в активном состоянии
			/// </summary>
			public Sprite ButtonDownImageActive
			{
				get { return mButtonDownImageActive; }
				set { mButtonDownImageActive = value; }
			}

			/// <summary>
			/// Цвет кнопок в обычном состоянии
			/// </summary>
			public Color ButtonColor
			{
				get { return mButtonColor; }
				set { mButtonColor = value; }
			}

			/// <summary>
			/// Цвет кнопок в обычном состоянии в активном состоянии
			/// </summary>
			public Color ButtonColorActive
			{
				get { return mButtonColorActive; }
				set { mButtonColorActive = value; }
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального стиля изображения первой управляющей кнопки к компоненту изображения
			/// </summary>
			/// <param name="image">Компонент изображения</param>
			/// <param name="enabled">Статус доступности изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetImageButtonUp(Image image, Boolean enabled)
			{
				if (image != null)
				{
					if (mButtonUpImage != null)
					{
						image.SetSprite(mButtonUpImage);
					}
					else
					{
						if (enabled)
						{
							image.material = null;
						}
						else
						{
							image.material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального стиля изображения второй управляющей кнопки к компоненту изображения
			/// </summary>
			/// <param name="image">Компонент изображения</param>
			/// <param name="enabled">Статус доступности изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetImageButtonDown(Image image, Boolean enabled)
			{
				if (image != null)
				{
					if (mButtonUpImage != null)
					{
						image.SetSprite(mButtonDownImage);
					}

					if (enabled)
					{
						image.material = null;
					}
					else
					{
						image.material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание полной копии визуального стиля
			/// </summary>
			/// <returns>Копия визуального стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyleSpinner CloneSpinner()
			{
				CVisualStyleSpinner style = new CVisualStyleSpinner();

				CopySpinner(style);

				return style;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование визуального стиля
			/// </summary>
			/// <param name="style">Визуальный стиль для куда будут скопированы свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void CopySpinner(CVisualStyleSpinner style)
			{
				style.mName = mName;
				style.mButtonUpImage = mButtonUpImage;
				style.mButtonUpImageActive = mButtonUpImageActive;
				style.mButtonDownImage = mButtonDownImage;
				style.mButtonDownImageActive = mButtonDownImageActive;
				style.mButtonColor = mButtonColor;
				style.mButtonColorActive = mButtonColorActive;
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных в формате XML.
			/// </summary>
			/// <param name="writer">Средство записи данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void WriteToXml(XmlWriter writer)
			{
				base.WriteToXml(writer);

				writer.WriteResourceToAttribute(nameof(ButtonUpImage), mButtonUpImage);
				writer.WriteResourceToAttribute(nameof(ButtonUpImageActive), mButtonUpImageActive);

				writer.WriteResourceToAttribute(nameof(ButtonDownImage), mButtonDownImage);
				writer.WriteResourceToAttribute(nameof(ButtonDownImageActive), mButtonDownImageActive);

				writer.WriteColorToAttribute(nameof(ButtonColor), mButtonColor);
				writer.WriteColorToAttribute(nameof(ButtonColorActive), mButtonColorActive);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных в формате XML.
			/// </summary>
			/// <param name="reader">Средство чтении данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public override void ReadFromXml(XmlReader reader)
			{
				base.ReadFromXml(reader);

				ButtonColorActive = reader.ReadUnityColorFromAttribute(nameof(ButtonColorActive), ButtonColorActive);
				ButtonColor = reader.ReadUnityColorFromAttribute(nameof(ButtonColor), ButtonColor);

				mButtonDownImageActive = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(ButtonDownImageActive), null);
				mButtonDownImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(ButtonDownImage), null);

				mButtonDownImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(ButtonDownImage), null);

				mButtonUpImageActive = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(ButtonUpImageActive), null);
				mButtonUpImage = reader.ReadUnityResourceFromAttribute<Sprite>(nameof(ButtonUpImage), null);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================