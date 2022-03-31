//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonStandard.cs
*		Стандартные управляющие элементы интерфейса пользователя.
*		Реализация управляющих элементов интерфейса пользователя для непосредственного взаимодействия с пользователем.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DImmedateGUIControls Общие элементы интерфейса
		//! Общие управляющие элементы интерфейса пользователя реализуют базовое взаимодействие с пользователем.
		//! Реализованы типовые управляющие элементы, элементы со списком однотипных данных, редакторы текста, ползунки и области просмотра
		//! \ingroup Unity2DImmedateGUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Кнопка
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Button.
		/// Реализация кнопки с поддержкой эмуляции кнопки подсистемы ввода данных
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIButton : CGUILabel, ILotusVirtualButton
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента по указанным параметрам и регистрация его в диспетчере
			/// </summary>
			/// <remarks>
			/// Если элемент с таким имением существует то происходит не создание, а обновление параметров
			/// </remarks>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="on_click">Обработчик события нажатия кнопки</param>
			/// <returns>Cозданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIButton CreateButton(String name, Single x, Single y, String text, Action on_click)
			{
				return CreateButton(name, x, y, 120, 30, text, "Button", on_click);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента по указанным параметрам и регистрация его в диспетчере
			/// </summary>
			/// <remarks>
			/// Если элемент с таким имением существует то происходит не создание, а обновление параметров
			/// </remarks>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="style_name">Стиль элемента</param>
			/// <param name="on_click">Обработчик события нажатия кнопки</param>
			/// <returns>Cозданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIButton CreateButton(String name, Single x, Single y, Single width, Single height,
				String text, String style_name, Action on_click)
			{
				CGUIButton element = null;

				// Ищем элемент по имени
				element = LotusGUIDispatcher.GetElement(name) as CGUIButton;

				// Если не нашли то создаем
				if (element == null)
				{
					element = new CGUIButton(name, x, y, text);
					element.mStyleMainName = style_name;
					element.mRectLocalDesignMain = new Rect(x, y, width, height);
					element.OnClick = on_click;

					// Добавляем
					LotusGUIDispatcher.RegisterElement(element);

					// Есть регистрация в диспетчере
					element.mIsRegisterDispatcher = true;
				}
				else
				{
					element.CaptionText = text;
					element.StyleMainName = style_name;
					element.SetFromScreen(x, y, width, height);
				}

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// События
			internal Action mOnClick;

			// Служебные данные
			internal Int32 mLastPressedFrame = -5;
			internal Int32 mReleasedFrame = -5;
			internal Boolean mPressed;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о щелчке на кнопку
			/// </summary>
			public Action OnClick
			{
				get { return mOnClick; }
				set
				{
					mOnClick = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА IVirtualButton ===================================
			/// <summary>
			/// Статус удержания нажатой кнопки
			/// </summary>
			public Boolean IsButtonPressed
			{
				get { return mPressed; }
			}

			/// <summary>
			/// Статус нажатия кнопки
			/// </summary>
			public Boolean IsButtonDown
			{
				get { return mLastPressedFrame - Time.frameCount == -1; }
			}

			/// <summary>
			/// Статус отпускания кнопки
			/// </summary>
			public Boolean IsButtonUp
			{
				get { return mReleasedFrame == Time.frameCount - 1; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIButton()
				: base()
			{
				mStyleMainName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIButton(String name)
				: base(name)
			{
				mStyleMainName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIButton(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIButton(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mStyleMainName = "Button";
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента GUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				if (GUI.Button(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleMain))
				{
					if (mOnClick != null) mOnClick();

					mPressed = true;
					mLastPressedFrame = Time.frameCount;
				}
				if(Event.current.type == EventType.MouseUp && mRectWorldScreenMain.Contains(Event.current.mousePosition))
				{
					mPressed = false;
					mReleasedFrame = Time.frameCount;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUIButton;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Кнопка
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Button.
		/// Реализация кнопки для постоянного вызова события нажатия при удерживании
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIRepeatButton : CGUILabel, ILotusVirtualButton
		{
			#region ======================================= ДАННЫЕ ====================================================
			// События
			internal Action mOnClick;

			// Служебные данные
			internal Int32 mLastPressedFrame = -5;
			internal Int32 mReleasedFrame = -5;
			internal Boolean mPressed;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о щелчке на кнопку
			/// </summary>
			public Action OnClick
			{
				get { return mOnClick; }
				set
				{
					mOnClick = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА IVirtualButton ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус удержания нажатой кнопки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsButtonPressed
			{
				get { return mPressed; }
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус нажатия кнопки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsButtonDown
			{
				get { return mLastPressedFrame - Time.frameCount == -1; }
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус отпускания кнопки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsButtonUp
			{
				get { return mReleasedFrame == Time.frameCount - 1; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIRepeatButton()
				: base()
			{
				mStyleMainName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIRepeatButton(String name)
				: base(name)
			{
				mStyleMainName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIRepeatButton(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIRepeatButton(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mStyleMainName = "Button";
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента GUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				if (GUI.RepeatButton(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleMain))
				{
					if (mOnClick != null) mOnClick();

					mPressed = true;
					mLastPressedFrame = Time.frameCount;
				}
				if (Event.current.type == EventType.MouseUp && mRectWorldScreenMain.Contains(Event.current.mousePosition))
				{
					mPressed = false;
					mReleasedFrame = Time.frameCount;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUIButton;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Кнопка - переключатель
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Toggle.
		/// Реализация кнопки которая может находится в двух состояниях
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIToogleButton : CGUILabel
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsSelected;
			[SerializeField]
			internal String mOffText;

			// Параметры размещения
			[SerializeField]
			internal TOrientation mOrientation;
			[SerializeField]
			internal Single mButtonSize;

			// Параметры визуального стиля
			[SerializeField]
			internal String mStyleButtonName;
			[NonSerialized]
			internal GUIStyle mStyleButton;

			// События
			internal Action<Boolean> mOnSelected;

			// Служебные данные
			[NonSerialized]
			internal String mOnText;
			[NonSerialized]
			internal Single mButtonSizeCurrent;
			[NonSerialized]
			internal Rect mRectWorldScreenButton;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус выбора переключателя
			/// </summary>
			public Boolean IsSelected
			{
				get { return mIsSelected; }
				set
				{
					if (mIsSelected != value)
					{
						mIsSelected = value;
						ComputeRectDrawButton();
						if (mOnSelected != null) mOnSelected(mIsSelected);
					}
				}
			}

			/// <summary>
			/// Текст значения элемента
			/// </summary>
			public String OffText
			{
				get { return mOffText; }
				set { mOffText = value; }
			}

			//
			// ПАРАМЕТРЫ ВИЗУАЛЬНОГО СТИЛЯ
			//
			/// <summary>
			/// Имя стиля для рисования кнопки
			/// </summary>
			public String StyleButtonName
			{
				get { return mStyleButtonName; }
				set
				{
					if (mStyleButtonName != value)
					{
						mStyleButtonName = value;
						mStyleButton = LotusGUIDispatcher.FindStyle(mStyleButtonName);
					}
				}
			}

			/// <summary>
			/// Стиль для рисования кнопки
			/// </summary>
			public GUIStyle StyleButton
			{
				get
				{
					if (mStyleButton == null)
					{
						mStyleButton = LotusGUIDispatcher.FindStyle(mStyleButtonName);
					}
					return mStyleButton;
				}
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Ориентация элемента
			/// </summary>
			public TOrientation Orientation
			{
				get { return mOrientation; }
				set
				{
					if (mOrientation != value)
					{
						mOrientation = value;
						ComputeRectDrawButton();
					}
				}
			}

			/// <summary>
			/// Размер кнопки
			/// </summary>
			public Single ButtonSize
			{
				get { return mButtonSize; }
				set
				{
					if (mButtonSize != value)
					{
						mButtonSize = value;
						ComputeRectDrawButton();
					}
				}
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о выборе/снятия выбора с переключателя. Аргумент - статус выбора
			/// </summary>
			public Action<Boolean> OnSelected
			{
				get { return mOnSelected; }
				set
				{
					mOnSelected = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIToogleButton()
				: base()
			{
				mStyleMainName = "Box";
				mStyleButtonName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIToogleButton(String name)
				: base(name)
			{
				mStyleMainName = "Box";
				mStyleButtonName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIToogleButton(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Box";
				mStyleButtonName = "Button";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIToogleButton(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mStyleMainName = "Box";
				mStyleButtonName = "Button";
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление области расположения кнопки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeRectDrawButton()
			{
				Single offset_x = 2;
				Single offset_y = 2;
				Single h = mRectWorldScreenMain.height - offset_y * 2;
				Single w = mRectWorldScreenMain.width - offset_x * 2;

				switch (mAspectMode)
				{
					case TAspectMode.None:
						{
							mButtonSizeCurrent = mButtonSize;
						}
						break;
					case TAspectMode.Proportional:
						{
							if (mOrientation == TOrientation.Horizontal)
							{
								mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenX;
							}
							else
							{
								mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenY;
							}
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}

				switch (mOrientation)
				{
					case TOrientation.Horizontal:
						{
							if (mIsSelected)
							{
								mRectWorldScreenButton.x = mRectWorldScreenMain.x + offset_x;
								mRectWorldScreenButton.width = mButtonSizeCurrent;
								mRectWorldScreenButton.y = mRectWorldScreenMain.y + offset_y;
								mRectWorldScreenButton.height = h;
							}
							else
							{
								mRectWorldScreenButton.x = mRectWorldScreenMain.xMax - offset_x - mButtonSizeCurrent;
								mRectWorldScreenButton.width = mButtonSizeCurrent;
								mRectWorldScreenButton.y = mRectWorldScreenMain.y + offset_y;
								mRectWorldScreenButton.height = h;
							}
						}
						break;
					case TOrientation.Vertical:
						{
							if (mIsSelected)
							{
								mRectWorldScreenButton.x = mRectWorldScreenMain.x + offset_x;
								mRectWorldScreenButton.width = w;
								mRectWorldScreenButton.y = mRectWorldScreenMain.y + offset_y;
								mRectWorldScreenButton.height = mButtonSizeCurrent;
							}
							else
							{
								mRectWorldScreenButton.x = mRectWorldScreenMain.x + offset_x;
								mRectWorldScreenButton.width = w;
								mRectWorldScreenButton.y = mRectWorldScreenMain.yMax - offset_y - mButtonSizeCurrent;
								mRectWorldScreenButton.height = mButtonSizeCurrent;
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции и размеров элемента по относительным данным
			/// </summary>
			/// <remarks>
			/// На основании относительной позиции элемента считается его абсолютная позиция в экранных координатах
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdatePlacement()
			{
				base.UpdatePlacementBase();

				ComputeRectDrawButton();
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusVisualStyle ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров отображения элемента по связанному стилю
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetFromOriginalStyle()
			{
				mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				mStyleMainName = mStyleMain.name;

				mStyleButton = LotusGUIDispatcher.FindStyle(mStyleButtonName);
				mStyleButtonName = mStyleButton.name;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка/обновление параметров
			/// </summary>
			/// <remarks>
			/// Вызывается центральным диспетчером в момент добавления(регистрации) элемента
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void OnReset()
			{
				SetEnabledElement();
				SetVisibleElement();
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				if (mStyleButton == null) mStyleButton = LotusGUIDispatcher.FindStyle(mStyleButtonName);
				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				LotusGUIDispatcher.CurrentContent.text = mIsSelected ? mCaptionText.Text : mOffText;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				GUI.Box(mRectWorldScreenMain, "", mStyleMain);

				GUI.changed = false;
				mIsSelected = GUI.Toggle(mRectWorldScreenButton, mIsSelected, LotusGUIDispatcher.CurrentContent, mStyleButton);
				if (GUI.changed)
				{
					if (mOnSelected != null) mOnSelected(mIsSelected);
					ComputeRectDrawButton();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUIToogleButton;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				CGUIToogleButton toogle_button = base_element as CGUIToogleButton;
				if (toogle_button != null)
				{
					mOffText = toogle_button.mOffText;
					mOrientation = toogle_button.mOrientation;
					mButtonSize = toogle_button.mButtonSize;
					mStyleButtonName = toogle_button.mStyleButtonName;
					mStyleButton = LotusGUIDispatcher.FindStyle(mStyleButtonName);
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Флажок с выбором
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Toggle
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUICheckBox : CGUILabel
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsSelected;
			[SerializeField]
			internal String mGroupID;

			// События
			internal Action<Boolean> mOnSelected;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус выбора переключателя
			/// </summary>
			public Boolean IsSelected
			{
				get { return mIsSelected; }
				set
				{
					if (mIsSelected != value)
					{
						mIsSelected = value;
						if (mOnSelected != null) mOnSelected(mIsSelected);
					}
				}
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о выборе/снятия выбора с переключателя. Аргумент - статус выбора
			/// </summary>
			public Action<Boolean> OnSelected
			{
				get { return mOnSelected; }
				set
				{
					mOnSelected = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUICheckBox()
				: base()
			{
				mStyleMainName = "Toggle";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUICheckBox(String name)
				: base(name)
			{
				mStyleMainName = "Toggle";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUICheckBox(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Toggle";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUICheckBox(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mStyleMainName = "Toggle";
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента GUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				GUI.changed = false;
				mIsSelected = GUI.Toggle(mRectWorldScreenMain, mIsSelected, LotusGUIDispatcher.CurrentContent, mStyleMain);
				if (GUI.changed)
				{
					if (mOnSelected != null) mOnSelected(mIsSelected);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUICheckBox;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Ползунок
		/// </summary>
		/// <remarks>
		/// В зависимости от ориентации элемента для рисования используется метод либо GUI.HorizontalSlider
		/// либо GUI.VerticalSlider
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUISlider : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Single mNumberValue;
			[SerializeField]
			internal Single mMinValue;
			[SerializeField]
			internal Single mMaxValue;

			// Параметры размещения
			[SerializeField]
			internal TOrientation mOrientation;

			// События
			internal Action mOnValueChanged;

			// Служебные данные
			[NonSerialized]
			internal GUIStyle mStyleThumb;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Значение числовой величины
			/// </summary>
			public Single NumberValue
			{
				get { return mNumberValue; }
				set
				{
					if (mNumberValue != value)
					{
						mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
						if (mOnValueChanged != null) mOnValueChanged();
					}
				}
			}

			/// <summary>
			/// Максимальное значение числовой величины
			/// </summary>
			public Single MaxValue
			{
				get { return mMaxValue; }
				set
				{
					if (mMaxValue != value)
					{
						mMaxValue = value;
						if (mNumberValue > mMaxValue)
						{
							mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
							if (mOnValueChanged != null) mOnValueChanged();
						}
					}
				}
			}

			/// <summary>
			/// Минимальное значение числовой величины
			/// </summary>
			public Single MinValue
			{
				get { return mMinValue; }
				set
				{
					if (mMinValue != value)
					{
						mMinValue = value;
						if (mNumberValue > mMinValue)
						{
							mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
							if (mOnValueChanged != null) mOnValueChanged();
						}
					}
				}
			}

			/// <summary>
			/// Нормализованное значение числовой величины в частях от 0 до 1
			/// </summary>
			public Single Percent
			{
				get { return (mNumberValue - mMinValue) / (mMaxValue - mMinValue); }
			}

			/// <summary>
			/// Статус минимального значения числовой величины
			/// </summary>
			public Boolean IsMinValue
			{
				get { return mNumberValue <= mMinValue; }
			}

			/// <summary>
			/// Статус максимального значения числовой величины
			/// </summary>
			public Boolean IsMaxValue
			{
				get { return mNumberValue >= mMaxValue; }
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Ориентация элемента
			/// </summary>
			public TOrientation Orientation
			{
				get { return mOrientation; }
				set
				{
					mOrientation = value;
				}
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о изменении числовой величины
			/// </summary>
			public Action OnValueChanged
			{
				get { return mOnValueChanged; }
				set
				{
					mOnValueChanged = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUISlider()
				: base()
			{
				mStyleMainName = "SliderHorizontal";
				mMinValue = 0;
				mMaxValue = 100;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISlider(String name)
				: base(name)
			{
				mStyleMainName = "SliderHorizontal";
				mMinValue = 0;
				mMaxValue = 100;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISlider(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "SliderHorizontal";
				mMinValue = 0;
				mMaxValue = 100;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusVisualStyle ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров отображения элемента по связанному стилю
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetFromOriginalStyle()
			{
				mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				mStyleMainName = mStyleMain.name;

				mStyleThumb = LotusGUIDispatcher.FindStyle(mStyleMainName + "Thumb");
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка/обновление параметров
			/// </summary>
			/// <remarks>
			/// Вызывается центральным диспетчером в момент добавления(регистрации) элемента
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void OnReset()
			{
				SetEnabledElement();
				SetVisibleElement();
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				if (mStyleThumb == null) mStyleThumb = LotusGUIDispatcher.FindStyle(mStyleMainName + "Thumb");
				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента GUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				GUI.changed = false;
				if (mOrientation == TOrientation.Horizontal)
				{
					mNumberValue = GUI.HorizontalSlider(mRectWorldScreenMain, mNumberValue, mMinValue, mMaxValue, mStyleMain,
						mStyleThumb);
				}
				else
				{
					mNumberValue = GUI.VerticalSlider(mRectWorldScreenMain, mNumberValue, mMinValue, mMaxValue, mStyleMain, mStyleThumb);
				}
				if (GUI.changed)
				{
					if (mOnValueChanged != null) mOnValueChanged();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUISlider;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				CGUISlider slider = base_element as CGUISlider;
				if (slider != null)
				{
					mOrientation = slider.mOrientation;
					mNumberValue = slider.mNumberValue;
					mMinValue = slider.mMinValue;
					mMaxValue = slider.mMaxValue;
					mStyleThumb = slider.mStyleThumb;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Область просмотра
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.GUI.BeginScrollView и стандартный стиль ScrollView.
		/// Поддерживает инерцию перемещения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIScrollView : CGUIElement, ILotusDraggable
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal TScrollDirection mScrollDirection;
			[SerializeField]
			internal Vector2 mContentSize;
			[NonSerialized]
			internal Vector2 mContentOffset;
			[SerializeField]
			internal Boolean mIsInertia;
			[SerializeField]
			internal Single mInertiaForce;
			[SerializeField]
			internal Single mInertiaRange;

			// Параметры перетаскивания
			[NonSerialized]
			internal Boolean mIsDragging;
			[NonSerialized]
			internal Boolean mIsDownDragging;
			[NonSerialized]
			internal Vector2 mDragStartPosition;

			// События
			[NonSerialized]
			internal Action<Vector2> mOnScrollChanged;

			// Служебные данные
			[NonSerialized]
			internal Vector2 mContentOffsetDest;
			[SerializeField]
			internal Rect mRectWorldScreenContentView;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Направления перемещения
			/// </summary>
			public TScrollDirection ScrollDirection
			{
				get
				{
					return mScrollDirection;
				}
				set
				{
					if(mScrollDirection != value)
					{
						mScrollDirection = value;
						this.UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Смещение области контента от верхнего/левого угла
			/// </summary>
			public Vector2 ContentOffset
			{
				get
				{
					return mContentOffset;
				}
				set
				{
					mContentOffset = value;
				}
			}

			/// <summary>
			/// Смещение по X области контента от верхнего/левого угла
			/// </summary>
			public Single ContentOffsetX
			{
				get
				{
					return mContentOffset.x;
				}
				set
				{
					mContentOffset.x = value;
				}
			}

			/// <summary>
			/// Смещение по Y области контента от верхнего/левого угла
			/// </summary>
			public Single ContentOffsetY
			{
				get
				{
					return mContentOffset.y;
				}
				set
				{
					mContentOffset.y = value;
				}
			}

			/// <summary>
			/// Размеры области контента
			/// </summary>
			public Vector2 ContentSize
			{
				get
				{
					return mContentSize;
				}
				set
				{
					mContentSize = value;
				}
			}

			/// <summary>
			/// Ширина области контента
			/// </summary>
			public Single ContentWidth
			{
				get
				{
					return mContentSize.x;
				}
				set
				{
					mContentSize.x = value;
				}
			}

			/// <summary>
			/// Высота области контента
			/// </summary>
			public Single ContentHeight
			{
				get
				{
					return mContentSize.y;
				}
				set
				{
					mContentSize.y = value;
				}
			}

			/// <summary>
			/// Статус инерции после перемещения
			/// </summary>
			public Boolean IsInertia
			{
				get { return mIsInertia; }
				set { mIsInertia = value; }
			}

			/// <summary>
			/// Сила инерции после перемещения
			/// </summary>
			public Single InertiaForce
			{
				get { return mInertiaForce; }
				set { mInertiaForce = value; }
			}

			/// <summary>
			/// Дальность по инерции
			/// </summary>
			public Single InertiaRange
			{
				get { return mInertiaRange; }
				set { mInertiaRange = value; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о перемещении области просмотра
			/// </summary>
			public Action<Vector2> OnScrollChanged
			{
				get { return mOnScrollChanged; }
				set { mOnScrollChanged = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusDraggable ==================================
			/// <summary>
			/// Статус перетаскивания элемента
			/// </summary>
			public Boolean IsDragging
			{
				get { return mIsDragging; }
			}

			/// <summary>
			/// Статус нажатия на перетаскиваемый элемент
			/// </summary>
			public Boolean IsDownDragging
			{
				get { return mIsDownDragging; }
			}

			/// <summary>
			/// Стартовая точки от которой началось перетаскивание
			/// </summary>
			public Vector2 DragStartPositionScreen
			{
				get { return mDragStartPosition; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIScrollView()
				: base()
			{
				mStyleMainName = "ScrollView";
				mRectLocalDesignMain.width = 300;
				mRectLocalDesignMain.height = 300;
				mRectWorldScreenContentView.width = mRectLocalDesignMain.width * 2;
				mRectWorldScreenContentView.height = mRectLocalDesignMain.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIScrollView(String name)
				: base(name)
			{
				mStyleMainName = "ScrollView";
				mRectLocalDesignMain.width = 300;
				mRectLocalDesignMain.height = 300;
				mRectWorldScreenContentView.width = mRectLocalDesignMain.width * 2;
				mRectWorldScreenContentView.height = mRectLocalDesignMain.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIScrollView(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "ScrollView";
				mRectLocalDesignMain.width = 300;
				mRectLocalDesignMain.height = 300;
				mRectWorldScreenContentView.width = mRectLocalDesignMain.width * 2;
				mRectWorldScreenContentView.height = mRectLocalDesignMain.height;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции и размеров элемента по относительным данным
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdatePlacement()
			{
				mRectWorldScreenContentView.width = Mathf.RoundToInt(mContentSize.x * LotusGUIDispatcher.ScaledScreenX);
				mRectWorldScreenContentView.height = Mathf.RoundToInt(mContentSize.y * LotusGUIDispatcher.ScaledScreenY);

				switch (mScrollDirection)
				{
					case TScrollDirection.Vertical:
						{
							mRectWorldScreenContentView.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight) -
								LotusGUIDispatcher.SizeScrollVertical - 2;
						}
						break;
					case TScrollDirection.Horizontal:
						{
							mRectWorldScreenContentView.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom) -
								LotusGUIDispatcher.SizeScrollHorizontal - 2;
						}
						break;
					default:
						break;
				}

				base.UpdatePlacement();
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusUIElement ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента в качестве дочернего
			/// </summary>
			/// <remarks>
			/// Метод не следует вызывать напрямую
			/// </remarks>
			/// <param name="child">Дочерний элемент></param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetChildren(ILotusElement child)
			{
				if (child != null)
				{
					child.SetVisibilityFlags(1);
					mCountChildren++;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена установка элемента в качестве дочернего
			/// </summary>
			/// <remarks>
			/// Метод не следует вызывать напрямую
			/// </remarks>
			/// <param name="child">Дочерний элемент></param>
			//---------------------------------------------------------------------------------------------------------
			public override void UnsetChildren(ILotusElement child)
			{
				if (child != null)
				{
					if (mCountChildren > 0)
					{
						mCountChildren--;
						child.ClearVisibilityFlags(1);
					}
					else
					{
						Debug.LogError("Count children == 0");
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка родительского элемента
			/// </summary>
			/// <remarks>
			/// При абсолютной позиции элемент не меняет своего местоположения относительно экрана
			/// </remarks>
			/// <param name="parent">Родительский элемент</param>
			/// <param name="absolute_pos">Абсолютная позиция элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetParent(ILotusElement parent, Boolean absolute_pos)
			{
				if (parent == null)
				{
					UpdatePlacement();
					mParent.UnsetChildren(this);
					mParent = null;
				}
				else
				{
					if (absolute_pos)
					{

					}
					else
					{
						mParent = parent;
						UpdatePlacement();
						if (mDepth <= mParent.Depth)
						{
							Depth = mParent.Depth + 1;
						}
					}

					mParent.SetChildren(this);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение области для размещения дочерних элементов
			/// </summary>
			/// <returns>Прямоугольник области для размещения дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Rect GetChildRectContent()
			{
				mRectWorldScreenContentView.width = mContentSize.x * LotusGUIDispatcher.ScaledScreenX;
				mRectWorldScreenContentView.height = mContentSize.y * LotusGUIDispatcher.ScaledScreenY;

				switch (mScrollDirection)
				{
					case TScrollDirection.Vertical:
						{
							mRectWorldScreenContentView.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight) - LotusGUIDispatcher.SizeScrollVertical - 2;
						}
						break;
					case TScrollDirection.Horizontal:
						{
							mRectWorldScreenContentView.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom) - LotusGUIDispatcher.SizeScrollHorizontal - 2;
						}
						break;
					default:
						break;
				}

				return mRectWorldScreenContentView;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка/обновление параметров
			/// </summary>
			/// <remarks>
			/// Вызывается центральным диспетчером в момент добавления(регистрации) элемента
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void OnReset()
			{
				base.OnReset();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				switch (mScrollDirection)
				{
					case TScrollDirection.Vertical:
						{
							mContentOffset.y = Mathf.Lerp(mContentOffset.y, mContentOffsetDest.y, Time.unscaledDeltaTime * mInertiaForce);
							if (Approximately(mContentOffset.y, mContentOffsetDest.y, 0.01f))
							{
								IsDirty = false;
							}
						}
						break;
					case TScrollDirection.Horizontal:
						{
							mContentOffset.x = Mathf.Lerp(mContentOffset.x, mContentOffsetDest.x, Time.unscaledDeltaTime * mInertiaForce);
							if (Approximately(mContentOffset.x, mContentOffsetDest.x, 0.01f))
							{
								IsDirty = false;
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				if(mIsInertia)
				{
					OnInertiaMoving();
				}

				mContentOffset = GUI.BeginScrollView(mRectWorldScreenMain, mContentOffset, mRectWorldScreenContentView, false, false,
					GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar);
				{
					if (mCountChildren > 0)
					{
						LotusGUIDispatcher.FromParentDrawElements(this);
					}
				}
				GUI.EndScrollView();
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUIScrollView;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				CGUIScrollView element = base_element as CGUIScrollView;
				if (element != null)
				{
					mScrollDirection = element.mScrollDirection;
					mRectWorldScreenContentView = element.mRectWorldScreenContentView;
					mContentOffset = element.mContentOffset;
					mIsInertia = element.mIsInertia;
					mInertiaForce = element.mInertiaForce;
					mInertiaRange = element.mInertiaRange;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ИНЕРЦИИ ============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение области просмотра по инерции
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnInertiaMoving()
			{
				switch (Event.current.type)
				{
					case EventType.MouseDown:
						{
							// Если мы попали в прямоугольник элемента
							if (mRectWorldScreenMain.Contains(Event.current.mousePosition))
							{
								// Считаем прямоугольник для перемещения исключая области полос прокуртки
								Rect rect = new Rect(mRectWorldScreenMain.x + PaddingLeft,
									mRectWorldScreenMain.y + PaddingTop,
									mRectWorldScreenMain.width - (PaddingLeft + PaddingRight + LotusGUIDispatcher.SizeScrollVertical),
									mRectWorldScreenMain.height - (PaddingTop + PaddingBottom + LotusGUIDispatcher.SizeScrollHorizontal));

								// Если есть попадание
								if (rect.Contains(Event.current.mousePosition))
								{
									mIsDownDragging = true;
									IsDirty = false;
									mDragStartPosition = Event.current.mousePosition;
								}
								else
								{
									// Попали по полосе прокрутки
									// Остановливаем и сбрасываем
									IsDirty = false;
									mContentOffsetDest = mContentOffset;
								}
							}
						}
						break;
					case EventType.MouseUp:
						{
							mIsDownDragging = false;

							// Если есть нажатие в области перемещения
							if (mIsDragging)
							{
								// Включаем инерцию
								IsDirty = true;
								mIsDragging = false;

								switch (mScrollDirection)
								{
									case TScrollDirection.Vertical:
										{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
											mContentOffsetDest.y = mContentOffset.y - Event.current.delta.y * mInertiaRange;
#else
											mContentOffsetDest.y = mContentOffset.y + (Event.current.delta.y) * mInertiaRange;
#endif
										}
										break;
									case TScrollDirection.Horizontal:
										{
											mContentOffsetDest.y = mContentOffset.x - Event.current.delta.x * mInertiaRange;
										}
										break;
									default:
										break;
								}
							}
						}
						break;
					case EventType.MouseDrag:
						{
							// Если есть нажатие в области перемещения
							if (mIsDownDragging && mIsDragging == false)
							{
								// Если будет смещение больше минимального то включаем перемещение 
								switch (mScrollDirection)
								{
									case TScrollDirection.Vertical:
										{
											if (Mathf.Abs(mDragStartPosition.y - Event.current.mousePosition.y) > LotusGUIDispatcher.DraggMinOffset.y)
											{
												mIsDragging = true;
											}
										}
										break;
									case TScrollDirection.Horizontal:
										{
											if (Mathf.Abs(mDragStartPosition.x - Event.current.mousePosition.x) > LotusGUIDispatcher.DraggMinOffset.x)
											{
												mIsDragging = true;
											}
										}
										break;
									default:
										break;
								}
							}

							// Если перемещение
							if (mIsDragging)
							{
								switch (mScrollDirection)
								{
									case TScrollDirection.Vertical:
										{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
											mContentOffset.y -= Event.current.delta.y;
#else
											mContentOffset.y += (Event.current.delta.y);
#endif
										}
										break;
									case TScrollDirection.Horizontal:
										{
											mContentOffset.x -= Event.current.delta.x;
										}
										break;
									default:
										break;
								}
							}
						}
						break;
					default:
						break;
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================