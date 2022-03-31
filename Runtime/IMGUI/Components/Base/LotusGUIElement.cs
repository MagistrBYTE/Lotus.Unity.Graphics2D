//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Компоненты IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIElement.cs
*		Компонент представляющий основной элемент интерфейса модуля IMGUI Unity.
*		Реализация компонента основного элемента GUI для построения полноценных адаптивных элементов интерфейса.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using UnityEngine;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DImmedateGUIComponent
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент представляющий базовый элемент интерфейса подсистемы IMGUI Unity
		/// </summary>
		/// <remarks>
		/// Реализация компонента основного элемента GUI для построения полноценных адаптивных элементов интерфейса
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XGUIEditorSettings.MenuPath + "Element")]
		public class LotusGUIElement : LotusGUIBaseElement, ILotusElement, ILotusScreenGameVisual, IComparable<ILotusElement>,
			IComparable<LotusGUIElement>, IComparable<CGUIElement>
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента
			/// </summary>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public new static LotusGUIElement CreateElement()
			{
				// 1) Создание объекта
				GameObject go = new GameObject("GUIElement");
				LotusGUIElement element = go.AddComponent<LotusGUIElement>();

				// 2) Конструктор элемента
				element.OnCreate();

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsEnabled;
			[NonSerialized]
			internal Boolean mIsEnabledElement;

			// Параметры размещения
			[SerializeField]
			internal TAspectMode mAspectMode;
			[SerializeField]
			internal Rect mRectLocalDesignMain;
			[SerializeField]
			internal Single mOffsetRight;
			[SerializeField]
			internal Single mOffsetBottom;
			[SerializeField]
			internal THorizontalAlignment mHorizontalAlignment;
			[SerializeField]
			internal TVerticalAlignment mVerticalAlignment;
			[NonSerialized]
			internal ILotusElement mParent;

			// Служебные данные
			[NonSerialized]
			internal Int32 mCountChildren;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Режим изменения размеров и соотношения сторон
			/// </summary>
			public TAspectMode AspectMode
			{
				get { return mAspectMode; }
				set
				{
					if (mAspectMode != value)
					{
						mAspectMode = value;
						UpdatePlacement();
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusElement ====================================
			/// <summary>
			/// Родительский элемент
			/// </summary>
			public ILotusElement IParent
			{
				get { return mParent; }
			}

			/// <summary>
			/// Ширина (размер по X) родительского элемента
			/// </summary>
			public Single ParentWidth
			{
				get
				{
					if (mParent == null)
					{
						return (LotusGUIDispatcher.ScreenWidth);
					}
					else
					{
						return (mParent.Width);
					}
				}
			}

			/// <summary>
			/// Актуальная ширина (размер по X) родительского элемента
			/// </summary>
			public Single ParentWidthScreen
			{
				get
				{
					if (mParent == null)
					{
						return (LotusGUIDispatcher.ScreenWidth);
					}
					else
					{
						return (mParent.WidthScreen);
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
					if (mParent == null)
					{
						return (LotusGUIDispatcher.ScreenHeight);
					}
					else
					{
						return (mParent.Height);
					}
				}
			}

			/// <summary>
			/// Актуальная высота (размер по Y) родительского элемента
			/// </summary>
			public Single ParentHeightScreen
			{
				get
				{
					if (mParent == null)
					{
						return (LotusGUIDispatcher.ScreenHeight);
					}
					else
					{
						return (mParent.HeightScreen);
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusEnabling ===================================
			/// <summary>
			/// Включение/отключение доступности элемента
			/// </summary>
			public Boolean IsEnabled
			{
				get { return mIsEnabled; }
				set
				{
					if (mIsEnabled != value)
					{
						mIsEnabled = value;
						SetEnabledElement();
					}
				}
			}

			/// <summary>
			/// Статус доступности элемента
			/// </summary>
			/// <remarks>
			/// Зависит как от статус самого элемента и так от статуса родительского элемента
			/// </remarks>
			public Boolean IsEnabledElement
			{
				get
				{
					return mIsEnabledElement;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusPlaceable2D ================================
			/// <summary>
			/// Позиция левого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Left
			{
				get { return mRectLocalDesignMain.x; }
				set
				{
					if (mRectLocalDesignMain.x != value)
					{
						mRectLocalDesignMain.x = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Позиция правого угла элемента по X от уровня родительской области
			/// </summary>
			/// <remarks>
			/// Установка возможно только в режиме растянутого элемента по ширине
			/// </remarks>
			public Single Right
			{
				get
				{
					if (mHorizontalAlignment == THorizontalAlignment.Stretch)
					{
						return mOffsetRight;
					}
					else
					{
						return mRectLocalDesignMain.xMax;
					}
				}
				set
				{
					if (mHorizontalAlignment == THorizontalAlignment.Stretch)
					{
						if (mOffsetRight != value)
						{
							mOffsetRight = value;
							UpdatePlacement();
						}
					}
					else
					{
						if (mRectLocalDesignMain.xMax != value)
						{
							mRectLocalDesignMain.xMax = value;
							UpdatePlacement();
						}
					}
				}
			}

			/// <summary>
			/// Позиция верхнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Top
			{
				get { return mRectLocalDesignMain.y; }
				set
				{
					if (mRectLocalDesignMain.y != value)
					{
						mRectLocalDesignMain.y = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Позиция нижнего угла элемента по Y от уровня родительской области
			/// </summary>
			/// <remarks>
			/// Установка возможно только в режиме растянутого элемента по высоте
			/// </remarks>
			public Single Bottom
			{
				get
				{
					if (mVerticalAlignment == TVerticalAlignment.Stretch)
					{
						return mOffsetBottom;
					}
					else
					{
						return mRectLocalDesignMain.yMax;
					}
				}
				set
				{
					if (mVerticalAlignment == TVerticalAlignment.Stretch)
					{
						if (mOffsetBottom != value)
						{
							mOffsetBottom = value;
							UpdatePlacement();
						}
					}
					else
					{
						if (mRectLocalDesignMain.yMax != value)
						{
							mRectLocalDesignMain.yMax = value;
							UpdatePlacement();
						}
					}
				}
			}

			/// <summary>
			/// Позиция верхнего-левого угла элемента от уровня родительской области
			/// </summary>
			public Vector2 Location
			{
				get { return mRectLocalDesignMain.position; }
				set
				{
					if (mRectLocalDesignMain.position != value)
					{
						mRectLocalDesignMain.x = value.x;
						mRectLocalDesignMain.y = value.y;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Высота(размер по Y) элемента
			/// </summary>
			public Single Width
			{
				get
				{
					return mRectLocalDesignMain.width;
				}
				set
				{
					if (mRectLocalDesignMain.width != value)
					{
						mRectLocalDesignMain.width = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Высота(размер по Y) элемента
			/// </summary>
			public Single Height
			{
				get
				{
					return mRectLocalDesignMain.height;
				}
				set
				{
					if (mRectLocalDesignMain.height != value)
					{
						mRectLocalDesignMain.height = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Размер элемента
			/// </summary>
			public Vector2 Size
			{
				get { return mRectLocalDesignMain.size; }
				set
				{
					if (mRectLocalDesignMain.size != value)
					{
						mRectLocalDesignMain.size = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Прямоугольника элемента от уровня родительской области
			/// </summary>
			public Rect RectLocalDesign
			{
				get { return mRectLocalDesignMain; }
				set { mRectLocalDesignMain = value; }
			}

			/// <summary>
			/// Горизонтальное выравнивание элемента
			/// </summary>
			public THorizontalAlignment HorizontalAlignment
			{
				get { return mHorizontalAlignment; }
				set
				{
					if (mHorizontalAlignment != value)
					{
						mHorizontalAlignment = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Вертикальное выравнивание элемента
			/// </summary>
			public TVerticalAlignment VerticalAlignment
			{
				get { return mVerticalAlignment; }
				set
				{
					if (mVerticalAlignment != value)
					{
						mVerticalAlignment = value;
						UpdatePlacement();
					}
				}
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов по порядку рисования
			/// </summary>
			/// <param name="other">Элемент</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(ILotusElement other)
			{
				return (this.Depth.CompareTo(other.Depth));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов по порядку рисования
			/// </summary>
			/// <param name="other">Элемент</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(LotusGUIElement other)
			{
				return (this.Depth.CompareTo(other.Depth));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов по порядку рисования
			/// </summary>
			/// <param name="other">Элемент</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CGUIElement other)
			{
				return (this.Depth.CompareTo(other.Depth));
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusVisibility ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка видимости элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetVisibleElement()
			{
				if (mCountChildren > 0)
				{
					LotusGUIDispatcher.FromParentSetVisibleElements(this);
				}

				if (mParent == null)
				{
					mIsVisibleElement = IsVisibleSelf;
				}
				else
				{
					ILotusVisibility element = mParent as ILotusVisibility;
					if (element != null)
					{
						mIsVisibleElement = IsVisibleSelf && element.IsVisibleSelf;
					}
					else
					{
						mIsVisibleElement = IsVisibleSelf;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение статуса видимости элемента
			/// </summary>
			/// <returns>Статус видимости элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean GetVisibleElement()
			{
				if (mParent == null)
				{
					return IsVisibleSelf;
				}
				else
				{
					ILotusVisibility element = mParent as ILotusVisibility;
					if (element != null)
					{
						return IsVisibleSelf && element.IsVisibleSelf;
					}
					else
					{
						return IsVisibleSelf;
					}
				}
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
				UpdatePlacementBase();

				// Считаем дочерние элементы
				if (mCountChildren > 0)
				{
					LotusGUIDispatcher.FromParentComputePositionElements(this);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции и размеров элемента по абсолютным данным
			/// </summary>
			/// <remarks>
			/// На основании абсолютной позиции элемент в экранных координатах считается его относительная позиция
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdatePlacementFromAbsolute()
			{
				Single width = LotusGUIDispatcher.ScreenWidth;
				Single height = LotusGUIDispatcher.ScreenHeight;

				Single offset_x = 0;
				Single offset_y = 0;

				mRectLocalDesignMain.width = mRectWorldScreenMain.width;
				mRectLocalDesignMain.height = mRectWorldScreenMain.height;

				if (mParent != null)
				{
					offset_x = mParent.Left;
					offset_y = mParent.Top;

					width = mParent.Width;
					height = mParent.Height;
				}

				switch (mHorizontalAlignment)
				{
					case THorizontalAlignment.Left:
						{
							mRectLocalDesignMain.x = mRectWorldScreenMain.x - offset_x;
						}
						break;
					case THorizontalAlignment.Center:
						{
							mRectLocalDesignMain.x = mRectWorldScreenMain.x - (offset_x + width / 2 - mRectLocalDesignMain.width / 2);
						}
						break;
					case THorizontalAlignment.Right:
						{
							mRectLocalDesignMain.x = width - mRectLocalDesignMain.width + offset_x - mRectWorldScreenMain.x;
						}
						break;
					case THorizontalAlignment.Stretch:
						{
							//mRectBounds.x = mRectDrawMain.x - offset_x;
							//mRectBounds.width = width - mRectDrawMain.xMax;
						}
						break;
					case THorizontalAlignment.Unknow:
						break;
					default:
						break;
				}

				switch (mVerticalAlignment)
				{
					case TVerticalAlignment.Top:
						{
							mRectLocalDesignMain.y = mRectWorldScreenMain.y - offset_y;
						}
						break;
					case TVerticalAlignment.Middle:
						{
							mRectLocalDesignMain.y = mRectWorldScreenMain.y - (offset_y + height / 2 - mRectLocalDesignMain.height / 2);
						}
						break;
					case TVerticalAlignment.Bottom:
						{
							mRectLocalDesignMain.y = height - mRectLocalDesignMain.height + offset_y - mRectWorldScreenMain.y;
						}
						break;
					case TVerticalAlignment.Stretch:
						{
							//mRectBounds.y = mRectDrawMain.y - offset_y;
							//mRectBounds.height = height - mRectDrawMain.yMax;
						}
						break;
					case TVerticalAlignment.Unknow:
						break;
					default:
						break;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusPlaceable2D ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции и размеров элемента по относительным данным
			/// </summary>
			/// <remarks>
			/// На основании относительной позиции элемента считается его абсолютная позиция в экранных координатах
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdatePlacementBase()
			{
				Single parent_width = LotusGUIDispatcher.ScreenWidth;
				Single parent_height = LotusGUIDispatcher.ScreenHeight;

				Single offset_x = 0;
				Single offset_y = 0;

				Single scaled_x = LotusGUIDispatcher.ScaledScreenX;
				Single scaled_y = LotusGUIDispatcher.ScaledScreenY;

				if (mParent != null)
				{
					Rect rect_parent = mParent.GetChildRectContent();
					offset_x = rect_parent.x;
					offset_y = rect_parent.y;

					parent_width = rect_parent.width;
					parent_height = rect_parent.height;
				}

				Single self_width = 0;
				Single self_height = 0;

				switch (mAspectMode)
				{
					case TAspectMode.None:
						{
							self_width = mRectLocalDesignMain.width;
							self_height = mRectLocalDesignMain.height;
						}
						break;
					case TAspectMode.Proportional:
						{
							self_width = mRectLocalDesignMain.width * scaled_x;
							self_height = mRectLocalDesignMain.height * scaled_y;
						}
						break;
					case TAspectMode.WidthControlsHeight:
						{
							self_width = mRectLocalDesignMain.width * scaled_x;
							self_height = mRectLocalDesignMain.height * scaled_x;
						}
						break;
					case TAspectMode.HeightControlsWidth:
						{
							self_width = mRectLocalDesignMain.width * scaled_y;
							self_height = mRectLocalDesignMain.height * scaled_y;
						}
						break;
					default:
						break;
				}

				switch (mHorizontalAlignment)
				{
					case THorizontalAlignment.Left:
						{
							mRectWorldScreenMain.x = offset_x + mRectLocalDesignMain.x * scaled_x;
							mRectWorldScreenMain.width = self_width;
							mOffsetRight = parent_width - mRectLocalDesignMain.xMax;
						}
						break;
					case THorizontalAlignment.Center:
						{
							mRectWorldScreenMain.x = mRectLocalDesignMain.x * scaled_x + offset_x + parent_width / 2 - self_width / 2;
							mRectWorldScreenMain.width = self_width;
							mOffsetRight = parent_width - mRectLocalDesignMain.xMax;
						}
						break;
					case THorizontalAlignment.Right:
						{
							mRectWorldScreenMain.x = parent_width - self_width - mRectLocalDesignMain.x * scaled_x + offset_x;
							mRectWorldScreenMain.width = self_width;
							mOffsetRight = parent_width - mRectLocalDesignMain.x;
						}
						break;
					case THorizontalAlignment.Stretch:
						{
							// Изменяем ширину
							mRectLocalDesignMain.width = parent_width - (mRectLocalDesignMain.x + mOffsetRight);
							mRectWorldScreenMain.x = offset_x + mRectLocalDesignMain.x * scaled_x;
							mRectWorldScreenMain.width = parent_width - (mRectLocalDesignMain.x + mOffsetRight) * scaled_x;
						}
						break;
					case THorizontalAlignment.Unknow:
						break;
					default:
						break;
				}

				switch (mVerticalAlignment)
				{
					case TVerticalAlignment.Top:
						{
							mRectWorldScreenMain.y = offset_y + mRectLocalDesignMain.y * scaled_y;
							mRectWorldScreenMain.height = self_height;
						}
						break;
					case TVerticalAlignment.Middle:
						{
							mRectWorldScreenMain.y = mRectLocalDesignMain.y * scaled_y + offset_y + parent_height / 2 - self_height / 2;
							mRectWorldScreenMain.height = self_height;
						}
						break;
					case TVerticalAlignment.Bottom:
						{
							mRectWorldScreenMain.y = parent_height - self_height - mRectLocalDesignMain.y * scaled_y + offset_y;
							mRectWorldScreenMain.height = self_height;
						}
						break;
					case TVerticalAlignment.Stretch:
						{
							// Изменяем высоту
							mRectLocalDesignMain.height = parent_height - (mRectLocalDesignMain.y + mOffsetBottom);
							mRectWorldScreenMain.y = offset_y + mRectLocalDesignMain.y * scaled_y;
							mRectWorldScreenMain.height = parent_height - (mRectLocalDesignMain.y + mOffsetBottom) * scaled_y;
						}
						break;
					case TVerticalAlignment.Unknow:
						break;
					default:
						break;
				}

				// Выравниваем
				mRectWorldScreenMain.x = Mathf.Ceil(mRectWorldScreenMain.x);
				mRectWorldScreenMain.y = Mathf.Ceil(mRectWorldScreenMain.y);
				mRectWorldScreenMain.width = Mathf.Ceil(mRectWorldScreenMain.width);
				mRectWorldScreenMain.height = Mathf.Ceil(mRectWorldScreenMain.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров элемента
			/// </summary>
			/// <param name="left">Позиция по X левого угла элемента от уровня родительской области</param>
			/// <param name="top">Позиция по Y верхнего угла элемента от уровня родительской области</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetFromLocalDesign(Single left, Single top, Single width, Single height)
			{
				mRectLocalDesignMain.x = left;
				mRectLocalDesignMain.y = top;
				mRectLocalDesignMain.width = width;
				mRectLocalDesignMain.height = height;
				UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка выравнивания элемента
			/// </summary>
			/// <param name="h_align">Горизонтальное выравнивание элемента</param>
			/// <param name="v_align">Вертикальное выравнивание элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetAlignment(THorizontalAlignment h_align, TVerticalAlignment v_align)
			{
				mHorizontalAlignment = h_align;
				mVerticalAlignment = v_align;
				UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вверх по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ToFrontSibling()
			{
				Depth++;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вниз по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ToBackSibling()
			{
				Depth--;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента первым в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetAsFirstSibling()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента последним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetAsLastSibling()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента предпоследним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetAsPreLastSibling()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusEnabling =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка видимости элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetEnabledElement()
			{
				if (mCountChildren > 0)
				{
					LotusGUIDispatcher.FromParentSetEnabledElements(this);
				}

				if (mParent == null)
				{
					mIsEnabledElement = mIsEnabled;
				}
				else
				{
					ILotusEnabling element = mParent as ILotusEnabling;
					if (element != null)
					{
						mIsEnabledElement = mIsEnabled && element.IsEnabled;
					}
					else
					{
						mIsEnabledElement = mIsEnabled;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение статуса доступности элемента
			/// </summary>
			/// <returns>Статус доступности элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			internal Boolean GetEnabledElement()
			{
				if (mParent == null)
				{
					return mIsEnabled;
				}
				else
				{
					ILotusEnabling element = mParent as ILotusEnabling;
					if (element != null)
					{
						return mIsEnabled && element.IsEnabled;
					}
					else
					{
						return mIsEnabled;
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBaseElement ==================================
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
				mOpacity = 1.0f;
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);

				// Добавляем все дочерние элементы первого уровня
				for (Int32 i = 0; i < transform.childCount; i++)
				{
					LotusGUIElement[] childs = transform.GetChild(i).GetComponents<LotusGUIElement>();
					if (childs != null && childs.Length > 0)
					{
						for (Int32 ic = 0; ic < childs.Length; ic++)
						{
							childs[ic].SetParent(this, false);
						}
					}
				}

				// Обновляем расположение
				this.UpdatePlacement();
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
			public virtual void SetChildren(ILotusElement child)
			{
				if (child != null)
				{
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
			public virtual void UnsetChildren(ILotusElement child)
			{
				if (child != null)
				{
					if (mCountChildren > 0)
					{
						mCountChildren--;
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
			public virtual void SetParent(ILotusElement parent, Boolean absolute_pos)
			{
				if (System.Object.ReferenceEquals(parent, this)) return;

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
				return mRectWorldScreenMain;
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
				gameObject.SetActive(visible);
				IsVisible = visible;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка прозрачности игрового экрана 
			/// </summary>
			/// <param name="opacity">Прозрачность игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGameOpacity(Single opacity)
			{
				Opacity = opacity;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка смещения глубины показа(порядка рисования) игрового экрана
			/// </summary>
			/// <param name="depth_offset">Смещение по глубине</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGameDepthOffset(Int32 depth_offset)
			{
				Depth += depth_offset;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			/// <remarks>
			/// Вызывается только в процессе добавления компонента к игровому объекта (метод Reset)
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void OnCreate()
			{
				mIsEnabled = true;
				mIsVisibleElement = true;
				mIsEnabledElement = true;
				mRectLocalDesignMain.x = 20;
				mRectLocalDesignMain.y = 20;
				mRectLocalDesignMain.width = 120;
				mRectLocalDesignMain.height = 30;
				mBackgroundColor = Color.white;
				mOpacity = 1.0f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавления элемента с список элементов диспетчера (регистрация)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void IncludeDispatcher()
			{
				LotusGUIDispatcher.RegisterElement(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Извлечение элемента со списока элементов диспетчера (отмена регистрации)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ExludeDispatcher()
			{
				LotusGUIDispatcher.UnRegisterElement(this);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override LotusGUIBaseElement Duplicate()
			{
				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(LotusGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				LotusGUIElement element = base_element as LotusGUIElement;
				if (element != null)
				{
					mIsEnabled = element.mIsEnabled;
					mIsEnabledElement = element.mIsEnabledElement;
					mAspectMode = element.mAspectMode;
					mRectLocalDesignMain = element.mRectLocalDesignMain;
					mOffsetRight = element.mOffsetRight;
					mOffsetBottom = element.mOffsetBottom;
					mHorizontalAlignment = element.mHorizontalAlignment;
					mVerticalAlignment = element.mVerticalAlignment;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАЗМЕЩЕНИЯ ЭЛЕМЕНТА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Присоединение элемента слева
			/// </summary>
			/// <param name="element">Элемент к которому присоединяются</param>
			/// <param name="space">Смещения по направлению присоединения</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DockAtLeft(CGUIElement element, Single space = 0)
			{
				if (element != null)
				{
					mRectLocalDesignMain.y = element.Top;
					mRectLocalDesignMain.x = element.Left - mRectLocalDesignMain.width - space;
					mHorizontalAlignment = element.HorizontalAlignment;
					mVerticalAlignment = element.VerticalAlignment;

					if (element.IParent == null)
					{
						UpdatePlacement();
					}
					else
					{
						SetParent(element.IParent, false);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Присоединение элемента слева и установка по высоте
			/// </summary>
			/// <param name="element">Элемент к которому присоединяются</param>
			/// <param name="space">Смещения по направлению присоединения</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DockAtLeftHeight(CGUIElement element, Single space = 0)
			{
				if (element != null)
				{
					mRectLocalDesignMain.y = element.Top;
					mRectLocalDesignMain.x = element.Left - mRectLocalDesignMain.width - space;
					mRectLocalDesignMain.height = element.Height;
					mHorizontalAlignment = element.HorizontalAlignment;
					mVerticalAlignment = element.VerticalAlignment;

					if (element.IParent == null)
					{
						UpdatePlacement();
					}
					else
					{
						SetParent(element.IParent, false);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Присоединение элемента сверху
			/// </summary>
			/// <param name="element">Элемент к которому присоединяются</param>
			/// <param name="space">Смещения по направлению присоединения</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DockAtTop(CGUIElement element, Single space = 0)
			{
				if (element != null)
				{
					mRectLocalDesignMain.x = element.Left;
					mRectLocalDesignMain.y = element.Top - mRectLocalDesignMain.height - space;
					mHorizontalAlignment = element.HorizontalAlignment;
					mVerticalAlignment = element.VerticalAlignment;

					if (element.IParent == null)
					{
						UpdatePlacement();
					}
					else
					{
						SetParent(element.IParent, false);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Присоединение элемента сверху и установка по ширине
			/// </summary>
			/// <param name="element">Элемент к которому присоединяются</param>
			/// <param name="space">Смещения по направлению присоединения</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DockAtTopWidth(CGUIElement element, Single space = 0)
			{
				if (element != null)
				{
					mRectLocalDesignMain.x = element.Left;
					mRectLocalDesignMain.y = element.Top - mRectLocalDesignMain.height - space;
					mRectLocalDesignMain.width = element.Width;
					mHorizontalAlignment = element.HorizontalAlignment;
					mVerticalAlignment = element.VerticalAlignment;

					if (element.IParent == null)
					{
						UpdatePlacement();
					}
					else
					{
						SetParent(element.IParent, false);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Присоединение элемента справа
			/// </summary>
			/// <param name="element">Элемент к которому присоединяются</param>
			/// <param name="space">Смещения по направлению присоединения</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DockAtRight(CGUIElement element, Single space = 0)
			{
				if (element != null)
				{
					mRectLocalDesignMain.y = element.Top;
					mRectLocalDesignMain.x = element.Right + space;
					mHorizontalAlignment = element.HorizontalAlignment;
					mVerticalAlignment = element.VerticalAlignment;

					if (element.IParent == null)
					{
						UpdatePlacement();
					}
					else
					{
						SetParent(element.IParent, false);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Присоединение элемента справа и установка по высоте
			/// </summary>
			/// <param name="element">Элемент к которому присоединяются</param>
			/// <param name="space">Смещения по направлению присоединения</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DockAtRightHeight(CGUIElement element, Single space = 0)
			{
				if (element != null)
				{
					mRectLocalDesignMain.y = element.Top;
					mRectLocalDesignMain.x = element.Right + space;
					mRectLocalDesignMain.height = element.Height;
					mHorizontalAlignment = element.HorizontalAlignment;
					mVerticalAlignment = element.VerticalAlignment;

					if (element.IParent == null)
					{
						UpdatePlacement();
					}
					else
					{
						SetParent(element.IParent, false);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Присоединение элемента снизу
			/// </summary>
			/// <param name="element">Элемент к которому присоединяются</param>
			/// <param name="space">Смещения по направлению присоединения</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DockAtBottom(CGUIElement element, Single space = 0)
			{
				if (element != null)
				{
					mRectLocalDesignMain.x = element.Left;
					mRectLocalDesignMain.y = element.Bottom + space;
					mHorizontalAlignment = element.HorizontalAlignment;
					mVerticalAlignment = element.VerticalAlignment;

					if (element.IParent == null)
					{
						UpdatePlacement();
					}
					else
					{
						SetParent(element.IParent, false);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Присоединение элемента снизу и установка по ширине
			/// </summary>
			/// <param name="element">Элемент к которому присоединяются</param>
			/// <param name="space">Смещения по направлению присоединения</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DockAtBottomWidth(CGUIElement element, Single space = 0)
			{
				if (element != null)
				{
					mRectLocalDesignMain.x = element.Left;
					mRectLocalDesignMain.y = element.Bottom + space;
					mRectLocalDesignMain.width = element.Width;
					mHorizontalAlignment = element.HorizontalAlignment;
					mVerticalAlignment = element.VerticalAlignment;

					if (element.IParent == null)
					{
						UpdatePlacement();
					}
					else
					{
						SetParent(element.IParent, false);
					}
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