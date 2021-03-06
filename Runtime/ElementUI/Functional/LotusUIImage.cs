//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIImage.cs
*		Визуальный компонент расширяющий возможности базового компонента изображения модуля компонентов Unity UI.
*		Реализация компонента определяющего дополнительные возможности базового компонента изображения и выступающего
*	в качественного основного компонента для вывода изображения.
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
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
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
		/// Визуальный компонент расширяющий возможности базового компонента изображения модуля компонентов Unity UI
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего дополнительные возможности базового компонента изображения
		/// и выступающего в качественного основного компонента для вывода изображения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathFunc + "Image")]
		public class LotusUIImage : Image, ILotusPlaceable2D, ILotusScreenGameVisual, ILotusPresentationBackcolor
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента "Image"
			/// </summary>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="rect_parent">Родительский компонент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusUIImage CreateImage(Single width, Single height, RectTransform rect_parent = null)
			{
				// 1) Создание объекта
				LotusUIImage ui_image = LotusElementUIDispatcher.CreateElement<LotusUIImage>("Image");

				// 2) Данные
				ui_image.Width = width;
				ui_image.Height = height;
				ui_image.SetSprite(XSprite.Default);

				// 3) Определение в иерархии
				if (rect_parent != null)
				{
					ui_image.transform.SetParent(rect_parent, false);
				}

				return ui_image;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Визуальная активность
			[SerializeField]
			internal Boolean mIsEnabled = true;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedSize;
			[SerializeField]
			internal Boolean mExpandedParam;
			[SerializeField]
			internal Boolean mExpandedOther;
			[SerializeField]
			internal Boolean mUseShadow;
			[SerializeField]
			internal Boolean mUseOutline;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Статус доступности компонента
			/// </summary>
			public Boolean IsEnabled
			{
				get { return mIsEnabled; }
				set
				{
					if (mIsEnabled != value)
					{
						mIsEnabled = value;
						this.UpdateVisualState();
#if UNITY_EDITOR
						LotusElementUIDispatcher.ForceUpdateEditor();
#endif
					}
				}
			}

				/// <summary>
			/// Имя спрайта
			/// </summary>
			public String SpriteName
			{
				get
				{
					if (sprite != null)
					{
						return sprite.name;
					}
					else
					{
						return null;
					}
				}
				set
				{
					this.SetSprite(XSprite.Find(value));
				}
			}

			/// <summary>
			/// Фоновый цвет элемента
			/// </summary>
			public Color BackgroundColor
			{
				get { return color; }
				set
				{
					color = value;
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
				get { return rectTransform.GetWorldScreenRect(); }
			}

			/// <summary>
			/// Глубина элемента интерфейса (влияет на последовательность прорисовки)
			/// </summary>
			public Int32 Depth
			{
				get { return rectTransform.GetSiblingIndex(); }
				set { rectTransform.SetSiblingIndex(value); }
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
					return rectTransform;
				}
			}

			/// <summary>
			/// Позиции левого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Left
			{
				get { return rectTransform.GetLeft(); }
				set { rectTransform.SetLeft(value); }
			}

			/// <summary>
			/// Позиции правого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Right
			{
				get { return rectTransform.GetRight(); }
				set { rectTransform.SetRight(value); }
			}

			/// <summary>
			/// Позиции верхнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Top
			{
				get { return rectTransform.GetTop(); }
				set { rectTransform.SetTop(value); }
			}

			/// <summary>
			/// Позиции нижнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Bottom
			{
				get { return rectTransform.GetBottom(); }
				set { rectTransform.SetBottom(value); }
			}

			/// <summary>
			/// Позиция верхнего-левого угла элемента
			/// </summary>
			public Vector2 Location
			{
				get { return new Vector2(Left, Top); }
				set
				{
					rectTransform.SetLeft(value.x);
					rectTransform.SetTop(value.y);
				}
			}

			/// <summary>
			/// Ширина(размер по X) элемента
			/// </summary>
			public Single Width
			{
				get { return rectTransform.rect.width; }
				set { rectTransform.SetWidth(value); }
			}

			/// <summary>
			/// Высота (размер по Y) элемента
			/// </summary>
			public Single Height
			{
				get { return rectTransform.rect.height; }
				set { rectTransform.SetHeight(value); }
			}

			/// <summary>
			/// Размер элемента
			/// </summary>
			public Vector2 Size
			{
				get { return new Vector2(rectTransform.rect.width, rectTransform.rect.height); }
				set
				{
					rectTransform.SetWidth(value.x);
					rectTransform.SetHeight(value.y);
				}
			}

			/// <summary>
			/// Ширина(размер по X) родительского элемента
			/// </summary>
			public Single ParentWidth
			{
				get
				{
					RectTransform rect_parent = rectTransform.parent as RectTransform;
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
					RectTransform rect_parent = rectTransform.parent as RectTransform;
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
				get { return rectTransform.GetHorizontalAlignment(); }
				set { rectTransform.SetHorizontalAlignment(value); }
			}

			/// <summary>
			/// Вертикальное выравнивание элемента
			/// </summary>
			public TVerticalAlignment VerticalAlignment
			{
				get { return rectTransform.GetVerticalAlignment(); }
				set { rectTransform.SetVerticalAlignment(value); }
			}

			/// <summary>
			/// Опорная точка элемента для привязки, масштабирования и вращения
			/// </summary>
			public Vector2 Pivot
			{
				get { return rectTransform.pivot; }
				set
				{
					Single left = rectTransform.GetLeft();
					Single top = rectTransform.GetTop();
					rectTransform.pivot = value;
					rectTransform.SetPosition(left, top);
				}
			}

			/// <summary>
			/// Опорная координата по X элемента для привязки, масштабирования и вращения
			/// </summary>
			public Single PivotX
			{
				get { return rectTransform.pivot.x; }
				set
				{
					Single left = rectTransform.GetLeft();
					rectTransform.pivot = new Vector2(value, rectTransform.pivot.y);
					rectTransform.SetLeft(left);
				}
			}

			/// <summary>
			/// Опорная координата по Y элемента для привязки, масштабирования и вращения
			/// </summary>
			public Single PivotY
			{
				get { return rectTransform.pivot.y; }
				set
				{
					Single top = rectTransform.GetTop();
					rectTransform.pivot = new Vector2(rectTransform.pivot.x, value);
					rectTransform.SetTop(top);
				}
			}

			/// <summary>
			/// Условная привязка элемента
			/// </summary>
			public Vector2 Anchored
			{
				get { return rectTransform.anchoredPosition; }
				set { rectTransform.anchoredPosition = value; }
			}

			/// <summary>
			/// Размер условной привязке элемента по X
			/// </summary>
			public Single AnchoredX
			{
				get { return rectTransform.anchoredPosition.x; }
				set { rectTransform.anchoredPosition = new Vector2(value, rectTransform.anchoredPosition.y); }
			}

			/// <summary>
			/// Размер условной привязке элемента по Y
			/// </summary>
			public Single AnchoredY
			{
				get { return rectTransform.anchoredPosition.y; }
				set { rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, value); }
			}

			/// <summary>
			/// Масштаб элемента
			/// </summary>
			public Vector2 Scale
			{
				get { return rectTransform.localScale; }
				set { rectTransform.localScale = new Vector3(value.x, value.y, rectTransform.localScale.z); }
			}

			/// <summary>
			/// Масштаб элемента по X
			/// </summary>
			public Single ScaleX
			{
				get { return rectTransform.localScale.x; }
				set { rectTransform.localScale = new Vector3(value, rectTransform.localScale.y, rectTransform.localScale.z); }
			}

			/// <summary>
			/// Масштаб элемента по Y 
			/// </summary>
			public Single ScaleY
			{
				get { return rectTransform.localScale.y; }
				set { rectTransform.localScale = new Vector3(rectTransform.localScale.x, value, rectTransform.localScale.z); }
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
			/// Включение компонента
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
				rectTransform.SetWidth(width);
				rectTransform.SetHeight(height);
				rectTransform.SetLeft(left);
				rectTransform.SetTop(top);
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
				rectTransform.SetHorizontalAlignment(h_align);
				rectTransform.SetVerticalAlignment(v_align);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вверх по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToFrontSibling()
			{
				rectTransform.SetSiblingIndex(rectTransform.GetSiblingIndex() + 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вниз по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToBackSibling()
			{
				rectTransform.SetSiblingIndex(rectTransform.GetSiblingIndex() - 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции элемента в иерархии родительской области
			/// </summary>
			/// <returns>Позиция в элемента родительской области</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetSiblingIndex()
			{
				return rectTransform.GetSiblingIndex();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента в определенную позицию в иерархии родительской области
			/// </summary>
			/// <param name="index">Позиция в иерархии родительской области</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetSiblingIndex(Int32 index)
			{
				rectTransform.SetSiblingIndex(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента первым в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsFirstSibling()
			{
				rectTransform.SetAsFirstSibling();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента последним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsLastSibling()
			{
				rectTransform.SetAsLastSibling();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента предпоследним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsPreLastSibling()
			{
				Transform parent = rectTransform.parent;
				Int32 child_count = parent.childCount;
				Int32 index = rectTransform.GetSiblingIndex();

				if (child_count > 1)
				{
					index = child_count - 2;
					rectTransform.SetSiblingIndex(index);
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

					rectTransform.SetParent(rect_parent, false);
					rectTransform.SetPosition(left, top);
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
					rect_child.SetParent(rectTransform);
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
					placeable.SetParent(rectTransform);
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

			#region ======================================= МЕТОДЫ ILotusScreenGameVisual =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка игрового экрана в указанную позицию
			/// </summary>
			/// <param name="position">Позиция в локальных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetScreenGamePosition(Vector2 position)
			{
				rectTransform.SetPosition(position.x, position.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка видимости игрового экрана 
			/// </summary>
			/// <param name="visible">Видимость игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetScreenGameVisible(Boolean visible)
			{
				enabled = visible;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка прозрачности игрового экрана
			/// </summary>
			/// <param name="opacity">Прозрачность игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetScreenGameOpacity(Single opacity)
			{
				color = new Color(color.r, color.g, color.b, opacity);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка смещения глубины показа(порядка рисования) игрового экрана
			/// </summary>
			/// <param name="depth_offset">Смещение по глубине</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetScreenGameDepthOffset(Int32 depth_offset)
			{
				Depth -= depth_offset;
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
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление визуального состояния компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdateVisualState()
			{
				if (mIsEnabled)
				{
					material = null;
				}
				else
				{
					material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КОНТЕНТОМ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка предпочтительного размера элемента по его текущим размерам
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetPreferredSizeFromSize()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка оптимального размера элемента по его текущим размерам
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetOptimalSizeFromSize()
			{

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
				rectTransform.SetWidth(minWidth);
				rectTransform.SetHeight(minHeight);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка оптимального размера элемента под содержимое
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetOptimalSize()
			{
				rectTransform.SetWidth(flexibleWidth);
				rectTransform.SetHeight(flexibleHeight);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка предпочтительного размера элемента под содержимое
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetPreferredSize()
			{
				rectTransform.SetWidth(preferredWidth);
				rectTransform.SetHeight(preferredHeight);
			}
			#endregion

			#region ======================================= ВИЗУАЛЬНЫЕ ЭФФЕКТЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавное скрытие компонента
			/// </summary>
			/// <param name="duration">Время скрытия</param>
			//---------------------------------------------------------------------------------------------------------
			public void Hide(Single duration = 0.3f)
			{
				CrossFadeAlpha(0, duration, true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный показ компонента
			/// </summary>
			/// <param name="duration">Время показа</param>
			//---------------------------------------------------------------------------------------------------------
			public void Show(Single duration = 0.3f)
			{
				canvasRenderer.SetAlpha(0);
				CrossFadeAlpha(1, duration, true);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================