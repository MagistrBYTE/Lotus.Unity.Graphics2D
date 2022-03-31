//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Макетное расположение и группировка
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUILayoutGroup.cs
*		Компонент группировки дочерних элементов в родительской области.
*		Реализация компонента определяющего различные модели группирования дочерних элементов в родительской области.
*	Он имеет приоритет перед макетным расположением элемента в родительской области.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DUILayout
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент группировки дочерних элементов в родительской области
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего различные модели группирования дочерних элементов в родительской области
		/// Он имеет приоритет перед макетным расположением элемента в родительской области
		/// </remarks>
		//------------------------------------------------------------------------------------------------------------
		[Serializable]
		[RequireComponent(typeof(RectTransform))]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathFunc + "Layout/LayoutGroup")]
		public class LotusUILayoutGroup : LotusUIPlaceable2D, ILayoutGroup
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal TLayoutGroupType mGroupType;
			[SerializeField]
			internal TLayoutGroupPlacement mGroupPlacement;
			[SerializeField]
			internal TLayoutGroupVerticalAlign mGroupVerticalAlign;
			[SerializeField]
			internal TLayoutGroupHorizontalAlign mGroupHorizontalAlign;
			[SerializeField]
			internal Single mMinimalWidth;
			[SerializeField]
			internal Single mMinimalHeight;
			[SerializeField]
			internal Boolean mIsAutoSizeWidth;
			[SerializeField]
			internal Boolean mIsAutoSizeHeight;
			[SerializeField]
			internal Boolean mIsWidthGroupOfItem;
			[SerializeField]
			internal Boolean mIsHeightGroupOfItem;
			[SerializeField]
			internal Boolean mIsUpdateDelegate;
			[SerializeField]
			internal Vector4 mPadding;
			[SerializeField]
			internal Single mItemWidth;
			[SerializeField]
			internal Single mItemHeight;
			[SerializeField]
			internal Single mSpacingX;
			[SerializeField]
			internal Single mSpacingY;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedParam;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Ширина элемента с учетом настроек макета
			/// </summary>
			public Single LayoutWidth
			{
				get
				{
					if(mGroupType != TLayoutGroupType.Grid)
					{
						if (mIsAutoSizeWidth)
						{
							return GetTotalWidthItems();
						}
						else
						{
							return mUIRectElement.rect.width;
						}
					}
					else
					{
						if (mMinimalWidth > 0)
						{
							return (Mathf.Max(mMinimalWidth, mUIRectElement.rect.width));
						}
						else
						{
							return (mUIRectElement.rect.width);
						}
					}
				}
			}

			/// <summary>
			/// Высота элемента с учетом настроек макета
			/// </summary>
			public Single LayoutHeight
			{
				get
				{
					if(mGroupType != TLayoutGroupType.Grid)
					{
						if (mIsAutoSizeHeight)
						{
							return GetTotalHeightItems();
						}
						else
						{
							return mUIRectElement.rect.height;
						}
					}
					else
					{
						if(mMinimalHeight > 0)
						{
							return (Mathf.Max(mMinimalHeight, mUIRectElement.rect.height));
						}
						else
						{
							return (mUIRectElement.rect.height);
						}
					}
				}
			}

			/// <summary>
			/// Тип группирования элементов
			/// </summary>
			public TLayoutGroupType GroupType
			{
				get { return mGroupType; }
				set
				{
					if (mGroupType != value)
					{
						mGroupType = value;
						this.UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Тип размещения элементов
			/// </summary>
			public TLayoutGroupPlacement GroupPlacement
			{
				get { return mGroupPlacement; }
				set
				{
					if (mGroupPlacement != value)
					{
						mGroupPlacement = value;
						this.UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Вертикальное расположение элементов при горизонтальном выравнивании
			/// </summary>
			public TLayoutGroupVerticalAlign GroupVerticalAlign
			{
				get { return mGroupVerticalAlign; }
				set
				{
					if (mGroupVerticalAlign != value)
					{
						mGroupVerticalAlign = value;
						this.UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Горизонтальное расположение элементов при вертикальном выравнивании
			/// </summary>
			public TLayoutGroupHorizontalAlign GroupHorizontalAlign
			{
				get { return mGroupHorizontalAlign; }
				set
				{
					if (mGroupHorizontalAlign != value)
					{
						mGroupHorizontalAlign = value;
						this.UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Минимальная ширина
			/// </summary>
			public Single MinimalWidth
			{
				get { return mMinimalWidth; }
				set
				{
					if (mMinimalWidth != value)
					{
						mMinimalWidth = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Минимальная высота
			/// </summary>
			public Single MinimalHeight
			{
				get { return mMinimalHeight; }
				set
				{
					if (mMinimalHeight != value)
					{
						mMinimalHeight = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Режим автоматической подстройки ширины по содержимое
			/// </summary>
			/// <remarks>
			/// При меняется только при последовательном размещении элементов
			/// </remarks>
			public Boolean IsAutoSizeWidth
			{
				get { return mIsAutoSizeWidth; }
				set
				{
					if (mIsAutoSizeWidth != value)
					{
						mIsAutoSizeWidth = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Режим автоматической подстройки высоты по содержимое
			/// </summary>
			/// <remarks>
			/// При меняется только при последовательном размещении элементов
			/// </remarks>
			public Boolean IsAutoSizeHeight
			{
				get { return mIsAutoSizeHeight; }
				set
				{
					if (mIsAutoSizeHeight != value)
					{
						mIsAutoSizeHeight = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			///Режим ширины компонента по наибольшей ширине элемента
			/// </summary>
			public Boolean IsWidthGroupOfItem
			{
				get { return mIsWidthGroupOfItem; }
				set
				{
					if (mIsWidthGroupOfItem != value)
					{
						mIsWidthGroupOfItem = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			///Режим высоты компонента по наибольшей высоте элемента
			/// </summary>
			public Boolean IsHeightGroupOfItem
			{
				get { return mIsHeightGroupOfItem; }
				set
				{
					if (mIsHeightGroupOfItem != value)
					{
						mIsHeightGroupOfItem = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Отступы от уровня родительской области
			/// </summary>
			public Vector4 Padding
			{
				get { return mPadding; }
				set
				{
					if (mPadding != value)
					{
						mPadding = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Внутренний отступ слева от уровня родительской области
			/// </summary>
			public Single PaddingLeft
			{
				get { return mPadding.x; }
				set
				{
					mPadding.x = value;
					UpdateLayout();
				}
			}

			/// <summary>
			/// Внутренний отступ сверху от уровня родительской области
			/// </summary>
			public Single PaddingTop
			{
				get { return mPadding.y; }
				set
				{
					mPadding.y = value;
					UpdateLayout();
				}
			}

			/// <summary>
			/// Внутренний отступ справа от уровня родительской области
			/// </summary>
			public Single PaddingRight
			{
				get { return mPadding.z; }
				set
				{
					mPadding.z = value;
					UpdateLayout();
				}
			}

			/// <summary>
			/// Внутренний отступ снизу от уровня родительской области
			/// </summary>
			public Single PaddingBottom
			{
				get { return mPadding.w; }
				set
				{
					mPadding.w = value;
					UpdateLayout();
				}
			}

			/// <summary>
			/// Расстояние между элементами по горизонтали
			/// </summary>
			public Single SpacingX
			{
				get { return mSpacingX; }
				set
				{
					if (mSpacingX != value)
					{
						mSpacingX = value;
						this.UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Расстояние между элементами по вертикали
			/// </summary>
			public Single SpacingY
			{
				get { return mSpacingY; }
				set
				{
					if (mSpacingY != value)
					{
						mSpacingY = value;
						this.UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Ширина элемента при фиксированном размере
			/// </summary>
			public Single ItemWidth
			{
				get { return mItemWidth; }
				set
				{
					if (mItemWidth != value)
					{
						mItemWidth = value;
						this.UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Высота элемента при фиксированном размере
			/// </summary>
			public Single ItemHeight
			{
				get { return mItemHeight; }
				set
				{
					if (mItemHeight != value)
					{
						mItemHeight = value;
						this.UpdateLayout();
					}
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void Reset()
			{
				base.Reset();
				this.Init();
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void Awake()
			{
				base.Awake();
				this.Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение размеров прямоугольника трансформации родительского элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnRectTransformDimensionsChange()
			{
				base.OnRectTransformDimensionsChange();
				this.UpdateLayout();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение размеров прямоугольника трансформации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnTransformParentChanged()
			{
				base.OnTransformParentChanged();
				this.UpdateLayout();
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ ==========================================

			#endregion

			#region ======================================= МЕТОДЫ ILayoutGroup =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка горизонтального выравнивания
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetLayoutHorizontal()
			{
				UpdateLayout();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка вертикального выравнивания
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetLayoutVertical()
			{
				//UpdateLayout();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных и параметров компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{
				if (mUIRectElement == null)
				{
					mUIRectElement = this.GetComponent<RectTransform>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подсчет общей высоты всех дочерних элементов с учетом отступов и расстояний
			/// </summary>
			/// <returns>Общая высота всех дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetTotalHeightItems()
			{
				Single height = mPadding.y;
				for (Int32 i = 0; i < this.transform.childCount; i++)
				{
					if(i > 0)
					{
						height += mSpacingY;
					}
					height += GetHeightItem(i);
				}

				height += mPadding.w;

				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подсчет общей высоты дочерних элементов с учетом отступов и расстояний
			/// </summary>
			/// <param name="count_item">Количество элементов</param>
			/// <returns>Общая высота всех дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetTotalHeightItems(Int32 count_item)
			{
				Single height = mPadding.y;
				for (Int32 i = 0; i < count_item; i++)
				{
					if (i > 0)
					{
						height += mSpacingY;
					}
					height += GetHeightItem(i);
				}

				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение максимальной высоты элемента
			/// </summary>
			/// <returns>Максимальная высота элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetMaxHeightItem()
			{
				Single height = 0;
				if (mItemHeight != 0)
				{
					height = mItemHeight;
				}
				else
				{
					for (Int32 i = 0; i < this.transform.childCount; i++)
					{
						RectTransform child = this.transform.GetChild(i) as RectTransform;
						if (child != null && child.gameObject.activeSelf)
						{
							if (child.rect.height >= height)
							{
								height = child.rect.height;
							}
						}
					}
				}
				height += mPadding.y + mPadding.w;
				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подсчет общей ширины всех дочерних элементов с учетом отступов и расстояний
			/// </summary>
			/// <returns>Общая ширина всех дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetTotalWidthItems()
			{
				Single width = mPadding.x;
				for (Int32 i = 0; i < this.transform.childCount; i++)
				{
					if (i > 0)
					{
						width += mSpacingX;
					}
					width += GetWidthItem(i);
				}

				width += mPadding.z;

				return width;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подсчет общей ширины дочерних элементов с учетом отступов и расстояний
			/// </summary>
			/// <param name="count_item">Количество элементов</param>
			/// <returns>Общая ширина всех дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetTotalWidthItems(Int32 count_item)
			{
				Single width = mPadding.x;

				for (Int32 i = 0; i < count_item; i++)
				{
					if (i > 0)
					{
						width += mSpacingX;
					}

					width += GetWidthItem(i);
				}

				width += mPadding.z;

				return width;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение максимальной ширины элемента
			/// </summary>
			/// <returns>Максимальная ширина элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetMaxWidthItem()
			{
				Single width = 0;
				if (mItemWidth != 0)
				{
					width = mItemWidth;
				}
				else
				{
					for (Int32 i = 0; i < this.transform.childCount; i++)
					{
						RectTransform child = this.transform.GetChild(i) as RectTransform;
						if (child != null && child.gameObject.activeSelf)
						{
							if(child.rect.width >= width)
							{
								width = child.rect.width;
							}
						}
					}
				}
				width += mPadding.x + mPadding.z;
				return width;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение требуемой ширины элемента при требуемом количестве столбцов сетки
			/// </summary>
			/// <param name="required_column">Требуемое количество столбцов сетки</param>
			/// <returns>Ширина элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetRequiredWidthGrid(Int32 required_column)
			{
				Single required_width = required_column * mItemWidth + (mSpacingX * (required_column - 1)) +
					(PaddingLeft + PaddingRight);

				return (required_width);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение требуемой высоты элемента при требуемом количестве строк сетки
			/// </summary>
			/// <param name="required_row">Требуемое количество строк сетки</param>
			/// <returns>Высота элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetRequiredHeightGrid(Int32 required_row)
			{
				Single required_height = required_row * mItemHeight + (mSpacingY * (required_row - 1)) +
					(PaddingTop + PaddingBottom);

				return (required_height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение требуемой высоты элемента при требуемом количестве столбцов сетки
			/// </summary>
			/// <param name="count_element">Общее количество элементов сетки</param>
			/// <param name="required_column">Требуемое количество столбцов сетки</param>
			/// <returns>Высота элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetRequiredHeightGrid(Int32 count_element, Int32 required_column)
			{
				Single count_row = (Single)count_element / (Single)(required_column);
				Int32 r = (Int32)(count_row + 0.5f);
				Single required_height = r * mItemHeight + (SpacingY * (r - 1)) + (PaddingTop + PaddingBottom);

				return (required_height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение оптимального количество столбцов сетки при текущем размере
			/// </summary>
			/// <returns>Оптимальное количество столбцов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetOptimalCountColumnGrid()
			{
				// Считаем рабочую ширину
				Single w = Width - (PaddingLeft + PaddingRight);

				// Предварительное количество столбцов
				Int32 column = (Int32)(w / mItemWidth);

				// Считаем повторно с учетом пространства между элементами
				Single aw = column * mItemWidth + ((column - 1) * mSpacingX) + (PaddingLeft + PaddingRight);
				if(aw > Width)
				{
					column--;
				}

				return (column);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка расположения элемента по горизонтальной оси. Применяется когда выравнивание идет по вертикальной оси
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void SetHorizontalAlign()
			{
				switch (mGroupHorizontalAlign)
				{
					case TLayoutGroupHorizontalAlign.None:
						break;
					case TLayoutGroupHorizontalAlign.Left:
						{
							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									child.SetLeft(mPadding.x);
									child.SetWidth(GetWidthItem(i));
								}
							}
						}
						break;
					case TLayoutGroupHorizontalAlign.Center:
						{
							Single base_width = this.Width - (mPadding.x + mPadding.z);

							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									Single width = GetWidthItem(i);
									Single left = (base_width - width) / 2 + mPadding.x;
									child.SetLeft(left);
									child.SetWidth(width);
								}
							}
						}
						break;
					case TLayoutGroupHorizontalAlign.Right:
						{
							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									Single width = GetWidthItem(i);
									Single left = this.Width - width - mPadding.z;

									child.SetLeft(left);
									child.SetWidth(width);
								}
							}
						}
						break;
					case TLayoutGroupHorizontalAlign.Stretch:
						{
							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									child.SetLeft(mPadding.x);
									child.SetWidth(this.Width - (mPadding.x + mPadding.z));
								}
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка расположения элемента по вертикальной оси. Применяется когда выравнивание идет по горизонтальной оси
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void SetVerticalAlign()
			{
				switch (mGroupVerticalAlign)
				{
					case TLayoutGroupVerticalAlign.None:
						break;
					case TLayoutGroupVerticalAlign.Top:
						{
							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									child.SetTop(mPadding.y);
									child.SetHeight(GetHeightItem(i));
								}
							}
						}
						break;
					case TLayoutGroupVerticalAlign.Middle:
						{
							Single base_height = this.Height - (mPadding.y + mPadding.w);

							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									Single height = GetHeightItem(i);
									Single top = (base_height - height) / 2 + mPadding.y;
									child.SetTop(top);
									child.SetHeight(height);
								}
							}
						}
						break;
					case TLayoutGroupVerticalAlign.Bottom:
						{
							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									Single height = GetHeightItem(i);
									Single top = this.Height - height - mPadding.w;

									child.SetTop(top);
									child.SetHeight(height);
								}
							}
						}
						break;
					case TLayoutGroupVerticalAlign.Stretch:
						{
							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									child.SetTop(mPadding.y);
									child.SetHeight(this.Height - (mPadding.y + mPadding.w));
								}
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение ширины дочернего элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <returns>Ширина дочернего элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single GetWidthItem(Int32 index)
			{
				RectTransform child = this.transform.GetChild(index) as RectTransform;
				if (child != null && child.gameObject.activeSelf)
				{
					if (mItemWidth != 0)
					{
						return mItemWidth;
					}
					else
					{
						return child.rect.width;
					}
				}

				return 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение высоты дочернего элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <returns>Высота дочернего элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single GetHeightItem(Int32 index)
			{
				RectTransform child = this.transform.GetChild(index) as RectTransform;
				if (child != null && child.gameObject.activeSelf)
				{
					if (mItemHeight != 0)
					{
						return mItemHeight;
					}
					else
					{
						return child.rect.height;
					}
				}
				return 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подсчет общей ширины всех дочерних элементов
			/// </summary>
			/// <returns>Общая ширина всех дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single CalcTotalWidthItem()
			{
				Single width = 0;
				for (Int32 i = 0; i < this.transform.childCount; i++)
				{
					width += GetWidthItem(i);
				}

				return width;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подсчет общей высоты всех дочерних элементов
			/// </summary>
			/// <returns>Общая высота всех дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single CalcTotalHeightItem()
			{
				Single height = 0;
				for (Int32 i = 0; i < this.transform.childCount; i++)
				{
					height += GetHeightItem(i);
				}

				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Размещение дочерних элементов по горизонтали
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void SetPlacementHorizontal()
			{
				switch (mGroupPlacement)
				{
					case TLayoutGroupPlacement.Series:
						{
							Single left = mPadding.x;

							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									Single width = GetWidthItem(i);
									child.SetLeft(left);
									child.SetWidth(width);
									left += width + mSpacingX;
								}
							}
						}
						break;
					case TLayoutGroupPlacement.Distributed:
						{
							Single base_width = this.Width - (mPadding.x + mPadding.z);
							Single total_width = this.CalcTotalWidthItem();

							if (this.transform.childCount > 1)
							{
								Single dist_width = (base_width - total_width) / (this.transform.childCount - 1);
								Single left = mPadding.x;

								for (Int32 i = 0; i < this.transform.childCount; i++)
								{
									RectTransform child = this.transform.GetChild(i) as RectTransform;
									if (child != null && child.gameObject.activeSelf)
									{
										Single width = GetWidthItem(i);
										child.SetLeft(left);
										child.SetWidth(width);
										left += dist_width + width;
									}
								}
							}

						}
						break;
					case TLayoutGroupPlacement.Expanded:
						{
							if (this.transform.childCount > 1)
							{
								Single add_space = (this.transform.childCount - 1) * mSpacingX;
								Single base_width = this.Width - (mPadding.x + mPadding.z);
								Single total_width = this.CalcTotalWidthItem();
								Single coeff = (base_width - add_space) / total_width;
								Single left = mPadding.x;

								for (Int32 i = 0; i < this.transform.childCount; i++)
								{
									RectTransform child = this.transform.GetChild(i) as RectTransform;
									if (child != null && child.gameObject.activeSelf)
									{
										Single actual_width = child.rect.width * coeff;
										child.SetLeft(left);
										child.SetWidth(actual_width);
										left += actual_width + mSpacingX;
									}
								}
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Размещение дочерних элементов по вертикали
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void SetPlacementVertical()
			{
				switch (mGroupPlacement)
				{
					case TLayoutGroupPlacement.Series:
						{
							Single top = mPadding.y;

							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									Single height = GetHeightItem(i);
									child.SetTop(top);
									child.SetHeight(height);
									top += height + mSpacingY;
								}
							}
						}
						break;
					case TLayoutGroupPlacement.Distributed:
						{
							Single base_height = this.Height - (mPadding.y + mPadding.w);
							Single total_height = this.CalcTotalHeightItem();

							if (this.transform.childCount > 1)
							{
								Single dist_height = (base_height - total_height) / (this.transform.childCount - 1);
								Single top = mPadding.y;

								for (Int32 i = 0; i < this.transform.childCount; i++)
								{
									RectTransform child = this.transform.GetChild(i) as RectTransform;
									if (child != null && child.gameObject.activeSelf)
									{
										Single height = GetHeightItem(i);
										child.SetTop(top);
										child.SetHeight(height);
										top += dist_height + height;
									}
								}
							}

						}
						break;
					case TLayoutGroupPlacement.Expanded:
						{
							if (this.transform.childCount > 1)
							{
								Single add_space = (this.transform.childCount - 1) * mSpacingY;
								Single base_height = this.Height - (mPadding.y + mPadding.w);
								Single total_height = this.CalcTotalHeightItem();
								Single coeff = (base_height - add_space) / total_height;
								Single top = mPadding.y;

								for (Int32 i = 0; i < this.transform.childCount; i++)
								{
									RectTransform child = this.transform.GetChild(i) as RectTransform;
									if (child != null && child.gameObject.activeSelf)
									{
										Single actual_height = child.rect.height * coeff;
										child.SetTop(top);
										child.SetHeight(actual_height);
										top += actual_height + mSpacingY;
									}
								}
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Размещение дочерних элементов по сетки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void SetPlacementGrid()
			{
				switch (mGroupPlacement)
				{
					case TLayoutGroupPlacement.Series:
						{
							Single top = mPadding.y;
							Single left = mPadding.x;

							// Размещение последовательно по горизонтали
							Single offset_x = 0;
							Single width_grid = this.Width;
							Int32 count_element = 0;
							Boolean is_new_row = false;
							for (Int32 i = 0; i < this.transform.childCount; i++)
							{
								RectTransform child = this.transform.GetChild(i) as RectTransform;
								if (child != null && child.gameObject.activeSelf)
								{
									Single height = GetHeightItem(i);
									Single width = GetWidthItem(i);

									child.Set(left, top, width, height);
									is_new_row = false;

									// Смещаем
									left += width + mSpacingX;

									// Сметрим выходи ли мы за пределы
									offset_x = left;
									if (offset_x + width > width_grid)
									{
										// Смещаем
										left = mPadding.x;
										top += height + mSpacingY;
										is_new_row = true;
									}

									count_element++;
								}
							}

							if (is_new_row)
							{
								top += PaddingBottom;
							}
							else
							{
								top += mItemHeight + PaddingBottom;
							}

							if (mMinimalHeight > 0)
							{
								top = Mathf.Max(mMinimalHeight, top);
							}

							this.Height = top;

						}
						break;
					case TLayoutGroupPlacement.Distributed:
						{

						}
						break;
					case TLayoutGroupPlacement.Expanded:
						{

						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление местоположения и размеров дочерних элементов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[ContextMenu("UpdateLayout")]
			public void UpdateLayout()
			{
				// Изменяем размер по ширине
				if(mIsAutoSizeWidth)
				{
					switch (mGroupType)
					{
						case TLayoutGroupType.Horizontal:
							{
								if (mGroupPlacement == TLayoutGroupPlacement.Series)
								{
									// Если элементы располагаются горизонтально
									// В данном случае берем ширину все элементов с учетом всех отступов
									Single width = GetTotalWidthItems();
									if (width > 0)
									{
										if(mMinimalWidth > 0)
										{
											width = Mathf.Max(mMinimalWidth, width);
										}

										this.Width = width;
									}
								}
							}
							break;
						case TLayoutGroupType.Vertical:
							{
								// Если элементы располагаются вертикально
								// В данном случае берем ширину самого широкого элемента, если конечно он не растянут
								if (mGroupHorizontalAlign != TLayoutGroupHorizontalAlign.Stretch)
								{
									this.Width = GetMaxWidthItem();
								}
							}
							break;
						case TLayoutGroupType.Grid:
							break;
						case TLayoutGroupType.Flow:
							break;
						default:
							break;
					}
				}

				if(mIsAutoSizeHeight)
				{
					switch (mGroupType)
					{
						case TLayoutGroupType.Horizontal:
							{
								// Если элементы располагаются горизонтально
								// В данном случае берем высоту самого высокого элемента, если конечно он не растянут
								if (mGroupVerticalAlign != TLayoutGroupVerticalAlign.Stretch)
								{
									this.Height = GetMaxHeightItem();
								}
							}
							break;
						case TLayoutGroupType.Vertical:
							{
								if (mGroupPlacement == TLayoutGroupPlacement.Series)
								{
									// Если элементы располагаются вертикально
									// В данном случае берем высоту все элементов с учетом всех отступов
									Single height = GetTotalHeightItems();
									if (height > 0)
									{
										if (mMinimalHeight > 0)
										{
											height = Mathf.Max(mMinimalHeight, height);
										}

										this.Height = height;
									}
								}
							}
							break;
						case TLayoutGroupType.Grid:
							break;
						case TLayoutGroupType.Flow:
							break;
						default:
							break;
					}
				}

				if(mIsWidthGroupOfItem && mGroupType == TLayoutGroupType.Vertical)
				{
					this.Width = GetMaxWidthItem();
				}

				if (mIsHeightGroupOfItem && mGroupType == TLayoutGroupType.Horizontal)
				{
					this.Height = GetMaxHeightItem();
				}

				switch (mGroupType)
				{
					case TLayoutGroupType.Horizontal:
						{
							this.SetPlacementHorizontal();
							this.SetVerticalAlign();
						}
						break;
					case TLayoutGroupType.Vertical:
						{
							this.SetPlacementVertical();
							this.SetHorizontalAlign();
						}
						break;
					case TLayoutGroupType.Grid:
						{
							SetPlacementGrid();
						}
						break;
					case TLayoutGroupType.Flow:
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