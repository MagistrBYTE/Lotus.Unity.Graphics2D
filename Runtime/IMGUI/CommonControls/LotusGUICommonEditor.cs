//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonEditor.cs
*		Управляющие элементы пользователя для редактирования и ввода данных.
*		Реализация управляющих элементов пользователя предназначенных для непосредственного редактирования и ввода данных.
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
		//! \addtogroup Unity2DImmedateGUIControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Текстовое поле
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.TextField.
		/// Поддерживается режим только для чтения и расширенная обработка событий
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUITextField : CGUIContentElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mMaxLength;
			[SerializeField]
			internal Boolean mIsReadOnly;

			// Служебные данные
			[NonSerialized]
			internal Boolean mIsFocused;

			// События
			internal Action mOnTextChanged;
			internal Action mOnFocused;
			internal Action mOnLostFocused;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Максимальное количество символов для отображения
			/// </summary>
			public Int32 MaxLength
			{
				get { return mMaxLength; }
				set
				{
					mMaxLength = value;
				}
			}

			/// <summary>
			/// Статус отображения только для чтения
			/// </summary>
			public Boolean IsReadOnly
			{
				get { return mIsReadOnly; }
				set
				{
					mIsReadOnly = value;
				}
			}

			/// <summary>
			/// Статус фокуса ввода клавиатуры
			/// </summary>
			public Boolean IsFocused
			{
				get { return mIsFocused; }
				set
				{
					if(mIsFocused != value)
					{
						if(value)
						{
							GUI.FocusControl(mName);
						}
						else
						{
							GUIUtility.keyboardControl = 0;
							Event.current.Use();
						}

						mIsFocused = value;
					}
				}
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о изменении текста пользователем
			/// </summary>
			public Action OnTextChanged
			{
				get { return mOnTextChanged; }
				set
				{
					mOnTextChanged = value;
				}
			}

			/// <summary>
			/// Событие для нотификации о получение фокуса ввода клавиатуры
			/// </summary>
			public Action OnFocused
			{
				get { return mOnFocused; }
				set
				{
					mOnFocused = value;
				}
			}

			/// <summary>
			/// Событие для нотификации о потери фокуса ввода клавиатуры
			/// </summary>
			public Action OnLostFocused
			{
				get { return mOnLostFocused; }
				set
				{
					mOnLostFocused = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUITextField()
				: base()
			{
				mStyleMainName = "TextField";
				mMaxLength = 12;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITextField(String name)
				: base(name)
			{
				mStyleMainName = "TextField";
				mMaxLength = 12;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITextField(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "TextField";
				mMaxLength = 12;
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
			public CGUITextField(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mStyleMainName = "TextField";
				mMaxLength = 12;
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

				if (mIsReadOnly)
				{
					GUI.Label(mRectWorldScreenMain, mText, mStyleMain);
				}
				else
				{
					GUI.SetNextControlName(mName);
					GUI.changed = false;
					mText = GUI.TextField(mRectWorldScreenMain, mText, mMaxLength, mStyleMain);
					if (GUI.changed)
					{
						if (mOnTextChanged != null) mOnTextChanged();
					}

					if(mIsFocused == false && GUI.GetNameOfFocusedControl() == mName)
					{
						mIsFocused = true;
						if (mOnFocused != null) mOnFocused();
					}

					if (mIsFocused && GUI.GetNameOfFocusedControl() != mName)
					{
						mIsFocused = false;
						if (mOnLostFocused != null) mOnLostFocused();
					}

					// При нажатии Return происходит потеря фокуса
					if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == mName)
					{
						GUIUtility.keyboardControl = 0;
						Event.current.Use();
					}
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
				return MemberwiseClone() as CGUITextField;
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

				CGUITextField text_field = base_element as CGUITextField;
				if (text_field != null)
				{
					mMaxLength = text_field.mMaxLength;
					mIsReadOnly = text_field.mIsReadOnly;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Счетчик - элемент GUI предназначенный для ввода и редактирования числовых данных
		/// </summary>
		/// <remarks>
		/// Поддерживается ручной ввод данных, различное местоположение управляющих кнопок счетчика
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUISpinner : CGUITextField
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsUserInput;
			[SerializeField]
			internal Single mNumberValue;
			[SerializeField]
			internal Single mMinValue;
			[SerializeField]
			internal Single mMaxValue;
			[SerializeField]
			internal Single mStepValue;
			[SerializeField]
			internal String mFormatValue;
			[SerializeField]
			internal String mSuffixValue;

			// Параметры размещения
			[SerializeField]
			internal Single mButtonSize;
			[SerializeField]
			internal TSpinnerButtonLocation mButtonLocation;

			// События
			internal Action mOnValueChanged;

			// Служебные данные
			[NonSerialized]
			internal Single mButtonSizeCurrent;
			[NonSerialized]
			internal Rect mRectWorldScreenButtonUp;
			[NonSerialized]
			internal Rect mRectWorldScreenButtonDown;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Возможность ввода значения вручную
			/// </summary>
			public Boolean IsUserInput
			{
				get { return mIsUserInput; }
				set
				{
					mIsUserInput = value;
				}
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
						mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
						if (mOnValueChanged != null) mOnValueChanged();
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
							mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
							if (mOnValueChanged != null) mOnValueChanged();
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
							mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
							if (mOnValueChanged != null) mOnValueChanged();
						}
					}
				}
			}

			/// <summary>
			/// Шаг приращения числовой величины
			/// </summary>
			public Single StepValue
			{
				get { return mStepValue; }
				set
				{
					mStepValue = value;
				}
			}

			/// <summary>
			/// Нормализованное значение числовой величины в частях от 0 до 1
			/// </summary>
			public Single Percent
			{
				get { return (mNumberValue - mMinValue) / (mMaxValue - mMinValue); }
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
						mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
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
						mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
					}
				}
			}

			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Размер управлющих кнопок счетчика
			/// </summary>
			public Single ButtonSize
			{
				get { return mButtonSize; }
				set
				{
					if (mButtonSize != value)
					{
						mButtonSize = value;
						UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Позиция управляющих кнопок счетчика
			/// </summary>
			public TSpinnerButtonLocation ButtonLocation
			{
				get { return mButtonLocation; }
				set
				{
					if (mButtonLocation != value)
					{
						mButtonLocation = value;
						UpdatePlacement();
					}
				}
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о изменении числовой величины
			/// </summary>
			public Action OnValueChanged
			{
				get { return mOnValueChanged; }
				set
				{
					mOnValueChanged = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUISpinner()
				: base()
			{
				mStyleMainName = "TextField";
				mMinValue = 0;
				mMaxValue = 100;
				mStepValue = 1;
				mFormatValue = "F1";
				mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
				mButtonSize = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISpinner(String name)
				: base(name)
			{
				mStyleMainName = "TextField";
				mMinValue = 0;
				mMaxValue = 100;
				mStepValue = 1;
				mFormatValue = "F1";
				mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
				mButtonSize = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISpinner(String name, Single x, Single y)
				: base(name, x, y)
			{
				mStyleMainName = "TextField";
				mMinValue = 0;
				mMaxValue = 100;
				mStepValue = 1;
				mFormatValue = "F1";
				mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
				mButtonSize = 30;
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
			public CGUISpinner(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mStyleMainName = "TextField";
				mMinValue = 0;
				mMaxValue = 100;
				mStepValue = 1;
				mFormatValue = "F1";
				mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
				mButtonSize = 30;
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
				base.UpdatePlacement();

				switch (mButtonLocation)
				{
					case TSpinnerButtonLocation.RightVertical:
					case TSpinnerButtonLocation.LeftVertical:
					case TSpinnerButtonLocation.BothSide:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenX;
						}
						break;
					case TSpinnerButtonLocation.TopHorizontal:
					case TSpinnerButtonLocation.BottomHorizontal:
						{
							mButtonSizeCurrent = mButtonSize * LotusGUIDispatcher.ScaledScreenY;
						}
						break;
					default:
						break;
				}

				switch (mButtonLocation)
				{
					case TSpinnerButtonLocation.RightVertical:
						{
							// Смещаем ширину поля на ширину управляющих кнопок
							mRectWorldScreenMain.width -= mButtonSizeCurrent;

							// Кнопка вверх
							mRectWorldScreenButtonUp.x = mRectWorldScreenMain.xMax;
							mRectWorldScreenButtonUp.y = mRectWorldScreenMain.y;
							mRectWorldScreenButtonUp.width = mButtonSizeCurrent;
							mRectWorldScreenButtonUp.height = mRectWorldScreenMain.height / 2;

							// Кнопка вниз
							mRectWorldScreenButtonDown.x = mRectWorldScreenMain.xMax;
							mRectWorldScreenButtonDown.y = mRectWorldScreenMain.y + mRectWorldScreenMain.height / 2;
							mRectWorldScreenButtonDown.width = mButtonSizeCurrent;
							mRectWorldScreenButtonDown.height = mRectWorldScreenMain.height / 2;
						}
						break;
					case TSpinnerButtonLocation.LeftVertical:
						{
							// Смещаем позицию поля на ширину управляющих кнопок
							mRectWorldScreenMain.x += mButtonSizeCurrent;
							mRectWorldScreenMain.width -= mButtonSizeCurrent;

							// Кнопка вверх
							mRectWorldScreenButtonUp.x = mRectWorldScreenMain.x - mButtonSizeCurrent;
							mRectWorldScreenButtonUp.y = mRectWorldScreenMain.y;
							mRectWorldScreenButtonUp.width = mButtonSizeCurrent;
							mRectWorldScreenButtonUp.height = mRectWorldScreenMain.height / 2;

							// Кнопка вниз
							mRectWorldScreenButtonDown.x = mRectWorldScreenMain.x - mButtonSizeCurrent;
							mRectWorldScreenButtonDown.y = mRectWorldScreenMain.y + mRectWorldScreenMain.height / 2;
							mRectWorldScreenButtonDown.width = mButtonSizeCurrent;
							mRectWorldScreenButtonDown.height = mRectWorldScreenMain.height / 2;
						}
						break;
					case TSpinnerButtonLocation.BothSide:
						{
							// Смещаем позицию поля на ширину управляющих кнопок
							mRectWorldScreenMain.x += mButtonSizeCurrent;

							// Смещаем ширину поля на ширину управляющих кнопок
							mRectWorldScreenMain.width -= mButtonSizeCurrent * 2;

							// Кнопка вверх
							mRectWorldScreenButtonUp.x = mRectWorldScreenMain.xMax;
							mRectWorldScreenButtonUp.y = mRectWorldScreenMain.y;
							mRectWorldScreenButtonUp.width = mButtonSizeCurrent;
							mRectWorldScreenButtonUp.height = mRectWorldScreenMain.height;

							// Кнопка вниз
							mRectWorldScreenButtonDown.x = mRectWorldScreenMain.x - mButtonSizeCurrent;
							mRectWorldScreenButtonDown.y = mRectWorldScreenMain.y;
							mRectWorldScreenButtonDown.width = mButtonSizeCurrent;
							mRectWorldScreenButtonDown.height = mRectWorldScreenMain.height;
						}
						break;
					case TSpinnerButtonLocation.TopHorizontal:
						{
							// Смещаем позицию поля на ширину управляющих кнопок
							mRectWorldScreenMain.y += mButtonSizeCurrent;

							// Смещаем ширину поля на ширину управляющих кнопок
							mRectWorldScreenMain.height -= mButtonSizeCurrent;

							// Кнопка вверх
							mRectWorldScreenButtonUp.x = mRectWorldScreenMain.x + mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonUp.y = mRectWorldScreenMain.y - mButtonSizeCurrent;
							mRectWorldScreenButtonUp.width = mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonUp.height = mButtonSizeCurrent;

							// Кнопка вниз
							mRectWorldScreenButtonDown.x = mRectWorldScreenMain.x;
							mRectWorldScreenButtonDown.y = mRectWorldScreenMain.y - mButtonSizeCurrent;
							mRectWorldScreenButtonDown.width = mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonDown.height = mButtonSizeCurrent;
						}
						break;
					case TSpinnerButtonLocation.BottomHorizontal:
						{
							// Смещаем высоту поля на ширину управляющих кнопок
							mRectWorldScreenMain.height -= mButtonSizeCurrent;

							// Кнопка вверх
							mRectWorldScreenButtonUp.x = mRectWorldScreenMain.x + mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonUp.y = mRectWorldScreenMain.yMax;
							mRectWorldScreenButtonUp.width = mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonUp.height = mButtonSizeCurrent;

							// Кнопка вниз
							mRectWorldScreenButtonDown.x = mRectWorldScreenMain.x;
							mRectWorldScreenButtonDown.y = mRectWorldScreenMain.yMax;
							mRectWorldScreenButtonDown.width = mRectWorldScreenMain.width / 2;
							mRectWorldScreenButtonDown.height = mButtonSizeCurrent;
						}
						break;
					default:
						break;
				}
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

				if (mIsReadOnly)
				{
					GUI.Label(mRectWorldScreenMain, mText, mStyleMain);
				}
				else
				{
					if (mIsUserInput)
					{
						GUI.SetNextControlName(mName);
						GUI.changed = false;
						mText = GUI.TextField(mRectWorldScreenMain, mText, mMaxLength, mStyleMain);
						if (GUI.changed)
						{
							if (mOnTextChanged != null) mOnTextChanged();
						}

						if (GUI.GetNameOfFocusedControl() == mName && mIsFocused == false)
						{
							mIsFocused = true;
						}

						if (GUI.GetNameOfFocusedControl() != mName && mIsFocused)
						{
							mIsFocused = false;
							Single value;
							if (Single.TryParse(mText, out value))
							{
								mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
								mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
								if (mOnValueChanged != null) mOnValueChanged();
							}
						}

						// При нажатии Return происходит потеря фокуса
						if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == mName)
						{
							GUIUtility.keyboardControl = 0;
							Event.current.Use();

							Single value;
							if (Single.TryParse(mText, out value))
							{
								mNumberValue = Mathf.Clamp(value, mMinValue, mMaxValue);
								mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
								if (mOnValueChanged != null) mOnValueChanged();
							}
						}
					}
					else
					{
						GUI.Label(mRectWorldScreenMain, mText, mStyleMain);
					}
				}

				// Кнопка вверх
				if (mButtonLocation == TSpinnerButtonLocation.LeftVertical || mButtonLocation == TSpinnerButtonLocation.RightVertical)
				{
					if (GUI.Button(mRectWorldScreenButtonUp, XString.TriangleUp))
					{
						mNumberValue = Mathf.Clamp(mNumberValue + mStepValue, mMinValue, mMaxValue);
						mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
						if (mOnValueChanged != null) mOnValueChanged();
					}

					// Кнопка вниз
					if (GUI.Button(mRectWorldScreenButtonDown, XString.TriangleDown))
					{
						mNumberValue = Mathf.Clamp(mNumberValue - mStepValue, mMinValue, mMaxValue);
						mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
						if (mOnValueChanged != null) mOnValueChanged();
					}
				}
				else
				{
					if (GUI.Button(mRectWorldScreenButtonUp, XString.TriangleRight))
					{
						mNumberValue = Mathf.Clamp(mNumberValue + mStepValue, mMinValue, mMaxValue);
						mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
						if (mOnValueChanged != null) mOnValueChanged();
					}

					// Кнопка вниз
					if (GUI.Button(mRectWorldScreenButtonDown, XString.TriangleLeft))
					{
						mNumberValue = Mathf.Clamp(mNumberValue - mStepValue, mMinValue, mMaxValue);
						mText = mNumberValue.ToStringFormat(mFormatValue) + mSuffixValue;
						if (mOnValueChanged != null) mOnValueChanged();
					}
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
				return MemberwiseClone() as CGUISpinner;
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

				CGUISpinner spinner = base_element as CGUISpinner;
				if (spinner != null)
				{
					mNumberValue = spinner.mNumberValue;
					mMinValue = spinner.mMinValue;
					mMaxValue = spinner.mMaxValue;
					mFormatValue = spinner.mFormatValue;
					mSuffixValue = spinner.mSuffixValue;
					mButtonSize = spinner.mButtonSize;
					mButtonLocation = spinner.mButtonLocation;
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