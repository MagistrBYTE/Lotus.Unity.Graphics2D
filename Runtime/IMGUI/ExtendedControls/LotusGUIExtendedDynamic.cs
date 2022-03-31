//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIExtendedDynamic.cs
*		Динамические элементы интерфейса пользователя.
*		Реализация кратковременных динамических элементов интерфейса пользователя изменяющих свои параметры во времени
*	для различных визуальных эффектов.
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
		//! \addtogroup Unity2DImmedateGUIExtended
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый динамический элемента
		/// </summary>
		/// <remarks>
		/// Динамический элемент меняет соответствующие выбранные параметры во времени
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIBaseDynamic : CGUIBaseElement, ILotusPoolObject
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание динамического элемента с изменяемой позицией
			/// </summary>
			/// <param name="duration">Продолжительность изменения</param>
			/// <param name="location">Начальная позиция</param>
			/// <param name="delta_location">Изменение позиции</param>
			/// <returns>Созданный динамический элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIBaseDynamic Create(Single duration, Vector2 location, Vector2 delta_location)
			{
				// Создаем элемент
				CGUIBaseDynamic dynamic_element = new CGUIBaseDynamic("D1", location.x, location.y);

				// Устанавливаем основные параметры
				dynamic_element.IsVisible = true;
				dynamic_element.ChangeParam = TDynamicParam.Location;
				dynamic_element.Duration = duration;
				dynamic_element.LocationStarting = location;
				dynamic_element.LocationTarget = location + delta_location;

				// Добавляем в диспетчер для рисования
				LotusGUIDispatcher.RegisterElement(dynamic_element);
				dynamic_element.mIsRegisterDispatcher = true;

				// Запускаем анимацию параметров
				dynamic_element.StartAnimation();

				return dynamic_element;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание динамического элемента из префаба с изменяемой позицией
			/// </summary>
			/// <param name="prefab">Префаб элемета</param>
			/// <param name="location">Начальная позиция</param>
			/// <param name="delta_location">Изменение позиции</param>
			/// <returns>Созданный динамический элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIBaseDynamic CreateFromPrefab(CGUIBaseDynamic prefab, Vector2 location, Vector2 delta_location)
			{
				// Создаем элемент из префаба
				CGUIBaseDynamic dynamic_element = prefab.Duplicate() as CGUIBaseDynamic;

				// Устанавливаем основные параметры
				dynamic_element.LocationStarting = location;
				dynamic_element.LocationTarget = location + delta_location;

				// Добавляем в диспетчер для рисования
				dynamic_element.mIsRegisterDispatcher = true;
				LotusGUIDispatcher.RegisterElement(dynamic_element);

				// Запускаем анимацию параметров
				dynamic_element.StartAnimation();

				return dynamic_element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal TDynamicParam mChangeParam;
			[SerializeField]
			internal Single mDuration;
			[SerializeField]
			internal TEasingType mEasing;
			[SerializeField]
			internal Vector2 mLocationStarting;
			[SerializeField]
			internal Vector2 mLocationTarget;
			[SerializeField]
			internal Vector2 mSizeStarting;
			[SerializeField]
			internal Vector2 mSizeTarget;
			[SerializeField]
			internal Boolean mIsUnRegister;

			// Служебные данные
			[NonSerialized]
			internal Boolean mIsPoolObject;
			[NonSerialized]
			internal Single mStartTime;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Набор параметров для изменения
			/// </summary>
			public TDynamicParam ChangeParam
			{
				get { return mChangeParam; }
				set { mChangeParam = value; }
			}

			/// <summary>
			/// Продолжительность изменения параметров в секундах
			/// </summary>
			public Single Duration
			{
				get { return mDuration; }
				set { mDuration = value; }
			}

			/// <summary>
			/// Статус отмены регистрации после окончания изменения параметров
			/// </summary>
			public Boolean IsUnRegister
			{
				get { return mIsUnRegister; }
				set { mIsUnRegister = value; }
			}

			/// <summary>
			/// Статус отмены регистрации после окончания изменения параметров
			/// </summary>
			public TEasingType Easing
			{
				get { return mEasing; }
				set { mEasing = value; }
			}

			/// <summary>
			/// Начальная позиция
			/// </summary>
			public Vector2 LocationStarting
			{
				get { return mLocationStarting; }
				set { mLocationStarting = value; }
			}

			/// <summary>
			/// Конечная позиция
			/// </summary>
			public Vector2 LocationTarget
			{
				get { return mLocationTarget; }
				set { mLocationTarget = value; }
			}

			/// <summary>
			/// Начальная позиция
			/// </summary>
			public Vector2 SizeStarting
			{
				get { return mSizeStarting; }
				set { mSizeStarting = value; }
			}

			/// <summary>
			/// Конечная позиция
			/// </summary>
			public Vector2 SizeTarget
			{
				get { return mSizeTarget; }
				set { mSizeTarget = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusPoolObject =================================
			/// <summary>
			/// Статус объекта из пула
			/// </summary>
			/// <remarks>
			/// Позволяет определять был ли объект взят из пула и значит его надо вернуть или создан обычным образом
			/// </remarks>
			public Boolean IsPoolObject
			{
				get { return mIsPoolObject; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseDynamic()
				: base()
			{
				mDuration = 0.6f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseDynamic(String name)
			{
				mName = name;
				mDuration = 0.6f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseDynamic(String name, Single x, Single y)
			{
				mName = name;
				mLocationStarting.x = x;
				mLocationStarting.y = y;
				mRectWorldScreenMain.position = mLocationStarting;
				mDuration = 0.6f;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusPoolObject ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдо-конструктор
			/// </summary>
			/// <remarks>
			/// Вызывается диспетчером пула в момент взятия объекта из пула
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void OnPoolTake()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдо-деструктор
			/// </summary>
			/// <remarks>
			/// Вызывается диспетчером пула в момент попадания объекта в пул
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void OnPoolRelease()
			{

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
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление значения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				// Считаем время
				Single delta_time = (Time.time - mStartTime) / mDuration;
				if (delta_time > 1)
				{
					delta_time = 1;
					if (mIsUnRegister)
					{
						LotusGUIDispatcher.UnRegisterElement(this);
					}
					else
					{
						this.IsVisible = false;
					}

					IsDirty = false;
				}

				// Флаг изменения позиции
				if (mChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					mRectWorldScreenMain.x = XEasing.Interpolation(mLocationStarting.x, mLocationTarget.x, delta_time, mEasing);
					mRectWorldScreenMain.y = XEasing.Interpolation(mLocationStarting.y, mLocationTarget.y, delta_time, mEasing);
				}

				// Флаг изменения размера
				if (mChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					mRectWorldScreenMain.width = XEasing.Interpolation(mSizeStarting.x, mSizeTarget.x, delta_time, mEasing);
					mRectWorldScreenMain.height = XEasing.Interpolation(mSizeStarting.y, mSizeTarget.y, delta_time, mEasing);
				}

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента GUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.Label(mRectWorldScreenMain, "", mStyleMain);
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
				return MemberwiseClone() as CGUIBaseDynamic;
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

				CGUIBaseDynamic base_dynamic = base_element as CGUIBaseDynamic;
				if (base_dynamic != null)
				{
					mChangeParam = base_dynamic.mChangeParam;
					mDuration = base_dynamic.mDuration;
					mEasing = base_dynamic.mEasing;
					mLocationStarting = base_dynamic.mLocationStarting;
					mLocationTarget = base_dynamic.mLocationTarget;
					mSizeStarting = base_dynamic.mSizeStarting;
					mSizeTarget = base_dynamic.mSizeTarget;
					mIsUnRegister = base_dynamic.mIsUnRegister;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ АНИМАЦИИ ===========================================
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск изменения параметров
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------
			public virtual void StartAnimation()
			{
				// Флаг изменения позиции
				if (mChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					mRectWorldScreenMain.position = mLocationStarting;
				}

				// Флаг изменения размера
				if (mChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					mRectWorldScreenMain.size = mSizeStarting;
				}

				IsDirty = true;
				mStartTime = Time.time;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Динамический элемент c содержимым
		/// </summary>
		/// <remarks>
		/// Динамический элемент меняет соответствующие выбранные параметры во времени
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIDynamicText : CGUIBaseDynamic
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание динамического элемента с изменяемой позицией
			/// </summary>
			/// <param name="duration">Продолжительность изменения</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="location">Начальная позиция</param>
			/// <param name="delta_location">Изменение позиции</param>
			/// <returns>Созданный динамический элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIDynamicText CreateText(Single duration, String text, Vector2 location, Vector2 delta_location)
			{
				// Создаем элемент
				CGUIDynamicText dynamic_text = new CGUIDynamicText("D1", location.x, location.y, text);

				// Устанавливаем основные параметры
				dynamic_text.IsVisible = true;
				dynamic_text.ChangeParam = TDynamicParam.Location;
				dynamic_text.Duration = duration;
				dynamic_text.LocationStarting = location;
				dynamic_text.LocationTarget = location + delta_location;

				// Добавляем в диспетчер для рисования
				LotusGUIDispatcher.RegisterElement(dynamic_text);
				dynamic_text.mIsRegisterDispatcher = true;

				// Запускаем анимацию параметров
				dynamic_text.StartAnimation();

				return dynamic_text;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание динамического элемента из префаба с изменяемой позицией
			/// </summary>
			/// <param name="prefab">Префаб элемета</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="location">Начальная позиция</param>
			/// <param name="delta_location">Изменение позиции</param>
			/// <returns>Созданный динамический элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIDynamicText CreateTextFromPrefab(CGUIDynamicText prefab, String text, Vector2 location, Vector2 delta_location)
			{
				// Создаем элемент из префаба
				CGUIDynamicText dynamic_text = prefab.Duplicate() as CGUIDynamicText;

				// Устанавливаем основные параметры
				dynamic_text.Text = text;
				dynamic_text.LocationStarting = location;
				dynamic_text.LocationTarget = location + delta_location;

				// Добавляем в диспетчер для рисования
				LotusGUIDispatcher.RegisterElement(dynamic_text);
				dynamic_text.mIsRegisterDispatcher = true;

				// Запускаем анимацию параметров
				dynamic_text.StartAnimation();

				return dynamic_text;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mText;
			[SerializeField]
			internal Color mColorStarting;
			[SerializeField]
			internal Color mColorTarget;
			[SerializeField]
			internal Int32 mFontSizeStarting;
			[SerializeField]
			internal Int32 mFontSizeTarget;

			// Служебные данные
			[NonSerialized]
			internal Color mCurrentColor;
			[NonSerialized]
			internal Int32 mCurrentFontSize;
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
			/// Начальный цвет
			/// </summary>
			public Color ColorStarting
			{
				get { return mColorStarting; }
				set { mColorStarting = value; }
			}

			/// <summary>
			/// Конечный цвет
			/// </summary>
			public Color ColorTarget
			{
				get { return mColorTarget; }
				set { mColorTarget = value; }
			}

			/// <summary>
			/// Начальный размер шрифта
			/// </summary>
			public Int32 FontSizeStarting
			{
				get { return mFontSizeStarting; }
				set { mFontSizeStarting = value; }
			}

			/// <summary>
			/// Конечный размер шрифта
			/// </summary>
			public Int32 FontSizeTarget
			{
				get { return mFontSizeTarget; }
				set { mFontSizeTarget = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDynamicText()
				: base()
			{
				mColorStarting = Color.white;
				mColorTarget = Color.red;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDynamicText(String name)
				: base(name)
			{
				mColorStarting = Color.white;
				mColorTarget = Color.red;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDynamicText(String name, Single x, Single y)
				: base(name, x, y)
			{
				mColorStarting = Color.white;
				mColorTarget = Color.red;
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
			public CGUIDynamicText(String name, Single x, Single y, String text)
				: base(name, x, y)
			{
				mText = text;
				mColorStarting = Color.white;
				mColorTarget = Color.red;
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
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление значения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				// Считаем время
				Single delta_time = (Time.unscaledTime - mStartTime) / mDuration;
				if (delta_time > 1)
				{
					delta_time = 1;
					if (mIsUnRegister)
					{
						LotusGUIDispatcher.UnRegisterElement(this);
					}
					else
					{
						this.IsVisible = false;
					}

					IsDirty = false;
				}

				// Флаг изменения позиции
				if (mChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					mRectWorldScreenMain.x = XEasing.Interpolation(mLocationStarting.x, mLocationTarget.x, delta_time, mEasing);
					mRectWorldScreenMain.y = XEasing.Interpolation(mLocationStarting.y, mLocationTarget.y, delta_time, mEasing);
				}

				// Флаг изменения размера
				if (mChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					mRectWorldScreenMain.width = XEasing.Interpolation(mSizeStarting.x, mSizeTarget.x, delta_time, mEasing);
					mRectWorldScreenMain.height = XEasing.Interpolation(mSizeStarting.y, mSizeTarget.y, delta_time, mEasing);
				}

				// Флаг изменения цвета
				if (mChangeParam.IsFlagSet(TDynamicParam.ColorText))
				{
					mCurrentColor = Color.Lerp(mColorStarting, mColorTarget, delta_time);
				}

				// Флаг изменения размера шрифта
				if (mChangeParam.IsFlagSet(TDynamicParam.FontSize))
				{
					mCurrentFontSize = mFontSizeStarting + (Int32)((mFontSizeTarget - mFontSizeStarting) * delta_time);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUIStyle style = GUIStyle.none;
				style.fontSize = mCurrentFontSize;
				style.normal.textColor = mCurrentColor;

				GUI.Label(mRectWorldScreenMain, mText, style);
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
				return MemberwiseClone() as CGUIDynamicText;
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

				CGUIDynamicText dynamic_text = base_element as CGUIDynamicText;
				if (dynamic_text != null)
				{
					mText = dynamic_text.mText;
					mColorStarting = dynamic_text.mColorStarting;
					mColorTarget = dynamic_text.mColorTarget;
					mFontSizeStarting = dynamic_text.mFontSizeStarting;
					mFontSizeTarget = dynamic_text.mFontSizeTarget;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ АНИМАЦИИ ===========================================
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск изменения параметров
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------
			public override void StartAnimation()
			{
				// Флаг изменения позиции
				if (mChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					mRectWorldScreenMain.position = mLocationStarting;
				}

				// Флаг изменения размера
				if (mChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					mRectWorldScreenMain.size = mSizeStarting;
				}

				// Флаг изменения цвета
				if (mChangeParam.IsFlagSet(TDynamicParam.ColorText))
				{
					mCurrentColor = mColorStarting;
				}

				// Флаг изменения размера шрифта
				if (mChangeParam.IsFlagSet(TDynamicParam.FontSize))
				{
					mCurrentFontSize = mFontSizeStarting;
				}

				IsDirty = true;
				mStartTime = Time.unscaledTime;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Динамический элемент для анимации спрайта
		/// </summary>
		/// <remarks>
		/// Анимационный элемент обеспечивает анимацию спрайтов из хранилища анимационных цепочек
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIDynamicSprite : CGUIBaseDynamic
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание динамического элемента для анимации спрайта из префаба в указанной позиции
			/// </summary>
			/// <param name="prefab">Префаб элемета</param>
			/// <param name="location">Позиция</param>
			/// <returns>Созданный динамический элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGUIDynamicSprite CreateSpriteFromPrefab(CGUIDynamicSprite prefab, Vector2 location)
			{
				// Создаем элемент из префаба
				CGUIDynamicSprite dynamic_sprite = prefab.Duplicate() as CGUIDynamicSprite;

				// Устанавливаем основные параметры
				dynamic_sprite.LocationScreen = location;

				// Добавляем в диспетчер для рисования
				LotusGUIDispatcher.RegisterElement(dynamic_sprite);
				dynamic_sprite.mIsRegisterDispatcher = true;

				// Запускаем анимацию параметров
				dynamic_sprite.StartAnimation();

				return dynamic_sprite;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mGroupIndex;
			[SerializeField]
			internal Int32 mGroupSpriteIndex;
			[SerializeField]
			internal Int32 mStorageSpriteIndex;
			[SerializeField]
			internal Int32 mStartFrame;
			[SerializeField]
			internal Int32 mTargetFrame;
			[SerializeField]
			internal Color mColorStarting;
			[SerializeField]
			internal Color mColorTarget;

			// Служебные данные
			[NonSerialized]
			internal Color mCurrentColor;
			[NonSerialized]
			internal Int32 mCurrentFrameIndex;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Single mWidthSprite;
			[SerializeField]
			internal Single mHeightSprite;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Индекс группы анимационных цепочек
			/// </summary>
			public Int32 GroupIndex
			{
				get { return mGroupIndex; }
				set
				{
					mGroupIndex = value;
					mStorageSpriteIndex = LotusTweenDispatcher.SpriteStorage.GetStorageSpriteIndex(mGroupIndex, mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Имя группы анимационных цепочек
			/// </summary>
			public String GroupName
			{
				get
				{
					if (mGroupIndex == -1)
					{
						return (String.Empty);
					}
					else
					{
						return LotusTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Name;
					}
				}
			}

			/// <summary>
			/// Индекс анимационной цепочки в группе
			/// </summary>
			public Int32 GroupSpriteIndex
			{
				get { return mGroupSpriteIndex; }
				set
				{
					if (value < 0) value = 0;
					if (value > LotusTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Count - 1)
					{
						value = LotusTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Count - 1;
					}
					mGroupSpriteIndex = value;
					mStorageSpriteIndex = LotusTweenDispatcher.SpriteStorage.GetStorageSpriteIndex(mGroupIndex, mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Имя анимационной цепочки из группы
			/// </summary>
			public String GroupSpriteName
			{
				get
				{
					if (mGroupSpriteIndex == -1)
					{
						return String.Empty;
					}
					else
					{
						return (LotusTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex][mGroupSpriteIndex].Name);
					}
				}
			}

			/// <summary>
			/// Индекс анимационной цепочки из хранилища анимаций спрайтов
			/// </summary>
			public Int32 StorageSpriteIndex
			{
				get { return mStorageSpriteIndex; }
				set
				{
					mStorageSpriteIndex = value;
					LotusTweenDispatcher.SpriteStorage.GetGroupIndexAndSpriteIndex(mStorageSpriteIndex, ref mGroupIndex, ref mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Начальный кадр
			/// </summary>
			public Int32 StartFrame
			{
				get { return mStartFrame; }
				set { mStartFrame = value; }
			}

			/// <summary>
			/// Целевой кадр
			/// </summary>
			public Int32 TargetFrame
			{
				get { return mTargetFrame; }
				set { mTargetFrame = value; }
			}

			/// <summary>
			/// Текущий проигрываемый спрайт
			/// </summary>
			public Sprite FrameSprite
			{
				get
				{
					if (mStorageSpriteIndex == -1)
					{
						return null;
					}
					else
					{
						return (LotusTweenDispatcher.SpriteStorage.GetFrameSprite(mGroupIndex, mGroupSpriteIndex, mCurrentFrameIndex));
					}
				}
			}

			/// <summary>
			/// Начальный цвет
			/// </summary>
			public Color ColorStarting
			{
				get { return mColorStarting; }
				set { mColorStarting = value; }
			}

			/// <summary>
			/// Конечный цвет
			/// </summary>
			public Color ColorTarget
			{
				get { return mColorTarget; }
				set { mColorTarget = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDynamicSprite()
				: base()
			{
				mColorStarting = Color.white;
				mColorTarget = Color.white;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDynamicSprite(String name)
				: base(name)
			{
				mColorStarting = Color.white;
				mColorTarget = Color.white;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDynamicSprite(String name, Single x, Single y)
				: base(name, x, y)
			{
				mColorStarting = Color.white;
				mColorTarget = Color.white;
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
#if UNITY_EDITOR
				if(Mathf.Approximately(mRectWorldScreenMain.width, 0.0f))
				{
					mRectWorldScreenMain.width = mWidthSprite;
				}
				if (Mathf.Approximately(mRectWorldScreenMain.height, 0.0f))
				{
					mRectWorldScreenMain.height = mHeightSprite;
				}
#endif
				if (mStyleMain == null) mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление значения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				// Считаем время
				Single delta_time = (Time.unscaledTime - mStartTime) / mDuration;
				if (delta_time > 1)
				{
					delta_time = 1;
					if (mIsUnRegister)
					{
						LotusGUIDispatcher.UnRegisterElement(this);
					}
					else
					{
						this.IsVisible = false;
					}

					IsDirty = false;
				}

				// Флаг изменения позиции
				if (mChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					mRectWorldScreenMain.x = XEasing.Interpolation(mLocationStarting.x, mLocationTarget.x, delta_time, mEasing);
					mRectWorldScreenMain.y = XEasing.Interpolation(mLocationStarting.y, mLocationTarget.y, delta_time, mEasing);
				}

				// Флаг изменения размера
				if (mChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					mRectWorldScreenMain.width = XEasing.Interpolation(mSizeStarting.x, mSizeTarget.x, delta_time, mEasing);
					mRectWorldScreenMain.height = XEasing.Interpolation(mSizeStarting.y, mSizeTarget.y, delta_time, mEasing);
				}

				// Флаг изменения цвета
				if (mChangeParam.IsFlagSet(TDynamicParam.ColorBackground))
				{
					mCurrentColor = Color.Lerp(mColorStarting, mColorTarget, delta_time);
				}

				// Флаг анимации спрайта
				if (mChangeParam.IsFlagSet(TDynamicParam.AnimationSprite))
				{
					mCurrentFrameIndex = mStartFrame + (Int32)((mTargetFrame - mStartFrame) * delta_time);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				Sprite sprite = FrameSprite;

				if (sprite != null)
				{
					Rect texture_coord = LotusTweenDispatcher.SpriteStorage.GetFrameUVRect(mGroupIndex, mGroupSpriteIndex, mCurrentFrameIndex);

					GUI.color = new Color(1, 1, 1, 0.5f);
					GUI.DrawTextureWithTexCoords(mRectWorldScreenMain, sprite.texture, texture_coord, true);
					GUI.color = Color.white;
				}
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
				return MemberwiseClone() as CGUIDynamicSprite;
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

				CGUIDynamicSprite dynamic_sprite = base_element as CGUIDynamicSprite;
				if (dynamic_sprite != null)
				{
					mGroupIndex = dynamic_sprite.mGroupIndex;
					mGroupSpriteIndex = dynamic_sprite.mGroupSpriteIndex;
					mStorageSpriteIndex = dynamic_sprite.mStorageSpriteIndex;
					mStartFrame = dynamic_sprite.mStartFrame;
					mTargetFrame = dynamic_sprite.mTargetFrame;
					mColorStarting = dynamic_sprite.mColorStarting;
					mColorTarget = dynamic_sprite.mColorTarget;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ АНИМАЦИИ ===========================================
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск изменения параметров
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------
			public override void StartAnimation()
			{
				// Флаг изменения позиции
				if (mChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					mRectWorldScreenMain.position = mLocationStarting;
				}

				// Флаг изменения размера
				if (mChangeParam.IsFlagSet(TDynamicParam.Size))
				{
					mRectWorldScreenMain.size = mSizeStarting;
				}

				// Флаг изменения цвета
				if (mChangeParam.IsFlagSet(TDynamicParam.ColorBackground))
				{
					mCurrentColor = mColorStarting;
				}

				// Флаг изменения размера шрифта
				if (mChangeParam.IsFlagSet(TDynamicParam.AnimationSprite))
				{
					mCurrentFrameIndex = mStartFrame;
				}

				IsDirty = true;
				mStartTime = Time.unscaledTime;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================