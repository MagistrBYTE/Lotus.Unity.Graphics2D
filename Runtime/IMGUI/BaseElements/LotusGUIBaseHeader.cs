//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseHeader.cs
*		Элементы интерфейса пользователя с отдельной заголовочной областью.
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
		//! \addtogroup Unity2DImmedateGUIBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый элемент с заголовочной областью
		/// </summary>
		/// <remarks>
		/// Элемент содержит область заголовка и область контента, а также отдельный стиль для области заголовка.
		/// Заголовк поддерживает локализацию.
		/// Для рисования используется метод GUI.Box для заголовочной области и метод GUI.Label для фона элемента
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIBaseHeader : CGUIElement, ILotusHeaderElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal TLocalizableText mHeaderText;
			[SerializeField]
			internal Texture2D mHeaderIcon;

			// Параметры размещения
			[SerializeField]
			internal Single mHeaderSize;
			[SerializeField]
			internal THeaderLocation mHeaderLocation;

			// Параметры визуального стиля
			[SerializeField]
			internal String mStyleHeaderName;
			[NonSerialized]
			internal GUIStyle mStyleHeader;

			// Служебные данные
			[NonSerialized]
			internal Single mHeaderSizeCurrent;
			[NonSerialized]
			internal Rect mRectWorldScreenHeader;
			[NonSerialized]
			internal Rect mRectWorldScreenContent;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Текст заголовка
			/// </summary>
			public String HeaderText
			{
				get { return mHeaderText.Text; }
				set { mHeaderText.Text = value; }
			}

			/// <summary>
			/// Текстура иконки заголовка
			/// </summary>
			public Texture2D HeaderIcon
			{
				get { return mHeaderIcon; }
				set { mHeaderIcon = value; }
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Размер заголовочной области
			/// </summary>
			public Single HeaderSize
			{
				get { return mHeaderSize; }
				set
				{
					if (mHeaderSize != value)
					{
						mHeaderSize = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Позиция размещения заголовочной области
			/// </summary>
			public THeaderLocation HeaderLocation
			{
				get { return mHeaderLocation; }
				set
				{
					if (mHeaderLocation != value)
					{
						mHeaderLocation = value;
						UpdatePlacement();
					}
				}
			}

			//
			// ПАРАМЕТРЫ ВИЗУАЛЬНОГО СТИЛЯ
			//
			/// <summary>
			/// Имя стиля для рисования заголовка элемента
			/// </summary>
			public String StyleHeaderName
			{
				get { return mStyleHeaderName; }
				set
				{
					if (mStyleHeaderName != value)
					{
						mStyleHeaderName = value;
						mStyleHeader = LotusGUIDispatcher.FindStyle(mStyleHeaderName);
					}
				}
			}

			/// <summary>
			/// Стиль для рисования заголовка элемента
			/// </summary>
			public GUIStyle StyleHeader
			{
				get
				{
					if (mStyleHeader == null)
					{
						mStyleHeader = LotusGUIDispatcher.FindStyle(mStyleHeaderName);
					}
					return mStyleHeader;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseHeader()
				: base()
			{
				mStyleMainName = "Box";
				mStyleHeaderName = "Box";
				mHeaderText.Text = "";
				mHeaderSize = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseHeader(String name)
				: base(name)
			{
				mStyleMainName = "Box";
				mStyleHeaderName = "Box";
				mHeaderText.Text = "";
				mHeaderSize = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseHeader(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Box";
				mStyleHeaderName = "Box";
				mHeaderText.Text = "";
				mHeaderSize = 30;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
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

				UpdateHeaderPlacement();

				// Считаем дочерние элементы
				if (mCountChildren > 0)
				{
					LotusGUIDispatcher.FromParentComputePositionElements(this);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление размера и положения заголовка
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void UpdateHeaderPlacement()
			{
				switch (mHeaderLocation)
				{
					case THeaderLocation.LeftSide:
						{
							mHeaderSizeCurrent = mHeaderSize * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case THeaderLocation.TopSide:
						{
							mHeaderSizeCurrent = mHeaderSize * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case THeaderLocation.RightSide:
						{
							mHeaderSizeCurrent = mHeaderSize * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case THeaderLocation.BottomSide:
						{
							mHeaderSizeCurrent = mHeaderSize * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case THeaderLocation.LeftTop:
					case THeaderLocation.RightTop:
					case THeaderLocation.LeftBottom:
					case THeaderLocation.RightBottom:
						{
							mHeaderSizeCurrent = mHeaderSize * LotusGUIDispatcher.ScaledScreenAverage;
						}
						break;
					default:
						break;
				}

				switch (mHeaderLocation)
				{
					case THeaderLocation.LeftSide:
						{
							// Заголовок
							mRectWorldScreenHeader.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenHeader.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenHeader.width = mHeaderSizeCurrent;
							mRectWorldScreenHeader.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom);

							// Основное поле
							mRectWorldScreenContent.x = mRectWorldScreenHeader.xMax;
							mRectWorldScreenContent.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenContent.width = mRectWorldScreenMain.width - mHeaderSizeCurrent - (PaddingLeft + PaddingRight);
							mRectWorldScreenContent.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom);
						}
						break;
					case THeaderLocation.TopSide:
						{
							// Заголовок
							mRectWorldScreenHeader.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenHeader.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenHeader.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
							mRectWorldScreenHeader.height = mHeaderSizeCurrent;

							// Основное поле
							mRectWorldScreenContent.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenContent.y = mRectWorldScreenHeader.yMax;
							mRectWorldScreenContent.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
							mRectWorldScreenContent.height = mRectWorldScreenMain.height - (mHeaderSizeCurrent + PaddingTop + PaddingBottom);
						}
						break;
					case THeaderLocation.RightSide:
						{
							// Основное поле
							mRectWorldScreenContent.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenContent.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenContent.width = mRectWorldScreenMain.width - mHeaderSizeCurrent - (PaddingLeft + PaddingRight);
							mRectWorldScreenContent.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom);

							// Заголовок
							mRectWorldScreenHeader.x = mRectWorldScreenContent.xMax + PaddingRight;
							mRectWorldScreenHeader.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenHeader.width = mHeaderSizeCurrent;
							mRectWorldScreenHeader.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom);
						}
						break;
					case THeaderLocation.BottomSide:
						{
							// Основное поле
							mRectWorldScreenContent.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenContent.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenContent.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
							mRectWorldScreenContent.height = mRectWorldScreenMain.height - mHeaderSizeCurrent - (PaddingTop + PaddingBottom);

							// Заголовок
							mRectWorldScreenHeader.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenHeader.y = mRectWorldScreenMain.yMax - PaddingBottom - mHeaderSizeCurrent;
							mRectWorldScreenHeader.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
							mRectWorldScreenHeader.height = mHeaderSizeCurrent;
						}
						break;
					case THeaderLocation.LeftTop:
						{
							// Основное поле
							mRectWorldScreenContent.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenContent.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenContent.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
							mRectWorldScreenContent.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom);

							// Заголовок
							mRectWorldScreenHeader.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenHeader.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenHeader.width = mHeaderSizeCurrent;
							mRectWorldScreenHeader.height = mHeaderSizeCurrent;
						}
						break;
					case THeaderLocation.RightTop:
						{
							// Основное поле
							mRectWorldScreenContent.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenContent.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenContent.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
							mRectWorldScreenContent.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom);

							// Заголовок
							mRectWorldScreenHeader.x = mRectWorldScreenMain.xMax - PaddingRight - mHeaderSizeCurrent;
							mRectWorldScreenHeader.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenHeader.width = mHeaderSizeCurrent;
							mRectWorldScreenHeader.height = mHeaderSizeCurrent;
						}
						break;
					case THeaderLocation.LeftBottom:
						{
							// Основное поле
							mRectWorldScreenContent.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenContent.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenContent.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
							mRectWorldScreenContent.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom);

							// Заголовок
							mRectWorldScreenHeader.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenHeader.y = mRectWorldScreenMain.yMax - PaddingBottom - mHeaderSizeCurrent;
							mRectWorldScreenHeader.width = mHeaderSizeCurrent;
							mRectWorldScreenHeader.height = mHeaderSizeCurrent;
						}
						break;
					case THeaderLocation.RightBottom:
						{
							// Основное поле
							mRectWorldScreenContent.x = mRectWorldScreenMain.x + PaddingLeft;
							mRectWorldScreenContent.y = mRectWorldScreenMain.y + PaddingTop;
							mRectWorldScreenContent.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
							mRectWorldScreenContent.height = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom);

							// Заголовок
							mRectWorldScreenHeader.x = mRectWorldScreenMain.xMax - PaddingRight - mHeaderSizeCurrent;
							mRectWorldScreenHeader.y = mRectWorldScreenMain.yMax - PaddingBottom - mHeaderSizeCurrent;
							mRectWorldScreenHeader.width = mHeaderSizeCurrent;
							mRectWorldScreenHeader.height = mHeaderSizeCurrent;
						}
						break;
					default:
						break;
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
				return mRectWorldScreenContent;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusDataExchange =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка данных
			/// </summary>
			/// <param name="text">Текст</param>
			/// <param name="icon">Иконка изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetData(String text, Texture2D icon)
			{
				mHeaderText.Text = text;
				mHeaderIcon = icon;
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

				mStyleHeader = LotusGUIDispatcher.FindStyle(mStyleHeaderName);
				mStyleHeaderName = mStyleHeader.name;
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
				if (mStyleHeader == null) mStyleHeader = LotusGUIDispatcher.FindStyle(mStyleHeaderName);
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

				LotusGUIDispatcher.CurrentContent.text = mHeaderText.Text;
				LotusGUIDispatcher.CurrentContent.image = mHeaderIcon;

				// Общий фон
				GUI.Label(mRectWorldScreenMain, "", mStyleMain);

				// Заголовок
				GUI.Box(mRectWorldScreenHeader, LotusGUIDispatcher.CurrentContent, mStyleHeader);
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
				return MemberwiseClone() as CGUIBaseHeader;
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

				CGUIBaseHeader base_header = base_element as CGUIBaseHeader;
				if (base_header != null)
				{
					mHeaderText = base_header.mHeaderText;
					mHeaderIcon = base_header.mHeaderIcon;
					mStyleHeaderName = base_header.mStyleHeaderName;
					mStyleHeader = base_header.mStyleHeader;
					mHeaderSize = base_header.mHeaderSize;
					mHeaderLocation = base_header.mHeaderLocation;
					mRectWorldScreenHeader = base_header.mRectWorldScreenHeader;
					mRectWorldScreenContent = base_header.mRectWorldScreenContent;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Панель с заголовочной областью и текстом
		/// </summary>
		/// <remarks>
		/// Заголовок и основной текст поддерживают локализацию.
		/// Элемент содержит область заголовка и область контента, а также отдельный стиль для области заголовка и текста
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIPanelHeader : CGUIBaseHeader
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal TLocalizableText mContentText;

			// Параметры визуального стиля
			[SerializeField]
			internal String mStyleContentName;
			[NonSerialized]
			internal GUIStyle mStyleContent;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Основной текст элемента
			/// </summary>
			public String ContentText
			{
				get { return mContentText.Text; }
				set { mContentText.Text = value; }
			}

			//
			// ПАРАМЕТРЫ ВИЗУАЛЬНОГО СТИЛЯ
			//
			/// <summary>
			/// Имя стиля для рисования заголовка элемента
			/// </summary>
			public String StyleContentName
			{
				get { return mStyleContentName; }
				set
				{
					if (mStyleContentName != value)
					{
						mStyleContentName = value;
						mStyleContent = LotusGUIDispatcher.FindStyle(mStyleContentName);
					}
				}
			}

			/// <summary>
			/// Стиль для рисования заголовка элемента
			/// </summary>
			public GUIStyle StyleContent
			{
				get
				{
					if (mStyleContent == null)
					{
						mStyleContent = LotusGUIDispatcher.FindStyle(mStyleContentName);
					}
					return mStyleContent;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanelHeader()
				: base()
			{
				mContentText.Text = "";
				mStyleContentName = "Label";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanelHeader(String name)
				: base(name)
			{
				mContentText.Text = "";
				mStyleContentName = "Label";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanelHeader(String name, Single x, Single y)
				: base(name, x, y)
			{
				mContentText.Text = "";
				mStyleContentName = "Label";
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
			public CGUIPanelHeader(String name, Single x, Single y, String text)
				: base(name, x, y)
			{
				mContentText.Text = text;
				mStyleContentName = "Label";
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusDataExchange =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка данных
			/// </summary>
			/// <param name="text">Текст</param>
			/// <param name="icon">Иконка изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetData(String text, Texture2D icon)
			{
				mContentText.Text = text;
				mHeaderIcon = icon;
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

				mStyleHeader = LotusGUIDispatcher.FindStyle(mStyleHeaderName);
				mStyleHeaderName = mStyleHeader.name;

				mStyleContent = LotusGUIDispatcher.FindStyle(mStyleContentName);
				mStyleContentName = mStyleContent.name;
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
				if (mStyleHeader == null) mStyleHeader = LotusGUIDispatcher.FindStyle(mStyleHeaderName);
				if (mStyleContent == null) mStyleContent = LotusGUIDispatcher.FindStyle(mStyleContentName);
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

				// Общий фон
				GUI.Label(mRectWorldScreenMain, "", mStyleMain);

				// Текст
				GUI.Label(mRectWorldScreenContent, mContentText.Text, mStyleContent);

				// Заголовок
				LotusGUIDispatcher.CurrentContent.text = mHeaderText.Text;
				LotusGUIDispatcher.CurrentContent.image = mHeaderIcon;
				GUI.Box(mRectWorldScreenHeader, LotusGUIDispatcher.CurrentContent, mStyleHeader);
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
				return MemberwiseClone() as CGUIPanelHeader;
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

				CGUIPanelHeader panel_header = base_element as CGUIPanelHeader;
				if (panel_header != null)
				{
					mContentText = panel_header.mContentText;
					mStyleContentName = panel_header.mStyleContentName;
					mStyleContent = panel_header.mStyleContent;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КОНТЕНТОМ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeSizeFromContent()
			{
				LotusGUIDispatcher.CurrentContent.text = mContentText.Text;
				LotusGUIDispatcher.CurrentContent.image = null;

				// Получаем размер
				Vector2 size = mStyleContent.CalcSize(LotusGUIDispatcher.CurrentContent);

				// Теперь обратно масштабируем в зависимости от режима масштаба
				switch (mAspectMode)
				{
					case TAspectMode.None:
						{
							mRectLocalDesignMain.width = RoundToNearest(size.x, 10);
							mRectLocalDesignMain.height = RoundToNearest(size.y + HeaderSize, 10);
						}
						break;
					case TAspectMode.Proportional:
						{
							mRectLocalDesignMain.width = RoundToNearest(size.x, 10) / LotusGUIDispatcher.ScaledScreenX;
							mRectLocalDesignMain.height = RoundToNearest(size.y + HeaderSize, 10) / LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mRectLocalDesignMain.width = RoundToNearest(size.x, 10) / LotusGUIDispatcher.ScaledScreenX;
							mRectLocalDesignMain.height = RoundToNearest(size.y + HeaderSize, 10) / LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mRectLocalDesignMain.width = RoundToNearest(size.x, 10) / LotusGUIDispatcher.ScaledScreenY;
							mRectLocalDesignMain.height = RoundToNearest(size.y + HeaderSize, 10) / LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeWidthFromContent()
			{
				LotusGUIDispatcher.CurrentContent.text = mContentText.Text;
				LotusGUIDispatcher.CurrentContent.image = null;

				Single min_width = 0, max_width = 0;
				mStyleContent.CalcMinMaxWidth(LotusGUIDispatcher.CurrentContent, out min_width, out max_width);

				// Теперь обратно масштабируем в зависимости от режима масштаба
				switch (mAspectMode)
				{
					case TAspectMode.None:
						{
							mRectLocalDesignMain.width = RoundToNearest((min_width + max_width) / 2, 10);
						}
						break;
					case TAspectMode.Proportional:
						{
							mRectLocalDesignMain.width = RoundToNearest((min_width + max_width) / 2, 10) / LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mRectLocalDesignMain.width = RoundToNearest((min_width + max_width) / 2, 10) / LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mRectLocalDesignMain.width = RoundToNearest((min_width + max_width) / 2, 10) / LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeHeightFromContent()
			{
				LotusGUIDispatcher.CurrentContent.text = mContentText.Text;
				LotusGUIDispatcher.CurrentContent.image = null;

				// Считаем ширину контента
				Single width_content = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);

				// Высота
				Single height = mStyleContent.CalcHeight(LotusGUIDispatcher.CurrentContent, width_content);

				// Требуемая высота
				Single required_height = PaddingTop + mHeaderSize + height + PaddingBottom;

				// Теперь обратно масштабируем в зависимости от режима масштаба
				switch (mAspectMode)
				{
					case TAspectMode.None:
						{
							mRectLocalDesignMain.height = required_height;
						}
						break;
					case TAspectMode.Proportional:
						{
							mRectLocalDesignMain.height = RoundToNearest(required_height / LotusGUIDispatcher.ScaledScreenY, 4);
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mRectLocalDesignMain.height = RoundToNearest(required_height / LotusGUIDispatcher.ScaledScreenX, 4);
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mRectLocalDesignMain.height = RoundToNearest(required_height / LotusGUIDispatcher.ScaledScreenY, 4);
						}
						break;
					default:
						break;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Cворачиваемая панель
		/// </summary>
		/// <remarks>
		/// Элемент содержит область заголовка и область контента, а также отдельный стиль для области заголовка.
		/// Заголовк поддерживает локализацию.
		/// Cворачиваемая панель представляет собой панель, которая может находится в развернутом состоянии, когда видны все
		/// её дочерние элементы, и свернутом состоянии, когда видно лишь её заголовок.
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIPanelSpoiler : CGUIBaseHeader
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsOpened;
			[SerializeField]
			internal Single mDuration;
			[SerializeField]
			internal Single mSizeView;

			// События
			internal Action<Boolean> mOnOpened;

			// Служебные данные
			[NonSerialized]
			internal Vector2 mCollapseSize;
			[NonSerialized]
			internal Vector2 mOpenedSize;
			[NonSerialized]
			internal Single mStartTime;
			[NonSerialized]
			internal Boolean mIsOpening;
			[NonSerialized]
			internal Boolean mIsClosing;
			[NonSerialized]
			internal Single mSizeViewCurrent;
			[NonSerialized]
			internal Rect mRectWorldScreenView;
			[NonSerialized]
			internal Rect mRectWorldScreenButton;
			[NonSerialized]
			internal Vector2 mScrollPos;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус открытия панели
			/// </summary>
			public Boolean IsOpened
			{
				get { return mIsOpened; }
				set
				{
					if(mIsOpened != value)
					{
						if(value)
						{
							Opening();
						}
						else
						{
							Closing();
						}
					}
				}
			}

			/// <summary>
			/// Продолжительность открытия/закрытия панели в секундах
			/// </summary>
			public Single Duration
			{
				get { return mDuration; }
				set { mDuration = value; }
			}

			/// <summary>
			/// Размер области видимости панели
			/// </summary>
			public Single SizeView
			{
				get { return mSizeView; }
				set { mSizeView = value; }
			}

			/// <summary>
			/// Статус открывания панели
			/// </summary>
			public Boolean IsOpening
			{
				get { return mIsOpening; }
			}

			/// <summary>
			/// Статус закрытия панели
			/// </summary>
			public Boolean IsClosing
			{
				get { return mIsClosing; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о раскрытии/закрытии элемента. Аргумент - статус раскрытия
			/// </summary>
			public Action<Boolean> OnOpened
			{
				get { return mOnOpened; }
				set
				{
					mOnOpened = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanelSpoiler()
				: base()
			{
				mSizeView = 600;
				mDuration = 0.3f;
				mIsOpened = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanelSpoiler(String name)
				: base(name)
			{
				mSizeView = 600;
				mDuration = 0.3f;
				mIsOpened = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanelSpoiler(String name, Single x, Single y)
				: base(name, x, y)
			{
				mSizeView = 600;
				mDuration = 0.3f;
				mIsOpened = true;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
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
				// Основа позиции
				base.UpdatePlacement();

				// Положение кнопки
				UpdateButtonExpandedPlacement();

				switch (mHeaderLocation)
				{
					case THeaderLocation.LeftSide:
						{
							mSizeViewCurrent = mSizeView * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case THeaderLocation.TopSide:
						{
							mSizeViewCurrent = mSizeView * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case THeaderLocation.RightSide:
						{
							mSizeViewCurrent = mSizeView * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case THeaderLocation.BottomSide:
						{
							mSizeViewCurrent = mSizeView * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case THeaderLocation.LeftTop:
					case THeaderLocation.RightTop:
					case THeaderLocation.LeftBottom:
					case THeaderLocation.RightBottom:
						{
							mSizeViewCurrent = mSizeView * LotusGUIDispatcher.ScaledScreenAverage;
						}
						break;
					default:
						break;
				}

				// Область просмотра
				switch (mHeaderLocation)
				{
					case THeaderLocation.RightSide:
					case THeaderLocation.LeftSide:
						{
							mRectWorldScreenView.width = mSizeViewCurrent;
							mRectWorldScreenView.height = mRectWorldScreenContent.height;

							// Если есть полоса прокрутки
							if (mRectWorldScreenView.width > mRectWorldScreenContent.width - 2)
							{
								mRectWorldScreenView.height -= LotusGUIDispatcher.SizeScrollHorizontal;
							}
						}
						break;
					case THeaderLocation.TopSide:
					case THeaderLocation.BottomSide:
						{
							mRectWorldScreenView.width = mRectWorldScreenContent.width;
							mRectWorldScreenView.height = mSizeViewCurrent;

							// Если есть полоса прокрутки
							if (mRectWorldScreenView.height > mRectWorldScreenContent.height - 2)
							{
								mRectWorldScreenView.width -= LotusGUIDispatcher.SizeScrollVertical;
							}
						}
						break;
					case THeaderLocation.LeftTop:
					case THeaderLocation.RightTop:
					case THeaderLocation.LeftBottom:
					case THeaderLocation.RightBottom:
						{
							mRectWorldScreenView.width = mSizeViewCurrent;
							mRectWorldScreenView.height = mSizeViewCurrent;

							// Если есть полоса прокрутки
							if (mRectWorldScreenView.height > mRectWorldScreenContent.height - 2)
							{
								mRectWorldScreenView.width -= LotusGUIDispatcher.SizeScrollVertical;
							}

							// Если есть полоса прокрутки
							if (mRectWorldScreenView.width > mRectWorldScreenContent.width - 2)
							{
								mRectWorldScreenView.height -= LotusGUIDispatcher.SizeScrollHorizontal;
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление размера и положения кнопки свернуть
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void UpdateButtonExpandedPlacement()
			{
				// Кнопка свернуть
				mRectWorldScreenButton.width = mHeaderSizeCurrent - 4;
				mRectWorldScreenButton.height = mHeaderSizeCurrent - 4;
				mRectWorldScreenButton.y = mRectWorldScreenHeader.y + 2;
				mRectWorldScreenButton.x = mRectWorldScreenHeader.xMax - mRectWorldScreenButton.width;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusElement ======================================
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
					mCountChildren++;
					child.SetVisibilityFlags(1);
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
				return mRectWorldScreenView;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Открытие панели
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void Opening()
			{
				mIsOpened = true;
				mIsOpening = true;
				mIsClosing = false;
				mStartTime = Time.time;
				IsDirty = true;

				if (mOnOpened != null) mOnOpened(true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Закрытие панели
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void Closing()
			{
				mIsClosing = true;
				mIsOpening = false;
				mStartTime = Time.time;
				IsDirty = true;
			}

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

				mOpenedSize = mRectLocalDesignMain.size;

				mCollapseSize.x = mHeaderSize + PaddingLeft + PaddingTop;
				mCollapseSize.y = mHeaderSize + PaddingTop + PaddingBottom;
				mIsOpened = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				// Открытие
				if (mIsOpening)
				{
					// Считаем время
					Single delta_time = (Time.time - mStartTime) / mDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mIsOpening = false;
						IsDirty = false;
					}

					switch (mHeaderLocation)
					{
						case THeaderLocation.LeftSide:
							{
								Width = Mathf.Lerp(mCollapseSize.x, mOpenedSize.x, delta_time);
							}
							break;
						case THeaderLocation.TopSide:
							{
								Height = Mathf.Lerp(mCollapseSize.y, mOpenedSize.y, delta_time);
							}
							break;
						case THeaderLocation.RightSide:
							{
								Width = Mathf.Lerp(mCollapseSize.x, mOpenedSize.x, delta_time);
							}
							break;
						case THeaderLocation.BottomSide:
							{
								Height = Mathf.Lerp(mCollapseSize.y, mOpenedSize.y, delta_time);
							}
							break;
						case THeaderLocation.LeftTop:
						case THeaderLocation.RightTop:
						case THeaderLocation.LeftBottom:
						case THeaderLocation.RightBottom:
							{
								Width = Mathf.Lerp(mCollapseSize.x, mOpenedSize.x, delta_time);
								Height = Mathf.Lerp(mCollapseSize.y, mOpenedSize.y, delta_time);
							}
							break;
						default:
							break;
					}
				}

				// Закрытие
				if (mIsClosing)
				{
					// Считаем время
					Single delta_time = (Time.time - mStartTime) / mDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mIsClosing = false;
						IsDirty = false;

						// Сообщить элементам что они невидимы
						if (mOnOpened != null) mOnOpened(false);
					}

					switch (mHeaderLocation)
					{
						case THeaderLocation.LeftSide:
							{
								Width = Mathf.Lerp(mOpenedSize.x, mCollapseSize.x, delta_time);

								// Пока панель не до конца закрыта
								if (mIsOpened && Width < HeaderSize * 2)
								{
									mIsOpened = false;
								}
							}
							break;
						case THeaderLocation.TopSide:
							{
								Height = Mathf.Lerp(mOpenedSize.y, mCollapseSize.y, delta_time);

								// Пока панель не до конца закрыта
								if (mIsOpened && Height < HeaderSize * 2)
								{
									mIsOpened = false;
								}
							}
							break;
						case THeaderLocation.RightSide:
							{
								Width = Mathf.Lerp(mOpenedSize.x, mCollapseSize.x, delta_time);

								// Пока панель не до конца закрыта
								if (mIsOpened && Width < HeaderSize * 2)
								{
									mIsOpened = false;
								}
							}
							break;
						case THeaderLocation.BottomSide:
							{
								Height = Mathf.Lerp(mOpenedSize.y, mCollapseSize.y, delta_time);

								// Пока панель не до конца закрыта
								if (mIsOpened && Height < HeaderSize * 2)
								{
									mIsOpened = false;
								}
							}
							break;
						case THeaderLocation.LeftTop:
						case THeaderLocation.RightTop:
						case THeaderLocation.LeftBottom:
						case THeaderLocation.RightBottom:
							{
								Width = Mathf.Lerp(mOpenedSize.x, mCollapseSize.x, delta_time);
								Height = Mathf.Lerp(mOpenedSize.y, mCollapseSize.y, delta_time);

								// Пока панель не до конца закрыта
								if (mIsOpened && Height < HeaderSize * 2)
								{
									mIsOpened = false;
								}
							}
							break;
						default:
							break;
					}
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

				// Общий фон
				GUI.Label(mRectWorldScreenMain, "", mStyleMain);

				// Выводим область списка
				if (mIsOpened)
				{
					mScrollPos = GUI.BeginScrollView(mRectWorldScreenContent, mScrollPos, mRectWorldScreenView, false, false);
					{
						if (mCountChildren > 0)
						{
							LotusGUIDispatcher.FromParentDrawElements(this);
						}
					}
					GUI.EndScrollView();
				}

				// Заголовок
				LotusGUIDispatcher.CurrentContent.text = mHeaderText.Text;
				LotusGUIDispatcher.CurrentContent.image = mHeaderIcon;
				GUI.Box(mRectWorldScreenHeader, LotusGUIDispatcher.CurrentContent, mStyleHeader);

				// Кнопка закрыть
				LotusGUIDispatcher.CurrentContent.text = mIsOpened ? XString.Plus : XString.Minus;
				LotusGUIDispatcher.CurrentContent.image = null;
				if (GUI.Button(mRectWorldScreenButton, LotusGUIDispatcher.CurrentContent))
				{
					IsOpened = !IsOpened;
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
				return MemberwiseClone() as CGUIPanelSpoiler;
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

				CGUIPanelSpoiler panel_spoiler = base_element as CGUIPanelSpoiler;
				if (panel_spoiler != null)
				{
					mDuration = panel_spoiler.mDuration;
					mSizeView = panel_spoiler.mSizeView;
					mCollapseSize = panel_spoiler.mCollapseSize;
					mOpenedSize = panel_spoiler.mOpenedSize;

					mStartTime = panel_spoiler.mStartTime;
					mIsOpening = panel_spoiler.mIsOpening;
					mIsClosing = panel_spoiler.mIsClosing;
					mRectWorldScreenView = panel_spoiler.mRectWorldScreenView;
					mRectWorldScreenButton = panel_spoiler.mRectWorldScreenButton;
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