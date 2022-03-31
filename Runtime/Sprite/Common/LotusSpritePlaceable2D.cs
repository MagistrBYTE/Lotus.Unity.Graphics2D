//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpritePlaceable2D.cs
*		Компонент размещения спрайта в двухмерной пространстве.
*		Реализация компонента для размещении спрайта в двухмерном пространстве в экранных координатах при ортографическом
*	фиксированном режиме камеры.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DSpriteCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент размещения спрайта в двухмерной пространстве
		/// </summary>
		/// <remarks>
		/// Реализация компонента для размещении спрайта в двухмерном пространстве в экранных координатах при
		/// ортографическом фиксированном режиме камеры
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[RequireComponent(typeof(SpriteRenderer))]
		[ExecuteInEditMode]
		[AddComponentMenu(XSpriteEditorSettings.MenuPath + "Placeable Sprite")]
		public class LotusSpritePlaceable2D : MonoBehaviour, IComparable<LotusSpritePlaceable2D>, ILotusPlaceable2D
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента SpritePlaceable2D
			/// </summary>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusSpritePlaceable2D CreateSprite(Single left, Single top, Single width, Single height, Transform parent = null)
			{
				// 1) Создание объекта
				LotusSpritePlaceable2D element = LotusSpriteDispatcher.CreateElement<LotusSpritePlaceable2D>("Sprite2D", left, top, width, height);

				// 2) Определение в иерархии
				element.SetParent(parent);

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Иерархические отношения
			[NonSerialized]
			internal Boolean mIsUnitParent;
			[SerializeField]
			internal LotusSpritePlaceable2D mParent;

			// Кэшированные данные
			[SerializeField]
			internal Transform mThisTransform;
			[SerializeField]
			internal SpriteRenderer mSpriteRenderer;
			[SerializeField]
			internal Rect mRectLocalDesign;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedSize;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Родительский элемент
			/// </summary>
			public LotusSpritePlaceable2D ParentElement
			{
				get
				{
#if UNITY_EDITOR
					if(UnityEditor.EditorApplication.isPlaying)
					{
						if (mIsUnitParent == false)
						{
							mParent = mThisTransform.GetComponentFromParentOrHisParent<LotusSpritePlaceable2D>();
							mIsUnitParent = true;
						}

						return (mParent);
					}
					else
					{
						mParent = mThisTransform.GetComponentFromParentOrHisParent<LotusSpritePlaceable2D>();
						return (mParent);
					}
#else
					if (mIsUnitParent == false)
					{
						mParent = mThisTransform.GetComponentFromParentOrHisParent<LotusSpritePlaceable2D>();
						mIsUnitParent = true;
					}

					return (mParent);
#endif
				}
			}

			/// <summary>
			/// Ширина (размер по X) родительского элемента
			/// </summary>
			public Single ParentWidth
			{
				get
				{
					if(ParentElement == null)
					{
						return (LotusSpriteDispatcher.ScreenWidth);
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
					if (ParentElement == null)
					{
						return (LotusSpriteDispatcher.ScreenWidth);
					}
					else
					{
						return (mParent.mRectLocalDesign.width * LotusSpriteDispatcher.ScaledScreenX);
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
					if (ParentElement == null)
					{
						return (LotusSpriteDispatcher.ScreenHeight);
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
					if (ParentElement == null)
					{
						return (LotusSpriteDispatcher.ScreenHeight);
					}
					else
					{
						return (mParent.mRectLocalDesign.height * LotusSpriteDispatcher.ScaledScreenY);
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
				get
				{
					return (mThisTransform.GetSpriteWorldScreenX() - WidthScreen / 2);
				}
				set
				{
					mThisTransform.SetSpriteWorldTransformFromWorldScreenX(value + WidthScreen / 2);
					SetLocalDesignFromLocalTransform();
				}
			}

			/// <summary>
			/// Позиция правого угла элемента по X в экранных координатах
			/// </summary>
			public Single RightScreen
			{
				get
				{
					return (mThisTransform.GetSpriteWorldScreenX() + WidthScreen / 2);
				}
				set
				{
					mThisTransform.SetSpriteWorldTransformFromWorldScreenX(value - WidthScreen / 2);
					SetLocalDesignFromLocalTransform();
					SetLocalDesignFromLocalScale();
				}
			}

			/// <summary>
			/// Позиция верхнего угла элемента по Y в экранных координатах
			/// </summary>
			public Single TopScreen
			{
				get
				{
					return (mThisTransform.GetSpriteWorldScreenY() - HeightScreen / 2);
				}
				set
				{
					mThisTransform.SetSpriteWorldTransformFromWorldScreenY(value + HeightScreen / 2);
					SetLocalDesignFromLocalTransform();
				}
			}

			/// <summary>
			/// Позиция нижнего угла элемента по Y в экранных координатах
			/// </summary>
			public Single BottomScreen
			{
				get
				{
					return (mThisTransform.GetSpriteWorldScreenY() + HeightScreen / 2);
				}
				set
				{
					mThisTransform.SetSpriteWorldTransformFromWorldScreenY(value - HeightScreen / 2);
					SetLocalDesignFromLocalTransform();
					SetLocalDesignFromLocalScale();
				}
			}

			/// <summary>
			/// Позиция верхнего-левого угла элемента в экранных координатах
			/// </summary>
			public Vector2 LocationScreen
			{
				get
				{
					return (mThisTransform.GetSpriteWorldScreen());
				}
				set
				{
					mThisTransform.SetSpriteWorldTransformFromWorldScreen(value.x + WidthScreen / 2, value.y + HeightScreen / 2);
					SetLocalDesignFromLocalTransform();
				}
			}

			/// <summary>
			/// Ширина(размер по X) элемента
			/// </summary>
			public Single WidthScreen
			{
				get
				{
					return (mRectLocalDesign.width * LotusSpriteDispatcher.ScaledScreenX);
				}
				set
				{
					mRectLocalDesign.width = value / LotusSpriteDispatcher.ScaledScreenX;
					SetLocalScaleFromLocalDesignWidth();
				}
			}

			/// <summary>
			/// Высота(размер по Y) элемента
			/// </summary>
			public Single HeightScreen
			{
				get
				{
					return (mRectLocalDesign.height * LotusSpriteDispatcher.ScaledScreenY);
				}
				set
				{
					mRectLocalDesign.height = value / LotusSpriteDispatcher.ScaledScreenY;
					SetLocalScaleFromLocalDesignHeight();
				}
			}

			/// <summary>
			/// Размеры элемента в экранных координатах
			/// </summary>
			public Vector2 SizeScreen
			{
				get
				{
					return (new Vector2(WidthScreen, HeightScreen));
				}
				set
				{
					WidthScreen = value.x;
					HeightScreen = value.y;
				}
			}

			/// <summary>
			/// Прямоугольника элемента в экранных координатах
			/// </summary>
			public Rect RectScreen
			{
				get
				{
					return (new Rect(LeftScreen, TopScreen, WidthScreen, HeightScreen));
				}
			}

			/// <summary>
			/// Глубина элемента интерфейса (влияет на последовательность прорисовки)
			/// </summary>
			public Int32 Depth
			{
				get
				{
					if (ParentElement != null)
					{
						return (mThisTransform.GetSiblingIndex());
					}
					else
					{
						return ((Int32)mThisTransform.position.z);
					}
				}
				set
				{
					if (ParentElement != null)
					{
						mThisTransform.SetSiblingIndex(value);
						Int32 i = mThisTransform.GetSiblingIndex();
						Single depth = -i - 1f;
						mThisTransform.localPosition = new Vector3(mThisTransform.localPosition.x, mThisTransform.localPosition.y, depth);
					}
					else
					{
						Single depth = Mathf.Clamp(value, LotusSpriteDispatcher.MinPositionDepth, LotusSpriteDispatcher.MaxPositionDepth);
						mThisTransform.position = new Vector3(mThisTransform.position.x, mThisTransform.position.y, depth);
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusPlaceable2D ================================
			/// <summary>
			/// Позиция левого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Left
			{
				get
				{
					return (mRectLocalDesign.x);
				}
				set
				{
					mRectLocalDesign.x = value;
					SetLocalTransformFromLocalDesignX();
				}
			}

			/// <summary>
			/// Позиция правого угла элемента по X от уровня родительской области
			/// </summary>
			public Single Right
			{
				get
				{
					return (mRectLocalDesign.xMax);
				}
				set
				{
					mRectLocalDesign.xMax = value;

					SetLocalScaleFromLocalDesignWidth();
					SetLocalTransformFromLocalDesignX();
				}
			}

			/// <summary>
			/// Позиция верхнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Top
			{
				get
				{
					return (mRectLocalDesign.y);
				}
				set
				{
					mRectLocalDesign.y = value;
					SetLocalTransformFromLocalDesignY();
				}
			}

			/// <summary>
			/// Позиция нижнего угла элемента по Y от уровня родительской области
			/// </summary>
			public Single Bottom
			{
				get
				{
					return (mRectLocalDesign.yMax);
				}
				set
				{
					mRectLocalDesign.yMax = value;

					SetLocalScaleFromLocalDesignHeight();
					SetLocalTransformFromLocalDesignY();
				}
			}

			/// <summary>
			/// Позиция верхнего-левого угла элемента от уровня родительской области
			/// </summary>
			public Vector2 Location
			{
				get
				{
					return (mRectLocalDesign.position);
				}
				set
				{
					mRectLocalDesign.position = value;
					SetLocalTransformFromLocalDesign();
				}
			}

			/// <summary>
			/// Ширина(размер по X) элемента
			/// </summary>
			public Single Width
			{
				get
				{
					return (mRectLocalDesign.width);
				}
				set
				{
					mRectLocalDesign.width = value;
					SetLocalScaleFromLocalDesignWidth();
					SetLocalTransformFromLocalDesignX();
				}
			}

			/// <summary>
			/// Высота(размер по Y) элемента
			/// </summary>
			public Single Height
			{
				get
				{
					return (mRectLocalDesign.height);
				}
				set
				{
					mRectLocalDesign.height = value;
					SetLocalScaleFromLocalDesignHeight();
					SetLocalTransformFromLocalDesignY();
				}
			}

			/// <summary>
			/// Размер элемента
			/// </summary>
			public Vector2 Size
			{
				get
				{
					return (mRectLocalDesign.size);
				}
				set
				{
					mRectLocalDesign.size = value;
					SetLocalScaleFromLocalDesign();
					SetLocalTransformFromLocalDesign();
				}
			}

			/// <summary>
			/// Прямоугольника элемента от уровня родительской области
			/// </summary>
			public Rect RectLocalDesign
			{
				get { return (mRectLocalDesign); }
			}

			/// <summary>
			/// Горизонтальное выравнивание элемента
			/// </summary>
			public THorizontalAlignment HorizontalAlignment { get; set; }

			/// <summary>
			/// Вертикальное выравнивание элемента
			/// </summary>
			public TVerticalAlignment VerticalAlignment { get; set; }
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
			public void Reset()
			{
				mThisTransform = this.transform;
				mSpriteRenderer = this.GetComponent<SpriteRenderer>();

				ResetElement();
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
			public void Awake()
			{
				mThisTransform = this.transform;
				mSpriteRenderer = this.GetComponent<SpriteRenderer>();
				UpdatePlacementFromLocalDesign();

				for (Int32 i = 0; i < transform.childCount; i++)
				{
					LotusSpritePlaceable2D sprite_placed = transform.GetChild(i).GetComponent<LotusSpritePlaceable2D>();
					if (sprite_placed != null)
					{
						sprite_placed.UpdatePlacementFromLocalDesign();
					}
				}

				ConstructorElement();

				Debug.Log($"{name} = Awake");
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление размеров и положения элемента в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void OnUpdateInEditorMode()
			{
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					if (transform.hasChanged)
					{
						UpdatePlacementFromLocalTransform();

						for (Int32 i = 0; i < transform.childCount; i++)
						{
							LotusSpritePlaceable2D sprite_placed = transform.GetChild(i).GetComponent<LotusSpritePlaceable2D>();
							if (sprite_placed != null)
							{
								sprite_placed.UpdatePlacementFromLocalDesign();
							}
						}

						transform.hasChanged = false;
					}
				}
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
#if UNITY_EDITOR
				OnUpdateInEditorMode();
#endif
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование UnityGUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnGUI()
			{
				XGUIRender.StrokeColor = Color.red;
				XGUIRender.DrawRect(RectScreen);
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void DestructorElement()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Игровой объект уничтожился
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDestroy()
			{
				DestructorElement();
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов списка в зависимости от их видимости
			/// </summary>
			/// <param name="other">Элемент списка</param>
			/// <returns>Статус сравнения элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(LotusSpritePlaceable2D other)
			{
				return (this.Depth.CompareTo(other.Depth));
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
				mRectLocalDesign.x = left / LotusSpriteDispatcher.ScaledScreenX;
				mRectLocalDesign.y = top / LotusSpriteDispatcher.ScaledScreenY;
				mRectLocalDesign.width = width / LotusSpriteDispatcher.ScaledScreenX;
				mRectLocalDesign.height = height / LotusSpriteDispatcher.ScaledScreenY;
				SetLocalTransformFromLocalDesign();
				SetLocalScaleFromLocalDesign();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Основной метод определяющий положение и размер элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdatePlacement()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление положения и размера элемента на основании его параметров разработки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdatePlacementFromLocalDesign()
			{
				SetLocalScaleFromLocalDesign();
				SetLocalTransformFromLocalDesign();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление положения и размера элемента на основании его трасформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdatePlacementFromLocalTransform()
			{
				SetLocalDesignFromLocalTransform();
				SetLocalDesignFromLocalScale();
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение экранных координат из локальных координат
			/// </summary>
			/// <param name="left">Позиция по X в локальных координатах</param>
			/// <param name="top">Позиция по Y в локальных координатах</param>
			/// <returns>Экранные координаты</returns>
			//-------------------------------------------------------------------------------------------------------------
			public Vector2 GetWorldScreenFromLocalDesign(Single left, Single top)
			{
				Vector2 screen = mThisTransform.GetSpriteWorldScreen();
				screen.x += (left * LotusSpriteDispatcher.ScaledScreenX) - WidthScreen / 2;
				screen.y += (top * LotusSpriteDispatcher.ScaledScreenY) - HeightScreen / 2;
				return (screen);
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение мировых координат трансформации из локальных координат
			/// </summary>
			/// <param name="left">Позиция по X в локальных координатах</param>
			/// <param name="top">Позиция по Y в локальных координатах</param>
			/// <returns>Мировые координаты</returns>
			//-------------------------------------------------------------------------------------------------------------
			public Vector3 GetWorldTransformFromLocalDesign(Single left, Single top)
			{
				Vector2 screen = mThisTransform.GetSpriteWorldScreen();
				screen.x += (left * LotusSpriteDispatcher.ScaledScreenX) - WidthScreen / 2;
				screen.y += (top * LotusSpriteDispatcher.ScaledScreenY) - HeightScreen / 2;

				Single w = LotusSpriteDispatcher.ScreenWidth;
				Single h = LotusSpriteDispatcher.ScreenHeight;
				Single pos_x = LotusSpriteDispatcher.CameraOrthoWidth * ((((screen.x / w) * 2) - 1));
				Single pos_y = LotusSpriteDispatcher.CameraOrthoHeight * ((((screen.y / h) * 2) - 1));
				return (new Vector3(pos_x, -pos_y, mThisTransform.position.z));
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
				mRectLocalDesign.x = left;
				mRectLocalDesign.y = top;
				mRectLocalDesign.width = width;
				mRectLocalDesign.height = height;
				SetLocalScaleFromLocalDesign();
				SetLocalTransformFromLocalDesign();
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

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вверх по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToFrontSibling()
			{
				Depth++;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вниз по иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToBackSibling()
			{
				Depth--;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента первым в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsFirstSibling()
			{
				if (ParentElement != null)
				{
					mThisTransform.SetAsFirstSibling();
					Int32 i = mThisTransform.GetSiblingIndex();
					Single depth = -i - 1f;
					mThisTransform.localPosition = new Vector3(mThisTransform.localPosition.x, mThisTransform.localPosition.y, depth);
				}
				else
				{
					mThisTransform.position = new Vector3(mThisTransform.position.x,
						mThisTransform.position.y, LotusSpriteDispatcher.MaxPositionDepth - 0.5f);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента последним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsLastSibling()
			{
				if (ParentElement != null)
				{
					mThisTransform.SetAsLastSibling();
					Int32 i = mThisTransform.GetSiblingIndex();
					Single depth = -i - 1f;
					mThisTransform.localPosition = new Vector3(mThisTransform.localPosition.x, mThisTransform.localPosition.y, depth);
				}
				else
				{
					mThisTransform.position = new Vector3(mThisTransform.position.x,
						mThisTransform.position.y, LotusSpriteDispatcher.MinPositionDepth + 0.5f);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента предпоследним в иерархии родительской области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetAsPreLastSibling()
			{
				if (ParentElement != null)
				{
					//mThisTransform.position = new Vector3(mThisTransform.localPosition.x,
					//	mThisTransform.localPosition.y, -mThisTransform.parent.childCount + 1);
				}
				else
				{
					mThisTransform.position = new Vector3(mThisTransform.position.x,
						mThisTransform.position.y, LotusSpriteDispatcher.MinPositionDepth + 1.5f);
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка компонента трансформации в качестве родительского
			/// </summary>
			/// <param name="parent">Компонент трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetParent(Transform parent)
			{
				if (parent != null)
				{
					// Сохраняем предварительно положение
					Single top = this.Top;
					Single left = this.Left;

					mThisTransform.SetParent(parent, false);
					mParent = parent.GetComponentFromParentOrHisParent<LotusSpritePlaceable2D>();
					Left = left;
					Top = top;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка компонента трансформации в качестве дочернего
			/// </summary>
			/// <param name="rect_child">Компонент трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetChild(Transform rect_child)
			{
				if (rect_child != null)
				{
					rect_child.SetParent(mThisTransform);
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

			#region ======================================= МЕТОДЫ LocalTransformFromLocalDesign ======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка локальной позиции трансформации посредством локальных координат
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetLocalTransformFromLocalDesignX()
			{
				Single correct_x = (WidthScreen - ParentWidthScreen) / LotusSpriteDispatcher.CameraPixelsPerUnit;
				Single pos_x = (mRectLocalDesign.x / LotusSpriteDispatcher.CameraPixelsPerUnit * LotusSpriteDispatcher.ScaledScreenX) + correct_x / 2;
				mThisTransform.localPosition = new Vector3(pos_x, mThisTransform.localPosition.y, mThisTransform.localPosition.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка локальной позиции трансформации посредством локальных координат
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetLocalTransformFromLocalDesignY()
			{
				Single correct_y = (HeightScreen - ParentHeightScreen) / LotusSpriteDispatcher.CameraPixelsPerUnit;
				Single pos_y = (mRectLocalDesign.y / LotusSpriteDispatcher.CameraPixelsPerUnit * LotusSpriteDispatcher.ScaledScreenY) + correct_y / 2;
				mThisTransform.localPosition = new Vector3(mThisTransform.localPosition.x, -pos_y, mThisTransform.localPosition.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка локальной позиции трансформации посредством локальных координат
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetLocalTransformFromLocalDesign()
			{
				Single correct_x = (WidthScreen - ParentWidthScreen) / LotusSpriteDispatcher.CameraPixelsPerUnit;
				Single correct_y = (HeightScreen - ParentHeightScreen) / LotusSpriteDispatcher.CameraPixelsPerUnit;

				Single pos_x = (mRectLocalDesign.x / LotusSpriteDispatcher.CameraPixelsPerUnit * LotusSpriteDispatcher.ScaledScreenX) + correct_x / 2;
				Single pos_y = (mRectLocalDesign.y / LotusSpriteDispatcher.CameraPixelsPerUnit * LotusSpriteDispatcher.ScaledScreenY) + correct_y / 2;

				mThisTransform.localPosition = new Vector3(pos_x, -pos_y, mThisTransform.localPosition.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка локального масштаба трансформации посредством локальных размеров
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetLocalScaleFromLocalDesignWidth()
			{
				// Актульная ширина
				if (mSpriteRenderer.drawMode == SpriteDrawMode.Simple)
				{
					Single actual_w = mRectLocalDesign.width / mSpriteRenderer.sprite.rect.width * LotusSpriteDispatcher.ScaledScreenX;
					transform.localScale = new Vector3(actual_w, transform.localScale.y, transform.localScale.z);
				}
				else
				{
					Single actual_w = mRectLocalDesign.width / LotusSpriteDispatcher.CameraPixelsPerUnit * LotusSpriteDispatcher.ScaledScreenX;
					mSpriteRenderer.size = new Vector2(actual_w, mSpriteRenderer.size.y);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка локального масштаба трансформации посредством локальных размеров
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetLocalScaleFromLocalDesignHeight()
			{
				// Актульная высота
				if (mSpriteRenderer.drawMode == SpriteDrawMode.Simple)
				{
					Single actual_h = mRectLocalDesign.height / mSpriteRenderer.sprite.rect.height * LotusSpriteDispatcher.ScaledScreenY;
					transform.localScale = new Vector3(transform.localScale.x, actual_h, transform.localScale.z);
				}
				else
				{
					Single actual_h = mRectLocalDesign.height / LotusSpriteDispatcher.CameraPixelsPerUnit * LotusSpriteDispatcher.ScaledScreenY;
					mSpriteRenderer.size = new Vector2(mSpriteRenderer.size.x, actual_h);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка локального масштаба трансформации посредством локальных размеров
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetLocalScaleFromLocalDesign()
			{
				// Актульные размеры
				if (mSpriteRenderer.drawMode == SpriteDrawMode.Simple)
				{
					Single actual_w = mRectLocalDesign.width / mSpriteRenderer.sprite.rect.width * LotusSpriteDispatcher.ScaledScreenX;
					Single actual_h = mRectLocalDesign.height / mSpriteRenderer.sprite.rect.height * LotusSpriteDispatcher.ScaledScreenY;
					transform.localScale = new Vector3(actual_w, actual_h, transform.localScale.z);
				}
				else
				{
					Single actual_w = mRectLocalDesign.width / LotusSpriteDispatcher.CameraPixelsPerUnit * LotusSpriteDispatcher.ScaledScreenX;
					Single actual_h = mRectLocalDesign.height / LotusSpriteDispatcher.CameraPixelsPerUnit * LotusSpriteDispatcher.ScaledScreenY;
					mSpriteRenderer.size = new Vector2(actual_w, actual_h);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ LocalDesignFromLocalTransform ======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение локальной позиции посредством локальной трансформации
			/// </summary>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Vector2 GetLocalDesignFromLocalTransform()
			{
				Single correct_x = (WidthScreen - ParentWidthScreen) / LotusSpriteDispatcher.CameraPixelsPerUnit;
				Single correct_y = (HeightScreen - ParentHeightScreen) / LotusSpriteDispatcher.CameraPixelsPerUnit;

				Single pos_x = mThisTransform.localPosition.x;
				Single pos_y = -mThisTransform.localPosition.y;

				Single x = (pos_x - correct_x / 2) * LotusSpriteDispatcher.CameraPixelsPerUnit / LotusSpriteDispatcher.ScaledScreenX;
				Single y = (pos_y - correct_y / 2) * LotusSpriteDispatcher.CameraPixelsPerUnit / LotusSpriteDispatcher.ScaledScreenY;
				return (new Vector2(x, y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка локальной позиции посредством локальной трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetLocalDesignFromLocalTransform()
			{
				Single correct_x = (WidthScreen - ParentWidthScreen) / LotusSpriteDispatcher.CameraPixelsPerUnit;
				Single correct_y = (HeightScreen - ParentHeightScreen) / LotusSpriteDispatcher.CameraPixelsPerUnit;

				Single pos_x = mThisTransform.localPosition.x;
				Single pos_y = -mThisTransform.localPosition.y;

				mRectLocalDesign.x = (pos_x - correct_x / 2) * LotusSpriteDispatcher.CameraPixelsPerUnit / LotusSpriteDispatcher.ScaledScreenX;
				mRectLocalDesign.y = (pos_y - correct_y / 2) * LotusSpriteDispatcher.CameraPixelsPerUnit / LotusSpriteDispatcher.ScaledScreenY;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка локальных размеров посредством локального масштаба трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetLocalDesignFromLocalScale()
			{
				if (mSpriteRenderer.drawMode == SpriteDrawMode.Simple)
				{
					mRectLocalDesign.width = (mSpriteRenderer.sprite.rect.width * transform.localScale.x) / LotusSpriteDispatcher.ScaledScreenX;
					mRectLocalDesign.height = (mSpriteRenderer.sprite.rect.height * transform.localScale.y) / LotusSpriteDispatcher.ScaledScreenY;
				}
				else
				{
					mRectLocalDesign.width = (mSpriteRenderer.size.x * LotusSpriteDispatcher.CameraPixelsPerUnit) / LotusSpriteDispatcher.ScaledScreenX;
					mRectLocalDesign.height = (mSpriteRenderer.size.y * LotusSpriteDispatcher.CameraPixelsPerUnit) / LotusSpriteDispatcher.ScaledScreenY;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОДПРОГРАММ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма плавного скрытия элемента
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator HideLerpCoroutine(Single duration)
			{
				Single time = 0;
				Single start_time = 0;
				Single current_alpha = mSpriteRenderer.color.a;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					mSpriteRenderer.SetAlphaColor(current_alpha - time);
					yield return null;
				}

				mSpriteRenderer.SetAlphaColor(0.0f);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма плавного показа элемента
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator ShowLerpCoroutine(Single duration)
			{
				Single time = 0;
				Single start_time = mSpriteRenderer.color.a;
				if (Mathf.Approximately(start_time, 0))
				{
					start_time = 0;
				}
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					mSpriteRenderer.SetAlphaColor(time);
					yield return null;
				}

				mSpriteRenderer.SetAlphaColor(1.0f);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения альфы компоненты цвета спрайта
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			/// <param name="target_alpha">Целевое значение</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator AlphaLerpCoroutine(Single duration, Single target_alpha)
			{
				Single time = 0;
				Single start_time = 0;
				Single current_alpha = mSpriteRenderer.color.a;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					mSpriteRenderer.SetAlphaColor(Mathf.Lerp(current_alpha, target_alpha, time));
					yield return null;
				}

				mSpriteRenderer.SetAlphaColor(target_alpha);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения цвета спрайта
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			/// <param name="target_color">Целевое значение</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator ColorLerpCoroutine(Single duration, Color target_color)
			{
				Single time = 0;
				Single start_time = 0;
				Color current_color = mSpriteRenderer.color;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					mSpriteRenderer.color = Color.Lerp(current_color, target_color, time);
					yield return null;
				}

				mSpriteRenderer.color = target_color;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма изменения размеров элемента
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			/// <param name="target_size">Целевое значение</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator SizeLerpCoroutine(Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_size = mRectLocalDesign.size;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					mRectLocalDesign.size = Vector2.Lerp(current_size, target_size, time);
					SetLocalScaleFromLocalDesign();
					yield return null;
				}

				mRectLocalDesign.size = target_size;
				SetLocalScaleFromLocalDesign();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================