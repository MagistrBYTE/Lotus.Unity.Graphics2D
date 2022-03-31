//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseIndication.cs
*		Элементы интерфейса пользователя для индикации процессов или величины.
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
		//! \addtogroup Unity2DImmedateGUIBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Индикатор прогресса - элемент GUI для индикации процесса
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Label и GUI.DrawTexture.
		/// Реализация элемента обеспечивающего визуальное отображение хода изменения определенного процесса или величины
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIProgressBar : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Single mPercent;

			// Параметры размещения
			[SerializeField]
			internal TOrientation mOrientation;

			// Параметры отображения
			[SerializeField]
			internal Vector4 mPaddingFill;
			[SerializeField]
			internal Boolean mIsTextureCoord;
			[SerializeField]
			internal Texture2D mTextureFill;

			// Служебные данные
			[NonSerialized]
			internal Rect mRectWorldScreenFill;
			[NonSerialized]
			internal Rect mRectWorldScreenFillCoord;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Индикатор процесса в частях от 0 до 1
			/// </summary>
			public Single Percent
			{
				get { return mPercent; }
				set
				{
					mPercent = value;
					ComputeRectDrawFill();
				}
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Ориентация элемента
			/// </summary>
			public TOrientation Orientation
			{
				get { return mOrientation; }
				set
				{
					if (mOrientation != value)
					{
						mOrientation = value;
						ComputeRectDrawFill();
					}
				}
			}

			//
			// ПАРАМЕТРЫ ОТОБРАЖЕНИЯ
			//
			/// <summary>
			/// Дополнительные отступы области заполнения
			/// </summary>
			public Vector4 PaddingFill
			{
				get { return mPaddingFill; }
				set { mPaddingFill = value; }
			}

			/// <summary>
			/// Дополнительные отступы области заполнения слева
			/// </summary>
			public Single PaddingFillLeft
			{
				get { return mPaddingFill.x; }
				set { mPaddingFill.x = value; }
			}

			/// <summary>
			/// Дополнительные отступы области заполнения сверху
			/// </summary>
			public Single PaddingFillTop
			{
				get { return mPaddingFill.y; }
				set { mPaddingFill.y = value; }
			}

			/// <summary>
			/// Дополнительные отступы области заполнения справа
			/// </summary>
			public Single PaddingFillRight
			{
				get { return mPaddingFill.z; }
				set { mPaddingFill.z = value; }
			}

			/// <summary>
			/// Дополнительные отступы области заполнения снизу
			/// </summary>
			public Single PaddingFillBottom
			{
				get { return mPaddingFill.w; }
				set { mPaddingFill.w = value; }
			}

			/// <summary>
			/// Использоваться пропорционально текстурные координаты
			/// </summary>
			public Boolean IsTextureCoord
			{
				get { return mIsTextureCoord; }
				set
				{
					if (mIsTextureCoord != value)
					{
						mIsTextureCoord = value;
						ComputeRectDrawFill();
					}
				}
			}

			/// <summary>
			/// Текстура для визуализации заполнения
			/// </summary>
			public Texture2D TextureFill
			{
				get { return mTextureFill; }
				set
				{
					mTextureFill = value;
					ComputeRectDrawFill();
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIProgressBar()
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
			public CGUIProgressBar(String name)
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
			public CGUIProgressBar(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Box";
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

				ComputeRectDrawFill();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление области заполнения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeRectDrawFill()
			{
				switch (mOrientation)
				{
					case TOrientation.Horizontal:
						{
							Single w = mRectWorldScreenMain.width - (PaddingFillLeft + PaddingFillRight);
							mRectWorldScreenFill.x = mRectWorldScreenMain.x + PaddingFillLeft;
							mRectWorldScreenFill.width = Mathf.Clamp(w * mPercent, 0, w);
							mRectWorldScreenFill.y = mRectWorldScreenMain.y + PaddingFillTop;
							mRectWorldScreenFill.height = mRectWorldScreenMain.height - (PaddingFillTop + PaddingFillBottom);

							if (mIsTextureCoord)
							{
								mRectWorldScreenFillCoord.x = 0;
								mRectWorldScreenFillCoord.y = 0;
								mRectWorldScreenFillCoord.width = mPercent;
								mRectWorldScreenFillCoord.height = 1;
							}
						}
						break;
					case TOrientation.Vertical:
						{
							Single h = mRectWorldScreenMain.height - (PaddingFillTop + PaddingFillBottom);
							mRectWorldScreenFill.x = mRectWorldScreenMain.x + PaddingFillLeft;
							mRectWorldScreenFill.width = mRectWorldScreenMain.width - (PaddingFillLeft + PaddingFillRight);
							mRectWorldScreenFill.height = Mathf.Clamp(h * mPercent, 0, h);
							mRectWorldScreenFill.y = mRectWorldScreenMain.yMax - PaddingFillBottom - mRectWorldScreenFill.height;

							if (mIsTextureCoord)
							{
								mRectWorldScreenFillCoord.x = 0;
								mRectWorldScreenFillCoord.y = 0;
								mRectWorldScreenFillCoord.width = 1;
								mRectWorldScreenFillCoord.height = mPercent;
							}
						}
						break;
					default:
						break;
				}
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

				GUI.Label(mRectWorldScreenMain, "", mStyleMain);

				if (mIsTextureCoord)
				{
					GUI.DrawTextureWithTexCoords(mRectWorldScreenFill, mTextureFill, mRectWorldScreenFillCoord, true);
				}
				else
				{
					GUI.DrawTexture(mRectWorldScreenFill, mTextureFill, ScaleMode.StretchToFill);
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
				return MemberwiseClone() as CGUIProgressBar;
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

				CGUIProgressBar progress_bar = base_element as CGUIProgressBar;
				if (progress_bar != null)
				{
					mOrientation = progress_bar.mOrientation;
					mPercent = progress_bar.mPercent;
					mTextureFill = progress_bar.mTextureFill;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Индикатор прогресса - элемент GUI для индикации процесса с областью для информирования
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Label и GUI.DrawTexture.
		/// Реализация элемента обеспечивающего визуальное отображение хода изменения определенного процесса или величины
		/// с отдельной областью для информирования
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIProgressBarValue : CGUIProgressBar
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Single mNumberValue;
			[SerializeField]
			internal Single mMinValue;
			[SerializeField]
			internal Single mMaxValue;
			[SerializeField]
			internal String mFormatValue;
			[SerializeField]
			internal String mSuffixValue;
			[NonSerialized]
			internal String mTextValue;

			// Параметры размещения
			[SerializeField]
			internal Single mValueSize;
			[SerializeField]
			internal TProgressBarValueLocation mValueLocation;

			// Параметры отображения
			[SerializeField]
			internal Boolean mIsInfoValue;
			[SerializeField]
			internal String mStyleValueName;
			[NonSerialized]
			internal GUIStyle mStyleValue;

			[NonSerialized]
			internal Single mChangeOffsetValue;
			[NonSerialized]
			internal Single mChangeTargetValue;

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
			/// Текстовой значение числовой величины
			/// </summary>
			public String TextValue
			{
				get { return mTextValue; }
			}

			/// <summary>
			/// Значение числовой величины
			/// </summary>
			public Single NumberValue
			{
				get { return mNumberValue; }
				set
				{
					if (mNumberValue != value)
					{
						mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
						mPercent = (mNumberValue - mMinValue) / (mMaxValue - mMinValue);
						mTextValue = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
						ComputeRectDrawFill();
					}
				}
			}

			/// <summary>
			/// Максимальное значение числовой величины
			/// </summary>
			public Single MaxValue
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
							mPercent = (mNumberValue - mMinValue) / (mMaxValue - mMinValue);
							mTextValue = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
							ComputeRectDrawFill();
						}
					}
				}
			}

			/// <summary>
			/// Минимальное значение числовой величины
			/// </summary>
			public Single MinValue
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
							mPercent = (mNumberValue - mMinValue) / (mMaxValue - mMinValue);
							mTextValue = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
							ComputeRectDrawFill();
						}
					}
				}
			}

			/// <summary>
			/// Нормализованное значение числовой величины в частях от 0 до 1
			/// </summary>
			public new Single Percent
			{
				get { return mPercent; }
				set
				{
					mPercent = Mathf.Clamp(value, 0, 1);
					mNumberValue = (mMaxValue - mMinValue) * mPercent + mMinValue;
					mTextValue = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
					ComputeRectDrawFill();
				}
			}

			/// <summary>
			/// Статус минимального значения числовой величины
			/// </summary>
			public Boolean IsMinValue
			{
				get { return mNumberValue <= mMinValue; }
			}

			/// <summary>
			/// Статус максимального значения числовой величины
			/// </summary>
			public Boolean IsMaxValue
			{
				get { return mNumberValue >= mMaxValue; }
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
						mTextValue = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
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
						mTextValue = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
					}
				}
			}

			//
			// ПАРАМЕТРЫ ОТОБРАЖЕНИЯ
			//
			/// <summary>
			/// Показывать область значения
			/// </summary>
			public Boolean IsInfoValue
			{
				get { return mIsInfoValue; }
				set
				{
					if (mIsInfoValue != value)
					{
						mIsInfoValue = value;
						UpdatePlacement();
					}
				}
			}

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
			public TProgressBarValueLocation ValueLocation
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
			public CGUIProgressBarValue()
				: base()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIProgressBarValue(String name)
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
			public CGUIProgressBarValue(String name, Single x, Single y)
				: base(name, x, y)
			{
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

				if (mIsInfoValue)
				{
					UpdateValuePlacement();
				}

				ComputeRectDrawFill();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление размера и положения области значения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void UpdateValuePlacement()
			{
				switch (mValueLocation)
				{
					case TProgressBarValueLocation.Center:
					case TProgressBarValueLocation.Right:
					case TProgressBarValueLocation.Left:
						{
							mValueSizeCurrent = mValueSize * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TProgressBarValueLocation.Top:
					case TProgressBarValueLocation.Bottom:
						{
							mValueSizeCurrent = mValueSize * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}

				switch (mValueLocation)
				{
					case TProgressBarValueLocation.Center:
						{
							mRectWorldScreenValue.x = mRectWorldScreenMain.x + (mRectWorldScreenMain.width / 2 - mValueSizeCurrent / 2);
							mRectWorldScreenValue.y = mRectWorldScreenMain.y;
							mRectWorldScreenValue.width = mValueSizeCurrent;
							mRectWorldScreenValue.height = mRectWorldScreenMain.height;
						}
						break;
					case TProgressBarValueLocation.Right:
						{
							mRectWorldScreenMain.width -= mValueSizeCurrent;

							mRectWorldScreenValue.x = mRectWorldScreenMain.xMax;
							mRectWorldScreenValue.y = mRectWorldScreenMain.y;
							mRectWorldScreenValue.width = mValueSizeCurrent;
							mRectWorldScreenValue.height = mRectWorldScreenMain.height;
						}
						break;
					case TProgressBarValueLocation.Left:
						{
							mRectWorldScreenMain.x += mValueSizeCurrent;
							mRectWorldScreenMain.width -= mValueSizeCurrent;

							mRectWorldScreenValue.x = mRectWorldScreenMain.x - mValueSizeCurrent;
							mRectWorldScreenValue.y = mRectWorldScreenMain.y;
							mRectWorldScreenValue.width = mValueSizeCurrent;
							mRectWorldScreenValue.height = mRectWorldScreenMain.height;
						}
						break;
					case TProgressBarValueLocation.Top:
						{
							mRectWorldScreenMain.y += mValueSizeCurrent;
							mRectWorldScreenMain.height -= mValueSizeCurrent;

							mRectWorldScreenValue.x = mRectWorldScreenMain.x;
							mRectWorldScreenValue.y = mRectWorldScreenMain.y - mValueSizeCurrent;
							mRectWorldScreenValue.width = mRectWorldScreenMain.width;
							mRectWorldScreenValue.height = mValueSizeCurrent;
						}
						break;
					case TProgressBarValueLocation.Bottom:
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
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusNumberChange =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт смещения числовой величины
			/// </summary>
			/// <param name="time">Время в течение которого должно произойти смещение</param>
			/// <param name="offset">Смещение величины</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartChangeValueOffset(Single time, Single offset)
			{
				mChangeTargetValue = Mathf.Clamp(mNumberValue + offset, mMinValue, mMaxValue);
				mChangeOffsetValue = offset / time;
				IsDirty = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт изменение числовой величины
			/// </summary>
			/// <param name="time">Время в течение которого должно произойти изменение величины</param>
			/// <param name="target">Целевое значение величины</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartChangeValueTarget(Single time, Single target)
			{
				Single offset = target - mNumberValue;
				mChangeTargetValue = Mathf.Clamp(target, mMinValue, mMaxValue);
				mChangeOffsetValue = offset / time;
				IsDirty = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт изменение числовой величины до максимального значения
			/// </summary>
			/// <param name="time">Время в течение которого должно произойти изменение величины</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartChangeValueToMax(Single time)
			{
				Single offset = mMaxValue - mNumberValue;
				mChangeTargetValue = mMaxValue;
				mChangeOffsetValue = offset / time;
				IsDirty = true;
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

				mTextValue = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				if (mChangeOffsetValue > 0)
				{
					mNumberValue += mChangeOffsetValue * Time.deltaTime;

					if (mNumberValue >= mChangeTargetValue)
					{
						IsDirty = false;
						mNumberValue = mChangeTargetValue;
					}
				}
				else
				{
					mNumberValue += mChangeOffsetValue * Time.deltaTime;

					if (mNumberValue <= mChangeTargetValue)
					{
						IsDirty = false;
						mNumberValue = mChangeTargetValue;
					}
				}
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

				GUI.Label(mRectWorldScreenMain, "", mStyleMain);

				if (mIsTextureCoord)
				{
					GUI.DrawTextureWithTexCoords(mRectWorldScreenFill, mTextureFill, mRectWorldScreenFillCoord, true);
				}
				else
				{
					GUI.DrawTexture(mRectWorldScreenFill, mTextureFill, ScaleMode.StretchToFill);
				}

				if (mIsInfoValue)
				{
					GUI.Label(mRectWorldScreenValue, mTextValue, mStyleValue);
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
				return MemberwiseClone() as CGUIProgressBarValue;
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

				CGUIProgressBarValue progressbar_value = base_element as CGUIProgressBarValue;
				if (progressbar_value != null)
				{
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент GUI для отображения рейтинга
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIRating : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIRating()
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
			public CGUIRating(String name)
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
			public CGUIRating(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "Box";
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
				return MemberwiseClone() as CGUIRating;
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

				CGUIRating rating = base_element as CGUIRating;
				if (rating != null)
				{
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