//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIScrollRect.cs
*		Расширенный компонент ScrollRect определяющий фиксированное перемещение контента в ограниченной области.
*		Реализация компонента определяющего на базе основного компонента окна просмотра дополнительную функциональность,
*	направленную на удобное размещение и фиксированное перемещение контента в ограниченной области.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Maths;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DUICommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Расширенный компонент ScrollRect определяющий размещение и перемещение контента в ограниченной области
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего на базе основного компонента окна просмотра дополнительную функциональность,
		/// направленную на удобное размещение и фиксированное перемещение контента в ограниченной области
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathFunc + "ScrollRect")]
		public class LotusUIScrollRect : ScrollRect, ILotusPlaceable2D
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента ScrollRect
			/// </summary>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="rect_parent">Родительский компонент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusUIScrollRect Create(Single width, Single height, RectTransform rect_parent = null)
			{
				// 1) Создание объекта
				LotusUIScrollRect scroll_rect = LotusElementUIDispatcher.CreateElement<LotusUIScrollRect>("ScrollRect");
				scroll_rect.gameObject.AddComponent<RectMask2D>();
				Image content_area = LotusElementUIDispatcher.CreateElement<Image>("ContentArea");
				content_area.rectTransform.SetParent(scroll_rect.UIRect, false);
				content_area.rectTransform.SetAlignment(THorizontalAlignment.Left, TVerticalAlignment.Top);
				content_area.rectTransform.Set(0, 0, width * 2, height);
				content_area.SetSprite(XSprite.Default);
				scroll_rect.content = content_area.rectTransform;

				// 2) Данные
				scroll_rect.Width = width;
				scroll_rect.Height = height;

				// 3) Определение в иерархии
				if (rect_parent != null)
				{
					scroll_rect.transform.SetParent(rect_parent, false);
				}

				return scroll_rect;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal RectTransform mUIRectElement;
			[SerializeField]
			internal Single mVelocityOffset = 0.1f;
			[SerializeField]
			internal TScrollDirection mScrollDirection;
			[SerializeField]
			internal Boolean mUseFixedOffsetX;
			[SerializeField]
			[HideInInspector]
			internal Boolean mUseFixedOffsetY;
			[SerializeField]
			internal Int32 mCountFixedOffsetX;
			[SerializeField]
			internal Int32 mCountFixedOffsetY;
			[SerializeField]
			internal Single mDeltaFixedOffsetX = 10.0f;
			[SerializeField]
			internal Single mDeltaFixedOffsetY = 10.0f;
			[NonSerialized]
			internal Int32 mDestNumberFixedOffsetX;
			[NonSerialized]
			internal Int32 mDestNumberFixedOffsetY;
			[NonSerialized]
			internal List<Single> mFixedPlacementX;
			[NonSerialized]
			internal List<Single> mFixedPlacementY;
			[NonSerialized]
			internal Boolean mIsMoveFixedOffsetX;
			[NonSerialized]
			internal Boolean mIsMoveFixedOffsetY;
			[NonSerialized]
			internal Single mStartFixedOffsetX;
			[NonSerialized]
			internal Single mStartFixedOffsetY;

			// События
			[NonSerialized]
			internal Action<Int32> mOnPageChanged;

			// Служебные данные
			[NonSerialized]
			internal Boolean mIsDragging;
			[NonSerialized]
			internal Boolean mIsCorotune;
			[NonSerialized]
			internal Vector2 mPosPressedPointer;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedSize;
			[SerializeField]
			internal Boolean mExpandedDefault;
			[SerializeField]
			internal Boolean mExpandedView;
			[NonSerialized]
			internal Action<Single, Single> mOnViewWindowSizeChanged;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус доступности элемента
			/// </summary>
			public Boolean IsEnabled
			{
				get { return this.enabled; }
				set
				{
					this.enabled = value;
				}
			}

			/// <summary>
			/// Прямоугольник трансформации области контента
			/// </summary>
			public RectTransform UIRectContent
			{
				get { return this.content; }
				set
				{
					content = value;
				}
			}

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
					mScrollDirection = value;
				}
			}

			/// <summary>
			/// Смещение области контента от верхнего/левого угла
			/// </summary>
			public Vector2 ContentOffset
			{
				get
				{
					return new Vector2(-content.GetLeft(), -content.GetTop());
				}
				set
				{
					content.SetPosition(-value.x, -value.y);
				}
			}

			/// <summary>
			/// Смещение по X области контента от верхнего/левого угла
			/// </summary>
			public Single ContentOffsetX
			{
				get
				{
					return -content.GetLeft();
				}
				set
				{
					content.SetLeft(-value);
				}
			}

			/// <summary>
			/// Максимальное возможно смещение по X области контента от верхнего/левого угла
			/// </summary>
			public Single MaxContentOffsetX
			{
				get
				{
					return Mathf.Floor(content.rect.width - Mathf.Abs(this.Width));
				}
			}

			/// <summary>
			/// Смещение по Y области контента от верхнего/левого угла
			/// </summary>
			public Single ContentOffsetY
			{
				get
				{
					return -content.GetTop();
				}
				set
				{
					content.SetTop(-value);
				}
			}

			/// <summary>
			/// Максимальное возможно смещение по Y области контента от верхнего/левого угла
			/// </summary>
			public Single MaxContentOffsetY
			{
				get
				{
					return Mathf.Floor(content.rect.height - Mathf.Abs(this.Height));
				}
			}

			/// <summary>
			/// Размеры области контента
			/// </summary>
			public Vector2 ContentSize
			{
				get
				{
					return new Vector2(content.rect.width, content.rect.height);
				}
				set
				{
					content.SetSize(value.x, value.y);
				}
			}

			/// <summary>
			/// Ширина области контента
			/// </summary>
			public Single ContentWidth
			{
				get
				{
					return content.rect.width;
				}
				set
				{
					content.SetWidth(value);
				}
			}

			/// <summary>
			/// Высота области контента
			/// </summary>
			public Single ContentHeight
			{
				get
				{
					return content.rect.height;
				}
				set
				{
					content.SetHeight(value);
				}
			}

			/// <summary>
			/// Индекс страницы по ширине
			/// </summary>
			public Int32 PageIndexWidth
			{
				get
				{
					return (Int32)(this.ContentOffsetX / this.Width);
				}
			}

			/// <summary>
			/// Индекс страницы по высоте
			/// </summary>
			public Int32 PageIndexHeight
			{
				get
				{
					return (Int32)(this.ContentOffsetY / this.Height);
				}
			}

			/// <summary>
			/// Возможность вертикальной прокрутки
			/// </summary>
			public Boolean IsVerticalScroll
			{
				get { return vertical; }
				set { vertical = value; }
			}

			/// <summary>
			/// Возможность горизонтальной прокрутки
			/// </summary>
			public Boolean IsHorizontalScroll
			{
				get { return horizontal; }
				set { horizontal = value; }
			}

			/// <summary>
			/// Нормализованная позиция горизонтального смещения
			/// </summary>
			public Single HorizontalNormalizedPosition
			{
				get { return horizontalNormalizedPosition; }
				set { horizontalNormalizedPosition = value; }
			}

			/// <summary>
			/// Нормализованная позиция вертикального смещения
			/// </summary>
			public Single VerticalNormalizedPosition
			{
				get { return verticalNormalizedPosition; }
				set { verticalNormalizedPosition = value; }
			}

			/// <summary>
			/// Использовать фиксированное смещение по оси X
			/// </summary>
			public Boolean UseFixedOffsetX
			{
				get { return mUseFixedOffsetX; }
				set 
				{
					mUseFixedOffsetX = value;
					if (mUseFixedOffsetX)
					{
						this.inertia = false;
					}
				}
			}

			/// <summary>
			/// Использовать фиксированное смещение по оси Y
			/// </summary>
			public Boolean UseFixedOffsetY
			{
				get { return mUseFixedOffsetY; }
				set
				{
					mUseFixedOffsetY = value;
					if (mUseFixedOffsetY)
					{
						this.inertia = false;
					}
				}
			}

			/// <summary>
			/// Скорость смещения
			/// </summary>
			public Single VelocityOffset
			{
				get { return mVelocityOffset; }
				set { mVelocityOffset = value; }
			}

			/// <summary>
			/// Количество фиксированных позиций по X
			/// </summary>
			public Int32 CountFixedOffsetX
			{
				get { return mCountFixedOffsetX; }
				set { mCountFixedOffsetX = value; }
			}

			/// <summary>
			/// Количество фиксированных позиций по Y
			/// </summary>
			public Int32 CountFixedOffsetY
			{
				get { return mCountFixedOffsetY; }
				set { mCountFixedOffsetY = value; }
			}

			/// <summary>
			/// Минимальное смещение по X для активации перемещения в следующую позицию (для стабилизации)
			/// </summary>
			public Single DeltaFixedOffsetX
			{
				get { return mDeltaFixedOffsetX; }
				set { mDeltaFixedOffsetX = value; }
			}

			/// <summary>
			/// Минимальное смещение по Y для активации перемещения в следующую позицию (для стабилизации)
			/// </summary>
			public Single DeltaFixedOffsetY
			{
				get { return mDeltaFixedOffsetY; }
				set { mDeltaFixedOffsetY = value; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о перемещении одной страницы при фиксированном смещении. Аргумент - активная страница
			/// </summary>
			public Action<Int32> OnPageChanged
			{
				get { return mOnPageChanged; }
				set { mOnPageChanged = value; }
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
				InitFixedOffset();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnEnable()
			{
				base.OnEnable();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отключение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnDisable()
			{
				base.OnDisable();
			}
			#endregion

			#region ======================================= МЕТОДЫ IDragHandler =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало перемещения
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public override void OnBeginDrag(PointerEventData event_data)
			{
				this.StopMovement();

				base.OnBeginDrag(event_data);

				if (mUseFixedOffsetX)
				{
					mStartFixedOffsetX = this.ContentOffsetX;
				}

				if (mUseFixedOffsetY)
				{
					mStartFixedOffsetY = this.ContentOffsetY;
				}

				mIsDragging = true;
				mPosPressedPointer = event_data.position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс перемещения
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDrag(PointerEventData event_data)
			{
				base.OnDrag(event_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончания перемещения
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public override void OnEndDrag(PointerEventData event_data)
			{
				base.OnEndDrag(event_data);

				mIsDragging = false;
				if (mUseFixedOffsetX)
				{
					mDestNumberFixedOffsetX = this.FindDestFixedOffsetHorizontal();

					Single target_x = 0;
					Single current_x = this.ContentOffsetX;

					if (Mathf.Abs(mStartFixedOffsetX - this.ContentOffsetX) > mDeltaFixedOffsetX)
					{
						target_x = mFixedPlacementX[mDestNumberFixedOffsetX];
					}
					else
					{
						target_x = mStartFixedOffsetX;
					}

					if (current_x < target_x)
					{
						IEnumerator routine = RightStepIteration(target_x);
						StartCoroutine(routine);
					}
					else
					{
						IEnumerator routine = LeftStepIteration(target_x);
						StartCoroutine(routine);
					}
				}

				if (mUseFixedOffsetY)
				{
					mDestNumberFixedOffsetY = this.FindDestFixedOffsetVertical();

					Single target_y = 0;
					Single current_y = this.ContentOffsetY;

					if (Mathf.Abs(mStartFixedOffsetY - this.ContentOffsetY) > mDeltaFixedOffsetY)
					{
						target_y = mFixedPlacementY[mDestNumberFixedOffsetY];
					}
					else
					{
						target_y = mStartFixedOffsetY;
					}

					if (current_y < target_y)
					{
						IEnumerator routine = DownStepIteration(target_y);
						StartCoroutine(routine);
					}
					else
					{
						IEnumerator routine = UpStepIteration(target_y);
						StartCoroutine(routine);
					}
				}
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
					viewport = mUIRectElement;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка и корректировка при необходимости позиции контента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void CheckContentPosition()
			{
				if (ContentOffsetX > MaxContentOffsetX)
				{
					ContentOffsetX = 0;
				}

				if (ContentOffsetY > MaxContentOffsetY)
				{
					ContentOffsetY = 0;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация данных системы фиксированного смещения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void InitFixedOffset()
			{
				if (content != null)
				{
					Int32 count_step_x = mCountFixedOffsetX;
					Single delta_offset_x = this.ContentWidth / count_step_x;

					count_step_x -= Mathf.FloorToInt(this.Width / delta_offset_x) - 1;

					mFixedPlacementX = new List<Single>();
					for (Int32 ix = 0; ix < count_step_x; ix++)
					{
						Single data = ix * delta_offset_x;
						mFixedPlacementX.Add(data);
					}

					Int32 count_step_y = mCountFixedOffsetY;
					Single delta_offset_y = this.ContentHeight / count_step_y;

					count_step_y -= Mathf.FloorToInt(this.Height / delta_offset_y) - 1;

					mFixedPlacementY = new List<Single>();
					for (Int32 iy = 0; iy < count_step_y; iy++)
					{
						Single data = iy * delta_offset_y;
						mFixedPlacementY.Add(data);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных системы фиксированного смещения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ResetFixedOffset()
			{
				if (mFixedPlacementX != null) mFixedPlacementX.Clear();
				if (mFixedPlacementY != null) mFixedPlacementY.Clear();

				InitFixedOffset();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск целевой позиции смещения по горизонтали
			/// </summary>
			/// <returns>Целевая позиция смещения по горизонтали</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindDestFixedOffsetHorizontal()
			{
				Int32 result = 0;

				if (this.ContentOffsetX <= 0)
				{
					result = 0;
					return result;
				}
				if (ContentOffsetX >= mFixedPlacementX[mFixedPlacementX.Count - 1])
				{
					result = mFixedPlacementX.Count - 1;
					return result;
				}

				for (Int32 i = 1; i < mFixedPlacementX.Count; i++)
				{
					if (ContentOffsetX < mFixedPlacementX[i])
					{
						if (mStartFixedOffsetX > ContentOffsetX)
						{
							result = i - 1;
						}
						else
						{
							result = i;
						}
						break;
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск позиции смещения по горизонтали
			/// </summary>
			/// <returns>Позиция смещения по горизонтали</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindFixedOffsetHorizontal()
			{
				Int32 result = 0;

				if (this.ContentOffsetX <= 0)
				{
					result = 0;
					return result;
				}
				if (ContentOffsetX >= mFixedPlacementX[mFixedPlacementX.Count - 1])
				{
					result = mFixedPlacementX.Count - 1;
					return result;
				}

				for (Int32 i = 1; i < mFixedPlacementX.Count; i++)
				{
					if (XMath.Approximately(ContentOffsetX, mFixedPlacementX[i], 2))
					{
						result = i;
						break;
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск целевой позиции смещения по вертикали
			/// </summary>
			/// <returns>Целевая позиция смещения по вертикали</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindDestFixedOffsetVertical()
			{
				Int32 result = 0;

				if (this.ContentOffsetY <= 0)
				{
					result = 0;
					return result;
				}

				if (this.ContentOffsetY >= mFixedPlacementY[mFixedPlacementY.Count - 1])
				{
					result = mFixedPlacementY.Count - 1;
					return result;
				}

				for (Int32 i = 1; i < mFixedPlacementY.Count; i++)
				{
					if (ContentOffsetY < mFixedPlacementY[i])
					{
						if (mStartFixedOffsetY > ContentOffsetY)
						{
							result = i - 1;
						}
						else
						{
							result = i;
						}
						break;
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск позиции смещения по вертикали
			/// </summary>
			/// <returns>Целевая смещения по вертикали</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindFixedOffsetVertical()
			{
				Int32 result = 0;

				if (this.ContentOffsetY <= 0)
				{
					result = 0;
					return result;
				}

				if (this.ContentOffsetY >= mFixedPlacementY[mFixedPlacementY.Count - 1])
				{
					result = mFixedPlacementY.Count - 1;
					return result;
				}

				for (Int32 i = 1; i < mFixedPlacementY.Count; i++)
				{
					if (XMath.Approximately(ContentOffsetY, mFixedPlacementY[i], 2))
					{
						result = i;
						break;
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текстового формата смещений по горизонтали
			/// </summary>
			/// <returns>Текстовый формат смещений по горизонтали</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetFixedOffsetHorizontal()
			{
				String result = "";

				for (Int32 i = 0; i < mFixedPlacementX.Count; i++)
				{
					result += mFixedPlacementX[i].ToString("F1") + "; ";
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текстового формата смещений по вертикали
			/// </summary>
			/// <returns>Текстовый формат смещений по вертикали</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetFixedOffsetVertical()
			{
				String result = "";

				if (!IsActive())
				{
					return result;
				}

				for (Int32 i = 0; i < mFixedPlacementY.Count; i++)
				{
					result += mFixedPlacementY[i].ToString("F1") + "; ";
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение на один шаг вправо при фиксированном смещении
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToRightStep()
			{
				if (mIsDragging == false)
				{
					Int32 index = FindFixedOffsetHorizontal();
					if (index < this.mFixedPlacementX.Count - 1)
					{
						Single dest_content_x = this.mFixedPlacementX[index + 1];
						IEnumerator routine = RightStepIteration(dest_content_x);
						StartCoroutine(routine);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение на один шаг влево при фиксированном смещении
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToLeftStep()
			{
				if (mIsDragging == false)
				{
					Int32 index = FindFixedOffsetHorizontal();
					if (index > 0)
					{
						Single dest_content_x = this.mFixedPlacementX[index - 1];
						IEnumerator routine = LeftStepIteration(dest_content_x);
						StartCoroutine(routine);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение на один шаг вниз при фиксированном смещении
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToDownStep()
			{
				if (mIsDragging == false)
				{
					Int32 index = FindFixedOffsetVertical();
					if (index < this.mFixedPlacementY.Count - 1)
					{
						Single dest_content_y = this.mFixedPlacementY[index + 1];
						IEnumerator routine = DownStepIteration(dest_content_y);
						StartCoroutine(routine);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение на один шаг вверх при фиксированном смещении
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToUpStep()
			{
				if (mIsDragging == false)
				{
					Int32 index = FindFixedOffsetVertical();
					if (index > 0)
					{
						Single dest_content_y = this.mFixedPlacementY[index - 1];
						IEnumerator routine = UpStepIteration(dest_content_y);
						StartCoroutine(routine);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение размеров области просмотра
			/// </summary>
			/// <param name="width">Новая ширина области просмотра</param>
			/// <param name="height">Новая высота области просмотра</param>
			//---------------------------------------------------------------------------------------------------------
			internal void OnResizeWindowView(Single width, Single height)
			{
				this.Width = width;
				this.Height = height;
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОДПРОГРАММ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения на один шаг вправо при фиксированном смещении
			/// </summary>
			/// <param name="dest_content_x">Целевая позиция смещения контента</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator RightStepIteration(Single dest_content_x)
			{
				mIsDragging = true;
				Single comparison = dest_content_x - 1f;
				while (this.ContentOffsetX <= comparison)
				{
					Single delta = dest_content_x - this.ContentOffsetX;
					delta = delta * mVelocityOffset;
					if (delta < 0.01) delta = 0.1f;
					this.ContentOffsetX += delta;
					yield return null;
				}

				this.ContentOffsetX = dest_content_x;
				mIsDragging = false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения на один шаг влево при фиксированном смещении
			/// </summary>
			/// <param name="dest_content_x">Целевая позиция смещения контента</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator LeftStepIteration(Single dest_content_x)
			{
				mIsDragging = true;
				Single comparison = dest_content_x + 1f;
				while (this.ContentOffsetX >= comparison)
				{
					Single delta = this.ContentOffsetX - dest_content_x;
					delta = delta * mVelocityOffset;
					if (delta < 0.01) delta = 0.1f;
					this.ContentOffsetX -= delta;
					yield return null;
				}

				this.ContentOffsetX = dest_content_x;
				mIsDragging = false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения на один шаг вниз при фиксированном смещении
			/// </summary>
			/// <param name="dest_content_y">Целевая позиция смещения контента</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator DownStepIteration(Single dest_content_y)
			{
				mIsDragging = true;
				Single comparison = dest_content_y - 1f;
				while (this.ContentOffsetY <= comparison)
				{
					Single delta = dest_content_y - this.ContentOffsetY;
					delta = delta * mVelocityOffset;
					if (delta < 0.01) delta = 0.1f;
					this.ContentOffsetY += delta;
					yield return null;
				}

				this.ContentOffsetY = dest_content_y;
				mIsDragging = false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения на один шаг вверх при фиксированном смещении
			/// </summary>
			/// <param name="dest_content_y">Целевая позиция смещения контента</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator UpStepIteration(Single dest_content_y)
			{
				mIsDragging = true;
				Single comparison = dest_content_y + 1f;
				while (this.ContentOffsetY >= comparison)
				{
					Single delta = this.ContentOffsetY - dest_content_y;
					delta = delta * mVelocityOffset;
					if (delta < 0.01) delta = 0.1f;
					this.ContentOffsetY -= delta;
					yield return null;
				}

				this.ContentOffsetY = dest_content_y;
				mIsDragging = false;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================