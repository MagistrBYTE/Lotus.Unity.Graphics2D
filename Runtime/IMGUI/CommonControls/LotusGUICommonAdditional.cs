//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonAdditional.cs
*		Дополнительные управляющие элементы интерфейса пользователя.
*		Реализация дополнительных управляющих элементов интерфейса пользователя для непосредственного взаимодействия 
*	с пользователем.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DImmedateGUIControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Ползунок c областью значения
		/// </summary>
		/// <remarks>
		/// В зависимости от ориентации элемента для рисования используется метод либо GUI.HorizontalSlider либо GUI.VerticalSlider.
		/// Поддерживается различное местоположение области значение, а также гибкое форматирование значения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUISliderValue : CGUISlider
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mFormatValue;
			[SerializeField]
			internal String mSuffixValue;
			[SerializeField]
			internal String mValueText;

			// Параметры визуального стиля
			[SerializeField]
			internal String mStyleValueName;
			[NonSerialized]
			internal GUIStyle mStyleValue;

			// Параметры размещения
			[SerializeField]
			internal Single mValueSize;
			[SerializeField]
			internal TValueLocation mValueLocation;

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
			/// Значение числовой величины
			/// </summary>
			public new Single NumberValue
			{
				get { return mNumberValue; }
				set
				{
					if (mNumberValue != value)
					{
						mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
						mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
						if (mOnValueChanged != null) mOnValueChanged();
					}
				}
			}

			/// <summary>
			/// Максимальное значение числовой величины
			/// </summary>
			public new Single MaxValue
			{
				get { return mMaxValue; }
				set
				{
					if (mMaxValue != value)
					{
						mMaxValue = value;
						if (mNumberValue > mMaxValue)
						{
							mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
							mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
							if (mOnValueChanged != null) mOnValueChanged();
						}
					}
				}
			}

			/// <summary>
			/// Минимальное значение числовой величины
			/// </summary>
			public new Single MinValue
			{
				get { return mMinValue; }
				set
				{
					if (mMinValue != value)
					{
						mMinValue = value;
						if (mNumberValue > mMinValue)
						{
							mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
							mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
							if (mOnValueChanged != null) mOnValueChanged();
						}
					}
				}
			}

			/// <summary>
			/// Формат отображения числовой величины
			/// </summary>
			public String FormatValue
			{
				get { return mFormatValue; }
				set
				{
					if (mFormatValue != value)
					{
						mFormatValue = value;
						mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
					}
				}
			}

			/// <summary>
			/// Дополнительное обозначение числовой величины
			/// </summary>
			public String SuffixValue
			{
				get { return mSuffixValue; }
				set
				{
					if (mSuffixValue != value)
					{
						mSuffixValue = value;
						mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
					}
				}
			}

			/// <summary>
			/// Текст значения элемента
			/// </summary>
			public String ValueText
			{
				get { return mValueText; }
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
			public CGUISliderValue()
				: base()
			{
				mStyleValueName = "Box";
				mValueSize = 30;
				mFormatValue = "F1";
				mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISliderValue(String name)
				: base(name)
			{
				mStyleValueName = "Box";
				mValueSize = 30;
				mFormatValue = "F1";
				mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISliderValue(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleValueName = "Box";
				mValueSize = 30;
				mFormatValue = "F1";
				mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
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

				if (mStyleThumb == null) mStyleThumb = LotusGUIDispatcher.FindStyle(mStyleMainName + "Thumb");
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
				if (mStyleThumb == null) mStyleThumb = LotusGUIDispatcher.FindStyle(mStyleMainName + "Thumb");
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

				GUI.changed = false;
				if (mOrientation == TOrientation.Horizontal)
				{
					mNumberValue = GUI.HorizontalSlider(mRectWorldScreenMain, mNumberValue, mMinValue, mMaxValue, mStyleMain,
						mStyleThumb);
				}
				else
				{
					mNumberValue = GUI.VerticalSlider(mRectWorldScreenMain, mNumberValue, mMinValue, mMaxValue, mStyleMain, mStyleThumb);
				}
				if (GUI.changed)
				{
					mValueText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
					if (mOnValueChanged != null) mOnValueChanged();
				}

				GUI.Label(mRectWorldScreenValue, mValueText, mStyleValue);
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
				return MemberwiseClone() as CGUISlider;
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

				CGUISliderValue slider_value = base_element as CGUISliderValue;
				if (slider_value != null)
				{
					mOrientation = slider_value.mOrientation;
					mNumberValue = slider_value.mNumberValue;
					mMinValue = slider_value.mMinValue;
					mMaxValue = slider_value.mMaxValue;
					mFormatValue = slider_value.mFormatValue;
					mSuffixValue = slider_value.mSuffixValue;
					mValueText = slider_value.mValueText;
					mValueSize = slider_value.mValueSize;
					mValueSizeCurrent = slider_value.mValueSizeCurrent;
					mStyleValue = slider_value.mStyleValue;
					mStyleThumb = slider_value.mStyleThumb;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Джойстик
		/// </summary>
		/// <remarks>
		/// Для рисования фона используется метод GUI.Box, для рисования ручки используется метод GUI.DrawTexture.
		/// Присутствует поддержка эмуляция перемещения по осям подсистемы ввода данных
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIJoystick : CGUIElement, ILotusVirtualJoystick
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Texture2D mImageHandle;
			[SerializeField]
			internal Rect mRectLocalDesignHandle;
			[SerializeField]
			internal Single mMaxOffsetX = 60;
			[SerializeField]
			internal Single mMaxOffsetY = 60;
			[SerializeField]
			internal Boolean mIsInverseY = false;
			[SerializeField]
			internal Boolean mIsNormalize = false;
			[SerializeField]
			internal Single mSpring = 25;
			[SerializeField]
			internal Single mDeadZone = 0.1f;
			[NonSerialized]
			internal Vector2 mValue;
			[NonSerialized]
			internal Vector2 mValueNormalize;

			// События
			[NonSerialized]
			internal Action mOnAxisChanged;

			// Служебные данные
			[NonSerialized]
			internal Boolean mIsDraggingHandle;
			[NonSerialized]
			internal Vector2 mPositionHandle;
			[NonSerialized]
			internal Rect mRectWorldScreenHandle;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Текстура изображения ручки
			/// </summary>
			public Texture2D ImageHandle
			{
				get { return mImageHandle; }
				set
				{
					mImageHandle = value;
				}
			}

			/// <summary>
			/// Размер изображения ручки
			/// </summary>
			public Vector2 HandleSize
			{
				get { return mRectLocalDesignHandle.size; }
				set
				{
					mRectLocalDesignHandle.size = value;
				}
			}

			/// <summary>
			/// Максимальное смещение джойстика по горизонтали по обеим сторонам
			/// </summary>
			public Single MaxOffsetX
			{
				get { return mMaxOffsetX; }
				set
				{
					mMaxOffsetX = value;
				}
			}

			/// <summary>
			/// Максимальное смещение джойстика по вертикали по обеим сторонам
			/// </summary>
			public Single MaxOffsetY
			{
				get { return mMaxOffsetY; }
				set
				{
					mMaxOffsetY = value;
				}
			}

			/// <summary>
			/// Инверсия координаты Y
			/// </summary>
			public Boolean IsInverseY
			{
				get { return mIsInverseY; }
				set
				{
					mIsInverseY = value;
				}
			}

			/// <summary>
			/// Статус передачи нормализованных осей
			/// </summary>
			public Boolean IsNormalize
			{
				get { return mIsNormalize; }
				set
				{
					mIsNormalize = value;
				}
			}

			/// <summary>
			/// Относительная скорость возврата джойстик в центр
			/// </summary>
			public Single Spring
			{
				get { return mSpring; }
				set
				{
					mSpring = value;
				}
			}

			/// <summary>
			/// Относительный размер «мертвой зоны»
			/// </summary>
			/// <remarks>
			/// Мертвая зона - область вокруг центра где положение джойстика приравниваются к нулю
			/// </remarks>
			public Single DeadZone
			{
				get { return mDeadZone; }
				set
				{
					mDeadZone = value;
				}
			}

			/// <summary>
			/// Значение в абсолютных координатах
			/// </summary>
			public Vector2 Value
			{
				get { return mValue; }
			}

			/// <summary>
			/// Значение в нормализованных координатах [-1; 1]
			/// </summary>
			public Vector2 ValueNormalize
			{
				get { return mValueNormalize; }
			}

			/// <summary>
			/// Статус перетаскивания
			/// </summary>
			public Boolean IsDraggingHandle
			{
				get { return mIsDraggingHandle; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о изменении осей джойстика
			/// </summary>
			public Action OnAxisChanged
			{
				get { return mOnAxisChanged; }
				set { mOnAxisChanged = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIJoystick()
				: base()
			{
				mStyleMainName = "Joystick";
				mRectLocalDesignMain.width = 120;
				mRectLocalDesignMain.height = 120;
				mRectLocalDesignHandle = new Rect(0, 0, 60, 60);
				mMaxOffsetX = 60;
				mMaxOffsetY = 60;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIJoystick(String name)
				: base(name)
			{
				mStyleMainName = "Joystick";
				mRectLocalDesignMain.width = 120;
				mRectLocalDesignMain.height = 120;
				mRectLocalDesignHandle = new Rect(0, 0, 60, 60);
				mMaxOffsetX = 60;
				mMaxOffsetY = 60;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIJoystick(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Joystick";
				mRectLocalDesignMain.width = 120;
				mRectLocalDesignMain.height = 120;
				mRectLocalDesignHandle = new Rect(0, 0, 60, 60);
				mMaxOffsetX = 60;
				mMaxOffsetY = 60;
			}
			#endregion

			#region ======================================= МЕТОДЫ IVirtualJoystick ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по осям
			/// </summary>
			/// <remarks>
			/// Перемещение/смещение всегда указывается в относительных единицах в диапазоне от [-1; 1]
			/// </remarks>
			/// <returns>Смещение/перемещение по осям</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2 GetAxis()
			{
				return mValueNormalize;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по горизонтальной оси
			/// </summary>
			/// <returns>Смещение/перемещение по горизонтальной оси</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetAxisHorizontal()
			{
				return mValueNormalize.x;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по вертикальной оси
			/// </summary>
			/// <returns>Смещение/перемещение по вертикальной оси</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetAxisVertical()
			{
				return mValueNormalize.y;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции и размеров элемента по относительным данным
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdatePlacement()
			{
				base.UpdatePlacement();

				Single scale = LotusGUIDispatcher.ScaledScreenAverage;
				mRectWorldScreenHandle.width = mRectLocalDesignHandle.width * scale;
				mRectWorldScreenHandle.height = mRectLocalDesignHandle.height * scale;
				mRectWorldScreenHandle.center = mRectWorldScreenMain.center;
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
				base.OnReset();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				mRectWorldScreenHandle.center = Vector2.Lerp(mRectWorldScreenHandle.center, mRectWorldScreenMain.center, Time.unscaledDeltaTime * mSpring);
				if (mRectWorldScreenHandle.center.Approximately(mRectWorldScreenMain.center))
				{
					mRectWorldScreenHandle.center = mRectWorldScreenMain.center;
					mValue = Vector2.zero;
					mValueNormalize = Vector2.zero;
					IsDirty = false;
				}

				mValue = mRectWorldScreenHandle.center - mRectWorldScreenMain.center;
				mValueNormalize.x = mValue.x / mMaxOffsetX;
				mValueNormalize.y = mValue.y / mMaxOffsetY;
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

				switch (Event.current.type)
				{
					case EventType.MouseDown:
						{
							if (mRectWorldScreenHandle.Contains(Event.current.mousePosition))
							{
								mIsDraggingHandle = true;
								IsDirty = false;
								mPositionHandle = Event.current.mousePosition;
							}
						}
						break;
					case EventType.MouseUp:
						{
							mIsDraggingHandle = false;
							IsDirty = true;
							LotusGUIDispatcher.IsUpdated = true;
						}
						break;
					case EventType.MouseDrag:
						{
							if (mIsDraggingHandle)
							{
								mRectWorldScreenHandle.x += Event.current.delta.x;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
								mRectWorldScreenHandle.y += Event.current.delta.y;
#else
								mRectWorldScreenHandle.y -= (Event.current.delta.y);
#endif
								// Ограничиваем перемещение
								mRectWorldScreenHandle.x = Mathf.Clamp(mRectWorldScreenHandle.x, mRectWorldScreenMain.center.x - mMaxOffsetX - mRectWorldScreenHandle.width / 2,
									mRectWorldScreenMain.center.x + mMaxOffsetX - mRectWorldScreenHandle.width / 2);
								mRectWorldScreenHandle.y = Mathf.Clamp(mRectWorldScreenHandle.y, mRectWorldScreenMain.center.y - mMaxOffsetY - mRectWorldScreenHandle.height / 2,
									mRectWorldScreenMain.center.y + mMaxOffsetY - mRectWorldScreenHandle.height / 2);

								// Основное смещение
								mValue = mRectWorldScreenHandle.center - mRectWorldScreenMain.center;

								// Нормализованное смещение
								mValueNormalize.x = mValue.x / mMaxOffsetX;
								mValueNormalize.y = mValue.y / mMaxOffsetY;

								// Если мы вышли за мертвую зону
								if (mValue.sqrMagnitude > mDeadZone)
								{
									if (mOnAxisChanged != null) mOnAxisChanged();
								}
							}
						}
						break;
					default:
						break;
				}

				GUI.Box(mRectWorldScreenMain, "", mStyleMainName);
				GUI.DrawTexture(mRectWorldScreenHandle, mImageHandle);
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
				return MemberwiseClone() as CGUIJoystick;
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

				CGUIJoystick joystick = base_element as CGUIJoystick;
				if (joystick != null)
				{
					mImageHandle = joystick.mImageHandle;
					mRectLocalDesignHandle = joystick.mRectLocalDesignHandle;
					mMaxOffsetX = joystick.mMaxOffsetX;
					mMaxOffsetY = joystick.mMaxOffsetY;
					mIsInverseY = joystick.mIsInverseY;
					mIsNormalize = joystick.mIsNormalize;
					mSpring = joystick.mSpring;
					mDeadZone = joystick.mDeadZone;
					mValue = joystick.mValue;
					mValueNormalize = joystick.mValueNormalize;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Область просмотра с прокруткой по фиксированным смещениям - страницам
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.BeginScrollView и стандартный стиль ScrollView
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIScrollViewSnap : CGUIScrollView
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mCountFixedOffset;

			// Служебные данные
			[NonSerialized]
			internal Int32 mDestNumberFixedOffset;
			[NonSerialized]
			internal List<Single> mFixedPlacement;
			[NonSerialized]
			internal Boolean mIsMoveFixedOffset;
			[NonSerialized]
			internal Single mStartFixedOffset;

			// События
			[NonSerialized]
			internal Action<Int32> mOnPageChanged;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Количество фиксированных позиций
			/// </summary>
			public Int32 CountFixedOffset
			{
				get { return mCountFixedOffset; }
				set { mCountFixedOffset = value; }
			}

			/// <summary>
			/// Индекс страницы по ширине
			/// </summary>
			public Int32 PageIndexWidth
			{
				get
				{
					Int32 result = 0;
					for (Int32 i = 0; i < mFixedPlacement.Count; i++)
					{
						if (Approximately(mContentOffset.x, mFixedPlacement[i], 2))
						{
							result = i;
							break;
						}
					}

					return result;
				}
				set
				{
					// 1) Получаем текущую
					Int32 current = 0;
					for (Int32 i = 0; i < mFixedPlacement.Count; i++)
					{
						if (Approximately(mContentOffset.x, mFixedPlacement[i], 2))
						{
							current = i;
							break;
						}
					}

					if(current != value)
					{
						// 2) Целевая позиция
						mContentOffsetDest.x = mFixedPlacement[value];
						if (mOnPageChanged != null) mOnPageChanged(value);
						if (mIsInertia == false)
						{
							mContentOffset.x = mContentOffsetDest.x;
						}
						else
						{
							IsDirty = true;
						}
					}
				}
			}

			/// <summary>
			/// Индекс страницы по высоте
			/// </summary>
			public Int32 PageIndexHeight
			{
				get
				{
					return (Int32)(this.mContentOffset.y / this.mRectWorldScreenMain.height);
				}
			}

			/// <summary>
			/// Возможность вертикальной прокрутки
			/// </summary>
			public Boolean IsVerticalScroll
			{
				get { return mScrollDirection != TScrollDirection.Horizontal; }
			}

			/// <summary>
			/// Возможность горизонтальной прокрутки
			/// </summary>
			public Boolean IsHorizontalScroll
			{
				get { return mScrollDirection != TScrollDirection.Vertical; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации об окончание перемещения одной страницы при фиксированном смещении
			/// </summary>
			public Action<Int32> OnPageChanged
			{
				get { return mOnPageChanged; }
				set { mOnPageChanged = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIScrollViewSnap()
				: base()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIScrollViewSnap(String name)
				: base(name)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIScrollViewSnap(String name, Single x, Single y)
				: base(name, x, y)
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
				base.OnReset();
				ResetFixedOffset();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента GUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				OnPageMoving();

				mContentOffset = GUI.BeginScrollView(mRectWorldScreenMain, mContentOffset, mRectWorldScreenContentView, false, false,
					GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar);
				{
					if (mCountChildren > 0)
					{
						LotusGUIDispatcher.FromParentDrawElements(this);
					}
				}
				GUI.EndScrollView();
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
				return MemberwiseClone() as CGUIScrollViewSnap;
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

			#region ======================================= МЕТОДЫ ПЕРЕМЕЩЕНИЯ СТРАНИЦ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение на один шаг вправо при фиксированном смещении
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ToRightStep()
			{
				if (mIsDragging == false)
				{
					Int32 index = FindFixedOffset();
					if (index < this.mFixedPlacement.Count - 1)
					{
						Single dest_content_x = this.mFixedPlacement[index + 1];
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
					Int32 index = FindFixedOffset();
					if (index > 0)
					{
						Single dest_content_x = this.mFixedPlacement[index - 1];
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
					Int32 index = FindFixedOffset();
					if (index < this.mFixedPlacement.Count - 1)
					{
						Single dest_content_y = this.mFixedPlacement[index + 1];
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
					Int32 index = FindFixedOffset();
					if (index > 0)
					{
						Single dest_content_y = this.mFixedPlacement[index - 1];
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация данных системы фиксированного смещения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void InitFixedOffset()
			{
				Int32 count_step = mCountFixedOffset;

				// Считаем размер одного смещения
				Single delta_offset = this.mRectWorldScreenContentView.width / count_step;
				if (mScrollDirection == TScrollDirection.Vertical)
				{
					delta_offset = this.mRectWorldScreenContentView.height / count_step;
				}

				// Кол-во шагов
				count_step -= Mathf.FloorToInt(this.mRectWorldScreenMain.width / delta_offset) - 1;
				if (mScrollDirection == TScrollDirection.Vertical)
				{
					count_step -= Mathf.FloorToInt(this.mRectWorldScreenMain.height / delta_offset) - 1;
				}

				// Создаем опорные точки для перемещения
				mFixedPlacement = new List<Single>();
				for (Int32 i = 0; i < count_step; i++)
				{
					Single data = i * delta_offset;
					mFixedPlacement.Add(data);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных системы фиксированного смещения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ResetFixedOffset()
			{
				if (mFixedPlacement == null)
				{
					mFixedPlacement = new List<Single>();
				}
				else
				{
					mFixedPlacement.Clear();
				}
				InitFixedOffset();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск позиции смещения
			/// </summary>
			/// <returns>Позиция смещения</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 FindFixedOffset()
			{
				Int32 result = 0;

				if (mScrollDirection == TScrollDirection.Horizontal)
				{
					if (this.ContentOffsetX <= 0)
					{
						result = 0;
						return result;
					}
					if (ContentOffsetX >= mFixedPlacement[mFixedPlacement.Count - 1])
					{
						result = mFixedPlacement.Count - 1;
						return result;
					}

					for (Int32 i = 1; i < mFixedPlacement.Count; i++)
					{
						if (Approximately(ContentOffsetX, mFixedPlacement[i], 2))
						{
							result = i;
							break;
						}
					}
				}
				else
				{
					if (this.ContentOffsetY <= 0)
					{
						result = 0;
						return result;
					}

					if (this.ContentOffsetY >= mFixedPlacement[mFixedPlacement.Count - 1])
					{
						result = mFixedPlacement.Count - 1;
						return result;
					}

					for (Int32 i = 1; i < mFixedPlacement.Count; i++)
					{
						if (Approximately(ContentOffsetY, mFixedPlacement[i], 2))
						{
							result = i;
							break;
						}
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск целевой позиции смещения
			/// </summary>
			/// <returns>Целевая позиция смещения</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 FindDestFixedOffset()
			{
				Int32 result = 0;

				if (mScrollDirection == TScrollDirection.Horizontal)
				{
					if (this.ContentOffsetX <= 0)
					{
						result = 0;
						return result;
					}
					if (ContentOffsetX >= mFixedPlacement[mFixedPlacement.Count - 1])
					{
						result = mFixedPlacement.Count - 1;
						return result;
					}

					for (Int32 i = 1; i < mFixedPlacement.Count; i++)
					{
						if (ContentOffsetX < mFixedPlacement[i])
						{
							if (mStartFixedOffset > ContentOffsetX)
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
				else
				{
					if (this.ContentOffsetY <= 0)
					{
						result = 0;
						return result;
					}

					if (this.ContentOffsetY >= mFixedPlacement[mFixedPlacement.Count - 1])
					{
						result = mFixedPlacement.Count - 1;
						return result;
					}

					for (Int32 i = 1; i < mFixedPlacement.Count; i++)
					{
						if (ContentOffsetY < mFixedPlacement[i])
						{
							if (mStartFixedOffset > ContentOffsetY)
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
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение области просмотра на следующую страницу
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnPageMoving()
			{
				switch (Event.current.type)
				{
					case EventType.MouseDown:
						{
							// Если мы попали в прямоугольник элемента
							if (mRectWorldScreenMain.Contains(Event.current.mousePosition))
							{
								// Считаем прямоугольник для перемещения исключая области полос прокуртки
								Rect rect = new Rect(mRectWorldScreenMain.x + PaddingLeft,
									mRectWorldScreenMain.y + PaddingTop,
									mRectWorldScreenMain.width - (PaddingLeft + PaddingRight + LotusGUIDispatcher.SizeScrollVertical),
									mRectWorldScreenMain.height - (PaddingTop + PaddingBottom + LotusGUIDispatcher.SizeScrollHorizontal));

								// Если есть попадание
								if (rect.Contains(Event.current.mousePosition))
								{
									mIsDownDragging = true;
									IsDirty = false;
									mDragStartPosition = Event.current.mousePosition;
								}
								else
								{
									// Попали по полосе прокрутки
									// Остановливаем и сбрасываем
									IsDirty = false;
									mContentOffsetDest = mContentOffset;
								}
							}
						}
						break;
					case EventType.MouseUp:
						{
							mIsDownDragging = false;

							// Если есть нажатие в области перемещения
							if (mIsDragging)
							{
								// Включаем инерцию
								IsDirty = true;
								mIsDragging = false;

								switch (mScrollDirection)
								{
									case TScrollDirection.Vertical:
										{
											mDestNumberFixedOffset = this.FindDestFixedOffset();
											mContentOffsetDest.y = mFixedPlacement[mDestNumberFixedOffset];
											if (mOnPageChanged != null) mOnPageChanged(mDestNumberFixedOffset);
										}
										break;
									case TScrollDirection.Horizontal:
										{
											mDestNumberFixedOffset = this.FindDestFixedOffset();
											mContentOffsetDest.x = mFixedPlacement[mDestNumberFixedOffset];
											if (mOnPageChanged != null) mOnPageChanged(mDestNumberFixedOffset);
										}
										break;
									default:
										break;
								}
							}
						}
						break;
					case EventType.MouseDrag:
						{
							// Если есть нажатие в области перемещения
							if (mIsDownDragging && mIsDragging == false)
							{
								// Если будет смещение больше минимального то включаем перемещение 
								switch (mScrollDirection)
								{
									case TScrollDirection.Vertical:
										{
											if (Mathf.Abs(mDragStartPosition.y - Event.current.mousePosition.y) > LotusGUIDispatcher.DraggMinOffset.y)
											{
												mIsDragging = true;
												mStartFixedOffset = mContentOffset.y;
											}
										}
										break;
									case TScrollDirection.Horizontal:
										{
											if (Mathf.Abs(mDragStartPosition.x - Event.current.mousePosition.x) > LotusGUIDispatcher.DraggMinOffset.x)
											{
												mIsDragging = true;
												mStartFixedOffset = mContentOffset.x;
											}
										}
										break;
									default:
										break;
								}
							}

							// Если перемещение
							if (mIsDragging)
							{
								switch (mScrollDirection)
								{
									case TScrollDirection.Vertical:
										{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
											mContentOffset.y -= Event.current.delta.y;
#else
											mContentOffset.y += (Event.current.delta.y);
#endif
										}
										break;
									case TScrollDirection.Horizontal:
										{
											mContentOffset.x -= Event.current.delta.x;
										}
										break;
									default:
										break;
								}
							}
						}
						break;
					default:
						break;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Чат - элемент GUI предназначенный отображения переписки
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.TextField
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIChat : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIChat()
				: base()
			{
				mStyleMainName = "TextField";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIChat(String name)
				: base(name)
			{
				mStyleMainName = "TextField";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIChat(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "TextField";
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
				return MemberwiseClone() as CGUIChat;
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

				CGUIChat chat = base_element as CGUIChat;
				if (chat != null)
				{
					//mMaxLength = element.mMaxLength;
					//mIsReadOnly = element.mIsReadOnly;
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