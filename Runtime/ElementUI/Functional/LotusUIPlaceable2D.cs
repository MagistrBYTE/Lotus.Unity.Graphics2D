//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPlaceable2D.cs
*		Функциональный компонент определяющий размещения элемента подсистемы Unity UI в 2D пространстве.
*		Реализация функционального компонента определяющего размещения элемента подсистемы Unity UI в 2D пространстве.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DUICommon Общая подсистема
		//! Общая подсистема модуля компонентов Unity UI
		//! \ingroup Unity2DUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Функциональный компонент определяющий размещения элемента подсистемы Unity UI в 2D пространстве
		/// </summary>
		/// <remarks>
		/// Реализация функционального компонента определяющего размещения элемента подсистемы Unity UI в 2D пространстве
		/// </remarks>
		//------------------------------------------------------------------------------------------------------------
		[Serializable]
		[RequireComponent(typeof(RectTransform))]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathFunc + "Placeable UI")]
		public class LotusUIPlaceable2D : UIBehaviour, ILotusPlaceable2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal RectTransform mUIRectElement;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedSize;
			[SerializeField]
			internal Boolean mDisableInspector;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Прямоугольник трансформации элемента
			/// </summary>
			public RectTransform UIRect
			{
				get
				{
#if UNITY_EDITOR
					if(mUIRectElement == null) mUIRectElement = this.GetComponent<RectTransform>();
#endif
					return mUIRectElement;
				}
			}

			/// <summary>
			/// Родительский элемент
			/// </summary>
			public LotusUIPlaceable2D ParentElement
			{
				get
				{
					LotusUIPlaceable2D element = transform.parent.GetComponent<LotusUIPlaceable2D>();
					if (element != null)
					{
						return element;
					}
					else
					{
						element = transform.parent.parent.GetComponent<LotusUIPlaceable2D>();
						if (element != null)
						{
							return element;
						}
					}

					return null;
				}
			}

			/// <summary>
			/// Ширина (размер по X) родительского элемента
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
			/// Актуальная ширина (размер по X) родительского элемента
			/// </summary>
			public Single ParentWidthScreen
			{
				get
				{
					if (ParentElement == null)
					{
						return (LotusElementUIDispatcher.ScreenWidth);
					}
					else
					{
						return (ParentElement.WidthScreen);
					}
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
			/// Актуальная высота (размер по Y) родительского элемента
			/// </summary>
			public Single ParentHeightScreen
			{
				get
				{
					if (ParentElement == null)
					{
						return (LotusElementUIDispatcher.ScreenHeight);
					}
					else
					{
						return (ParentElement.HeightScreen);
					}
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
			/// Позиция верхнего-левого угла элемента в экранных координатах
			/// </summary>
			public Vector2 CenterLocationScreen
			{
				get { return RectScreen.center; }
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
				get { return UIRect.GetWorldScreenRect(); }
			}

			/// <summary>
			/// Глубина элемента интерфейса (влияет на последовательность прорисовки)
			/// </summary>
			public Int32 Depth
			{
				get { return UIRect.GetSiblingIndex(); }
				set
				{
					UIRect.SetSiblingIndex(value);
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusPlaceable2D ================================
			/// <summary>
			/// Позиция левого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Left
			{
				get { return mUIRectElement.GetLeft(); }
				set 
				{
					mUIRectElement.SetLeft(value);
				}
			}

			/// <summary>
			/// Позиция правого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Right
			{
				get { return mUIRectElement.GetRight(); }
				set
				{
					mUIRectElement.SetRight(value);
				}
			}

			/// <summary>
			/// Позиция верхнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Top
			{
				get { return mUIRectElement.GetTop(); }
				set
				{
					mUIRectElement.SetTop(value);
				}
			}

			/// <summary>
			/// Позиция нижнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Bottom
			{
				get { return mUIRectElement.GetBottom(); }
				set
				{
					mUIRectElement.SetBottom(value);
				}
			}

			/// <summary>
			/// Позиция верхнего-левого угла элемента от уровня родительской области
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
			/// Позиция центра элента от уровня родительской области
			/// </summary>
			public Vector2 CenterLocation
			{
				get { return new Vector2(Left + mUIRectElement.rect.width/2, Top + mUIRectElement.rect.height / 2); }
			}

			/// <summary>
			/// Ширина(размер по X) элемента
			/// </summary>
			public Single Width
			{
				get { return mUIRectElement.rect.width; }
				set
				{
					mUIRectElement.SetWidth(value);
				}
			}

			/// <summary>
			/// Высота (размер по Y) элемента
			/// </summary>
			public Single Height
			{
				get { return mUIRectElement.rect.height; }
				set
				{
					mUIRectElement.SetHeight(value);
				}
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
				set
				{
					mUIRectElement.SetHorizontalAlignment(value);
				}
			}

			/// <summary>
			/// Вертикальное выравнивание элемента
			/// </summary>
			public TVerticalAlignment VerticalAlignment
			{
				get { return mUIRectElement.GetVerticalAlignment(); }
				set
				{
					mUIRectElement.SetVerticalAlignment(value);
				}
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
			/// Инициализация элемента при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------

			protected virtual void ResetElement()
			{
			}
#endif


#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------

			protected override void Reset()
			{
				base.Reset();
				if (mUIRectElement == null)
				{
					mUIRectElement = this.GetComponent<RectTransform>();
				}
				this.ResetElement();
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void ConstructorElement()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void Awake()
			{
				base.Awake();
				if (mUIRectElement == null)
				{
					mUIRectElement = this.GetComponent<RectTransform>();
				}
				this.ConstructorElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование UnityGUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			//public void OnGUI()
			//{
			//	XRenderer2D.DrawBox(RectScreen, Color.red);
			//}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на вхождение точки в область элемента
			/// </summary>
			/// <param name="point">Точка в экранных координатах</param>
			/// <returns>Статус вхождения точки</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean ContainsScreen(Vector2 point)
			{
				return (RectScreen.Contains(point));
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
			public virtual void SetFromScreen(Single left, Single top, Single width, Single height)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление положение и размера элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdatePlacement()
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

			#region ======================================= МЕТОДЫ ILotusUIPlaceable2D ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка прямоугольника трансформации в качестве родительского
			/// </summary>
			/// <param name="rect_parent">Прямоугольник трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetParent(RectTransform rect_parent)
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
			public virtual void SetChild(RectTransform rect_child)
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
			public virtual void SetChild(LotusUIPlaceable2D placeable)
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

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на вхождение точки в перевёрнутых экранных координатах в область элемента
			/// </summary>
			/// <remarks>
			/// Позиция точки должна быть инвертирована, то есть начала координат по Y внизу экрана
			/// Такая позиция передаётся стандартными аргументами событий подсистемы EventSystems, например <see cref="PointerEventData"/>
			/// </remarks>
			/// <param name="position">Позиция точки в перевёрнутых экранных координатах</param>
			/// <returns>Статус вхождения</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean CheckContainsPointWorldScreenInv(Vector2 position)
			{
				return RectTransformUtility.RectangleContainsScreenPoint(mUIRectElement, position);
			}
			#endregion

			#region ======================================= МЕТОДЫ IScreenGameVisual ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка игрового экрана в указанную позицию
			/// </summary>
			/// <param name="position">Позиция в локальных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGamePosition(Vector2 position)
			{
				Location = position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка видимости игрового экрана 
			/// </summary>
			/// <param name="visible">Видимость игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGameVisible(Boolean visible)
			{
				CanvasGroup canvas_group = GetComponent<CanvasGroup>();

				if (canvas_group != null)
				{
					if (visible)
					{
						canvas_group.alpha = 1.0f;
					}
					else
					{
						canvas_group.alpha = 0.0f;
					}
				}

				gameObject.SetActive(visible);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка прозрачности игрового экрана 
			/// </summary>
			/// <param name="opacity">Прозрачность игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGameOpacity(Single opacity)
			{
				CanvasGroup canvas_group = GetComponent<CanvasGroup>();

				if (canvas_group != null)
				{
					canvas_group.alpha = opacity;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка смещения глубины показа(порядка рисования) игрового экрана
			/// </summary>
			/// <param name="depth_offset">Смещение по глубине</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGameDepthOffset(Int32 depth_offset)
			{
				Transform parent = UIRect.parent;
				Int32 child_count = parent.childCount;
				Int32 index = UIRect.GetSiblingIndex();

				if (depth_offset > 0)
				{
					if (index + depth_offset >= child_count)
					{
						if (child_count > 1)
						{
							UIRect.SetSiblingIndex(child_count - 2);
						}
					}
					else
					{
						UIRect.SetSiblingIndex(index + depth_offset);
					}
				}
				else
				{
					if (index + depth_offset < 0)
					{
						UIRect.SetSiblingIndex(0);
					}
					else
					{
						UIRect.SetSiblingIndex(index + depth_offset);
					}
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление отдельной канвы для элемента
			/// </summary>
			/// <returns>Канва</returns>
			//---------------------------------------------------------------------------------------------------------
			public CanvasGroup AddCanvasGroup()
			{
				return this.EnsureComponent<CanvasGroup>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление отдельной канвы для элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveCanvasGroup()
			{
				this.RemoveComponent<CanvasGroup>();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================