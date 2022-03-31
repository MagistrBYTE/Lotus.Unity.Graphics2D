//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Макетное расположение и группировка
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUILayoutElement.cs
*		Компонент макета расположения элемента.
*		Реализация компонента определяющего макетное расположение элемента - выравнивания элемента в родительской области
*	и управления размерами элемента.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DUILayout Макетное расположение и группировка
		//! Макетное расположение определяет выравнивания элемента в родительской области и управления размерами элемента.
		//! \ingroup Unity2DUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент макета расположения элемента
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего макетное расположение элемента - выравнивания элемента в родительской
		/// области и управления размерами элемента
		/// </remarks>
		//------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathFunc + "Layout/LayoutElement")]
		public class LotusUILayoutElement : LayoutElement, ILotusLayoutElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			private RectTransform mUIRectElement;
			[SerializeField]
			internal TLayoutAlignment mAlignment;
			[SerializeField]
			internal Boolean mAutoAnchor;
			[SerializeField]
			internal TLayoutSizeMode mWidthMode;
			[SerializeField]
			internal TLayoutSizeMode mHeightMode;
			[SerializeField]
			internal Vector4 mPadding;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedParam;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Использовать размещение элемента по макету
			/// </summary>
			/// <remarks>
			/// Влияет на выполнимость метода UpdateLayout
			/// </remarks>
			public Boolean UseLayout
			{
				get { return this.enabled; }
				set { this.enabled = value; }
			}

			/// <summary>
			/// Выравнивание элемента в родительской области
			/// </summary>
			public TLayoutAlignment LayoutAlignment
			{
				get { return mAlignment; }
				set
				{
					if (mAlignment != value)
					{
						mAlignment = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Автоматическая привязка прямоугольника трансформации по выравниванию элемента
			/// </summary>
			public Boolean AutoAnchor
			{
				get { return mAutoAnchor; }
				set
				{
					if (mAutoAnchor != value)
					{
						mAutoAnchor = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Режим изменения размера по ширине
			/// </summary>
			public TLayoutSizeMode LayoutWidthMode
			{
				get { return mWidthMode; }
				set
				{
					if (mWidthMode != value)
					{
						mWidthMode = value;
						UpdateLayout();
					}
				}
			}

			/// <summary>
			/// Режим изменения размера по высоте
			/// </summary>
			public TLayoutSizeMode LayoutHeightMode
			{
				get { return mHeightMode; }
				set
				{
					if (mHeightMode != value)
					{
						mHeightMode = value;
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
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusBasePlaceable2D ============================
			/// <summary>
			/// Позиция левого угла элемента по X в экранных координатах
			/// </summary>
			public Single LeftScreen
			{
				get { return RectScreen.x; }
				set { }
			}

			/// <summary>
			/// Позиция правого угла элемента по X в экранных координатах
			/// </summary>
			public Single RightScreen
			{
				get { return RectScreen.xMax; }
				set { }
			}

			/// <summary>
			/// Позиция верхнего угла элемента по Y в экранных координатах
			/// </summary>
			public Single TopScreen
			{
				get { return RectScreen.y; }
				set { }
			}

			/// <summary>
			/// Позиция нижнего угла элемента по Y в экранных координатах
			/// </summary>
			public Single BottomScreen
			{
				get { return RectScreen.yMax; }
				set { }
			}

			/// <summary>
			/// Позиция верхнего-левого угла элемента в экранных координатах
			/// </summary>
			public Vector2 LocationScreen
			{
				get { return RectScreen.position; }
				set { }
			}

			/// <summary>
			/// Ширина(размер по X) элемента
			/// </summary>
			public Single WidthScreen
			{
				get { return RectScreen.width; }
				set { }
			}

			/// <summary>
			/// Высота(размер по Y) элемента
			/// </summary>
			public Single HeightScreen
			{
				get { return RectScreen.height; }
				set { }
			}

			/// <summary>
			/// Размеры элемента в экранных координатах
			/// </summary>
			public Vector2 SizeScreen
			{
				get { return RectScreen.size; }
				set { }
			}

			/// <summary>
			/// Прямоугольника элемента в экранных координатах
			/// </summary>
			public Rect RectScreen
			{
				get { return mUIRectElement.GetWorldScreenRect(); }
			}

			/// <summary>
			/// Глубина элемента интерфейса (влияет на последовательность прорисовки)
			/// </summary>
			public Int32 Depth
			{
				get { return mUIRectElement.GetSiblingIndex(); }
				set { mUIRectElement.SetSiblingIndex(value); }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusPlaceable2D ================================
			/// <summary>
			/// Прямоугольник трансформации элемента
			/// </summary>
			public RectTransform UIRect
			{
				get
				{
					return mUIRectElement;
				}
			}

			/// <summary>
			/// Позиции левого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Left
			{
				get { return mUIRectElement.GetLeft(); }
				set { mUIRectElement.SetLeft(value); }
			}

			/// <summary>
			/// Позиции правого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Right
			{
				get { return mUIRectElement.GetRight(); }
				set { mUIRectElement.SetRight(value); }
			}

			/// <summary>
			/// Позиции верхнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Top
			{
				get { return mUIRectElement.GetTop(); }
				set { mUIRectElement.SetTop(value); }
			}

			/// <summary>
			/// Позиции нижнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Bottom
			{
				get { return mUIRectElement.GetBottom(); }
				set { mUIRectElement.SetBottom(value); }
			}

			/// <summary>
			/// Позиция верхнего-левого угла элемента
			/// </summary>
			public Vector2 Location
			{
				get { return new Vector2(Left, Top); }
				set
				{
					mUIRectElement.SetLeft(value.x);
					mUIRectElement.SetTop(value.y);
				}
			}

			/// <summary>
			/// Ширина(размер по X) элемента
			/// </summary>
			public Single Width
			{
				get { return mUIRectElement.rect.width; }
				set { mUIRectElement.SetWidth(value); }
			}

			/// <summary>
			/// Высота (размер по Y) элемента
			/// </summary>
			public Single Height
			{
				get { return mUIRectElement.rect.height; }
				set { mUIRectElement.SetHeight(value); }
			}

			/// <summary>
			/// Размер элемента
			/// </summary>
			public Vector2 Size
			{
				get { return new Vector2(mUIRectElement.rect.width, mUIRectElement.rect.height); }
				set
				{
					mUIRectElement.SetWidth(value.x);
					mUIRectElement.SetHeight(value.y);
				}
			}

			/// <summary>
			/// Ширина(размер по X) родительского элемента
			/// </summary>
			public Single ParentWidth
			{
				get
				{
					RectTransform rect_parent = mUIRectElement.parent as RectTransform;
					if (rect_parent == null)
					{
						return LotusElementUIDispatcher.ScreenWidth;
					}

					return rect_parent.rect.width;
				}
			}

			/// <summary>
			/// Высота (размер по Y) родительского элемента
			/// </summary>
			public Single ParentHeight
			{
				get
				{
					RectTransform rect_parent = mUIRectElement.parent as RectTransform;
					if (rect_parent == null)
					{
						return LotusElementUIDispatcher.ScreenHeight;
					}

					return rect_parent.rect.height;
				}
			}

			/// <summary>
			/// Прямоугольника элемента от уровня родительской области
			/// </summary>
			public Rect RectLocalDesign
			{
				get { return new Rect(Left, Top, Width, Height); }
			}

			/// <summary>
			/// Горизонтальное выравнивание элемента
			/// </summary>
			public THorizontalAlignment HorizontalAlignment
			{
				get { return mUIRectElement.GetHorizontalAlignment(); }
				set { mUIRectElement.SetHorizontalAlignment(value); }
			}

			/// <summary>
			/// Вертикальное выравнивание элемента
			/// </summary>
			public TVerticalAlignment VerticalAlignment
			{
				get { return mUIRectElement.GetVerticalAlignment(); }
				set { mUIRectElement.SetVerticalAlignment(value); }
			}

			/// <summary>
			/// Опорная точка элемента для привязки, масштабирования и вращения
			/// </summary>
			public Vector2 Pivot
			{
				get { return mUIRectElement.pivot; }
				set
				{
					Single left = mUIRectElement.GetLeft();
					Single top = mUIRectElement.GetTop();
					mUIRectElement.pivot = value;
					mUIRectElement.SetPosition(left, top);
				}
			}

			/// <summary>
			/// Опорная координата по X элемента для привязки, масштабирования и вращения
			/// </summary>
			public Single PivotX
			{
				get { return mUIRectElement.pivot.x; }
				set
				{
					Single left = mUIRectElement.GetLeft();
					mUIRectElement.pivot = new Vector2(value, mUIRectElement.pivot.y);
					mUIRectElement.SetLeft(left);
				}
			}

			/// <summary>
			/// Опорная координата по Y элемента для привязки, масштабирования и вращения
			/// </summary>
			public Single PivotY
			{
				get { return mUIRectElement.pivot.y; }
				set
				{
					Single top = mUIRectElement.GetTop();
					mUIRectElement.pivot = new Vector2(mUIRectElement.pivot.x, value);
					mUIRectElement.SetTop(top);
				}
			}

			/// <summary>
			/// Условная привязка элемента
			/// </summary>
			public Vector2 Anchored
			{
				get { return mUIRectElement.anchoredPosition; }
				set { mUIRectElement.anchoredPosition = value; }
			}

			/// <summary>
			/// Размер условной привязке элемента по X
			/// </summary>
			public Single AnchoredX
			{
				get { return mUIRectElement.anchoredPosition.x; }
				set { mUIRectElement.anchoredPosition = new Vector2(value, mUIRectElement.anchoredPosition.y); }
			}

			/// <summary>
			/// Размер условной привязке элемента по Y
			/// </summary>
			public Single AnchoredY
			{
				get { return mUIRectElement.anchoredPosition.y; }
				set { mUIRectElement.anchoredPosition = new Vector2(mUIRectElement.anchoredPosition.x, value); }
			}

			/// <summary>
			/// Масштаб элемента
			/// </summary>
			public Vector2 Scale
			{
				get { return mUIRectElement.localScale; }
				set { mUIRectElement.localScale = new Vector3(value.x, value.y, mUIRectElement.localScale.z); }
			}

			/// <summary>
			/// Масштаб элемента по X
			/// </summary>
			public Single ScaleX
			{
				get { return mUIRectElement.localScale.x; }
				set { mUIRectElement.localScale = new Vector3(value, mUIRectElement.localScale.y, mUIRectElement.localScale.z); }
			}

			/// <summary>
			/// Масштаб элемента по Y 
			/// </summary>
			public Single ScaleY
			{
				get { return mUIRectElement.localScale.y; }
				set { mUIRectElement.localScale = new Vector3(mUIRectElement.localScale.x, value, mUIRectElement.localScale.z); }
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
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
#if UNITY_EDITOR
				this.UpdateLayout();
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение размеров прямоугольника трансформации родительского элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnTransformParentChanged()
			{
				base.OnTransformParentChanged();
				this.UpdateLayout();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение размеров прямоугольника трансформации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnRectTransformDimensionsChange()
			{
				base.OnRectTransformDimensionsChange();
				this.UpdateLayout();
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на вхождение точки в область элемента
			/// </summary>
			/// <param name="point">Точка в экранных координатах</param>
			/// <returns>статус вхождения</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean ContainsScreen(Vector2 point)
			{
				Rect rect = RectScreen;
				return rect.Contains(point);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров элемента
			/// </summary>
			/// <param name="left">Позиция по X левого угла элемента в экранных координатах</param>
			/// <param name="top">Позиция по Y верхнего угла элемента в экранных координатах</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetFromScreen(Single left, Single top, Single width, Single height)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Основной метод определяющий положение и размер элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdatePlacement()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusPlaceable2D ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров элемента
			/// </summary>
			/// <param name="left">Позиция по X левого угла элемента от уровня родительской области</param>
			/// <param name="top">Позиция по Y верхнего угла элемента от уровня родительской области</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetFromLocalDesign(Single left, Single top, Single width, Single height)
			{
				mUIRectElement.SetWidth(width);
				mUIRectElement.SetHeight(height);
				mUIRectElement.SetLeft(left);
				mUIRectElement.SetTop(top);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка выравнивания элемента
			/// </summary>
			/// <param name="h_align">Горизонтальное выравнивание элемента</param>
			/// <param name="v_align">Вертикальное выравнивание элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetAlignment(THorizontalAlignment h_align, TVerticalAlignment v_align)
			{
				mUIRectElement.SetHorizontalAlignment(h_align);
				mUIRectElement.SetVerticalAlignment(v_align);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вверх по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToFrontSibling()
			{
				mUIRectElement.SetSiblingIndex(mUIRectElement.GetSiblingIndex() + 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вниз по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToBackSibling()
			{
				mUIRectElement.SetSiblingIndex(mUIRectElement.GetSiblingIndex() - 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции элемента в иерархии родительской области
			/// </summary>
			/// <returns>Позиция в элемента родительской области</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetSiblingIndex()
			{
				return mUIRectElement.GetSiblingIndex();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента в определенную позицию в иерархии родительской области
			/// </summary>
			/// <param name="index">Позиция в иерархии родительской области</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetSiblingIndex(Int32 index)
			{
				mUIRectElement.SetSiblingIndex(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента первым в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsFirstSibling()
			{
				mUIRectElement.SetAsFirstSibling();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента последним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsLastSibling()
			{
				mUIRectElement.SetAsLastSibling();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента предпоследним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsPreLastSibling()
			{
				Transform parent = mUIRectElement.parent;
				Int32 child_count = parent.childCount;
				Int32 index = mUIRectElement.GetSiblingIndex();

				if (child_count > 1)
				{
					index = child_count - 2;
					mUIRectElement.SetSiblingIndex(index);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusPlaceable2D ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка прямоугольника трансформации в качестве родительского
			/// </summary>
			/// <param name="rect_parent">Прямоугольник трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetParent(RectTransform rect_parent)
			{
				if (rect_parent != null)
				{
					// Сохраняем предварительно положение
					Single top = this.Top;
					Single left = this.Left;

					mUIRectElement.SetParent(rect_parent, false);
					mUIRectElement.SetPosition(left, top);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка прямоугольника трансформации в качестве дочернего
			/// </summary>
			/// <param name="rect_child">Прямоугольник трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetChild(RectTransform rect_child)
			{
				if (rect_child != null)
				{
					rect_child.SetParent(mUIRectElement);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размещаемого элемента в качестве дочернего
			/// </summary>
			/// <param name="placeable">Размещаемый элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetChild(LotusUIPlaceable2D placeable)
			{
				if (placeable != null)
				{
					placeable.SetParent(mUIRectElement);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка относительной позиции в родительской области
			/// </summary>
			/// <param name="percent_left">Процент смещения слева</param>
			/// <param name="percent_top">Процент смещения сверху</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetRelativePosition(Single percent_left, Single percent_top)
			{
				Single left = (this.ParentWidth - this.Width) * percent_left;
				Single top = (this.ParentHeight - this.Height) * percent_top;
				this.Left = left;
				this.Top = top;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение относительной позиции в родительской области
			/// </summary>
			/// <param name="percent_left">Процент смещения слева</param>
			/// <param name="percent_top">Процент смещения сверху</param>
			/// <returns>Относительная позиции в родительской области</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2 GetRelativePosition(Single percent_left, Single percent_top)
			{
				Single left = (this.ParentWidth - this.Width) * percent_left;
				Single top = (this.ParentHeight - this.Height) * percent_top;
				return new Vector2(left, top);
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusLayoutElement ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление местоположения и размеров элемента по данным макета
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateLayout()
			{
				UpdateLayout(mPadding, TAspectMode.None);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление местоположения и размера элемента по данным макета
			/// </summary>
			/// <param name="margin">Дополнительные внутренние отступы от уровня родительской области</param>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateLayout(Vector4 margin)
			{
				UpdateLayout(margin, TAspectMode.None);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление местоположения и размера элемента по данным макета
			/// </summary>
			/// <param name="margin">Дополнительные внутренние отступы от уровня родительской области</param>
			/// <param name="aspect_mode">Режим изменения размеров и соотношения сторон</param>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateLayout(Vector4 margin, TAspectMode aspect_mode)
			{
				if (ignoreLayout) return;

				RectTransform rect_parent = mUIRectElement.parent as RectTransform;

				if (rect_parent == null)
				{
					return;
				}

				Single width = mUIRectElement.rect.width;
				Single height = mUIRectElement.rect.height;
				Single left = margin.x;
				Single top = margin.y;
				Single right = margin.z;
				Single bottom = margin.w;
				Single base_width = rect_parent.rect.width - (left + right);
				Single base_height = rect_parent.rect.height - (top + bottom);
				Single min_width = 16;
				Single min_height = 16;

				switch (mWidthMode)
				{
					case TLayoutSizeMode.Fixed:
						break;
					case TLayoutSizeMode.Optimal:
						{
							width = preferredWidth;
							if (width > base_width)
							{
								if (base_width < min_width) base_width = min_width + 1;
								width = Mathf.Clamp(width, min_width, base_width);
							}
							mUIRectElement.SetWidth(width);
						}
						break;
					case TLayoutSizeMode.Stretch:
						{
							width = base_width;
							mUIRectElement.SetWidth(width);
						}
						break;
					default:
						break;
				}

				switch (mHeightMode)
				{
					case TLayoutSizeMode.Fixed:
						break;
					case TLayoutSizeMode.Optimal:
						{
							height = preferredHeight;
							if (height > base_height)
							{
								if (base_height < min_height) base_height = min_height + 1;
								height = Mathf.Clamp(height, min_height, base_height);
							}
							mUIRectElement.SetHeight(height);
						}
						break;
					case TLayoutSizeMode.Stretch:
						{
							height = base_height;
							mUIRectElement.SetHeight(height);
						}
						break;
					default:
						break;
				}


				switch (aspect_mode)
				{
					case TAspectMode.None:
						break;
					case TAspectMode.WidthControlsHeight:
						{
							if (mHeightMode == TLayoutSizeMode.Optimal)
							{

							}
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							if (mWidthMode == TLayoutSizeMode.Optimal)
							{

							}
						}
						break;
					default:
						break;
				}

				switch (mAlignment)
				{
					case TLayoutAlignment.LeftTop:
						{
							mUIRectElement.SetLeft(left);
							mUIRectElement.SetTop(top);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Left, TVerticalAlignment.Top);
							}
						}
						break;
					case TLayoutAlignment.LeftMiddle:
						{
							mUIRectElement.SetLeft(left);
							mUIRectElement.SetTop((base_height - height) / 2 + top);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Left, TVerticalAlignment.Middle);
							}
						}
						break;
					case TLayoutAlignment.LeftBottom:
						{
							mUIRectElement.SetLeft(left);
							mUIRectElement.SetTop(rect_parent.rect.height - height - bottom);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Left, TVerticalAlignment.Bottom);
							}
						}
						break;
					case TLayoutAlignment.CenterTop:
						{
							mUIRectElement.SetLeft((base_width - width) / 2 + left);
							mUIRectElement.SetTop(top);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Center, TVerticalAlignment.Top);
							}
						}
						break;
					case TLayoutAlignment.CenterMiddle:
						{
							mUIRectElement.SetLeft((base_width - width) / 2 + left);
							mUIRectElement.SetTop((base_height - height) / 2 + top);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Center, TVerticalAlignment.Middle);
							}
						}
						break;
					case TLayoutAlignment.CenterBottom:
						{
							mUIRectElement.SetLeft((base_width - width) / 2 + left);
							mUIRectElement.SetTop(rect_parent.rect.height - height - bottom);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Center, TVerticalAlignment.Bottom);
							}
						}
						break;
					case TLayoutAlignment.RightTop:
						{
							mUIRectElement.SetLeft(rect_parent.rect.width - width - right);
							mUIRectElement.SetTop(bottom);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Right, TVerticalAlignment.Top);
							}
						}
						break;
					case TLayoutAlignment.RightMiddle:
						{
							mUIRectElement.SetLeft(rect_parent.rect.width - width - right);
							mUIRectElement.SetTop((base_height - height) / 2 + top);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Right, TVerticalAlignment.Middle);
							}
						}
						break;
					case TLayoutAlignment.RightBottom:
						{
							mUIRectElement.SetLeft(rect_parent.rect.width - width - right);
							mUIRectElement.SetTop(rect_parent.rect.height - height - bottom);
							if (mAutoAnchor)
							{
								mUIRectElement.SetAlignment(THorizontalAlignment.Right, TVerticalAlignment.Bottom);
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка предпочтительного размера элемента по его текущим размерам
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetPreferredSizeFromSize()
			{
				preferredWidth = this.mUIRectElement.rect.width;
				preferredHeight = this.mUIRectElement.rect.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка оптимального размера элемента по его текущим размерам
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetOptimalSizeFromSize()
			{
				flexibleWidth = this.mUIRectElement.rect.width;
				flexibleHeight = this.mUIRectElement.rect.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение оптимальной ширины элемента
			/// </summary>
			/// <returns>Оптимальная ширина элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetOptimalWidth()
			{
				return this.flexibleWidth;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение оптимальной высоты элемента
			/// </summary>
			/// <returns>Оптимальная высоты элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetOptimalHeight()
			{
				return this.flexibleHeight;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка минимального размера элемента под содержимое
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetMinimalSize()
			{
				mUIRectElement.SetWidth(minWidth);
				mUIRectElement.SetHeight(minHeight);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка оптимального размера элемента под содержимое
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetOptimalSize()
			{
				mUIRectElement.SetWidth(flexibleWidth);
				mUIRectElement.SetHeight(flexibleHeight);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка предпочтительного размера элемента под содержимое
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetPreferredSize()
			{
				mUIRectElement.SetWidth(preferredWidth);
				mUIRectElement.SetHeight(preferredHeight);
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
			/// Установка минимальной ширины элемента по текущей ширине прямоугольника трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetMinWidthFromRect()
			{
				this.minWidth = mUIRectElement.rect.width;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка минимальной высоты элемента по текущей высоте прямоугольника трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetMinHeightFromRect()
			{
				this.minHeight = mUIRectElement.rect.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка ширины прямоугольника трансформации по минимальной ширины элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetMinWidthToRect()
			{
				mUIRectElement.SetWidth(this.minWidth);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты прямоугольника трансформации по минимальной высоте элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetMinHeightToRect()
			{
				mUIRectElement.SetHeight(this.minHeight);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка предпочитаемой ширины элемента по текущей ширине прямоугольника трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetPreferredWidthFromRect()
			{
				this.preferredWidth = mUIRectElement.rect.width;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка предпочитаемой высоты элемента по текущей высоте прямоугольника трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetPreferredHeightFromRect()
			{
				this.preferredHeight = mUIRectElement.rect.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка ширины прямоугольника трансформации по предпочитаемой ширины элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetPreferredWidthToRect()
			{
				mUIRectElement.SetWidth(this.preferredWidth);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты прямоугольника трансформации по предпочитаемой высоте элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetPreferredHeightToRect()
			{
				mUIRectElement.SetHeight(this.preferredHeight);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================