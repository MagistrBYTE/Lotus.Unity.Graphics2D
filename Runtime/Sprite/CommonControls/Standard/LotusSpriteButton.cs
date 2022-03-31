//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteButton.cs
*		Button - управляющий элемент типа кнопки.
*		Реализация элемента обеспечивающего стандартный механизм взаимодействия с пользователем посредством
*	щелчка/нажатия на кнопке, механизм постоянного вызова события при удержании кнопки и механизм события при длительном
*	удержания кнопки нажатой.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
using UnityEngine.Events;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DSpriteControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип события для кнопки с учетом получения источника события
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CButtonSpriteClickedEvent : UnityEvent<LotusSpriteButton>
		{
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Button - управляющий элемент типа кнопки
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XSpriteEditorSettings.MenuPathCommon + "Button")]
		public class LotusSpriteButton : LotusSpriteControl, ILotusVirtualButton, ILotusPointerDown, ILotusPointerUp
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента Button
			/// </summary>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public new static LotusSpriteButton Create(Single left, Single top, Single width, Single height, Transform parent = null)
			{
				// 1) Создание объекта
				LotusSpriteButton element = LotusSpriteDispatcher.CreateElement<LotusSpriteButton>("Button", left, top, width, height);

				// 2) Определение в иерархии
				element.SetParent(parent);

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsImmediate;
			[SerializeField]
			internal Single mIntervalImmediate;
			[SerializeField]
			internal LotusSpriteButtonGroup mButtonGroup;

			// События
			[SerializeField]
			internal CButtonSpriteClickedEvent mOnClickEx;
			[NonSerialized]
			internal Action mOnDown;
			[NonSerialized]
			internal Action mOnUp;

			// Служебные данные
			[NonSerialized]
			internal Boolean mIsPressed;
			[NonSerialized]
			internal Int32 mLastPressedFrame = -5;
			[NonSerialized]
			internal Int32 mReleasedFrame = -5;
			[NonSerialized]
			internal Single mTimeImmediate;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Режим постоянного вызова события при нажатии на кнопку
			/// </summary>
			public Boolean IsImmediate
			{
				get { return mIsImmediate; }
				set
				{
					if (mIsImmediate != value)
					{
						mIsImmediate = value;
					}
				}
			}

			/// <summary>
			/// Интервал при вызове события (в секундах )
			/// </summary>
			public Single IntervalImmediate
			{
				get { return mIntervalImmediate; }
				set
				{
					mIntervalImmediate = value;
				}
			}

			/// <summary>
			/// Группа которой принадлежит данный кнопка
			/// </summary>
			public LotusSpriteButtonGroup ButtonGroup
			{
				get { return mButtonGroup; }
				set { mButtonGroup = value; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о щелчке на кнопку. Аргумент - источник события
			/// </summary>
			public CButtonSpriteClickedEvent OnClickEx
			{
				get { return mOnClickEx; }
				set { mOnClickEx = value; }
			}

			/// <summary>
			/// Событие для нотификации о нажатии на кнопке
			/// </summary>
			public Action OnDown
			{
				get { return mOnDown; }
				set { mOnDown = value; }
			}

			/// <summary>
			/// Событие для нотификации о отпускании кнопки
			/// </summary>
			public Action OnUp
			{
				get { return mOnUp; }
				set { mOnUp = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА IVirtualButton ===================================
			/// <summary>
			/// Статус удержания нажатой кнопки
			/// </summary>
			public Boolean IsButtonPressed
			{
				get { return mIsPressed; }
			}

			/// <summary>
			/// Статус нажатия кнопки
			/// </summary>
			public Boolean IsButtonDown
			{
				get { return mLastPressedFrame - Time.frameCount == -1; }
			}

			/// <summary>
			/// Статус отпускания кнопки
			/// </summary>
			public Boolean IsButtonUp
			{
				get { return mReleasedFrame == Time.frameCount - 1; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация элемента при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ResetElement()
			{
				base.ResetElement();
				if (mOnClickEx == null) mOnClickEx = new CButtonSpriteClickedEvent();
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ConstructorElement()
			{
				base.ConstructorElement();
				XInputDispatcher.RegisterPointerDown(this);
				XInputDispatcher.RegisterPointerUp(this);
				if (mOnClickEx == null) mOnClickEx = new CButtonSpriteClickedEvent();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void DestructorElement()
			{
				base.DestructorElement();
				XInputDispatcher.UnRegisterPointerDown(this);
				XInputDispatcher.UnRegisterPointerUp(this);
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusPointer ======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие нажатия
			/// </summary>
			/// <param name="pointer_id">Идентификатор указателя</param>
			//---------------------------------------------------------------------------------------------------------
			public void PointerDown(Int32 pointer_id)
			{
				// Проверка на вхождение
				if(ContainsScreen(XInputDispatcher.PositionPointer))
				{
					// Только если элемент доступен 
					if (mIsEnabled)
					{
						if (mInteractiveSource != TInteractiveSource.Manual)
						{
							switch (mInteractiveMode)
							{
								case TInteractiveMode.None:
									break;
								case TInteractiveMode.Button:
									{
										mIsVisualActive = true;
										SetActivedEffect(TInteractiveState.Pressed);
									}
									break;
								case TInteractiveMode.Toogle:
									{
										if (mButtonGroup != null && mButtonGroup.IsAllOff == false)
										{
											if (mIsVisualActive == false)
											{
												mIsVisualActive = true;
												SetActivedEffect(TInteractiveState.Selected);
											}
										}
										else
										{
											mIsVisualActive = !mIsVisualActive;

											if (mIsVisualActive)
											{
												SetActivedEffect(TInteractiveState.Selected);
											}
											else
											{
												SetDeactivedEffect(TInteractiveState.Selected);
											}
										}
									}
									break;
								default:
									break;
							}
						}

						mLastPressedFrame = Time.frameCount;
					}

					mIsPressed = true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие отпускания
			/// </summary>
			/// <param name="pointer_id">Идентификатор указателя</param>
			//---------------------------------------------------------------------------------------------------------
			public void PointerUp(Int32 pointer_id)
			{
				if (mIsEnabled && mIsPressed)
				{
					if (mInteractiveSource != TInteractiveSource.Manual)
					{
						switch (mInteractiveMode)
						{
							case TInteractiveMode.None:
								break;
							case TInteractiveMode.Button:
								{
									mIsVisualActive = false;
									SetDeactivedEffect(TInteractiveState.Pressed);
								}
								break;
							case TInteractiveMode.Toogle:
								{

								}
								break;
							default:
								break;
						}
					}

					mReleasedFrame = Time.frameCount;
				}

				mIsPressed = false;

				if (mOnUp != null) mOnUp();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================