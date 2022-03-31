//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonItemsContent.cs
*		Элементы интерфейса пользователя со списком однотипных данных.
*		Реализация элементов интерфейса пользователя со списком однотипных данных обеспечивающих различный режим отображения.
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
		//! \addtogroup Unity2DImmedateGUIControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент списка однотипных данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIContentItem : IComparable<CGUIContentItem>
		{
			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal String mText;
			[SerializeField]
			internal Texture2D mIcon;
			[NonSerialized]
			internal Int32 mData;
			[NonSerialized]
			internal Int32 mIndex;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Основной текст элемента
			/// </summary>
			public String Text
			{
				get { return mText; }
				set { mText = value; }
			}

			/// <summary>
			/// Текстура иконки изображения элемента
			/// </summary>
			public Texture2D Icon
			{
				get { return mIcon; }
				set { mIcon = value; }
			}

			/// <summary>
			/// Дополнительные данные элемента списка
			/// </summary>
			/// <remarks>
			/// Дополнительные данные используется для фильтрования элементов списка
			/// </remarks>
			public Int32 Data
			{
				get { return mData; }
				set { mData = value; }
			}

			/// <summary>
			/// Индекс элемента списка
			/// </summary>
			public Int32 Index
			{
				get { return mIndex; }
				set { mIndex = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentItem()
			{
				mText = "";
				mIcon = null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentItem(String text)
			{
				mText = text;
				mIcon = null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="icon">Текстура иконки</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentItem(Texture2D icon)
			{
				mText = "";
				mIcon = icon;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="text">Текст элемента</param>
			/// <param name="icon">Текстура иконки</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentItem(String text, Texture2D icon)
			{
				mText = text;
				mIcon = icon;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов списка
			/// </summary>
			/// <param name="other">Элемент списка</param>
			/// <returns>Статус сравнения элементов списка</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CGUIContentItem other)
			{
				if (mData > other.mData)
				{
					return 1;
				}
				else
				{
					if (mData < other.mData)
					{
						return -1;
					}
					else
					{
						return 0;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return mText;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual CGUIContentItem Duplicate()
			{
				return MemberwiseClone() as CGUIContentItem;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс элементы интерфейса - контейнер для храненения однотипного списка данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIContainerContents : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal List<CGUIContentItem> mContentItems;
			[NonSerialized]
			internal Int32 mSelectedIndex;

			// Служебные данные
			[NonSerialized]
			internal GUIContent[] mContents;

			// События
			internal Action<Int32> mOnSelectedIndex;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Список элементов
			/// </summary>
			public List<CGUIContentItem> ContentItems
			{
				get { return mContentItems; }
			}

			/// <summary>
			/// Массив контента элементов
			/// </summary>
			public GUIContent[] Contents
			{
				get { return mContents; }
				set
				{
					mContents = value;
				}
			}

			/// <summary>
			/// Индекс выбранного элемента
			/// </summary>
			public Int32 SelectedIndex
			{
				get { return mSelectedIndex; }
				set
				{
					if (mSelectedIndex != value)
					{
						mSelectedIndex = value;
						if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
					}
				}
			}

			/// <summary>
			/// Выбранный элемент
			/// </summary>
			public CGUIContentItem SelectedItem
			{
				get { return mContentItems[mSelectedIndex]; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о выборе элемента. Аргумент - индекс выбранного элемента
			/// </summary>
			public Action<Int32> OnSelectedIndex
			{
				get { return mOnSelectedIndex; }
				set
				{
					mOnSelectedIndex = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContainerContents()
				: base()
			{
				mContentItems = new List<CGUIContentItem>();
				mContentItems.Add(new CGUIContentItem("Item"));
				mContents = new GUIContent[1];
				mContents[0] = new GUIContent (mContentItems[0].Text);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContainerContents(String name)
				: base(name)
			{
				mContentItems = new List<CGUIContentItem>();
				mContentItems.Add(new CGUIContentItem("Item"));
				mContents = new GUIContent[1];
				mContents[0] = new GUIContent(mContentItems[0].Text);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContainerContents(String name, Single x, Single y)
				: base(name, x, y)
			{
				mContentItems = new List<CGUIContentItem>();
				mContentItems.Add(new CGUIContentItem("Item"));
				mContents = new GUIContent[1];
				mContents[0] = new GUIContent(mContentItems[0].Text);
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
				this.ResetFromList();
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
				CGUIContainerContents container = MemberwiseClone() as CGUIContainerContents;

				// Копируем список данных
				container.mContentItems = new List<CGUIContentItem>();
				for (Int32 i = 0; i < mContentItems.Count; i++)
				{
					container.mContentItems.Add(mContentItems[i].Duplicate());
				}
				return container;
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

				CGUIContainerContents container_contents = base_element as CGUIContainerContents;
				if (container_contents != null)
				{
					// Копируем список данных
					mContentItems = new List<CGUIContentItem>();
					for (Int32 i = 0; i < container_contents.mContentItems.Count; i++)
					{
						mContentItems.Add(container_contents.mContentItems[i].Duplicate());
					}
				}
			}
			#endregion

			#region ======================================= РАБОТА С ДОЧЕРНИМИ ЭЛЕМЕНТАМИ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных элементов по списку элементов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ResetFromList()
			{
				if (mContents == null || mContents.Length != mContentItems.Count)
				{
					mContents = new GUIContent[mContentItems.Count];

					for (Int32 i = 0; i < mContentItems.Count; i++)
					{
						mContents[i] = new GUIContent(mContentItems[i].Text, mContentItems[i].Icon);
					}
				}
				else
				{
					for (Int32 i = 0; i < mContentItems.Count; i++)
					{
						mContents[i].text = mContentItems[i].Text;
						mContents[i].image = mContentItems[i].Icon;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных элементов по массиву контента элементов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ResetFromContent()
			{
				if (mContents.Length != mContentItems.Count)
				{
					mContentItems.Clear();

					for (Int32 i = 0; i < mContents.Length; i++)
					{
						CGUIContentItem item = new CGUIContentItem(mContents[i].text, mContents[i].image as Texture2D);
						mContentItems.Add(item);
					}
				}
				else
				{
					for (Int32 i = 0; i < mContents.Length; i++)
					{
						mContentItems[i].Text = mContents[i].text;
						mContentItems[i].Icon = mContents[i].image as Texture2D;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элемента
			/// </summary>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(String text)
			{
				mContentItems.Add(new CGUIContentItem(text));
				ResetFromList();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элемента
			/// </summary>
			/// <param name="icon">Текстура иконки</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(Texture2D icon)
			{
				mContentItems.Add(new CGUIContentItem(icon));
				ResetFromList();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элемента
			/// </summary>
			/// <param name="text">Текст элемента</param>
			/// <param name="icon">Текстура иконки</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(String text, Texture2D icon)
			{
				mContentItems.Add(new CGUIContentItem(icon));
				ResetFromList();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента по индексу
			/// </summary>
			/// <param name="index">Индекс удаляемого элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void Remove(Int32 index)
			{
				mContentItems.RemoveAt(index);
				ResetFromList();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента
			/// </summary>
			/// <param name="item">Удаляемый элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public void Remove(CGUIContentItem item)
			{
				mContentItems.Remove(item);
				ResetFromList();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление всех элементов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				mContentItems.Clear();
				mContents = null;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Панель инструментов
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Toolbar
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIToolbarContents : CGUIContainerContents
		{
			#region ======================================= ДАННЫЕ ====================================================
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIToolbarContents()
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
			public CGUIToolbarContents(String name)
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
			public CGUIToolbarContents(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Button";
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				GUI.changed = false;
				mSelectedIndex = GUI.Toolbar(mRectWorldScreenMain, mSelectedIndex, mContents, mStyleMain, GUI.ToolbarButtonSize.Fixed);
				if (GUI.changed)
				{
					if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
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
				CGUIToolbarContents container = MemberwiseClone() as CGUIToolbarContents;

				// Копируем список данных
				container.mContentItems = new List<CGUIContentItem>();
				for (Int32 i = 0; i < mContentItems.Count; i++)
				{
					container.mContentItems.Add(mContentItems[i].Duplicate());
				}
				return container;
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
		/// Элемент обеспечивающий просмотр списка данных в виде сетки
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.SelectionGrid
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIGridContents : CGUIContainerContents
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mCountColumn = 1;
			[SerializeField]
			internal Boolean mIsSelectedEmpty;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Количество колонок сетки
			/// </summary>
			public Int32 CountColumn
			{
				get { return mCountColumn; }
				set
				{
					mCountColumn = value;
				}
			}

			/// <summary>
			/// Статус возможности убрать выделения с таблицы
			/// </summary>
			public Boolean IsSelectedEmpty
			{
				get { return mIsSelectedEmpty; }
				set
				{
					mIsSelectedEmpty = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIGridContents()
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
			public CGUIGridContents(String name)
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
			public CGUIGridContents(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Button";
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				GUI.changed = false;
				mSelectedIndex = GUI.SelectionGrid(mRectWorldScreenMain, mSelectedIndex, mContents, mCountColumn, mStyleMain);
				if (GUI.changed)
				{
					if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
				}

				// Сбрасываем выбор
				if (mIsSelectedEmpty)
				{
					if (Event.current.type == EventType.MouseDown && mRectWorldScreenMain.Contains(Event.current.mousePosition) == false)
					{
						if (mSelectedIndex != -1)
						{
							mSelectedIndex = -1;
							if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
						}
					}
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
				CGUIGridContents container = MemberwiseClone() as CGUIGridContents;

				// Копируем список данных
				container.mContentItems = new List<CGUIContentItem>();
				for (Int32 i = 0; i < mContentItems.Count; i++)
				{
					container.mContentItems.Add(mContentItems[i].Duplicate());
				}
				return container;
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

				CGUIGridContents grid_contents = base_element as CGUIGridContents;
				if (grid_contents != null)
				{
					mCountColumn = grid_contents.mCountColumn;
					mIsSelectedEmpty = grid_contents.mIsSelectedEmpty;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент обеспечивающий просмотр списка данных в виде прокрутки
		/// </summary>
		/// <remarks>
		/// Поддерживается различное расположение кнопок прокрутки
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUISpinnerContents : CGUIContainerContents
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Параметры размещения
			[SerializeField]
			internal Single mButtonSize;
			[SerializeField]
			internal TSpinnerButtonLocation mButtonLocation;

			// Служебные данные
			[NonSerialized]
			internal Single mButtonSizeCurrent;
			[NonSerialized]
			internal Rect mRectWorldScreenButtonNext;
			[NonSerialized]
			internal Rect mRectWorldScreenButtonPrev;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Размер управлющих кнопок счетчика
			/// </summary>
			public Single ButtonSize
			{
				get { return mButtonSize; }
				set
				{
					if (mButtonSize != value)
					{
						mButtonSize = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Позиция управляющих кнопок счетчика
			/// </summary>
			public TSpinnerButtonLocation ButtonLocation
			{
				get { return mButtonLocation; }
				set
				{
					if (mButtonLocation != value)
					{
						mButtonLocation = value;
						UpdatePlacement();
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUISpinnerContents()
				: base()
			{
				mStyleMainName = "Button";
				mButtonSize = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISpinnerContents(String name)
				: base(name)
			{
				mStyleMainName = "Button";
				mButtonSize = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISpinnerContents(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Button";
				mButtonSize = 30;
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
				base.UpdatePlacement();

				switch (mButtonLocation)
				{
					case TSpinnerButtonLocation.RightVertical:
					case TSpinnerButtonLocation.LeftVertical:
					case TSpinnerButtonLocation.BothSide:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TSpinnerButtonLocation.TopHorizontal:
					case TSpinnerButtonLocation.BottomHorizontal:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}

				switch (mButtonLocation)
				{
					case TSpinnerButtonLocation.RightVertical:
						{
							// Смещаем ширину поля на ширину управляющих кнопок
							mRectWorldScreenMain.width -= mButtonSizeCurrent;

							// Кнопка вверх
							mRectWorldScreenButtonNext.x = mRectWorldScreenMain.xMax;
							mRectWorldScreenButtonNext.y = mRectWorldScreenMain.y;
							mRectWorldScreenButtonNext.width = mButtonSizeCurrent;
							mRectWorldScreenButtonNext.height = mRectWorldScreenMain.height / 2;

							// Кнопка вниз
							mRectWorldScreenButtonPrev.x = mRectWorldScreenMain.xMax;
							mRectWorldScreenButtonPrev.y = mRectWorldScreenMain.y + mRectWorldScreenMain.height / 2;
							mRectWorldScreenButtonPrev.width = mButtonSizeCurrent;
							mRectWorldScreenButtonPrev.height = mRectWorldScreenMain.height / 2;
						}
						break;
					case TSpinnerButtonLocation.LeftVertical:
						{
							// Смещаем позицию поля на ширину управляющих кнопок
							mRectWorldScreenMain.x += mButtonSizeCurrent;
							mRectWorldScreenMain.width -= mButtonSizeCurrent;

							// Кнопка вверх
							mRectWorldScreenButtonNext.x = mRectWorldScreenMain.x - mButtonSizeCurrent;
							mRectWorldScreenButtonNext.y = mRectWorldScreenMain.y;
							mRectWorldScreenButtonNext.width = mButtonSizeCurrent;
							mRectWorldScreenButtonNext.height = mRectWorldScreenMain.height / 2;

							// Кнопка вниз
							mRectWorldScreenButtonPrev.x = mRectWorldScreenMain.x - mButtonSizeCurrent;
							mRectWorldScreenButtonPrev.y = mRectWorldScreenMain.y + mRectWorldScreenMain.height / 2;
							mRectWorldScreenButtonPrev.width = mButtonSizeCurrent;
							mRectWorldScreenButtonPrev.height = mRectWorldScreenMain.height / 2;
						}
						break;
					case TSpinnerButtonLocation.BothSide:
						{
							// Смещаем позицию поля на ширину управляющих кнопок
							mRectWorldScreenMain.x += mButtonSizeCurrent;

							// Смещаем ширину поля на ширину управляющих кнопок
							mRectWorldScreenMain.width -= mButtonSizeCurrent * 2;

							// Кнопка вверх
							mRectWorldScreenButtonNext.x = mRectWorldScreenMain.xMax;
							mRectWorldScreenButtonNext.y = mRectWorldScreenMain.y;
							mRectWorldScreenButtonNext.width = mButtonSizeCurrent;
							mRectWorldScreenButtonNext.height = mRectWorldScreenMain.height;

							// Кнопка вниз
							mRectWorldScreenButtonPrev.x = mRectWorldScreenMain.x - mButtonSizeCurrent;
							mRectWorldScreenButtonPrev.y = mRectWorldScreenMain.y;
							mRectWorldScreenButtonPrev.width = mButtonSizeCurrent;
							mRectWorldScreenButtonPrev.height = mRectWorldScreenMain.height;
						}
						break;
					case TSpinnerButtonLocation.TopHorizontal:
						{
							// Смещаем позицию поля на ширину управляющих кнопок
							mRectWorldScreenMain.y += mButtonSizeCurrent;

							// Смещаем ширину поля на ширину управляющих кнопок
							mRectWorldScreenMain.height -= mButtonSizeCurrent;

							// Кнопка вверх
							mRectWorldScreenButtonNext.x = mRectWorldScreenMain.x + mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonNext.y = mRectWorldScreenMain.y - mButtonSizeCurrent;
							mRectWorldScreenButtonNext.width = mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonNext.height = mButtonSizeCurrent;

							// Кнопка вниз
							mRectWorldScreenButtonPrev.x = mRectWorldScreenMain.x;
							mRectWorldScreenButtonPrev.y = mRectWorldScreenMain.y - mButtonSizeCurrent;
							mRectWorldScreenButtonPrev.width = mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonPrev.height = mButtonSizeCurrent;
						}
						break;
					case TSpinnerButtonLocation.BottomHorizontal:
						{
							// Смещаем высоту поля на ширину управляющих кнопок
							mRectWorldScreenMain.height -= mButtonSizeCurrent;

							// Кнопка вверх
							mRectWorldScreenButtonNext.x = mRectWorldScreenMain.x + mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonNext.y = mRectWorldScreenMain.yMax;
							mRectWorldScreenButtonNext.width = mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonNext.height = mButtonSizeCurrent;

							// Кнопка вниз
							mRectWorldScreenButtonPrev.x = mRectWorldScreenMain.x;
							mRectWorldScreenButtonPrev.y = mRectWorldScreenMain.yMax;
							mRectWorldScreenButtonPrev.width = mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonPrev.height = mButtonSizeCurrent;
						}
						break;
					default:
						break;
				}
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
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				// Основной контент
				GUI.Label(mRectWorldScreenMain, mContents[mSelectedIndex], mStyleMain);

				if (mButtonLocation == TSpinnerButtonLocation.LeftVertical || mButtonLocation == TSpinnerButtonLocation.RightVertical)
				{
					// Кнопка вверх
					if (GUI.Button(mRectWorldScreenButtonNext, XString.TriangleUp))
					{
						if (mSelectedIndex < mContents.Length - 1)
						{
							mSelectedIndex++;
							if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
						}
					}

					// Кнопка вниз
					if (GUI.Button(mRectWorldScreenButtonPrev, XString.TriangleDown))
					{
						if (mSelectedIndex > 0)
						{
							mSelectedIndex--;
							if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
						}
					}
				}
				else
				{
					// Кнопка вверх
					if (GUI.Button(mRectWorldScreenButtonNext, XString.TriangleRight))
					{
						if (mSelectedIndex < mContents.Length - 1)
						{
							mSelectedIndex++;
							if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
						}
					}

					// Кнопка вниз
					if (GUI.Button(mRectWorldScreenButtonPrev, XString.TriangleLeft))
					{
						if (mSelectedIndex > 0)
						{
							mSelectedIndex--;
							if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
						}
					}
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
				CGUISpinnerContents container = MemberwiseClone() as CGUISpinnerContents;

				// Копируем список данных
				container.mContentItems = new List<CGUIContentItem>();
				for (Int32 i = 0; i < mContentItems.Count; i++)
				{
					container.mContentItems.Add(mContentItems[i].Duplicate());
				}
				return container;
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

				CGUISpinnerContents spinner_contents = base_element as CGUISpinnerContents;
				if (spinner_contents != null)
				{
					mButtonSize = spinner_contents.mButtonSize;
					mButtonLocation = spinner_contents.mButtonLocation;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Контекстное меню
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.SelectionGrid.
		/// Реализация контекстного меню появляющегося по нажатию правой кнопки мыши (в стандартной версии) или 
		/// при длительном нажатие на элемент (мобильная версия). 
		/// Поддерживается различное местоположение открытия списка и различный режим отображения элементов списка
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIContextMenuContents : CGUIContainerContents
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mCountColumn = 1;
			[SerializeField]
			internal Single mWidthItem = 60;
			[SerializeField]
			internal Single mHeightItem = 60;
			[SerializeField]
			internal Int32 mCountVisibleItemX = 1;
			[SerializeField]
			internal Int32 mCountVisibleItemY = 2;
			[SerializeField]
			internal TContextOpenLocation mOpenLocation;
			[SerializeField]
			internal Single mDuration;
			[SerializeField]
			internal Boolean mIsOpened;

			// События
			internal Action<Boolean> mOnOpened;

			// Служебные данные
			[NonSerialized]
			internal Vector2 mPointOpened;
			[NonSerialized]
			internal Single mStartTime;
			[NonSerialized]
			internal Boolean mIsOpening;
			[NonSerialized]
			internal Boolean mIsClosing;
			[NonSerialized]
			internal Single mTargetViewWidth;
			[NonSerialized]
			internal Single mTargetViewHeight;
			[NonSerialized]
			internal Single mTargetContentWidth;
			[NonSerialized]
			internal Single mTargetContentHeight;
			[NonSerialized]
			internal Single mWidthItemCurrent;
			[NonSerialized]
			internal Single mHeightItemCurrent;
			[NonSerialized]
			internal Boolean mIsVisibleContent;
			[NonSerialized]
			internal Rect mRectWorldScreenContent;
			[NonSerialized]
			internal Rect mRectWorldScreenView;
			[NonSerialized]
			internal Vector2 mScrollData;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Количество колонок элемента
			/// </summary>
			public Int32 CountColumn
			{
				get { return mCountColumn; }
				set
				{
					mCountColumn = value;
				}
			}

			/// <summary>
			/// Ширина элемента списка
			/// </summary>
			public Single WidthItem
			{
				get { return mWidthItem; }
				set
				{
					mWidthItem = value;
				}
			}

			/// <summary>
			/// Высота элемента списка
			/// </summary>
			public Single HeightItem
			{
				get { return mHeightItem; }
				set
				{
					mHeightItem = value;
				}
			}

			/// <summary>
			/// Количество видимых элементов по ширине
			/// </summary>
			public Int32 CountVisibleItemX
			{
				get { return mCountVisibleItemX; }
				set
				{
					mCountVisibleItemX = value;
				}
			}

			/// <summary>
			/// Количество видимых элементов по высоте
			/// </summary>
			public Int32 CountVisibleItemY
			{
				get { return mCountVisibleItemY; }
				set
				{
					mCountVisibleItemY = value;
				}
			}

			/// <summary>
			/// Местоположения раскрытия списка
			/// </summary>
			public TContextOpenLocation OpenLocation
			{
				get { return mOpenLocation; }
				set
				{
					mOpenLocation = value;
				}
			}

			/// <summary>
			/// Продолжительность открытия/закрытия списка в секундах
			/// </summary>
			public Single Duration
			{
				get { return mDuration; }
				set { mDuration = value; }
			}

			/// <summary>
			/// Статус открытия списка
			/// </summary>
			public Boolean IsOpened
			{
				get { return mIsOpened; }
				set
				{
					if (mIsOpened != value)
					{
						if (value)
						{
							Opening();

							if (mOnOpened != null) mOnOpened(false);
						}
						else
						{
							Closing();
						}
					}
				}
			}

			/// <summary>
			/// Статус открывания списка
			/// </summary>
			public Boolean IsOpening
			{
				get { return mIsOpening; }
			}

			/// <summary>
			/// Статус закрытия списка
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
			public CGUIContextMenuContents()
				: base()
			{
				mStyleMainName = "Button";
				mDuration = 0.4f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContextMenuContents(String name)
				: base(name)
			{
				mStyleMainName = "Button";
				mDuration = 0.4f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContextMenuContents(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Button";
				mDuration = 0.4f;
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
				switch (mAspectMode)
				{
					case TAspectMode.None:
						break;
					case TAspectMode.Proportional:
						{
							mWidthItemCurrent = mWidthItem * LotusGUIDispatcher.ScaledScreenX;
							mHeightItemCurrent = mHeightItem * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mWidthItemCurrent = mWidthItem * LotusGUIDispatcher.ScaledScreenX;
							mHeightItemCurrent = mHeightItem * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mWidthItemCurrent = mWidthItem * LotusGUIDispatcher.ScaledScreenY;
							mHeightItemCurrent = mHeightItem * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}

				base.UpdatePlacementBase();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление местоположения и размеров области контента раскрывающего списка
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void ComputeRectContentArea()
			{
				// 1) Количество элементов по высоте
				Int32 count_height = Mathf.CeilToInt((Single)mContentItems.Count / (Single)mCountColumn);

				// 2) Размер прямоугольника для вывода всех данных
				mRectWorldScreenView.x = 0;
				mRectWorldScreenView.y = 0;

				// Число показываемых элементом по горизонтали меньше чем есть
				// - значит будет горизонтальная полоса прокрутки 
				// - значит нам нужно уменьшить высоту области на высоту горизонтали полосы прокрутки
				if (mCountVisibleItemX < mCountColumn)
				{
					mTargetViewHeight = count_height * (mHeightItemCurrent + mStyleMain.margin.top) - LotusGUIDispatcher.SizeScrollVertical - 2;
				}
				else
				{
					mTargetViewHeight = count_height * (mHeightItemCurrent + mStyleMain.margin.top);
				}

				// Число показываемых элементом по вертикали меньше чем есть
				// - значит будет вертикальная полоса прокрутки
				// - значит нам нужно уменьшить ширину области на ширину вертикальной полосы прокрутки
				if (mCountVisibleItemY < count_height)
				{
					mTargetViewWidth = mCountColumn * (mWidthItemCurrent + mStyleMain.margin.left + mStyleMain.margin.right) - LotusGUIDispatcher.SizeScrollVertical - 2;
				}
				else
				{
					mTargetViewWidth = mCountColumn * (mWidthItemCurrent + mStyleMain.margin.left + mStyleMain.margin.right);
				}

				// Корректируем количество видимых столбцов и строк
				if (mCountVisibleItemX > mCountColumn) mCountVisibleItemX = mCountColumn;
				if (mCountVisibleItemY > count_height) mCountVisibleItemY = count_height;

				// 3) Размер прямоугольника для отображения данных
				mTargetContentWidth = mCountVisibleItemX * (mWidthItemCurrent + StyleMain.margin.left + mStyleMain.margin.right);
				mTargetContentHeight = mCountVisibleItemY * (mHeightItemCurrent + StyleMain.margin.top);

				// 4) Определяем раскрытие списка
				switch (mOpenLocation)
				{
					case TContextOpenLocation.Up:
						{
							mRectWorldScreenView.width = mTargetViewWidth;
							mRectWorldScreenContent.width = mTargetContentWidth;
						}
						break;
					case TContextOpenLocation.Down:
						{
							mRectWorldScreenView.width = mTargetViewWidth;
							mRectWorldScreenContent.width = mTargetContentWidth;
						}
						break;
					case TContextOpenLocation.SideLeft:
						{
							mRectWorldScreenView.width = mTargetViewWidth;
							mRectWorldScreenView.height = mTargetViewHeight;
							mRectWorldScreenContent.width = mTargetContentWidth;
							mRectWorldScreenContent.height = mTargetContentHeight;
						}
						break;
					case TContextOpenLocation.SideRight:
						{
							mRectWorldScreenView.width = mTargetViewWidth;
							mRectWorldScreenView.height = mTargetViewHeight;
							mRectWorldScreenContent.width = mTargetContentWidth;
							mRectWorldScreenContent.height = mTargetContentHeight;
						}
						break;
					default:
						break;
				}

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Открытие списка
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void Opening()
			{
				mIsOpened = true;
				mIsOpening = true;
				mIsClosing = false;
				mStartTime = Time.time;
				IsDirty = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Закрытие списка
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
					Single delta_time = (Time.unscaledTime - mStartTime) / mDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mIsOpening = false;
						IsDirty = false;
					}

					switch (mOpenLocation)
					{
						case TContextOpenLocation.Up:
							{
								// Плавно показываем область просмотра и контента
								mRectWorldScreenContent.y = mPointOpened.y - Mathf.Lerp(0, mTargetContentHeight, delta_time);
								mRectWorldScreenContent.height = Mathf.Lerp(0, mTargetContentHeight, delta_time);
								mRectWorldScreenView.height = Mathf.Lerp(0, mTargetViewHeight, delta_time);

								// Если область еще не видима
								if (mIsVisibleContent == false)
								{
									// Если видимая область уже достаточная большая
									// то мы можем показать контент, это связано
									// с тем что в маленькой области некорректно отображается полоса прокрутки
									if (mRectWorldScreenView.height > LotusGUIDispatcher.SizeScrollHorizontal * 2)
									{
										mIsVisibleContent = true;
									}
								}
							}
							break;
						case TContextOpenLocation.Down:
							{
								// Плавно показываем область просмотра и контента
								mRectWorldScreenContent.height = Mathf.Lerp(0, mTargetContentHeight, delta_time);
								mRectWorldScreenView.height = Mathf.Lerp(0, mTargetViewHeight, delta_time);

								// Если область еще не видима
								if (mIsVisibleContent == false)
								{
									// Если видимая область уже достаточная большая
									// то мы можем показать контент, это связано
									// с тем что в маленькой области некорректно отображается полоса прокрутки
									if (mRectWorldScreenView.height > LotusGUIDispatcher.SizeScrollHorizontal * 2)
									{
										mIsVisibleContent = true;
									}
								}
							}
							break;
						case TContextOpenLocation.SideLeft:
							{
								// Плавно показываем область просмотра и контента
								mRectWorldScreenContent.x = mPointOpened.x - Mathf.Lerp(0, mTargetContentHeight, delta_time);
								mRectWorldScreenContent.width = Mathf.Lerp(0, mTargetContentWidth, delta_time);
								mRectWorldScreenContent.height = Mathf.Lerp(0, mTargetContentHeight, delta_time);

								mRectWorldScreenView.width = Mathf.Lerp(0, mTargetViewWidth, delta_time);
								mRectWorldScreenView.height = Mathf.Lerp(0, mTargetViewHeight, delta_time);

								// Проверка на корректность
								if (mRectWorldScreenContent.width - mRectWorldScreenView.width < LotusGUIDispatcher.SizeScrollVertical + 2)
								{
									mRectWorldScreenContent.width = mRectWorldScreenView.width + LotusGUIDispatcher.SizeScrollVertical + 2;
								}

								// Если видимая область уже достаточная большая
								// то мы можем показать контент, это связано
								// с тем что в маленькой области некорректно отображается полоса прокрутки
								if (mIsVisibleContent == false)
								{
									if (mRectWorldScreenView.height > LotusGUIDispatcher.SizeScrollHorizontal * 2 &&
										mRectWorldScreenView.width > LotusGUIDispatcher.SizeScrollVertical * 2)
									{
										mIsVisibleContent = true;
									}
								}
							}
							break;
						case TContextOpenLocation.SideRight:
							{
								// Плавно показываем область просмотра и контента
								mRectWorldScreenView.width = Mathf.Lerp(0, mTargetViewWidth, delta_time);
								mRectWorldScreenView.height = Mathf.Lerp(0, mTargetViewHeight, delta_time);

								mRectWorldScreenContent.width = Mathf.Lerp(0, mTargetContentWidth, delta_time);
								mRectWorldScreenContent.height = Mathf.Lerp(0, mTargetContentHeight, delta_time);

								// Проверка на корректность
								if(mRectWorldScreenContent.width - mRectWorldScreenView.width < LotusGUIDispatcher.SizeScrollVertical + 2)
								{
									mRectWorldScreenContent.width = mRectWorldScreenView.width + LotusGUIDispatcher.SizeScrollVertical + 2;
								}

								// Если видимая область уже достаточная большая
								// то мы можем показать контент, это связано
								// с тем что в маленькой области некорректно отображается полоса прокрутки
								if (mIsVisibleContent == false)
								{
									if (mRectWorldScreenView.height > LotusGUIDispatcher.SizeScrollHorizontal * 2 &&
										mRectWorldScreenView.width > LotusGUIDispatcher.SizeScrollVertical * 2)
									{
										mIsVisibleContent = true;
									}
								}
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
					Single delta_time = (Time.unscaledTime - mStartTime) / mDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mIsClosing = false;
						mIsOpened = false;
						IsDirty = false;

						if (mOnOpened != null) mOnOpened(false);
					}

					switch (mOpenLocation)
					{
						case TContextOpenLocation.Up:
							{
								// Плавно скрываем область просмотра и контента
								mRectWorldScreenContent.y = mPointOpened.y - Mathf.Lerp(mTargetContentHeight, 0, delta_time);
								mRectWorldScreenContent.height = Mathf.Lerp(mTargetContentHeight, 0, delta_time);
								mRectWorldScreenView.height = Mathf.Lerp(mTargetViewHeight, 0, delta_time);

								// Если контент виден
								if (mIsVisibleContent)
								{
									// Не будем ждать пока он полностью исчезнет, как только область контента станет небольшой скроем ее
									if (mRectWorldScreenView.height < LotusGUIDispatcher.SizeScrollHorizontal * 2)
									{
										mIsVisibleContent = false;
									}
								}
							}
							break;
						case TContextOpenLocation.Down:
							{
								// Плавно скрываем область просмотра и контента
								mRectWorldScreenContent.height = Mathf.Lerp(mTargetContentHeight, 0, delta_time);
								mRectWorldScreenView.height = Mathf.Lerp(mTargetViewHeight, 0, delta_time);

								// Если контент виден
								if (mIsVisibleContent)
								{
									// Не будем ждать пока он полностью исчезнет, как только область контента станет небольшой скроем ее
									if (mRectWorldScreenView.height < LotusGUIDispatcher.SizeScrollHorizontal * 2)
									{
										mIsVisibleContent = false;
									}
								}
							}
							break;
						case TContextOpenLocation.SideLeft:
							{
								// Плавно скрываем область просмотра и контента
								mRectWorldScreenContent.x = mPointOpened.x - Mathf.Lerp(mTargetContentHeight, 0, delta_time);
								mRectWorldScreenContent.width = Mathf.Lerp(mTargetContentWidth, 0, delta_time);
								mRectWorldScreenContent.height = Mathf.Lerp(mTargetContentHeight, 0, delta_time);

								mRectWorldScreenView.width = Mathf.Lerp(mTargetViewWidth, 0, delta_time);
								mRectWorldScreenView.height = Mathf.Lerp(mTargetViewHeight, 0, delta_time);

								// Проверка на корректность
								if (mRectWorldScreenContent.width - mRectWorldScreenView.width < LotusGUIDispatcher.SizeScrollVertical + 2)
								{
									mRectWorldScreenContent.width = mRectWorldScreenView.width + LotusGUIDispatcher.SizeScrollVertical + 2;
								}

								// Если контент виден
								if (mIsVisibleContent)
								{
									// Не будем ждать пока он полностью исчезнет, как только область контента станет небольшой скроем ее
									if (mRectWorldScreenView.height < LotusGUIDispatcher.SizeScrollHorizontal * 2)
									{
										mIsVisibleContent = false;
									}

									// Не будем ждать пока он полностью исчезнет, как только область контента станет небольшой скроем ее
									if (mRectWorldScreenView.width < LotusGUIDispatcher.SizeScrollVertical * 2)
									{
										mIsVisibleContent = false;
									}
								}
							}
							break;
						case TContextOpenLocation.SideRight:
							{
								// Плавно скрываем область просмотра и контента
								mRectWorldScreenContent.width = Mathf.Lerp(mTargetContentWidth, 0, delta_time);
								mRectWorldScreenContent.height = Mathf.Lerp(mTargetContentHeight, 0, delta_time);

								mRectWorldScreenView.width = Mathf.Lerp(mTargetViewWidth, 0, delta_time);
								mRectWorldScreenView.height = Mathf.Lerp(mTargetViewHeight, 0, delta_time);

								// Проверка на корректность
								if (mRectWorldScreenContent.width - mRectWorldScreenView.width < LotusGUIDispatcher.SizeScrollVertical + 2)
								{
									mRectWorldScreenContent.width = mRectWorldScreenView.width + LotusGUIDispatcher.SizeScrollVertical + 2;
								}

								// Если контент виден
								if (mIsVisibleContent)
								{
									if (mRectWorldScreenView.height < LotusGUIDispatcher.SizeScrollHorizontal * 2)
									{
										mIsVisibleContent = false;
									}

									if (mRectWorldScreenView.width < LotusGUIDispatcher.SizeScrollVertical * 2)
									{
										mIsVisibleContent = false;
									}
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
				// Если нет родительской области то открываем в любом месте
				if (mParent == null)
				{
					if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
					{
						if (IsOpened == false)
						{
							ComputeRectContentArea();
							mPointOpened = Event.current.mousePosition;
							mRectWorldScreenContent.position = mPointOpened;
							IsOpened = true;
						}
						else
						{
							mPointOpened = Event.current.mousePosition;
							mRectWorldScreenContent.position = mPointOpened;
						}
					}
				}
				else
				{
					if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
					{
						// Открываем если только мы попали в область родителя
						if (mParent.RectScreen.Contains(Event.current.mousePosition))
						{
							if (IsOpened == false)
							{
								ComputeRectContentArea();
								mPointOpened = Event.current.mousePosition;
								mRectWorldScreenContent.position = mPointOpened;
								IsOpened = true;
							}
							else
							{
								mPointOpened = Event.current.mousePosition;
								mRectWorldScreenContent.position = mPointOpened;
							}
						}
					}
				}

				// Выводим область списка
				if (mIsOpened && mIsVisibleContent)
				{
					mScrollData = GUI.BeginScrollView(mRectWorldScreenContent, mScrollData, mRectWorldScreenView, false, false);
					{
						GUI.changed = false;
						mSelectedIndex = GUI.SelectionGrid(mRectWorldScreenView, mSelectedIndex, mContents, mCountColumn, mStyleMain);
						if (GUI.changed)
						{
							IsOpened = false;
							if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
						}
					}
					GUI.EndScrollView();
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
				CGUIContextMenuContents container = MemberwiseClone() as CGUIContextMenuContents;

				// Копируем список данных
				container.mContentItems = new List<CGUIContentItem>();
				for (Int32 i = 0; i < mContentItems.Count; i++)
				{
					container.mContentItems.Add(mContentItems[i].Duplicate());
				}
				return container;
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

				CGUIContextMenuContents menu_contents = base_element as CGUIContextMenuContents;
				if (menu_contents != null)
				{
					mCountColumn = menu_contents.mCountColumn;
					mWidthItem = menu_contents.mWidthItem;
					mHeightItem = menu_contents.mHeightItem;
					mCountVisibleItemX = menu_contents.mCountVisibleItemX;
					mCountVisibleItemY = menu_contents.mCountVisibleItemY;
					mOpenLocation = menu_contents.mOpenLocation;
					mDuration = menu_contents.mDuration;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Выпадающий список
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.SelectionGrid.
		/// Поддерживается различное местоположение открытия списка и различный режим отображения элементов списка
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIDropDownContents : CGUIContextMenuContents
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mCaptionText;
			[SerializeField]
			internal Texture2D mCaptionIcon;

			// Параметры визуального стиля
			[SerializeField]
			internal String mStyleButtonName;
			[NonSerialized]
			internal GUIStyle mStyleButton;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Основной текст элемента
			/// </summary>
			public String CaptionText
			{
				get { return mCaptionText; }
				set { mCaptionText = value; }
			}

			/// <summary>
			/// Текстура иконки элемента
			/// </summary>
			public Texture2D CaptionIcon
			{
				get { return mCaptionIcon; }
				set { mCaptionIcon = value; }
			}

			//
			// ПАРАМЕТРЫ ВИЗУАЛЬНОГО СТИЛЯ
			//
			/// <summary>
			/// Имя стиля для кнопки элемента
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
			/// Стиль для рисования кнопки элемента
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
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDropDownContents()
				: base()
			{
				mStyleMainName = "Button";
				mStyleButtonName = "Button";
				mCaptionText = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDropDownContents(String name)
				: base(name)
			{
				mStyleMainName = "Button";
				mStyleButtonName = "Button";
				mCaptionText = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDropDownContents(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Button";
				mStyleButtonName = "Button";
				mCaptionText = "";
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
				mCaptionText = text;
				mCaptionIcon = icon;
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
				if (mStyleButton == null) mStyleButton = LotusGUIDispatcher.FindStyle(mStyleButtonName);
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

				LotusGUIDispatcher.CurrentContent.text = mCaptionText;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				// Если нажали на открытие 
				if (GUI.Button(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleButton))
				{
					// Вычисляем область промотра
					this.ComputeRectContentArea();

					switch (mOpenLocation)
					{
						case TContextOpenLocation.Up:
							{
								mPointOpened.x = mRectWorldScreenMain.x;
								mPointOpened.y = mRectWorldScreenMain.y;

								mRectWorldScreenContent.x = mRectWorldScreenMain.x;
								mRectWorldScreenContent.y = mRectWorldScreenMain.y;
							}
							break;
						case TContextOpenLocation.Down:
							{
								mPointOpened.x = mRectWorldScreenMain.x;
								mPointOpened.y = mRectWorldScreenMain.yMax;

								mRectWorldScreenContent.x = mRectWorldScreenMain.x;
								mRectWorldScreenContent.y = mRectWorldScreenMain.yMax;
							}
							break;
						case TContextOpenLocation.SideLeft:
							{
								mPointOpened.x = mRectWorldScreenMain.x;
								mPointOpened.y = mRectWorldScreenMain.y;

								mRectWorldScreenContent.x = mRectWorldScreenMain.x;
								mRectWorldScreenContent.y = mRectWorldScreenMain.y;
							}
							break;
						case TContextOpenLocation.SideRight:
							{
								mPointOpened.x = mRectWorldScreenMain.xMax;
								mPointOpened.y = mRectWorldScreenMain.y;

								mRectWorldScreenContent.x = mRectWorldScreenMain.xMax;
								mRectWorldScreenContent.y = mRectWorldScreenMain.y;
							}
							break;
						default:
							break;
					}

					IsOpened = !IsOpened;
				}

				// Выводим область списка
				if (mIsOpened && mIsVisibleContent)
				{
					mScrollData = GUI.BeginScrollView(mRectWorldScreenContent, mScrollData, mRectWorldScreenView, false, false);
					{
						GUI.changed = false;
						mSelectedIndex = GUI.SelectionGrid(mRectWorldScreenView, mSelectedIndex, mContents, mCountColumn, mStyleMain);
						if (GUI.changed)
						{
							IsOpened = false;
							mCaptionText = mContentItems[mSelectedIndex].Text;
							mCaptionIcon = mContentItems[mSelectedIndex].Icon;

							if (mOnSelectedIndex != null) mOnSelectedIndex(mSelectedIndex);
						}
					}
					GUI.EndScrollView();
				}

				if (Event.current.type == EventType.MouseDown && mRectWorldScreenMain.Contains(Event.current.mousePosition) == false)
				{
					if (IsOpened && mIsOpening == false && mIsClosing == false)
					{
						IsOpened = false;
					}
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
				CGUIDropDownContents container = MemberwiseClone() as CGUIDropDownContents;

				// Копируем список данных
				container.mContentItems = new List<CGUIContentItem>();
				for (Int32 i = 0; i < mContentItems.Count; i++)
				{
					container.mContentItems.Add(mContentItems[i].Duplicate());
				}
				return container;
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

				CGUIDropDownContents element = base_element as CGUIDropDownContents;
				if (element != null)
				{
					mCaptionText = element.mCaptionText;
					mCaptionIcon = element.mCaptionIcon;
					mStyleButtonName = element.mStyleButtonName;
					mStyleButton = LotusGUIDispatcher.FindStyle(mStyleButtonName);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КОНТЕНТОМ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeSizeFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mCaptionText;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				Vector2 size = style.CalcSize(LotusGUIDispatcher.CurrentContent);
				mRectLocalDesignMain.width = RoundToNearest(size.x, 10);
				mRectLocalDesignMain.height = RoundToNearest(size.y, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeWidthFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mCaptionText;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				Single min_width = 0, max_width = 0;
				style.CalcMinMaxWidth(LotusGUIDispatcher.CurrentContent, out min_width, out max_width);
				mRectLocalDesignMain.width = RoundToNearest((min_width + max_width) / 2, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeHeightFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mCaptionText;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				Single height = style.CalcHeight(LotusGUIDispatcher.CurrentContent, mRectLocalDesignMain.width - (PaddingLeft + PaddingRight));
				mRectLocalDesignMain.height = RoundToNearest(height, 10);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================