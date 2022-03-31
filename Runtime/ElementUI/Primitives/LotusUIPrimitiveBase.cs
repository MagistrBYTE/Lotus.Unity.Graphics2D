//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitiveBase.cs
*		Базовые компонент для отображения векторного примитива.
*		Реализация базового компонента для отображения векторного примитива.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
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
		//! \defgroup Unity2DUIPrimitive Подсистема геометрических примитивов
		//! Подсистема геометрических примитивов обеспечивает создание и отображение различных геометрических примитивов
		//! \ingroup Unity2DUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовые компонент для отображения векторного примитива
		/// </summary>
		/// <remarks>
		/// Реализация базового компонента для отображения векторного примитива
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathPrimitive + "Base")]
		public class LotusUIPrimitiveBase : MaskableGraphic, ILotusPlaceable2D, ILayoutElement, ICanvasRaycastFilter
		{
			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal Sprite mSprite;
			[NonSerialized]
			internal Sprite mOverrideSprite;
			[NonSerialized]
			internal Single mEventAlphaThreshold = 1;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedSize;
			[SerializeField]
			internal Boolean mExpandedBase;
			[SerializeField]
			internal Boolean mExpandedMainParam;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Основной спрайт
			/// </summary>
			public Sprite sprite
			{
				get { return mSprite; }
				set
				{
					if (XValueSet.SetClass(ref mSprite, value))
					{
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Переопределенный спрайт
			/// </summary>
			public Sprite overrideSprite
			{
				get { return mOverrideSprite == null ? sprite : mOverrideSprite; }
				set
				{
					if (XValueSet.SetClass(ref mOverrideSprite, value))
					{
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Тест на проверку пересечения
			/// </summary>
			public Single eventAlphaThreshold
			{
				get { return mEventAlphaThreshold; }
				set { mEventAlphaThreshold = value; }
			}

			/// <summary>
			/// Разрешение спрайта
			/// </summary>
			public Single pixelsPerUnit
			{
				get
				{
					Single sprite_pixels_PerUnit = 100;
					if (sprite != null)
					{
						sprite_pixels_PerUnit = sprite.pixelsPerUnit;
					}

					Single reference_PixelsPerUnit = 100;
					if (canvas != null)
					{
						reference_PixelsPerUnit = canvas.referencePixelsPerUnit;
					}

					return sprite_pixels_PerUnit / reference_PixelsPerUnit;
				}
			}

			/// <summary>
			/// Основная текстура
			/// </summary>
			public override Texture mainTexture
			{
				get
				{
					if (overrideSprite == null)
					{
						if (material != null && material.mainTexture != null)
						{
							return material.mainTexture;
						}

						return s_WhiteTexture;
					}

					return overrideSprite.texture;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusBasePlaceable2D ============================
			/// <summary>
			/// Позиция левого угла примитива по X в экранных координатах
			/// </summary>
			public Single LeftScreen
			{
				get { return RectScreen.x; }
				set { }
			}

			/// <summary>
			/// Позиция правого угла примитива по X в экранных координатах
			/// </summary>
			public Single RightScreen
			{
				get { return RectScreen.xMax; }
				set { }
			}

			/// <summary>
			/// Позиция верхнего угла примитива по Y в экранных координатах
			/// </summary>
			public Single TopScreen
			{
				get { return RectScreen.y; }
				set { }
			}

			/// <summary>
			/// Позиция нижнего угла примитива по Y в экранных координатах
			/// </summary>
			public Single BottomScreen
			{
				get { return RectScreen.yMax; }
				set { }
			}

			/// <summary>
			/// Позиция верхнего-левого угла примитива в экранных координатах
			/// </summary>
			public Vector2 LocationScreen
			{
				get { return RectScreen.position; }
				set { }
			}

			/// <summary>
			/// Ширина(размер по X) примитива
			/// </summary>
			public Single WidthScreen
			{
				get { return RectScreen.width; }
				set { }
			}

			/// <summary>
			/// Высота(размер по Y) примитива
			/// </summary>
			public Single HeightScreen
			{
				get { return RectScreen.height; }
				set { }
			}

			/// <summary>
			/// Размеры примитива в экранных координатах
			/// </summary>
			public Vector2 SizeScreen
			{
				get { return RectScreen.size; }
				set { }
			}

			/// <summary>
			/// Прямоугольника примитива в экранных координатах
			/// </summary>
			public Rect RectScreen
			{
				get { return rectTransform.GetWorldScreenRect(); }
			}

			/// <summary>
			/// Глубина примитива интерфейса (влияет на последовательность прорисовки)
			/// </summary>
			public Int32 Depth
			{
				get { return rectTransform.GetSiblingIndex(); }
				set { rectTransform.SetSiblingIndex(value); }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusPlaceable2D ================================
			/// <summary>
			/// Прямоугольник трансформации примитива
			/// </summary>
			public RectTransform UIRect
			{
				get
				{
					return rectTransform;
				}
			}

			/// <summary>
			/// Позиции левого угла примитива по X от уровня родительской области
			/// </summary>
			public Single Left
			{
				get { return rectTransform.GetLeft(); }
				set { rectTransform.SetLeft(value); }
			}

			/// <summary>
			/// Позиции правого угла примитива по X от уровня родительской области
			/// </summary>
			public Single Right
			{
				get { return rectTransform.GetRight(); }
				set { rectTransform.SetRight(value); }
			}

			/// <summary>
			/// Позиции верхнего угла примитива по Y от уровня родительской области
			/// </summary>
			public Single Top
			{
				get { return rectTransform.GetTop(); }
				set { rectTransform.SetTop(value); }
			}

			/// <summary>
			/// Позиции нижнего угла примитива по Y от уровня родительской области
			/// </summary>
			public Single Bottom
			{
				get { return rectTransform.GetBottom(); }
				set { rectTransform.SetBottom(value); }
			}

			/// <summary>
			/// Позиция верхнего-левого угла примитива
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
			/// Ширина(размер по X) примитива
			/// </summary>
			public Single Width
			{
				get { return rectTransform.rect.width; }
				set { rectTransform.SetWidth(value); }
			}

			/// <summary>
			/// Высота (размер по Y) примитива
			/// </summary>
			public Single Height
			{
				get { return rectTransform.rect.height; }
				set { rectTransform.SetHeight(value); }
			}

			/// <summary>
			/// Размер примитива
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
			/// Ширина(размер по X) родительского примитива
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
			/// Высота (размер по Y) родительского примитива
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
			/// Прямоугольника примитива от уровня родительской области
			/// </summary>
			public Rect RectLocalDesign
			{
				get { return new Rect(Left, Top, Width, Height); }
			}

			/// <summary>
			/// Горизонтальное выравнивание примитива
			/// </summary>
			public THorizontalAlignment HorizontalAlignment
			{
				get { return rectTransform.GetHorizontalAlignment(); }
				set { rectTransform.SetHorizontalAlignment(value); }
			}

			/// <summary>
			/// Вертикальное выравнивание примитива
			/// </summary>
			public TVerticalAlignment VerticalAlignment
			{
				get { return rectTransform.GetVerticalAlignment(); }
				set { rectTransform.SetVerticalAlignment(value); }
			}

			/// <summary>
			/// Опорная точка примитива для привязки, масштабирования и вращения
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
			/// Опорная координата по X примитива для привязки, масштабирования и вращения
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
			/// Опорная координата по Y примитива для привязки, масштабирования и вращения
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
			/// Условная привязка примитива
			/// </summary>
			public Vector2 Anchored
			{
				get { return rectTransform.anchoredPosition; }
				set { rectTransform.anchoredPosition = value; }
			}

			/// <summary>
			/// Размер условной привязке примитива по X
			/// </summary>
			public Single AnchoredX
			{
				get { return rectTransform.anchoredPosition.x; }
				set { rectTransform.anchoredPosition = new Vector2(value, rectTransform.anchoredPosition.y); }
			}

			/// <summary>
			/// Размер условной привязке примитива по Y
			/// </summary>
			public Single AnchoredY
			{
				get { return rectTransform.anchoredPosition.y; }
				set { rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, value); }
			}

			/// <summary>
			/// Масштаб примитива
			/// </summary>
			public Vector2 Scale
			{
				get { return rectTransform.localScale; }
				set { rectTransform.localScale = new Vector3(value.x, value.y, rectTransform.localScale.z); }
			}

			/// <summary>
			/// Масштаб примитива по X
			/// </summary>
			public Single ScaleX
			{
				get { return rectTransform.localScale.x; }
				set { rectTransform.localScale = new Vector3(value, rectTransform.localScale.y, rectTransform.localScale.z); }
			}

			/// <summary>
			/// Масштаб примитива по Y 
			/// </summary>
			public Single ScaleY
			{
				get { return rectTransform.localScale.y; }
				set { rectTransform.localScale = new Vector3(rectTransform.localScale.x, value, rectTransform.localScale.z); }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILayoutElement ===================================
			/// <summary>
			/// Минимальная ширина примитива
			/// </summary>
			public virtual Single minWidth { get { return 0; } }

			/// <summary>
			/// Минимальная высота примитива
			/// </summary>
			public virtual Single minHeight { get { return 0; } }

			/// <summary>
			/// Предпочитаемая ширина примитива
			/// </summary>
			public virtual Single preferredWidth
			{
				get
				{
					if (overrideSprite == null)
					{
						return 0;
					}

					return overrideSprite.rect.size.x / pixelsPerUnit;
				}
			}

			/// <summary>
			/// Предпочитаемая высота примитива
			/// </summary>
			public virtual Single preferredHeight
			{
				get
				{
					if (overrideSprite == null)
					{
						return 0;
					}

					return overrideSprite.rect.size.y / pixelsPerUnit;
				}
			}

			/// <summary>
			/// Гибкая ширина примитива
			/// </summary>
			public virtual Single flexibleWidth { get { return -1; } }

			/// <summary>
			/// Гибкая высота примитива
			/// </summary>
			public virtual Single flexibleHeight { get { return -1; } }

			/// <summary>
			/// Приоритет макета
			/// </summary>
			public virtual int layoutPriority { get { return 0; } }
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnEnable()
			{
				base.OnEnable();

				UpdateGeometryForced();
				SetAllDirty();
			}
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
			public virtual void SetChild(RectTransform rect_child)
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
			public virtual void SetChild(LotusUIPlaceable2D placeable)
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
				return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position);
			}
			#endregion

			#region ======================================= МЕТОДЫ ILayoutElement =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Определение макета примитива по горизонтали
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CalculateLayoutInputHorizontal()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Определение макета примитива по вертикали
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CalculateLayoutInputVertical() { }
			#endregion

			#region ======================================= МЕТОДЫ ICanvasRaycastFilter ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на пересечение проекции примитива с точкой на экране
			/// </summary>
			/// <param name="screen_point">Точка на экране</param>
			/// <param name="event_camera">Камера</param>
			/// <returns>Статус пересечение проекции</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean IsRaycastLocationValid(Vector2 screen_point, Camera event_camera)
			{
				if (mEventAlphaThreshold >= 1)
				{
					return true;
				}

				Sprite sprite = overrideSprite;
				if (sprite == null)
				{
					return true;
				}

				Vector2 local;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screen_point, event_camera, out local);

				Rect rect = GetPixelAdjustedRect();

				// Convert to have lower left corner as reference point.
				local.x += rectTransform.pivot.x * rect.width;
				local.y += rectTransform.pivot.y * rect.height;

				local = MapCoordinate(local, rect);

				// Normalize local coordinates.
				Rect sprite_rect = sprite.textureRect;
				Vector2 normalized = new Vector2(local.x / sprite_rect.width, local.y / sprite_rect.height);

				// Convert to texture space.
				Single x = Mathf.Lerp(sprite_rect.x, sprite_rect.xMax, normalized.x) / sprite.texture.width;
				Single y = Mathf.Lerp(sprite_rect.y, sprite_rect.yMax, normalized.y) / sprite.texture.height;

				try
				{
					return sprite.texture.GetPixelBilinear(x, y).a >= mEventAlphaThreshold;
				}
				catch (UnityException e)
				{
					Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + e.Message + " Also make sure to disable sprite packing for this sprite.", this);
					return true;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Заполнение массива 4 вершин
			/// </summary>
			/// <param name="vertices">Набор вершин</param>
			/// <param name="uvs">Набор текстурных координат</param>
			/// <returns>Массив вершин</returns>
			//---------------------------------------------------------------------------------------------------------
			protected UIVertex[] SetVertexBufferQuad(Vector2[] vertices, Vector2[] uvs)
			{
				UIVertex[] vbo = new UIVertex[4];
				for (Int32 i = 0; i < vertices.Length; i++)
				{
					var vert = UIVertex.simpleVert;
					vert.color = color;
					vert.position = vertices[i];
					vert.uv0 = uvs[i];
					vbo[i] = vert;
				}
				return vbo;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Заполнение массива 4 вершин
			/// </summary>
			/// <param name="vertices">Набор вершин</param>
			/// <param name="uvs">Набор текстурных координат</param>
			/// <param name="color_vertex">Цвет</param>
			/// <returns>Массив вершин</returns>
			//---------------------------------------------------------------------------------------------------------
			protected UIVertex[] SetVertexBufferQuad(Vector2[] vertices, Vector2[] uvs, Color color_vertex)
			{
				UIVertex[] vbo = new UIVertex[4];
				for (Int32 i = 0; i < vertices.Length; i++)
				{
					var vert = UIVertex.simpleVert;
					vert.color = color_vertex;
					vert.position = vertices[i];
					vert.uv0 = uvs[i];
					vbo[i] = vert;
				}
				return vbo;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение нормализованных координат объекта
			/// </summary>
			/// <param name="local">Локальные координаты</param>
			/// <param name="rect">Прямоугольник</param>
			/// <returns>Нормализованные координаты</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Vector2 MapCoordinate(Vector2 local, Rect rect)
			{
				Rect sprite_rect = sprite.rect;
				return new Vector2(local.x * sprite_rect.width / rect.width, local.y * sprite_rect.height / rect.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение выровненного бордюра
			/// </summary>
			/// <param name="border">Бордюры</param>
			/// <param name="rect">Прямоугольник</param>
			/// <returns>Выровненный бордюр</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
			{
				for (Int32 axis = 0; axis <= 1; axis++)
				{
					// If the rect is smaller than the combined borders, then there's not room for the borders at their normal size.
					// In order to avoid artefacts with overlapping borders, we scale the borders down to fit.
					Single combined_borders = border[axis] + border[axis + 2];
					if (rect.size[axis] < combined_borders && combined_borders != 0)
					{
						Single border_scale_ratio = rect.size[axis] / combined_borders;
						border[axis] *= border_scale_ratio;
						border[axis + 2] *= border_scale_ratio;
					}
				}

				return border;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительное обновление геометрии
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateGeometryForced()
			{
				// Пока такое обновление
				Vector2 old_pivot = rectTransform.pivot;
				rectTransform.pivot = Vector2.zero;
				rectTransform.pivot = old_pivot;
				UpdateGeometry();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================