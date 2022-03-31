//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIExtendedHierarchy.cs
*		Элементы интерфейса пользователя с иерархическим (древовидным) отображением информации.
*		Реализация элементов интерфейса пользователя с иерархическим (древовидным) отображением информации.
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
		//! \defgroup Unity2DImmedateGUIExtended Расширенные элементы управления
		//! Расширенные и специфичные элементы интерфейса. В данной группе реализованы элементы с иерархическим (древовидным) 
		//! отображением информации, динамические и тайловые элементы
		//! \ingroup Unity2DImmedateGUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Строка данных - запись списка строковых данных
		/// </summary>
		/// <remarks>
		/// Под строкой данных понимается набор последовательных ячеек которые отображается стилем соответствующего столбца.
		/// Доступ к ячейки осуществляется через индексацию посредством последовательного номера (индекса) или имени столбца
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIListItem : IComparable<CGUIListItem>, IComparer<CGUIListItem>
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal TListRowType mItemType;
			[NonSerialized]
			internal GUIContent[] mContents;
			[NonSerialized]
			internal Boolean mIsSelected;
			[NonSerialized]
			internal Int32 mFilter;
			[NonSerialized]
			internal Int32 mIndex;
			[NonSerialized]
			internal CGUIListView mOwner;

			// Параметры размещения
			[NonSerialized]
			internal Single mHeight;
			[NonSerialized]
			internal Single mHeightActual;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Тип строки данных
			/// </summary>
			public TListRowType ItemType
			{
				get { return mItemType; }
				set { mItemType = value; }
			}

			/// <summary>
			/// Массив контента данных(ячеек)
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
			/// Количества контента данных (ячеек)
			/// </summary>
			public Int32 Count
			{
				get { return mContents.Length; }
			}

			/// <summary>
			/// Статус выбора строки данных
			/// </summary>
			public Boolean IsSelected
			{
				get { return mIsSelected; }
				set { mIsSelected = value; }
			}

			/// <summary>
			/// Фильтр для строки данных
			/// </summary>
			/// <remarks>
			/// Нулевое значение означает что строка видна
			/// </remarks>
			public Int32 Filter
			{
				get { return mFilter; }
				set { mFilter = value; }
			}

			/// <summary>
			/// Индекс строки данных в списке
			/// </summary>
			public Int32 Index
			{
				get { return mIndex; }
				set { mIndex = value; }
			}

			/// <summary>
			/// Элемент владелец
			/// </summary>
			public CGUIListView Owner
			{
				get { return mOwner; }
				set { mOwner = value; }
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Высота строка данных
			/// </summary>
			public Single Height
			{
				get { return mHeight; }
				set
				{
					mHeight = value;
					ComputeHeightActual();
				}
			}

			/// <summary>
			/// Актуальная высота строки данных
			/// </summary>
			public Single HeightActual
			{
				get { return mHeightActual; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListItem()
			{
				mHeight = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="owner">Владелец</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListItem(CGUIListView owner)
			{
				mHeight = 30;
				mOwner = owner;
				mContents = new GUIContent[mOwner.Columns.Count];
				for (Int32 i = 0; i < mContents.Length; i++)
				{
					mContents[i] = new GUIContent();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="owner">Владелец</param>
			/// <param name="texts">Массив текстовых данных</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListItem(CGUIListView owner, params String[] texts)
			{
				mHeight = 30;
				mOwner = owner;
				mContents = new GUIContent[mOwner.Columns.Count];
				for (Int32 i = 0; i < mContents.Length; i++)
				{
					if (i < texts.Length)
					{
						mContents[i] = new GUIContent(texts[i]);
					}
					else
					{
						mContents[i] = new GUIContent();
					}
				}
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение строк данных
			/// </summary>
			/// <param name="other">Строка данных</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CGUIListItem other)
			{
				if (mItemType == TListRowType.Header) return -1;

				String t1 = mContents[mOwner.IndexSortColumn].text;
				String t2 = other.mContents[mOwner.IndexSortColumn].text;

				if(mOwner.Columns[mOwner.IndexSortColumn].ColumnType == TListColumnType.Number)
				{
					Single n1 = 0;
					Single n2 = 0;
					if (Single.TryParse(t1, out n1))
					{
						if (Single.TryParse(t2, out n2))
						{
							if (n1 > n2)
							{
								return 1;
							}
							else
							{
								if (n1 < n2)
								{
									return -1;
								}
								else
								{
									return 0;
								}
							}
						}
						else
						{
							return 0;
						}
					}
					else
					{
						return 0;
					}
				}
				else
				{
					return String.Compare(t1, t2);
				}

				//if (mFilter > other.mFilter)
				//{
				//	return (1);
				//}
				//else
				//{
				//	if (mFilter < other.mFilter)
				//	{
				//		return (-1);
				//	}
				//	else
				//	{
				//		return (0);
				//	}
				//}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение строк данных
			/// </summary>
			/// <param name="x">Первая строка данных</param>
			/// <param name="y">Вторая строка данных</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 Compare(CGUIListItem x, CGUIListItem y)
			{
				return (x.CompareTo(y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление строки данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return mIndex.ToString();
			}
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация ячеек на основе индекса
			/// </summary>
			/// <param name="index">Индекс ячееки</param>
			/// <returns>Данные ячейки</returns>
			//---------------------------------------------------------------------------------------------------------
			public GUIContent this[Int32 index]
			{
				get
				{
					return mContents[index];
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление актуальной высоты строки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			internal void ComputeHeightActual()
			{
				switch (mOwner.AspectMode)
				{
					case TAspectMode.None:
						{
							mHeightActual = mHeight;
						}
						break;
					case TAspectMode.Proportional:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата строки данных
			/// </summary>
			/// <returns>Полный дубликат строки данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListItem Duplicate()
			{
				CGUIListItem dublicate = MemberwiseClone() as CGUIListItem;
				dublicate.mContents = new GUIContent[mContents.Length];

				for (Int32 i = 0; i < mContents.Length; i++)
				{
					dublicate.mContents[i] = new GUIContent(mContents[i]);
				}

				return dublicate;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Столбец данных - поле списка строковых данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIListColumn
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mName;
			[SerializeField]
			internal TListColumnType mColumnType;
			[NonSerialized]
			internal Int32 mIndex;
			[NonSerialized]
			internal CGUIListView mOwner;

			// Параметры отображения
			[SerializeField]
			internal String mStyleSimpleName;
			[SerializeField]
			internal String mStyleHeaderName;
			[SerializeField]
			internal String mStyleResultsName;
			[NonSerialized]
			internal GUIStyle mStyleSimple;
			[NonSerialized]
			internal GUIStyle mStyleHeader;
			[NonSerialized]
			internal GUIStyle mStyleResults;

			// Параметры размещения
			[SerializeField]
			internal Single mWidth;
			[NonSerialized]
			internal Single mWidthActual;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя столбца
			/// </summary>
			/// <remarks>
			/// Имя столбца должно быть уникальным в пределах одного элемента
			/// </remarks>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Тип столбца
			/// </summary>
			public TListColumnType ColumnType
			{
				get { return mColumnType; }
				set { mColumnType = value; }
			}

			/// <summary>
			/// Индекс столбца в списке столбцов
			/// </summary>
			public Int32 Index
			{
				get { return mIndex; }
				set { mIndex = value; }
			}

			/// <summary>
			/// Элемент владелец
			/// </summary>
			public CGUIListView Owner
			{
				get { return mOwner; }
				set { mOwner = value; }
			}

			//
			// ПАРАМЕТРЫ ОТОБРАЖЕНИЯ
			//
			/// <summary>
			/// Имя стиля для рисования обычной ячейки данных
			/// </summary>
			public String StyleSimpleName
			{
				get { return mStyleSimpleName; }
				set
				{
					if (mStyleSimpleName != value)
					{
						mStyleSimpleName = value;
						mStyleSimple = LotusGUIDispatcher.FindStyle(mStyleSimpleName);
					}
				}
			}

			/// <summary>
			/// Имя стиля для рисования заголовка ячейки данных
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
			/// Имя стиля для рисования итогов ячейки данных
			/// </summary>
			public String StyleResultsName
			{
				get { return mStyleResultsName; }
				set
				{
					if (mStyleResultsName != value)
					{
						mStyleResultsName = value;
						mStyleResults = LotusGUIDispatcher.FindStyle(mStyleResultsName);
					}
				}
			}

			/// <summary>
			/// Стиль для рисования обычной ячейки данных
			/// </summary>
			public GUIStyle StyleSimple
			{
				get
				{
					if (mStyleSimple == null)
					{
						mStyleSimple = LotusGUIDispatcher.FindStyle(mStyleSimpleName);
					}
					return mStyleSimple;
				}
			}

			/// <summary>
			/// Стиль для рисования заголовка ячейки данных
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

			/// <summary>
			/// Стиль для рисования итогов ячейки данных
			/// </summary>
			public GUIStyle StyleResults
			{
				get
				{
					if (mStyleResults == null)
					{
						mStyleResults = LotusGUIDispatcher.FindStyle(mStyleResultsName);
					}
					return mStyleResults;
				}
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Ширина столбца
			/// </summary>
			public Single Width
			{
				get { return mWidth; }
				set
				{
					mWidth = value;
					if(mOwner != null)
					{
						mOwner.ComputeColumnWidth();
					}
				}
			}

			/// <summary>
			/// Актуальная ширина столбца
			/// </summary>
			public Single WidthActual
			{
				get { return mWidthActual; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListColumn()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListColumn(String name)
			{
				mName = name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя столбца</param>
			/// <param name="owner">Владелец</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListColumn(String name, CGUIListView owner)
			{
				mName = name;
				mOwner = owner;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя столбца</param>
			/// <param name="owner">Владелец</param>
			/// <param name="width">Ширина столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListColumn(String name, CGUIListView owner, Single width)
			{
				mName = name;
				mOwner = owner;
				mWidth = width;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ СО СТИЛЕМ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление стилей ячеек
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RecoveryStyles()
			{
				if (mStyleSimple == null) mStyleSimple = LotusGUIDispatcher.FindStyle(mStyleSimpleName);
				if (mStyleHeader == null) mStyleHeader = LotusGUIDispatcher.FindStyle(mStyleHeaderName);
				if (mStyleResults == null) mStyleResults = LotusGUIDispatcher.FindStyle(mStyleResultsName);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент для отображения списка строковых данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIListView : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal List<CGUIListColumn> mColumns;
			[SerializeField]
			internal List<CGUIListItem> mItems;
			[NonSerialized]
			internal CGUIListItem mSelectedItem;
			[NonSerialized]
			internal Int32 mIndexSortColumn;
			[SerializeField]
			internal Boolean mIsBackSelected;
			[SerializeField]
			internal Color mBackSelectedColor;

			// Событие
			internal Action<CGUIListColumn> mOnHeaderClick;

			// Служебные данные
			[NonSerialized]
			internal Rect mRectDrawView;
			[NonSerialized]
			internal Vector2 mScrollData;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Список столбцов
			/// </summary>
			public List<CGUIListColumn> Columns
			{
				get { return mColumns; }
			}

			/// <summary>
			/// Список строк данных
			/// </summary>
			public List<CGUIListItem> Items
			{
				get { return mItems; }
			}

			/// <summary>
			/// Выбранная строка данных
			/// </summary>
			public CGUIListItem SelectedItem
			{
				get { return mSelectedItem; }
				set
				{
					mSelectedItem = value;
				}
			}

			/// <summary>
			/// Индекс столбца для сортировки строк данных
			/// </summary>
			public Int32 IndexSortColumn
			{
				get { return mIndexSortColumn; }
				set
				{
					mIndexSortColumn = value;
				}
			}

			/// <summary>
			/// Статус обозначения фоновым цветом выбранной строки
			/// </summary>
			public Boolean IsBackSelected
			{
				get { return mIsBackSelected; }
				set
				{
					if (mIsBackSelected != value)
					{
						mIsBackSelected = value;
					}
				}
			}

			/// <summary>
			/// Фоновый цвет выбранной строки
			/// </summary>
			public Color BackSelectedColor
			{
				get { return mBackSelectedColor; }
				set
				{
					mBackSelectedColor = value;
				}
			}

			/// <summary>
			/// Событие для нотификации о щелчке на строку заголовка. Аргумент - столбец
			/// </summary>
			public Action<CGUIListColumn> OnHeaderClick
			{
				get { return mOnHeaderClick; }
				set
				{
					mOnHeaderClick = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует данные редактора значениями по умолчанию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListView()
				:base()
			{
				mStyleMainName = "ScrollView";

				mColumns = new List<CGUIListColumn>(3);
				mColumns.Add(new CGUIListColumn("First", this, 30));
				mColumns.Add(new CGUIListColumn("Second", this, 30));
				mColumns.Add(new CGUIListColumn("Tree", this, 30));

				mItems = new List<CGUIListItem>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListView(String name)
				: base(name)
			{
				mStyleMainName = "ScrollView";

				mColumns = new List<CGUIListColumn>(3);
				mColumns.Add(new CGUIListColumn("First", this, 30));
				mColumns.Add(new CGUIListColumn("Second", this, 30));
				mColumns.Add(new CGUIListColumn("Tree", this, 30));

				mItems = new List<CGUIListItem>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIListView(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "ScrollView";

				mColumns = new List<CGUIListColumn>(3);
				mColumns.Add(new CGUIListColumn("First", this, 30));
				mColumns.Add(new CGUIListColumn("Second", this, 30));
				mColumns.Add(new CGUIListColumn("Tree", this, 30));

				mItems = new List<CGUIListItem>();
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

				UpdateHeight();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdateHeight()
			{
				ComputeRowHeight();

				mRectDrawView.x = PaddingLeft;
				mRectDrawView.y = PaddingTop;
				mRectDrawView.height = ComputeItemsTotalHeight();
				if (mRectDrawView.height > mRectWorldScreenMain.height - (PaddingTop + PaddingBottom))
				{
					mRectDrawView.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight + LotusGUIDispatcher.SizeScrollVertical);
				}
				else
				{
					mRectDrawView.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
				}

				ComputeColumnWidth();
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
				ResetColumns();
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

				mScrollData = GUI.BeginScrollView(mRectWorldScreenMain, mScrollData, mRectDrawView);
				{
					Rect rect_node = new Rect(PaddingLeft, PaddingTop, mRectDrawView.width, mItems[0].mHeightActual);
					for (Int32 i = 0; i < mItems.Count; i++)
					{
						// Снимаем/выбираем текущий узел
						if (Event.current.type == EventType.MouseDown && 
							(mItems[i].ItemType == TListRowType.Simple || mItems[i].ItemType == TListRowType.Alternative))
						{
							if (rect_node.Contains(Event.current.mousePosition))
							{
								// Снимаем со-всех остальных
								UnselectItems();

								// Выбираем текущий
								mItems[i].mIsSelected = true;
								mSelectedItem = mItems[i];
							}
						}

						DrawItem(ref rect_node, mItems[i]);
						rect_node.y += mItems[i].mHeightActual;
					}
				}
				GUI.EndScrollView();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование строки данных
			/// </summary>
			/// <param name="rect_item">Прямоугольник для строки данных</param>
			/// <param name="item">Строка данных</param>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawItem(ref Rect rect_item, CGUIListItem item)
			{
				// Рисуем сам узел
				Rect rect_current = rect_item;

				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					if (i > 0)
					{
						// Смещаем
						rect_current.x += mColumns[i - 1].mWidthActual;
					}
					rect_current.width = mColumns[i].mWidthActual;

					// При выбранном элементе рисуем фон
					if (item.mIsSelected && IsBackSelected)
					{
						GUI.backgroundColor = BackSelectedColor;
						GUI.Box(rect_current, "");
						GUI.backgroundColor = Color.white;
					}

					switch (item.ItemType)
					{
						case TListRowType.Simple:
							{
								GUI.Label(rect_current, item.Contents[i], mColumns[i].StyleSimple);
							}
							break;
						case TListRowType.Alternative:
							{
								GUI.Label(rect_current, item.Contents[i], mColumns[i].StyleSimple);
							}
							break;
						case TListRowType.Header:
							{
								if (GUI.Button(rect_current, item.Contents[i], mColumns[i].StyleHeader))
								{
									if (mOnHeaderClick != null) mOnHeaderClick(mColumns[i]);
								}
							}
							break;
						case TListRowType.Results:
							{
								GUI.Label(rect_current, item.Contents[i], mColumns[i].StyleResults);
							}
							break;
						default:
							break;
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
				return MemberwiseClone() as CGUIListView;
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

				CGUIListView list_view = base_element as CGUIListView;
				if (list_view != null)
				{

				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ СО СТОЛБЦАМИ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных столбцов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ResetColumns()
			{
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					mColumns[i].Index = i;
					mColumns[i].RecoveryStyles();
				}
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление актуальной ширины столбцов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeColumnWidth()
			{
				// 1) Считаем общую ширину столбцов
				Single total_width = 0;
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					total_width += mColumns[i].Width;
				}

				// 2) Считаем коэффициент коррекции
				Single correct = mRectDrawView.width / total_width;

				// 3) Корректируем
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					mColumns[i].mWidthActual = mColumns[i].mWidth * correct;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление столбца с указанными параметрами
			/// </summary>
			/// <param name="name">Имя столбца</param>
			/// <param name="width">Ширина столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddColumn(String name, Single width)
			{
				mColumns.Add(new CGUIListColumn(name, this, width));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление столбца по индексу
			/// </summary>
			/// <param name="index">Индекс столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveColumn(Int32 index)
			{
				mColumns.RemoveAt(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление столбца по имени
			/// </summary>
			/// <param name="name">Имя столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveColumn(String name)
			{
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					if(mColumns[i].Name == name)
					{
						mColumns.RemoveAt(i);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление стилей столбцов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RecoveryColumnsStyle()
			{
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					mColumns[i].RecoveryStyles();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ СО СТРОКАМИ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление общей высоты всех строк данных
			/// </summary>
			/// <returns>Общая высота всех строк данных</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single ComputeItemsTotalHeight()
			{
				Single total_height = 0;
				for (Int32 i = 0; i < mItems.Count; i++)
				{
					total_height += mItems[i].mHeightActual;
				}

				return total_height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление актуальной высоты строк данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeRowHeight()
			{
				for (Int32 i = 0; i < mItems.Count; i++)
				{
					mItems[i].ComputeHeightActual();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление данных
			/// </summary>
			/// <param name="texts">Массив данных</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddItem(params String[] texts)
			{
				mItems.Add(new CGUIListItem(this, texts));
				UpdateHeight();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сортировка списка строк данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SortItems()
			{
				mItems.Sort();
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из текстового ресурса
			/// </summary>
			/// <remarks>
			/// Данный должны быть разделены построчно
			/// </remarks>
			/// <param name="file_asset">Текстовый ресурс - данные в формате XML</param>
			//-------------------------------------------------------------------------------------------------------------
			public void LoadItemsFromAssest(TextAsset file_asset)
			{
				// 1) Делим по строчно
				String[] items = file_asset.GetAllLines();

				for (Int32 i = 0; i < items.Length; i++)
				{
					// 2) Делим по столбцам
					String[] texts = items[i].Split(XChar.SeparatorDotComma);

					// 3) Если есть данные
					if (texts.Length > 1)
					{
						CGUIListItem item = new CGUIListItem(this);

						// 4) Корректируем
						for (Int32 ic = 0; ic < mColumns.Count; ic++)
						{
							item[ic].text = texts[ic].Trim(new char[] { ' ', ';', '	' });
						}

						mItems.Add(item);
					}
				}

				UpdateHeight();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Снятие выбора со всех строк данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void UnselectItems()
			{
				for (Int32 i = 0; i < mItems.Count; i++)
				{
					mItems[i].mIsSelected = false;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент списка однотипных данных в иерархических отношениях
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUITreeItem : CGUIContentItem
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal Int32 mDepth;
			[NonSerialized]
			internal Boolean mIsChecked;
			[NonSerialized]
			internal Boolean mIsSelected;
			[NonSerialized]
			internal Boolean mIsOpened;
			[NonSerialized]
			internal List<CGUITreeItem> mChildren;
			[NonSerialized]
			internal CGUITreeItem mParent;
			[NonSerialized]
			internal CGUITreeView mOwner;

			// Параметры размещения
			[NonSerialized]
			internal Single mHeight;
			[NonSerialized]
			internal Single mHeightActual;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Глубина вложенности узла
			/// </summary>
			/// <remarks>
			/// Корневые узлы дерева имеют глубину 0
			/// </remarks>
			public Int32 Depth
			{
				get { return mDepth; }
				set { mDepth = value; }
			}

			/// <summary>
			/// Статус отмеченного узла
			/// </summary>
			/// <remarks>
			/// Узлы можно отмечать для выполнения групповых операции над ними
			/// </remarks>
			public Boolean IsChecked
			{
				get { return mIsChecked; }
				set { mIsChecked = value; }
			}

			/// <summary>
			/// Статус выбора узла
			/// </summary>
			public Boolean IsSelected
			{
				get { return mIsSelected; }
				set { mIsSelected = value; }
			}

			/// <summary>
			/// Статус раскрытия узла
			/// </summary>
			public Boolean IsOpened
			{
				get { return mIsOpened; }
				set { mIsOpened = value; }
			}

			/// <summary>
			/// Список дочерных узлов
			/// </summary>
			public List<CGUITreeItem> Children
			{
				get { return mChildren; }
			}

			/// <summary>
			/// Родительский узел
			/// </summary>
			public CGUITreeItem Parent
			{
				get { return mParent; }
				set { mParent = value; }
			}

			/// <summary>
			/// Элемент владелец данным узлом
			/// </summary>
			public CGUITreeView Owner
			{
				get { return mOwner; }
				set { mOwner = value; }
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Высота узла
			/// </summary>
			public Single Height
			{
				get { return mHeight; }
				set
				{
					if(mHeight != value)
					{
						mHeight = value;
						UpdateNodeHeight();
						mOwner.UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Актуальная высота узла
			/// </summary>
			public Single HeightActual
			{
				get { return mHeightActual; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует данные узла предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeItem()
			{
				Text = "";
				IsOpened = true;
				mChildren = new List<CGUITreeItem>();
				mHeight = 32;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные узла указанными значениями
			/// </summary>
			/// <param name="text">Название узла</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeItem(String text)
			{
				Text = text;
				IsOpened = true;
				mChildren = new List<CGUITreeItem>();
				mHeight = 32;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные узла указанными значениями
			/// </summary>
			/// <param name="text">Название узла</param>
			/// <param name="parent">Родительский узел</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeItem(String text, CGUITreeItem parent)
			{
				mText = text;
				mIsOpened = true;
				mChildren = new List<CGUITreeItem>();
				mParent = parent;
				mHeight = 32;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateNodeHeight()
			{
				switch (mOwner.AspectMode)
				{
					case TAspectMode.None:
						{
							mHeightActual = mHeight;
						}
						break;
					case TAspectMode.Proportional:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Снятие выбора с узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UnsetSelecting()
			{
				mIsSelected = false;
				if (mChildren.Count > 0)
				{
					for (Int32 i = 0; i < mChildren.Count; i++)
					{
						mChildren[i].UnsetSelecting();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление потомка к узлу
			/// </summary>
			/// <param name="tree_item">Добавляемый узел</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddChild(CGUITreeItem tree_item)
			{
				tree_item.Parent = this;
				mChildren.Add(tree_item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление потомка к узлу
			/// </summary>
			/// <param name="text">Название добавляемого узла</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddChild(String text)
			{
				CGUITreeItem tree_item = new CGUITreeItem(text);
				tree_item.Parent = this;
				mChildren.Add(tree_item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование узла
			/// </summary>
			/// <param name="rect_node">Прямоугольник</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DrawNode(ref Rect rect_node)
			{
				// Снимаем/выбираем текущий узел
				if (Event.current.type == EventType.MouseDown)
				{
					if (rect_node.Contains(Event.current.mousePosition))
					{
						// Снимаем со-всех остальных
						mOwner.UnsetSelecting();

						// Выбираем текущий
						mIsSelected = true;
						mOwner.SelectedItem = this;
					}
				}

				// Рисуем кнопку
				Single button_size = mOwner.mButtonSizeCurrent;
				if (mChildren.Count > 0)
				{
					Rect rect_button = rect_node;
					rect_button.width = button_size;
					rect_button.height = Mathf.Clamp(button_size, 24, rect_node.height);

					GUIStyle.none.alignment = TextAnchor.MiddleCenter;
					if (GUI.Button(rect_button, IsOpened ? XString.TriangleDown : XString.TriangleRight, GUIStyle.none))
					{
						IsOpened = !IsOpened;
						mOwner.UpdateHeightTree();
					}
				}

				// Смещаем на величину
				rect_node.x += button_size - 2;
				rect_node.width -= button_size - 2;

				// Если есть выбор
				if(mOwner.IsChecked)
				{
					Rect rect_toogle = rect_node;
					rect_toogle.width = button_size;
					rect_toogle.height = Mathf.Clamp(button_size, 24, rect_node.height);
					mIsChecked = GUI.Toggle(rect_toogle, IsChecked, "");

					rect_node.x += button_size - 2;
					rect_node.width -= button_size - 2;
				}

				// При выбранном элементе рисуем фон
				if (mIsSelected && mOwner.IsBackSelected)
				{
					GUI.backgroundColor = mOwner.BackSelectedColor;
					GUI.Box(rect_node, "");
					GUI.backgroundColor = Color.white;
				}

				LotusGUIDispatcher.CurrentContent.text = mText;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				GUI.Button(rect_node, LotusGUIDispatcher.CurrentContent, mOwner.StyleItem);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент интерфейса GUI для отображения иерархических (древовидных) данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUITreeView : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal CGUITreeItem mSelectedItem;
			[NonSerialized]
			internal List<CGUITreeItem> mRoots;
			[NonSerialized]
			internal Boolean mIsChecked;
			[SerializeField]
			internal Boolean mIsBackSelected;
			[SerializeField]
			internal Color mBackSelectedColor;
			[SerializeField]
			internal String mStyleItemName;

			// Параметры размещения
			[SerializeField]
			internal Single mOffsetDepth;
			[SerializeField]
			internal Single mSpaceItem;
			[SerializeField]
			internal Single mButtonSize;

			// Служебные данные
			[NonSerialized]
			internal GUIStyle mStyleItem;
			[NonSerialized]
			internal Single mOffsetDepthCurrent;
			[NonSerialized]
			internal Single mButtonSizeCurrent;
			[NonSerialized]
			internal Rect mRectDrawView;
			[NonSerialized]
			internal Vector2 mScrollData;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Выбранный узел дерева
			/// </summary>
			public CGUITreeItem SelectedItem
			{
				get { return mSelectedItem; }
				set
				{
					mSelectedItem = value;
				}
			}

			/// <summary>
			/// Корни дерева
			/// </summary>
			public List<CGUITreeItem> Roots
			{
				get { return mRoots; }
			}

			/// <summary>
			/// Режим редактирования описания
			/// </summary>
			public Boolean IsChecked
			{
				get { return mIsChecked; }
				set
				{
					if (mIsChecked != value)
					{
						mIsChecked = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Статус обозначения фоновым цветом выбранного узла
			/// </summary>
			public Boolean IsBackSelected
			{
				get { return mIsBackSelected; }
				set
				{
					if (mIsBackSelected != value)
					{
						mIsBackSelected = value;
					}
				}
			}

			/// <summary>
			/// Фоновый цвет выбранного узла
			/// </summary>
			public Color BackSelectedColor
			{
				get { return mBackSelectedColor; }
				set
				{
					mBackSelectedColor = value;
				}
			}

			/// <summary>
			/// Имя стиля для рисования узла дерева
			/// </summary>
			public String StyleItemName
			{
				get { return mStyleItemName; }
				set
				{
					if (mStyleItemName != value)
					{
						mStyleItemName = value;
						mStyleItem = LotusGUIDispatcher.FindStyle(mStyleItemName);
					}
				}
			}

			/// <summary>
			/// Стиль для рисования узла дерева
			/// </summary>
			public GUIStyle StyleItem
			{
				get
				{
					if (mStyleItem == null)
					{
						mStyleItem = LotusGUIDispatcher.FindStyle(mStyleItemName);
					}
					return mStyleItem;
				}
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Смещение глубины списка
			/// </summary>
			public Single OffsetDepth
			{
				get { return mOffsetDepth; }
				set
				{
					if (mOffsetDepth != value)
					{
						mOffsetDepth = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Расстояние между элементами
			/// </summary>
			public Single SpaceItem
			{
				get { return mSpaceItem; }
				set
				{
					if (mSpaceItem != value)
					{
						mSpaceItem = value;
						UpdatePlacement();
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
						UpdatePlacement();
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует данные редактора значениями по умолчанию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeView()
				:base()
			{
				mRoots = new List<CGUITreeItem>();
				mBackSelectedColor = Color.green;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeView(String name)
				: base(name)
			{
				mRoots = new List<CGUITreeItem>();
				mBackSelectedColor = Color.green;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeView(String name, Single x, Single y)
				: base(name, x, y)
			{
				mRoots = new List<CGUITreeItem>();
				mBackSelectedColor = Color.green;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует данные редактора значениями по умолчанию
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="text">Название первого корневого узла</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeView(String name, String text)
				:base(name)
			{
				mRoots = new List<CGUITreeItem>();
				CGUITreeItem node = new CGUITreeItem(text);
				node.Owner = this;
				Roots.Add(node);
				mBackSelectedColor = Color.green;
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

				UpdateNodeHeight();

				switch (mAspectMode)
				{
					case TAspectMode.None:
						{
							mButtonSizeCurrent = mButtonSize;
							mOffsetDepthCurrent = mOffsetDepth;
						}
						break;
					case TAspectMode.Proportional:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenX;
							mOffsetDepthCurrent = mOffsetDepth * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenX;
							mOffsetDepthCurrent = mOffsetDepth * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenY;
							mOffsetDepthCurrent = mOffsetDepth * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}

				UpdateHeightTree();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление общей высоты рисования дерева с учетом раскрытия узлов
			/// </summary>
			/// <returns>Общая высота рисования дерева с учетом раскрытия узлов</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single ComputeTotalHeight()
			{
				Single height = 0;
				for (Int32 i = 0; i < Roots.Count; i++)
				{
					height += ComputeTotalHeight(Roots[i]);
				}

				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление общей высоты рисования дерева с учетом раскрытия узлов
			/// </summary>
			/// <param name="node">Узел</param>
			/// <returns>Общая высота рисования дерева с учетом раскрытия узлов</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single ComputeTotalHeight(CGUITreeItem node)
			{
				Single height = node.mHeightActual + mSpaceItem;
				if (node.Children.Count > 0 && node.IsOpened)
				{
					for (Int32 i = 0; i < node.Children.Count; i++)
					{
						height += ComputeTotalHeight(node.Children[i]);
					}
				}

				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			internal void UpdateHeightTree()
			{
				mRectDrawView.height = ComputeTotalHeight();
				if (mRectDrawView.height > mRectWorldScreenMain.height - (PaddingTop + PaddingBottom))
				{
					mRectDrawView.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingTop + LotusGUIDispatcher.SizeScrollVertical + 2);
				}
				else
				{
					mRectDrawView.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingTop);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdateNodeHeight()
			{
				for (Int32 i = 0; i < Roots.Count; i++)
				{
					UpdateNodeHeight(Roots[i]);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			/// <param name="node">Узел</param>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdateNodeHeight(CGUITreeItem node)
			{
				node.UpdateNodeHeight();
				if (node.Children.Count > 0)
				{
					for (Int32 i = 0; i < node.Children.Count; i++)
					{
						UpdateNodeHeight(node.Children[i]);
					}
				}
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
				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				if (mRoots.Count > 0)
				{
					mScrollData = GUI.BeginScrollView(mRectWorldScreenMain, mScrollData, mRectDrawView);
					{
						Rect rect_node = new Rect(0, 0, mRectDrawView.width - 2, Roots[0].mHeightActual);
						for (Int32 i = 0; i < Roots.Count; i++)
						{
							DrawTree(Roots[i], ref rect_node, 0);
						}
					}
					GUI.EndScrollView();
				}
				else
				{
					GUI.Box(mRectWorldScreenMain, "");
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование узла дерева
			/// </summary>
			/// <param name="node">Узел</param>
			/// <param name="rect_node">Прямоугольник для отображения узла</param>
			/// <param name="depth">Глубина</param>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawTree(CGUITreeItem node, ref Rect rect_node, Int32 depth)
			{
				// Рисуем сам узел
				Rect rect_current_node = rect_node;

				rect_current_node.x += mOffsetDepthCurrent * depth;
				rect_current_node.width -= mOffsetDepthCurrent * depth;
				rect_current_node.height = node.mHeightActual;

				node.Depth = depth;
				node.DrawNode(ref rect_current_node);

				rect_node.y += node.mHeightActual + mSpaceItem;

				if (node.Children.Count > 0 && node.IsOpened)
				{
					depth++;
					for (Int32 i = 0; i < node.Children.Count; i++)
					{
						DrawTree(node.Children[i], ref rect_node, depth);
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С УЗЛАМИ ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Снятие выбора с узлов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UnsetSelecting()
			{
				for (Int32 i = 0; i < Roots.Count; i++)
				{
					Roots[i].UnsetSelecting();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание узла
			/// </summary>
			/// <param name="text">Название узла</param>
			/// <returns>Созданный узел</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual CGUITreeItem CreateNode(String text)
			{
				var node = new CGUITreeItem(text);
				node.Owner = this;
				return node;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Строка данных - запись списка строковых данных в иерархических отношениях
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUITreeListItem
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal Int32 mDepth;
			[NonSerialized]
			internal Boolean mIsChecked;
			[NonSerialized]
			internal Boolean mIsSelected;
			[NonSerialized]
			internal Boolean mIsOpened;
			[NonSerialized]
			internal List<CGUITreeListItem> mChildren;
			[NonSerialized]
			internal TListRowType mItemType;
			[NonSerialized]
			internal GUIContent[] mContents;
			[NonSerialized]
			internal Int32 mFilter;
			[NonSerialized]
			internal Int32 mIndex;
			[NonSerialized]
			internal CGUITreeListItem mParent;
			[NonSerialized]
			internal CGUITreeListView mOwner;

			// Параметры размещения
			[NonSerialized]
			internal Single mHeight;
			[NonSerialized]
			internal Single mHeightActual;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Глубина вложенности узла
			/// </summary>
			/// <remarks>
			/// Корневые узлы дерева имеют глубину 0
			/// </remarks>
			public Int32 Depth
			{
				get { return mDepth; }
				set { mDepth = value; }
			}

			/// <summary>
			/// Статус отмеченного узла
			/// </summary>
			/// <remarks>
			/// Узлы можно отмечать для выполнения групповых операции над ними
			/// </remarks>
			public Boolean IsChecked
			{
				get { return mIsChecked; }
				set { mIsChecked = value; }
			}

			/// <summary>
			/// Статус выбора узла
			/// </summary>
			public Boolean IsSelected
			{
				get { return mIsSelected; }
				set { mIsSelected = value; }
			}

			/// <summary>
			/// Статус раскрытия узла
			/// </summary>
			public Boolean IsOpened
			{
				get { return mIsOpened; }
				set { mIsOpened = value; }
			}

			/// <summary>
			/// Список дочерных узлов
			/// </summary>
			public List<CGUITreeListItem> Children
			{
				get { return mChildren; }
			}

			/// <summary>
			/// Тип строки данных
			/// </summary>
			public TListRowType ItemType
			{
				get { return mItemType; }
				set { mItemType = value; }
			}

			/// <summary>
			/// Массив контента данных(ячеек)
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
			/// Количества контента данных (ячеек)
			/// </summary>
			public Int32 Count
			{
				get { return mContents.Length; }
			}

			/// <summary>
			/// Фильтр для строки данных
			/// </summary>
			/// <remarks>
			/// Нулевое значение означает что строка видна
			/// </remarks>
			public Int32 Filter
			{
				get { return mFilter; }
				set { mFilter = value; }
			}

			/// <summary>
			/// Индекс строки данных в списке
			/// </summary>
			public Int32 Index
			{
				get { return mIndex; }
				set { mIndex = value; }
			}

			/// <summary>
			/// Элемент владелец
			/// </summary>
			public CGUITreeListView Owner
			{
				get { return mOwner; }
				set { mOwner = value; }
			}

			/// <summary>
			/// Родительский узел
			/// </summary>
			public CGUITreeListItem Parent
			{
				get { return mParent; }
				set { mParent = value; }
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Высота строка данных
			/// </summary>
			public Single Height
			{
				get { return mHeight; }
				set
				{
					mHeight = value;
					//ComputeHeightActual();
				}
			}

			/// <summary>
			/// Актуальная высота строки данных
			/// </summary>
			public Single HeightActual
			{
				get { return mHeightActual; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует данные узла предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListItem()
			{
				IsOpened = true;
				mChildren = new List<CGUITreeListItem>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="owner">Владелец</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListItem(CGUITreeListView owner)
			{
				IsOpened = true;
				mChildren = new List<CGUITreeListItem>();
				mHeight = 30;
				mOwner = owner;
				mContents = new GUIContent[mOwner.Columns.Count];
				for (Int32 i = 0; i < mContents.Length; i++)
				{
					mContents[i] = new GUIContent();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="owner">Владелец</param>
			/// <param name="texts">Массив текстовых данных</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListItem(CGUITreeListView owner, params String[] texts)
			{
				IsOpened = true;
				mChildren = new List<CGUITreeListItem>();
				mHeight = 30;
				mOwner = owner;
				mContents = new GUIContent[mOwner.Columns.Count];
				for (Int32 i = 0; i < mContents.Length; i++)
				{
					if (i < texts.Length)
					{
						mContents[i] = new GUIContent(texts[i]);
					}
					else
					{
						mContents[i] = new GUIContent();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные узла указанными значениями
			/// </summary>
			/// <param name="owner">Владелец</param>
			/// <param name="parent">Родительский узел</param>
			/// <param name="texts">Массив текстовых данных</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListItem(CGUITreeListView owner, CGUITreeListItem parent, params String[] texts)
			{
				IsOpened = true;
				mChildren = new List<CGUITreeListItem>();
				mHeight = 30;
				mOwner = owner;
				mContents = new GUIContent[mOwner.Columns.Count];
				for (Int32 i = 0; i < mContents.Length; i++)
				{
					if (i < texts.Length)
					{
						mContents[i] = new GUIContent(texts[i]);
					}
					else
					{
						mContents[i] = new GUIContent();
					}
				}
				mParent = parent;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateNodeHeight()
			{
				switch (mOwner.AspectMode)
				{
					case TAspectMode.None:
						{
							mHeightActual = mHeight;
						}
						break;
					case TAspectMode.Proportional:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mHeightActual = mHeight * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Снятие выбора с узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UnsetSelecting()
			{
				mIsSelected = false;
				if (mChildren.Count > 0)
				{
					for (Int32 i = 0; i < mChildren.Count; i++)
					{
						mChildren[i].UnsetSelecting();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление потомка к узлу
			/// </summary>
			/// <param name="tree_item">Добавляемый узел</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddChild(CGUITreeListItem tree_item)
			{
				tree_item.Parent = this;
				mChildren.Add(tree_item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление потомка к узлу
			/// </summary>
			/// <param name="text">Название добавляемого узла</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddChild(params String[] text)
			{
				CGUITreeListItem tree_item = new CGUITreeListItem(mOwner, this, text);
				mChildren.Add(tree_item);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Столбец данных - поле списка иерархических строковых данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUITreeListColumn
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mName;
			[SerializeField]
			internal TListColumnType mColumnType;
			[NonSerialized]
			internal Int32 mIndex;
			[NonSerialized]
			internal CGUITreeListView mOwner;

			// Параметры отображения
			[SerializeField]
			internal String mStyleSimpleName;
			[SerializeField]
			internal String mStyleHeaderName;
			[SerializeField]
			internal String mStyleResultsName;
			[NonSerialized]
			internal GUIStyle mStyleSimple;
			[NonSerialized]
			internal GUIStyle mStyleHeader;
			[NonSerialized]
			internal GUIStyle mStyleResults;

			// Параметры размещения
			[SerializeField]
			internal Single mWidth;
			[NonSerialized]
			internal Single mWidthActual;
			[NonSerialized]
			internal Rect mRectContent;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя столбца
			/// </summary>
			/// <remarks>
			/// Имя столбца должно быть уникальным в пределах одного элемента
			/// </remarks>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Тип столбца
			/// </summary>
			public TListColumnType ColumnType
			{
				get { return mColumnType; }
				set { mColumnType = value; }
			}

			/// <summary>
			/// Индекс столбца в списке столбцов
			/// </summary>
			public Int32 Index
			{
				get { return mIndex; }
				set { mIndex = value; }
			}

			/// <summary>
			/// Элемент владелец
			/// </summary>
			public CGUITreeListView Owner
			{
				get { return mOwner; }
				set { mOwner = value; }
			}

			//
			// ПАРАМЕТРЫ ОТОБРАЖЕНИЯ
			//
			/// <summary>
			/// Имя стиля для рисования обычной ячейки данных
			/// </summary>
			public String StyleSimpleName
			{
				get { return mStyleSimpleName; }
				set
				{
					if (mStyleSimpleName != value)
					{
						mStyleSimpleName = value;
						mStyleSimple = LotusGUIDispatcher.FindStyle(mStyleSimpleName);
					}
				}
			}

			/// <summary>
			/// Имя стиля для рисования заголовка ячейки данных
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
			/// Имя стиля для рисования итогов ячейки данных
			/// </summary>
			public String StyleResultsName
			{
				get { return mStyleResultsName; }
				set
				{
					if (mStyleResultsName != value)
					{
						mStyleResultsName = value;
						mStyleResults = LotusGUIDispatcher.FindStyle(mStyleResultsName);
					}
				}
			}

			/// <summary>
			/// Стиль для рисования обычной ячейки данных
			/// </summary>
			public GUIStyle StyleSimple
			{
				get
				{
					if (mStyleSimple == null)
					{
						mStyleSimple = LotusGUIDispatcher.FindStyle(mStyleSimpleName);
					}
					return mStyleSimple;
				}
			}

			/// <summary>
			/// Стиль для рисования заголовка ячейки данных
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

			/// <summary>
			/// Стиль для рисования итогов ячейки данных
			/// </summary>
			public GUIStyle StyleResults
			{
				get
				{
					if (mStyleResults == null)
					{
						mStyleResults = LotusGUIDispatcher.FindStyle(mStyleResultsName);
					}
					return mStyleResults;
				}
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Ширина столбца
			/// </summary>
			public Single Width
			{
				get { return mWidth; }
				set
				{
					mWidth = value;
					if (mOwner != null)
					{
						mOwner.ComputeColumnWidth();
					}
				}
			}

			/// <summary>
			/// Актуальная ширина столбца
			/// </summary>
			public Single WidthActual
			{
				get { return mWidthActual; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListColumn()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListColumn(String name)
			{
				mName = name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя столбца</param>
			/// <param name="owner">Владелец</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListColumn(String name, CGUITreeListView owner)
			{
				mName = name;
				mOwner = owner;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя столбца</param>
			/// <param name="owner">Владелец</param>
			/// <param name="width">Ширина столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListColumn(String name, CGUITreeListView owner, Single width)
			{
				mName = name;
				mOwner = owner;
				mWidth = width;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ СО СТИЛЕМ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление стилей ячеек
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RecoveryStyles()
			{
				if (mStyleSimple == null) mStyleSimple = LotusGUIDispatcher.FindStyle(mStyleSimpleName);
				if (mStyleHeader == null) mStyleHeader = LotusGUIDispatcher.FindStyle(mStyleHeaderName);
				if (mStyleResults == null) mStyleResults = LotusGUIDispatcher.FindStyle(mStyleResultsName);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент интерфейса GUI для отображения иерархических (древовидных) строковых данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUITreeListView : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal CGUITreeListItem mSelectedItem;
			[NonSerialized]
			internal List<CGUITreeListItem> mRoots;
			[SerializeField]
			internal List<CGUITreeListColumn> mColumns;
			[SerializeField]
			internal Boolean mIsBackSelected;
			[SerializeField]
			internal Color mBackSelectedColor;

			// Параметры размещения
			[SerializeField]
			internal Single mOffsetDepth;
			[SerializeField]
			internal Single mSpaceItem;
			[SerializeField]
			internal Single mButtonSize;

			// Служебные данные
			[NonSerialized]
			internal Single mOffsetDepthCurrent;
			[NonSerialized]
			internal Single mButtonSizeActual;
			[NonSerialized]
			internal Rect mRectDrawView;
			[NonSerialized]
			internal Vector2 mScrollData;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Выбранный узел дерева
			/// </summary>
			public CGUITreeListItem SelectedItem
			{
				get { return mSelectedItem; }
				set
				{
					mSelectedItem = value;
				}
			}

			/// <summary>
			/// Корни дерева
			/// </summary>
			public List<CGUITreeListItem> Roots
			{
				get { return mRoots; }
			}

			/// <summary>
			/// Список столбцов
			/// </summary>
			public List<CGUITreeListColumn> Columns
			{
				get { return mColumns; }
			}

			/// <summary>
			/// Статус обозначения фоновым цветом выбранного узла
			/// </summary>
			public Boolean IsBackSelected
			{
				get { return mIsBackSelected; }
				set
				{
					if (mIsBackSelected != value)
					{
						mIsBackSelected = value;
					}
				}
			}

			/// <summary>
			/// Фоновый цвет выбранного узла
			/// </summary>
			public Color BackSelectedColor
			{
				get { return mBackSelectedColor; }
				set
				{
					mBackSelectedColor = value;
				}
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Смещение глубины списка
			/// </summary>
			public Single OffsetDepth
			{
				get { return mOffsetDepth; }
				set
				{
					if (mOffsetDepth != value)
					{
						mOffsetDepth = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Расстояние между элементами
			/// </summary>
			public Single SpaceItem
			{
				get { return mSpaceItem; }
				set
				{
					if (mSpaceItem != value)
					{
						mSpaceItem = value;
						UpdatePlacement();
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
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Актуальный размер кнопки
			/// </summary>
			public Single ButtonSizeActual
			{
				get { return mButtonSizeActual; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует данные редактора значениями по умолчанию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListView()
				: base()
			{
				mStyleMainName = "ScrollView";
				mRoots = new List<CGUITreeListItem>();
				mColumns = new List<CGUITreeListColumn>();
				mColumns.Add(new CGUITreeListColumn("Main"));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListView(String name)
				: base(name)
			{
				mStyleMainName = "ScrollView";
				mRoots = new List<CGUITreeListItem>();
				mColumns = new List<CGUITreeListColumn>();
				mColumns.Add(new CGUITreeListColumn("Main"));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITreeListView(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "ScrollView";
				mRoots = new List<CGUITreeListItem>();
				mColumns = new List<CGUITreeListColumn>();
				mColumns.Add(new CGUITreeListColumn("Main"));
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

				UpdateNodeHeight();

				UpdateColumnWidth();

				switch (mAspectMode)
				{
					case TAspectMode.None:
						{
							mButtonSizeActual = mButtonSize;
							mOffsetDepthCurrent = mOffsetDepth;
						}
						break;
					case TAspectMode.Proportional:
						{
							mButtonSizeActual = mButtonSize * LotusGUIDispatcher.ScaledScreenX;
							mOffsetDepthCurrent = mOffsetDepth * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							mButtonSizeActual = mButtonSize * LotusGUIDispatcher.ScaledScreenX;
							mOffsetDepthCurrent = mOffsetDepth * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							mButtonSizeActual = mButtonSize * LotusGUIDispatcher.ScaledScreenY;
							mOffsetDepthCurrent = mOffsetDepth * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}

				UpdateHeightTree();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции узлов
			/// </summary>
			/// <param name="depth">Глубина узла</param>
			/// <param name="y">Позиция по Y</param>
			/// <param name="h">Высота узла</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeRectsContent(Int32 depth, Single y, Single h)
			{
				Single offset_x = PaddingLeft + mOffsetDepthCurrent * depth + mButtonSizeActual;
				mColumns[0].mRectContent.x = offset_x;
				mColumns[0].mRectContent.y = y;
				mColumns[0].mRectContent.width = mColumns[0].mWidthActual - offset_x;
				mColumns[0].mRectContent.height = h;

				for (Int32 i = 1; i < mColumns.Count; i++)
				{
					mColumns[i].mRectContent.x = mColumns[i - 1].mRectContent.xMax;
					mColumns[i].mRectContent.y = y;
					mColumns[i].mRectContent.width = mColumns[i].mWidthActual;
					mColumns[i].mRectContent.height = h;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление общей высоты рисования дерева с учетом раскрытия узлов
			/// </summary>
			/// <returns>Общая высота дерева</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single ComputeTotalHeight()
			{
				Single height = 0;
				for (Int32 i = 0; i < Roots.Count; i++)
				{
					height += ComputeTotalHeight(Roots[i]);
				}

				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление обще высоты рисования дерева с учетом раскрытия узлов
			/// </summary>
			/// <param name="node">Узел</param>
			/// <returns>Общая высота дерева</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single ComputeTotalHeight(CGUITreeListItem node)
			{
				Single height = node.mHeightActual;
				if (node.Children.Count > 0 && node.IsOpened)
				{
					for (Int32 i = 0; i < node.Children.Count; i++)
					{
						height += ComputeTotalHeight(node.Children[i]);
					}
				}

				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			internal void UpdateHeightTree()
			{
				mRectDrawView.height = ComputeTotalHeight();
				if (mRectDrawView.height > mRectWorldScreenMain.height - (PaddingTop + PaddingBottom))
				{
					mRectDrawView.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingTop + LotusGUIDispatcher.SizeScrollVertical + 2);
				}
				else
				{
					mRectDrawView.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingTop);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdateNodeHeight()
			{
				for (Int32 i = 0; i < Roots.Count; i++)
				{
					UpdateNodeHeight(Roots[i]);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdateColumnWidth()
			{
				// 1) Считаем общую ширину столбцов
				Single total_width = 0;
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					total_width += mColumns[i].Width;
				}

				// 2) Считаем коэффициент коррекции
				Single correct = mRectDrawView.width / total_width;

				// 3) Корректируем
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					mColumns[i].mWidthActual = mColumns[i].mWidth * correct;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление высоты узла
			/// </summary>
			/// <param name="node">Узел</param>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdateNodeHeight(CGUITreeListItem node)
			{
				node.UpdateNodeHeight();
				if (node.Children.Count > 0)
				{
					for (Int32 i = 0; i < node.Children.Count; i++)
					{
						UpdateNodeHeight(node.Children[i]);
					}
				}
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
				ResetColumns();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование дерева
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				if (mRoots.Count > 0)
				{
					mScrollData = GUI.BeginScrollView(mRectWorldScreenMain, mScrollData, mRectDrawView);
					{
						Rect rect_node = new Rect(PaddingLeft, PaddingTop, mRectDrawView.width, mRoots[0].mHeightActual);
						for (Int32 i = 0; i < Roots.Count; i++)
						{
							DrawTree(Roots[i], ref rect_node, 0);
						}
					}
					GUI.EndScrollView();
				}
				else
				{
					GUI.Box(mRectWorldScreenMain, "");
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование узла дерева
			/// </summary>
			/// <param name="node">Узел</param>
			/// <param name="rect_node">Прямоугольник для отображения узла</param>
			/// <param name="depth">Глубина</param>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawTree(CGUITreeListItem node, ref Rect rect_node, Int32 depth)
			{
				// Обозначаем глубину узла
				node.Depth = depth;

				// Прямоугольник для вывод текущего узла
				Rect rect_current_node = rect_node;

				// Смещение в начале
				Single offset_x = PaddingLeft + mOffsetDepthCurrent * depth;

				rect_current_node.x = offset_x;
				rect_current_node.width = mRectDrawView.width - offset_x;
				rect_current_node.height = node.mHeightActual;

				// Рисуем кнопку
				Single button_size = mButtonSizeActual;
				if (node.mChildren.Count > 0)
				{
					Rect rect_button = rect_current_node;
					rect_button.width = button_size;
					rect_button.height = Mathf.Clamp(button_size, 24, rect_node.height);

					GUIStyle.none.alignment = TextAnchor.MiddleCenter;
					if (GUI.Button(rect_button, node.IsOpened ? XString.TriangleDown : XString.TriangleRight, GUIStyle.none))
					{
						node.IsOpened = !node.IsOpened;
						UpdateHeightTree();
					}
				}

				rect_current_node.x += button_size;
				rect_current_node.width -= button_size;

				// Снимаем/выбираем текущий узел
				if (Event.current.type == EventType.MouseDown)
				{
					if (rect_current_node.Contains(Event.current.mousePosition))
					{
						// Снимаем со-всех остальных
						UnsetSelecting();

						// Выбираем текущий
						node.mIsSelected = true;
						mSelectedItem = node;
					}
				}

				// При выбранном элементе рисуем фон
				if (node.mIsSelected && mIsBackSelected)
				{
					GUI.backgroundColor = mBackSelectedColor;
					GUI.Box(rect_current_node, "");
					GUI.backgroundColor = Color.white;
				}

				// Вычисляем позиции ячеек
				ComputeRectsContent(depth, rect_node.y, node.mHeightActual);

				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					switch (node.ItemType)
					{
						case TListRowType.Simple:
							{
								GUI.Label(mColumns[i].mRectContent, node.mContents[i], mColumns[i].StyleSimple);
							}
							break;
						case TListRowType.Alternative:
							{
								GUI.Label(mColumns[i].mRectContent, node.mContents[i], mColumns[i].StyleSimple);
							}
							break;
						case TListRowType.Header:
							{
								if (GUI.Button(mColumns[i].mRectContent, node.mContents[i], mColumns[i].StyleHeader))
								{
									//if (mOnHeaderClick != null) mOnHeaderClick(mColumns[i]);
								}
							}
							break;
						case TListRowType.Results:
							{
								GUI.Label(mColumns[i].mRectContent, node.mContents[i], mColumns[i].StyleResults);
							}
							break;
						default:
							break;
					}
				}

				rect_node.y += node.mHeightActual + mSpaceItem;

				if (node.Children.Count > 0 && node.IsOpened)
				{
					depth++;
					for (Int32 i = 0; i < node.Children.Count; i++)
					{
						DrawTree(node.Children[i], ref rect_node, depth);
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ СО СТОЛБЦАМИ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных столбцов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ResetColumns()
			{
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					mColumns[i].Index = i;
					mColumns[i].RecoveryStyles();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление актуальной ширины столбцов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeColumnWidth()
			{
				// 1) Считаем общую ширину столбцов
				Single total_width = 0;
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					total_width += mColumns[i].Width;
				}

				// 2) Считаем коэффициент коррекции
				Single correct = mRectDrawView.width / total_width;

				// 3) Корректируем
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					mColumns[i].mWidthActual = mColumns[i].mWidth * correct;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление столбца с указанными параметрами
			/// </summary>
			/// <param name="name">Имя столбца</param>
			/// <param name="width">Ширина столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddColumn(String name, Single width)
			{
				mColumns.Add(new CGUITreeListColumn(name, this, width));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление столбца по индексу
			/// </summary>
			/// <param name="index">Индекс столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveColumn(Int32 index)
			{
				mColumns.RemoveAt(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление столбца по имени
			/// </summary>
			/// <param name="name">Имя столбца</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveColumn(String name)
			{
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					if (mColumns[i].Name == name)
					{
						mColumns.RemoveAt(i);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление стилей столбцов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RecoveryColumnsStyle()
			{
				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					mColumns[i].RecoveryStyles();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ СО СТРОКАМИ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление данных
			/// </summary>
			/// <param name="texts">Массив данных</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddItem(params String[] texts)
			{
				//mItems.Add(new CGUIListItem(this, texts));
				//UpdateHeight();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сортировка списка строк данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SortItems()
			{
				//mItems.Sort();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Снятие выбора с узлов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UnsetSelecting()
			{
				for (Int32 i = 0; i < Roots.Count; i++)
				{
					Roots[i].UnsetSelecting();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание узла
			/// </summary>
			/// <param name="list">Название узла</param>
			/// <returns>Созданный узел</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual CGUITreeListItem CreateNode(params String[] list)
			{
				var node = new CGUITreeListItem(this, list);
				node.Owner = this;
				return node;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из текстового ресурса
			/// </summary>
			/// <remarks>
			/// Данный должны быть разделены построчно
			/// </remarks>
			/// <param name="file_asset">Текстовый ресурс - данные в формате XML</param>
			//---------------------------------------------------------------------------------------------------------
			public void LoadItemsFromAssest(TextAsset file_asset)
			{
				// 1) Делим по строчно
				String[] items = file_asset.GetAllLines();

				for (Int32 i = 0; i < items.Length; i++)
				{
					// 2) Делим по столбцам
					String[] texts = items[i].Split(XChar.SeparatorDotComma);

					// 3) Если есть данные
					if (texts.Length > 1)
					{
						//CGUIListItem item = new CGUIListItem(this);

						//// 4) Корректируем
						//for (Int32 ic = 0; ic < mColumns.Count; ic++)
						//{
						//	item[ic].text = texts[ic].Trim(new char[] { ' ', ';', '	' });
						//}

						//mItems.Add(item);
					}
				}

				//UpdateHeight();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================