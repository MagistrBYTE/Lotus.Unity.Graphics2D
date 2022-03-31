//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBase.cs
*		Базовые элементы интерфейса пользователя.
*		Реализация базовых элементов интерфейса пользователя обеспечивающих представление информации без прямого 
*	взаимодействия с пользователем.
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
		//! \defgroup Unity2DImmedateGUIBase Базовые элементы интерфейса
		//! Базовые элементы интерфейса предназначены для отображения информации без взаимодействия с пользователем. 
		//! Реализованы базовые элементы, элементы с заголовочной областью и элементы индикации
		//! \ingroup Unity2DImmedateGUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый элемент GUI
		/// </summary>
		/// <remarks>
		/// Базовый элемент представляет собой базовую инфраструктуру для построения всех остальных элементов интерфейса
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIBaseElement : ILotusBaseElement, ILotusDataExchange, IComparable<ILotusBaseElement>, 
			IComparable<CGUIBaseElement>
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений
			/// </summary>
			/// <param name="a">Первое значение</param>
			/// <param name="b">Второе значение</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(Single a, Single b, Single epsilon = 0.001f)
			{
				if (Math.Abs(a - b) < epsilon)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Округление до нужного целого
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="round">Степень округления</param>
			/// <returns>Округленное значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single RoundToNearest(Single value, Int32 round)
			{
				if (value >= 0)
				{
					return (Single)(Math.Floor((value + (Single)round / 2) / round) * round);
				}
				else
				{
					return (Single)(Math.Ceiling((value - (Single)round / 2) / round) * round);
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mName;
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

			// Параметры размещения
			[SerializeField]
			internal Int32 mDepth;

			// Служебные данные
			[NonSerialized]
			internal Rect mRectWorldScreenMain;
			[NonSerialized]
			internal Boolean mIsRegisterDispatcher = false;
			#endregion

			#region ======================================= СВОЙСТВА ILotusBaseElement ================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя элемента
			/// </summary>
			/// <remarks>
			/// Должно быть уникально в пределах проекта
			/// </remarks>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
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

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseElement()
			{
				mIsVisibleElement = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseElement(String name)
			{
				mName = name;
				mIsVisibleElement = true;
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
				return (mName);
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
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование визуального стиля элемента с другого элемента
			/// </summary>
			/// <param name="source">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CopyVisualStyle(ILotusVisualStyle source)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров отображения элемента по связанному стилю
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetFromOriginalStyle()
			{
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
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

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный показ элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Show()
			{

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
			public virtual CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUIBaseElement;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CopyFrom(CGUIBaseElement base_element)
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
		/// <summary>
		/// Основной элемент GUI
		/// </summary>
		/// <remarks>
		/// Основной элемент представляет собой инфраструктуру для построения полноценых адаптивных элементов интерфейса
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIElement : CGUIBaseElement, ILotusElement, IComparable<CGUIElement>
		{
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

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIElement()
			{
				mName = this.GetType().Name.Replace("CGUI", "") + "_" + this.GetHashCode().ToString();
				mIsEnabled = true;
				mIsVisibleElement = true;
				mIsEnabledElement = true;
				mRectLocalDesignMain.x = 20;
				mRectLocalDesignMain.y = 20;
				mRectLocalDesignMain.width = 120;
				mRectLocalDesignMain.height = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIElement(String name)
			{
				mName = name;
				mIsEnabled = true;
				mIsVisibleElement = true;
				mIsEnabledElement = true;
				mRectLocalDesignMain.x = 20;
				mRectLocalDesignMain.y = 20;
				mRectLocalDesignMain.width = 120;
				mRectLocalDesignMain.height = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIElement(String name, Single x, Single y)
			{
				mName = name;
				mRectLocalDesignMain.x = x;
				mRectLocalDesignMain.y = y;
				mRectLocalDesignMain.width = 120;
				mRectLocalDesignMain.height = 30;
				mIsEnabled = true;
				mIsVisibleElement = true;
				mIsEnabledElement = true;
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
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
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
				if (parent == this) return;

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

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUIElement;
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

				CGUIElement element = base_element as CGUIElement;
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
		/// <summary>
		/// Элемент GUI содержащий базовый контент: текст и иконку изображения
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Label
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIContentElement : CGUIElement
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента по указанным параметрам и регистрация его в диспетчере
			/// </summary>
			/// <remarks>
			/// Если элемент с таким имением существует то происходит не создание, а обновление параметров
			/// </remarks>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			/// <returns>Cозданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIContentElement Create(String name, Single x, Single y, String text)
			{
				return Create(name, x, y, 120, 30, text, "Label");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента по указанным параметрам и регистрация его в диспетчере
			/// </summary>
			/// <remarks>
			/// Если элемент с таким имением существует то происходит не создание, а обновление параметров
			/// </remarks>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="style_name">Стиль элемента</param>
			/// <returns>Cозданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIContentElement Create(String name, Single x, Single y, Single width, Single height,
				String text, String style_name)
			{
				CGUIContentElement element = null;

				// Ищем элемент по имени
				element = LotusGUIDispatcher.GetElement(name) as CGUIContentElement;

				// Если не нашли то создаем
				if (element == null)
				{
					element = new CGUIContentElement(name, x, y, text, style_name);
					element.mRectLocalDesignMain = new Rect(x, y, width, height);

					// Добавляем
					LotusGUIDispatcher.RegisterElement(element);

					// Есть регистрация в диспетчере
					element.mIsRegisterDispatcher = true;
				}
				else
				{
					element.Text = text;
					element.StyleMainName = style_name;
					element.SetFromLocalDesign(x, y, width, height);
				}

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mText;
			[SerializeField]
			internal Texture2D mIcon;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Основной текст элемента
			/// </summary>
			public String Text
			{
				get { return mText; }
				set { mText = value; }
			}

			/// <summary>
			/// Текстура иконки элемента
			/// </summary>
			public Texture2D Icon
			{
				get { return mIcon; }
				set { mIcon = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentElement()
				: base()
			{
				mStyleMainName = "Label";
				mText = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentElement(String name)
				: base(name)
			{
				mStyleMainName = "Label";
				mText = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentElement(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Label";
				mText = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentElement(String name, Single x, Single y, String text)
				: base(name, x, y)
			{
				mStyleMainName = "Label";
				mText = text;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="style_name">Имя стиля элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIContentElement(String name, Single x, Single y, String text, String style_name)
				: base(name, x, y)
			{
				mStyleMainName = style_name;
				mText = text;
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
			public override void SetData(String text, Texture2D icon)
			{
				mText = text;
				mIcon = icon;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				LotusGUIDispatcher.CurrentContent.text = mText;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				GUI.Label(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleMain);
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
				return MemberwiseClone() as CGUIContentElement;
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

				CGUIContentElement element = base_element as CGUIContentElement;
				if (element != null)
				{
					mText = element.mText;
					mIcon = element.mIcon;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КОНТЕНТОМ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeSizeFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = Text;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				Vector2 size = style.CalcSize(LotusGUIDispatcher.CurrentContent);
				mRectLocalDesignMain.width = RoundToNearest(size.x, 10);
				mRectLocalDesignMain.height = RoundToNearest(size.y, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeWidthFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mText;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				Single min_width = 0, max_width = 0;
				style.CalcMinMaxWidth(LotusGUIDispatcher.CurrentContent, out min_width, out max_width);
				mRectLocalDesignMain.width = RoundToNearest((min_width + max_width) / 2, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeHeightFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mText;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				Single height = style.CalcHeight(LotusGUIDispatcher.CurrentContent, mRectLocalDesignMain.width - (PaddingLeft + PaddingRight));
				mRectLocalDesignMain.height = RoundToNearest(height, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeSizeFromContent()
			{
				ComputeSizeFromContent(mStyleMain);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeWidthFromContent()
			{
				ComputeWidthFromContent(mStyleMain);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeHeightFromContent()
			{
				ComputeHeightFromContent(mStyleMain);
			}
			#endregion

		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Простая надпись - элемент GUI содержащий базовый контент: текст с поддержкой локализации и иконку изображения
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Label
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUILabel : CGUIElement
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента по указанным параметрам и регистрация его в диспетчере
			/// </summary>
			/// <remarks>
			/// Если элемент с таким имением существует то происходит не создание, а обновление параметров
			/// </remarks>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			/// <returns>Cозданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUILabel CreateLabel(String name, Single x, Single y, String text)
			{
				return CreateLabel(name, x, y, 120, 30, text, "Label");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента по указанным параметрам и регистрация его в диспетчере
			/// </summary>
			/// <remarks>
			/// Если элемент с таким имением существует то происходит не создание, а обновление параметров
			/// </remarks>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="style_name">Стиль элемента</param>
			/// <returns>Cозданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUILabel CreateLabel(String name, Single x, Single y, Single width, Single height,
				String text, String style_name)
			{
				CGUILabel element = null;

				// Ищем элемент по имени
				element = LotusGUIDispatcher.GetElement(name) as CGUILabel;

				// Если не нашли то создаем
				if (element == null)
				{
					element = new CGUILabel(name, x, y, text);
					element.mStyleMainName = style_name;
					element.mRectLocalDesignMain = new Rect(x, y, width, height);

					// Добавляем
					LotusGUIDispatcher.RegisterElement(element);

					// Есть регистрация в диспетчере
					element.mIsRegisterDispatcher = true;
				}
				else
				{
					element.CaptionText = text;
					element.StyleMainName = style_name;
					element.SetFromLocalDesign(x, y, width, height);
				}

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal TLocalizableText mCaptionText;
			[SerializeField]
			internal Texture2D mCaptionIcon;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Основной текст элемента
			/// </summary>
			public String CaptionText
			{
				get { return mCaptionText.Text; }
				set { mCaptionText.Text = value; }
			}

			/// <summary>
			/// Ключ локализации текста элемента
			/// </summary>
			public Int32 IDKeyLocalize
			{
				get { return mCaptionText.IDKeyLocalize; }
				set { mCaptionText.IDKeyLocalize = value; }
			}

			/// <summary>
			/// Статус локализации текста элемента
			/// </summary>
			public Boolean IsLocalize
			{
				get { return mCaptionText.IsLocalize; }
			}

			/// <summary>
			/// Текстура иконки элемента
			/// </summary>
			public Texture2D CaptionIcon
			{
				get { return mCaptionIcon; }
				set { mCaptionIcon = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUILabel()
				: base()
			{
				mStyleMainName = "Label";
				mCaptionText = new TLocalizableText("");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUILabel(String name)
				: base(name)
			{
				mStyleMainName = "Label";
				mCaptionText = new TLocalizableText("");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUILabel(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Label";
				mCaptionText = new TLocalizableText("");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUILabel(String name, Single x, Single y, String text)
				: base(name, x, y)
			{
				mStyleMainName = "Label";
				mCaptionText = new TLocalizableText(text);
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
			public override void SetData(String text, Texture2D icon)
			{
				mCaptionText.Text = text;
				mCaptionIcon = icon;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента GUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;
				GUI.Label(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleMain);
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
				return MemberwiseClone() as CGUILabel;
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

				CGUILabel label = base_element as CGUILabel;
				if (label != null)
				{
					mCaptionText = label.mCaptionText;
					mCaptionIcon = label.mCaptionIcon;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КОНТЕНТОМ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeSizeFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				Vector2 size = style.CalcSize(LotusGUIDispatcher.CurrentContent);
				mRectLocalDesignMain.width = RoundToNearest(size.x, 10);
				mRectLocalDesignMain.height = RoundToNearest(size.y, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeWidthFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				Single min_width = 0, max_width = 0;
				style.CalcMinMaxWidth(LotusGUIDispatcher.CurrentContent, out min_width, out max_width);
				mRectLocalDesignMain.width = RoundToNearest((min_width + max_width) / 2, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeHeightFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				Single height = style.CalcHeight(LotusGUIDispatcher.CurrentContent, mRectLocalDesignMain.width - (PaddingLeft + PaddingRight));
				mRectLocalDesignMain.height = RoundToNearest(height, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeSizeFromContent()
			{
				ComputeSizeFromContent(StyleMain);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeWidthFromContent()
			{
				ComputeWidthFromContent(StyleMain);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeHeightFromContent()
			{
				ComputeHeightFromContent(StyleMain);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Панель - элемент GUI предназначенный в основном для размещения и группирования других элементов
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Box
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIPanel : CGUILabel
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента по указанным параметрам и регистрация его в диспетчере
			/// </summary>
			/// <remarks>
			/// Если элемент с таким имением существует то происходит не создание, а обновление параметров
			/// </remarks>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			/// <returns>Cозданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIPanel CreatePanel(String name, Single x, Single y, String text)
			{
				return CreatePanel(name, x, y, 120, 30, text, "Box");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента по указанным параметрам и регистрация его в диспетчере
			/// </summary>
			/// <remarks>
			/// Если элемент с таким имением существует то происходит не создание, а обновление параметров
			/// </remarks>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="width">Ширина элемента</param>
			/// <param name="height">Высота элемента</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="style_name">Стиль элемента</param>
			/// <returns>Cозданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIPanel CreatePanel(String name, Single x, Single y, Single width, Single height,
				String text, String style_name)
			{
				CGUIPanel element = null;

				// Ищем элемент по имени
				element = LotusGUIDispatcher.GetElement(name) as CGUIPanel;

				// Если не нашли то создаем
				if (element == null)
				{
					element = new CGUIPanel(name, x, y, text);
					element.mStyleMainName = style_name;
					element.mRectLocalDesignMain = new Rect(x, y, width, height);

					// Добавляем
					element.mAspectMode = TAspectMode.Proportional;
					LotusGUIDispatcher.RegisterElement(element);

					// Есть регистрация в диспетчере
					element.mIsRegisterDispatcher = true;
				}
				else
				{
					element.CaptionText = text;
					element.StyleMainName = style_name;
					element.SetFromLocalDesign(x, y, width, height);
				}

				return element;
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanel()
				: base()
			{
				mStyleMainName = "Box";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanel(String name)
				: base(name)
			{
				mStyleMainName = "Box";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanel(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Box";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIPanel(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mStyleMainName = "Box";
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				GUI.Box(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleMain);
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
				return MemberwiseClone() as CGUIPanel;
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
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Надпись со значением - элемент GUI содержащий базовый контент: текст и иконку изображения и дополнительный
		/// контент значения
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Label.
		/// Можно задать местоположение области значения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUILabelValue : CGUILabel
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mValueText;
			[SerializeField]
			internal Texture2D mValueIcon;

			// Параметры размещения
			[SerializeField]
			internal Single mValueSize;
			[SerializeField]
			internal TValueLocation mValueLocation;

			// Параметры визуального стиля
			[SerializeField]
			internal String mStyleValueName;
			[NonSerialized]
			internal GUIStyle mStyleValue;

			// Служебные данные
			[NonSerialized]
			internal Single mValueSizeCurrent;
			[NonSerialized]
			internal Rect mRectWorldScreenValue;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Текст значения элемента
			/// </summary>
			public String ValueText
			{
				get { return mValueText; }
				set { mValueText = value; }
			}

			/// <summary>
			/// Текстура значения иконки элемента
			/// </summary>
			public Texture2D ValueIcon
			{
				get { return mValueIcon; }
				set { mValueIcon = value; }
			}

			//
			// ПАРАМЕТРЫ ВИЗУАЛЬНОГО СТИЛЯ
			//
			/// <summary>
			/// Имя стиля для рисования значения элемента
			/// </summary>
			public String StyleValueName
			{
				get { return mStyleValueName; }
				set
				{
					if (mStyleValueName != value)
					{
						mStyleValueName = value;
						mStyleValue = LotusGUIDispatcher.FindStyle(mStyleValueName);
					}
				}
			}

			/// <summary>
			/// Стиль для рисования значения элемента
			/// </summary>
			public GUIStyle StyleValue
			{
				get
				{
					if (mStyleValue == null)
					{
						mStyleValue = LotusGUIDispatcher.FindStyle(mStyleValueName);
					}
					return mStyleValue;
				}
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Размер области значения
			/// </summary>
			public Single ValueSize
			{
				get { return mValueSize; }
				set
				{
					if (mValueSize != value)
					{
						mValueSize = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Позиция области значения
			/// </summary>
			public TValueLocation ValueLocation
			{
				get { return mValueLocation; }
				set
				{
					if (mValueLocation != value)
					{
						mValueLocation = value;
						UpdatePlacement();
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUILabelValue()
				: base()
			{
				mStyleValueName = "Label";
				mValueSize = 10;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUILabelValue(String name)
				: base(name)
			{
				mStyleValueName = "Label";
				mValueSize = 10;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUILabelValue(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleValueName = "Label";
				mValueSize = 10;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUILabelValue(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mStyleValueName = "Label";
				mValueSize = 10;
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

				switch (mValueLocation)
				{
					case TValueLocation.Right:
					case TValueLocation.Left:
						{
							mValueSizeCurrent = mValueSize * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TValueLocation.Top:
					case TValueLocation.Bottom:
						{
							mValueSizeCurrent = mValueSize * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}

				switch (mValueLocation)
				{
					case TValueLocation.Right:
						{
							mRectWorldScreenMain.width -= mValueSizeCurrent;

							mRectWorldScreenValue.x = mRectWorldScreenMain.xMax;
							mRectWorldScreenValue.y = mRectWorldScreenMain.y;
							mRectWorldScreenValue.width = mValueSizeCurrent;
							mRectWorldScreenValue.height = mRectWorldScreenMain.height;
						}
						break;
					case TValueLocation.Left:
						{
							mRectWorldScreenMain.x += mValueSizeCurrent;
							mRectWorldScreenMain.width -= mValueSizeCurrent;

							mRectWorldScreenValue.x = mRectWorldScreenMain.x - mValueSizeCurrent;
							mRectWorldScreenValue.y = mRectWorldScreenMain.y;
							mRectWorldScreenValue.width = mValueSizeCurrent;
							mRectWorldScreenValue.height = mRectWorldScreenMain.height;
						}
						break;
					case TValueLocation.Top:
						{
							mRectWorldScreenMain.y += mValueSizeCurrent;
							mRectWorldScreenMain.height -= mValueSizeCurrent;

							mRectWorldScreenValue.x = mRectWorldScreenMain.x;
							mRectWorldScreenValue.y = mRectWorldScreenMain.y - mValueSizeCurrent;
							mRectWorldScreenValue.width = mRectWorldScreenMain.width;
							mRectWorldScreenValue.height = mValueSizeCurrent;
						}
						break;
					case TValueLocation.Bottom:
						{
							mRectWorldScreenMain.height -= mValueSizeCurrent;

							mRectWorldScreenValue.x = mRectWorldScreenMain.x;
							mRectWorldScreenValue.y = mRectWorldScreenMain.yMax;
							mRectWorldScreenValue.width = mRectWorldScreenMain.width;
							mRectWorldScreenValue.height = mValueSizeCurrent;
						}
						break;
					default:
						break;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusVisualStyle ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров отображения элемента по связанному стилю
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetFromOriginalStyle()
			{
				mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				mStyleMainName = mStyleMain.name;

				mStyleValue = LotusGUIDispatcher.FindStyle(mStyleValueName);
				mStyleValueName = mStyleValue.name;
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
				if (mStyleValue == null) mStyleValue = LotusGUIDispatcher.FindStyle(mStyleValueName);
				this.UpdatePlacement();
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

				LotusGUIDispatcher.CurrentContent.text = mCaptionText.Text;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;
				GUI.Label(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleMain);

				LotusGUIDispatcher.CurrentContent.text = mValueText;
				LotusGUIDispatcher.CurrentContent.image = mValueIcon;
				GUI.Label(mRectWorldScreenValue, LotusGUIDispatcher.CurrentContent, mStyleValue);
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
				return MemberwiseClone() as CGUILabelValue;
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

				CGUILabelValue label_value = base_element as CGUILabelValue;
				if (label_value != null)
				{
					mValueText = label_value.mValueText;
					mValueIcon = label_value.mValueIcon;
					mValueSize = label_value.mValueSize;
					mValueLocation = label_value.mValueLocation;
					mStyleValueName = label_value.mStyleValueName;
					mStyleValue = label_value.mStyleValue;
					mValueSizeCurrent = label_value.mValueSizeCurrent;
					mRectWorldScreenValue = label_value.mRectWorldScreenValue;
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