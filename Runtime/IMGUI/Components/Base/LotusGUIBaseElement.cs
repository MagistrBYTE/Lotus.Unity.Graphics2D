//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Компоненты IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseElement.cs
*		Компонент представляющий базовый элемент интерфейса модуля IMGUI Unity.
*		Реализация компонента базового элемента обеспечивающего начальную инфраструктуру для построения элементов интерфейса.
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
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DImmedateGUIComponent Компоненты IMGUI Unity
		//! Компонентная оболочка для подсистемы рисования IMGUI Unity. Реализация компонентов которые обеспечивают
		//! представления различных элементов интерфейса пользователя с помощью подсистемы рисования IMGUI Unity.
		//! \ingroup Unity2DImmedateGUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент представляющий базовый элемент интерфейса подсистемы IMGUI Unity
		/// </summary>
		/// <remarks>
		/// Реализация компонента базового элемента обеспечивающего начальную инфраструктуру для построения элементов интерфейса
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XGUIEditorSettings.MenuPath + "Base Element")]
		public class LotusGUIBaseElement : MonoBehaviour, ILotusBaseElement, ILotusDataExchange, ILotusPresentationBackcolor,
			IComparable<ILotusBaseElement>, IComparable<LotusGUIBaseElement>, IComparable<CGUIBaseElement>
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание базового элемента
			/// </summary>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusGUIBaseElement CreateElement()
			{
				// 1) Создание объекта
				GameObject go = new GameObject("GUIBaseElement");
				LotusGUIBaseElement element = go.AddComponent<LotusGUIBaseElement>();

				// 2) Конструктор элемента
				element.OnCreate();

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mUserTag = -1;
			[NonSerialized]
			private Boolean mIsDirty = false;

			// Параметры видимости
			[SerializeField]
			internal Int32 mVisibility;
			[SerializeField]
			internal Boolean mIsVisibleElement;
			[SerializeField]
			internal Single mOpacity;

			// Параметры визуального стиля
			[SerializeField]
			internal Int32 mStyleMainIndex;
			[SerializeField]
			internal String mStyleMainName;
			[NonSerialized]
			internal GUIStyle mStyleMain;
			[SerializeField]
			internal Boolean mUseBackground;
			[SerializeField]
			internal Color mBackgroundColor = Color.white;

			// Параметры размещения
			[SerializeField]
			internal Int32 mDepth;

			// Служебные данные
			[NonSerialized]
			internal Boolean mIsRegisterDispatcher = false;
			[NonSerialized]
			internal Rect mRectWorldScreenMain;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedElement;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Внутренний отступ слева области контента от края элемента
			/// </summary>
			public Single PaddingLeft
			{
				get { return StyleMain.padding.left; }
			}

			/// <summary>
			/// Внутренний отступ сверху области контента от края элемента
			/// </summary>
			public Single PaddingTop
			{
				get { return StyleMain.padding.top; }
			}

			/// <summary>
			/// Внутренний отступ справа области контента от края элемента
			/// </summary>
			public Single PaddingRight
			{
				get { return StyleMain.padding.right; }
			}

			/// <summary>
			/// Внутренний отступ снизу области контента от края элемента
			/// </summary>
			public Single PaddingBottom
			{
				get { return StyleMain.padding.bottom; }
			}

			/// <summary>
			/// Отступ слева границы элемента
			/// </summary>
			public Single BorderLeft
			{
				get { return StyleMain.border.left; }
			}

			/// <summary>
			/// Отступ сверху границы элемента
			/// </summary>
			public Single BorderTop
			{
				get { return StyleMain.border.top; }
			}

			/// <summary>
			/// Отступ справа границы элемента
			/// </summary>
			public Single BorderRight
			{
				get { return StyleMain.border.right; }
			}

			/// <summary>
			/// Отступ снизу границы элемента
			/// </summary>
			public Single BorderBottom
			{
				get { return StyleMain.border.bottom; }
			}

			//
			// ДОПОЛНИТЕЛЬНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус размещения элемента (регистрации) в списке диспетчера
			/// </summary>
			/// <remarks>
			/// Большинство элементов отображается непосредственно диспетчером(после их регистрации), однако некоторые элементы
			/// отображают сам свои дочерние элементы, поэтому чтобы не происходила двойное рисование этих
			/// элементов их надо, по необходимости исключать из диспетчера элементов <see cref="LotusGUIDispatcher"/>
			/// </remarks>
			public Boolean IsRegisterDispatcher
			{
				get { return mIsRegisterDispatcher; }
				set
				{
					if (mIsRegisterDispatcher != value)
					{
						mIsRegisterDispatcher = value;

						if (mIsRegisterDispatcher)
						{
							IncludeDispatcher();
						}
						else
						{
							ExludeDispatcher();
						}
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusBaseElement ================================
			/// <summary>
			/// Имя элемента
			/// </summary>
			/// <remarks>
			/// Должно быть уникально в пределах проекта
			/// </remarks>
			public String Name
			{
				get { return (name); }
				set { name = value; }
			}

			/// <summary>
			/// Тэг для определения пользовательских данных
			/// </summary>
			public Int32 UserTag
			{
				get { return mUserTag; }
				set
				{
					mUserTag = value;
				}
			}

			/// <summary>
			/// Статус обновления элемента
			/// </summary>
			public Boolean IsDirty
			{
				get { return mIsDirty; }
				set
				{
					mIsDirty = value;
					if (mIsDirty)
					{
						// Если мы обновляем элемент то должны в обязательном порядке проинформировать и диспетчер
						LotusGUIDispatcher.IsUpdated = true;
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusVisualStyle ================================
			/// <summary>
			/// Индекс используемого базового визуального стиля из хранилища
			/// </summary>
			public Int32 StyleMainIndex
			{
				get { return (mStyleMainIndex); }
				set
				{
					mStyleMainIndex = value;
				}
			}

			/// <summary>
			/// Имя используемого базового визуального стиля
			/// </summary>
			public String StyleMainName
			{
				get { return mStyleMainName; }
				set
				{
					mStyleMainName = value;
					mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				}
			}

			/// <summary>
			/// Основной базовый визуальный стиль для рисования элемента
			/// </summary>
			public GUIStyle StyleMain
			{
				get
				{
					if (mStyleMain == null)
					{
						mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
					}
					return mStyleMain;
				}
			}

			/// <summary>
			/// Использование общего фонового изображения
			/// </summary>
			public Boolean UseBackground
			{
				get { return (mUseBackground); }
				set
				{
					mUseBackground = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusVisibility =================================
			/// <summary>
			/// Видимость элемента
			/// </summary>
			/// <remarks>
			/// Определение видимости элемента пользователем. Используеся первый бит
			/// </remarks>
			public Boolean IsVisible
			{
				get { return !XPacked.UnpackBoolean(mVisibility, 0); }
				set
				{
					if (value)
					{
						XPacked.PackBoolean(ref mVisibility, 0, false);
					}
					else
					{
						XPacked.PackBoolean(ref mVisibility, 0, true);
					}

					SetVisibleElement();
				}
			}

			/// <summary>
			/// Битовое поле видимости элемента
			/// </summary>
			/// <remarks>
			/// Если установлены флаги значит элемент не видим, ноль элемент виден. Применяется для работы с фильтрами
			/// Можно задавать от четвертого бита
			/// </remarks>
			public Int32 Visibility
			{
				get { return mVisibility; }
				set
				{
					if (mVisibility != value)
					{
						mVisibility = value;
						SetVisibleElement();
					}
				}
			}

			/// <summary>
			/// Видимость непосредственно элемента
			/// </summary>
			/// <remarks>
			/// Определение видимости элемента. Зависит от фильтров и указания видимости пользователем
			/// </remarks>
			public Boolean IsVisibleSelf
			{
				get { return mVisibility == 0; }
			}

			/// <summary>
			/// Видимость элемента
			/// </summary>
			/// <remarks>
			/// Зависит как от статус самого элемента и так от статуса родительского элемента
			/// </remarks>
			public Boolean IsVisibleElement
			{
				get { return mIsVisibleElement; }
				set { }
			}

			/// <summary>
			/// Прозрачность элемента
			/// </summary>
			public Single Opacity
			{
				get { return mOpacity; }
				set { mOpacity = value; }
			}

			/// <summary>
			/// Фоновый цвет для рисования элемента
			/// </summary>
			public Color BackgroundColor
			{
				get { return mBackgroundColor; }
				set { mBackgroundColor = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusBasePlaceable2D ============================
			/// <summary>
			/// Позиция левого угла элемента по X в экранных координатах
			/// </summary>
			public Single LeftScreen
			{
				get { return mRectWorldScreenMain.x; }
				set
				{
					if (mRectWorldScreenMain.x != value)
					{
						mRectWorldScreenMain.x = value;
						UpdatePlacementFromAbsolute();
					}
				}
			}

			/// <summary>
			/// Позиция правого угла элемента по X в экранных координатах
			/// </summary>
			public Single RightScreen
			{
				get { return mRectWorldScreenMain.xMax; }
				set
				{
					if (mRectWorldScreenMain.xMax != value)
					{
						mRectWorldScreenMain.xMax = value;
						UpdatePlacementFromAbsolute();
					}
				}
			}

			/// <summary>
			/// Позиция верхнего угла элемента по Y в экранных координатах
			/// </summary>
			public Single TopScreen
			{
				get { return mRectWorldScreenMain.y; }
				set
				{
					if (mRectWorldScreenMain.y != value)
					{
						mRectWorldScreenMain.y = value;
						UpdatePlacementFromAbsolute();
					}
				}
			}

			/// <summary>
			/// Позиция нижнего угла элемента по Y в экранных координатах
			/// </summary>
			public Single BottomScreen
			{
				get { return mRectWorldScreenMain.yMax; }
				set
				{
					if (mRectWorldScreenMain.yMax != value)
					{
						mRectWorldScreenMain.yMax = value;
						UpdatePlacementFromAbsolute();
					}
				}
			}

			/// <summary>
			/// Позиция верхнего-левого угла элемента в экранных координатах
			/// </summary>
			public Vector2 LocationScreen
			{
				get { return new Vector2(mRectWorldScreenMain.x, mRectWorldScreenMain.y); }
				set
				{
					if (mRectWorldScreenMain.position != value)
					{
						mRectWorldScreenMain.x = value.x;
						mRectWorldScreenMain.y = value.y;
						UpdatePlacementFromAbsolute();
					}
				}
			}

			/// <summary>
			///  Ширина(размер по X) элемента
			/// </summary>
			public Single WidthScreen
			{
				get
				{
					return mRectWorldScreenMain.width;
				}
				set
				{
					if (mRectWorldScreenMain.width != value)
					{
						mRectWorldScreenMain.width = value;
						UpdatePlacementFromAbsolute();
					}
				}
			}

			/// <summary>
			/// Высота(размер по Y) элемента
			/// </summary>
			public Single HeightScreen
			{
				get
				{
					return mRectWorldScreenMain.height;
				}
				set
				{
					if (mRectWorldScreenMain.height != value)
					{
						mRectWorldScreenMain.height = value;
						UpdatePlacementFromAbsolute();
					}
				}
			}

			/// <summary>
			/// Размеры элемента в экранных координатах
			/// </summary>
			public Vector2 SizeScreen
			{
				get { return new Vector2(mRectWorldScreenMain.width, mRectWorldScreenMain.height); }
				set
				{
					if (mRectWorldScreenMain.size != value)
					{
						mRectWorldScreenMain.width = value.x;
						mRectWorldScreenMain.height = value.y;
						UpdatePlacementFromAbsolute();
					}
				}
			}

			/// <summary>
			/// Прямоугольника элемента в экранных координатах
			/// </summary>
			public Rect RectScreen
			{
				get
				{
					return mRectWorldScreenMain;
				}
			}

			/// <summary>
			/// Глубина элемента интерфейса (влияет на последовательность прорисовки)
			/// </summary>
			public Int32 Depth
			{
				get { return mDepth; }
				set
				{
					if (mDepth != value)
					{
						mDepth = value;
#if UNITY_EDITOR
						if (UnityEditor.EditorApplication.isPlaying)
						{
							LotusGUIDispatcher.SortElements();
						}
#else
						LotusGUIDispatcher.SortElements();
#endif
					}
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				OnCreate();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				OnReset();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
				IncludeDispatcher();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отключение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDisable()
			{
				ExludeDispatcher();
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
			public Int32 CompareTo(ILotusBaseElement other)
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
			public Int32 CompareTo(LotusGUIBaseElement other)
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
			public Int32 CompareTo(CGUIBaseElement other)
			{
				return (this.Depth.CompareTo(other.Depth));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return (name);
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusVisualStyle ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление визуального стиля элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateVisualStyle()
			{
				mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование визуального стиля элемента с другого элемента
			/// </summary>
			/// <param name="source">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CopyVisualStyle(ILotusVisualStyle source)
			{
				mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров отображения элемента по связанному стилю
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetFromOriginalStyle()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusVisibility ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка видимости элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetVisibleElement()
			{
				mIsVisibleElement = IsVisibleSelf;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка флага видимости. Если флаг установлен то элемент невидим
			/// </summary>
			/// <remarks>
			/// Номера флагов
			/// 0 - Обозначение что отображения зависит от установки пользователя
			/// 1 - Обозначение что отображения зависит от родителя
			/// 2 - Обозначение что отображения зависит от структруной части родителя
			/// 3 - Обозначение что отображения зависит от фильтра
			/// </remarks>
			/// <param name="number">Номер флага</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetVisibilityFlags(Int32 number)
			{
				XPacked.PackBoolean(ref mVisibility, number, true);
				SetVisibleElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка флага видимости
			/// </summary>
			/// <param name="number">Номер флага</param>
			//---------------------------------------------------------------------------------------------------------
			public void ClearVisibilityFlags(Int32 number)
			{
				XPacked.PackBoolean(ref mVisibility, number, false);
				SetVisibleElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавное скрытие элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Hide()
			{
				StartCoroutine(HideElementCoroutine());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный показ элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Show()
			{
				StartCoroutine(ShowElementCoroutine());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма скрытия элемента
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator HideElementCoroutine()
			{
				Single time = 0;
				Single start_time = 0;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / 0.4f;
					mOpacity = Mathf.Lerp(1, 0, time);
					yield return null;
				}

				mOpacity = 0;
				IsVisible = false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма показа элемента
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator ShowElementCoroutine()
			{
				Single time = 0;
				Single start_time = 0;
				IsVisible = true;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / 0.4f;
					mOpacity = Mathf.Lerp(0, 1, time);
					yield return null;
				}

				mOpacity = 1;
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
				return mRectWorldScreenMain.Contains(point);
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
			/// Основной метод определяющий положение и размер элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdatePlacement()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции и размеров элемента по абсолютным данным
			/// </summary>
			/// <remarks>
			/// На основании абсолютной позиции элемент в экранных координатах считается его относительная позиция
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdatePlacementFromAbsolute()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение области для размещения дочерних элементов
			/// </summary>
			/// <returns>Прямоугольник области для размещения дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Rect GetChildRectContent()
			{
				return mRectWorldScreenMain;
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
			public virtual void SetData(String text, Texture2D icon)
			{
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
			public virtual void OnReset()
			{
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnUpdate()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnDraw()
			{
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
			public virtual void OnCreate()
			{
				mIsVisibleElement = true;
				mOpacity = 1.0f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавления элемента с список элементов диспетчера (регистрация)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void IncludeDispatcher()
			{
				LotusGUIDispatcher.RegisterElement(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Извлечение элемента со списока элементов диспетчера (отмена регистрации)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void ExludeDispatcher()
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
			public virtual LotusGUIBaseElement Duplicate()
			{
				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CopyFrom(LotusGUIBaseElement base_element)
			{
				if (base_element != null)
				{
					mVisibility = base_element.mVisibility;
					mIsVisibleElement = base_element.mIsVisibleElement;
					mRectWorldScreenMain = base_element.mRectWorldScreenMain;
					mStyleMainName = base_element.mStyleMainName;
					mStyleMain = base_element.mStyleMain;
					mDepth = base_element.mDepth;
					mUserTag = base_element.mUserTag;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КОНТЕНТОМ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ComputeSizeFromContent()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ComputeWidthFromContent()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ComputeHeightFromContent()
			{

			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================