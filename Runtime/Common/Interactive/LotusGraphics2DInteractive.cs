//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Подсистема интерактивности
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DInteractive.cs
*		Функциональной компонент определяющий режим взаимодействия и визуальное реагирование на действия пользователя.
*		Реализация компонента определяющего режим и модель взаимодействия, а также визуальные реагирование на действия
*	пользователя через соответствующие(управляемые) компоненты изображения и текста.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DCommonInteractive
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Функциональной компонент определяющий режим взаимодействия и визуальное реагирование на действия пользователя
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего режим и модель взаимодействия, а также визуальные реагирование на действия
		/// пользователя через соответствующие(управляемые) компоненты изображения и текста
		/// </remarks>
		//------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/Interactive/Interactive")]
		public class LotusGraphics2DInteractive : MonoBehaviour, ILotusInteractive, IPointerDownHandler, IPointerUpHandler
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[LotusDisplayName(nameof(InteractiveSource))]
			internal TInteractiveSource mInteractiveSource;
			[SerializeField]
			[LotusDisplayName(nameof(mInteractiveMode))]
			internal TInteractiveMode mInteractiveMode;
			[SerializeField]
			[LotusDisplayName(nameof(EffectDuration))]
			[Range(0.2f, 5)]
			internal Single mEffectDuration = 0.3f;
			[NonSerialized]
			internal Boolean mIsVisualActive;
			[NonSerialized]
			internal List<ILotusVisualEffect> mVisualEffects;

			// События
			[NonSerialized]
			internal Action<Boolean> mOnVisualActived;
            #endregion

            #region ======================================= СВОЙСТВА ==================================================
            //
            // ОСНОВНЫЕ ПАРАМЕТРЫ
            //
            /// <summary>
            /// Источник инициализации процесса реагирования
            /// </summary>
            public TInteractiveSource InteractiveSource
            {
                get { return mInteractiveSource; }
                set
                {
                    if (mInteractiveSource != value)
                    {
                        mInteractiveSource = value;

                        // Информируем
                        RaiseInteractiveSourceChanged();
                    }
                }
            }

            /// <summary>
            /// Тип визуального реагирования на действия пользователя
            /// </summary>
            public TInteractiveMode InteractiveMode
            {
                get { return mInteractiveMode; }
                set
                {
                    if (mInteractiveMode != value)
                    {
                        mInteractiveMode = value;

                        // Информируем
                        RaiseInteractiveModeChanged();
                    }
                }
            }

            /// <summary>
            /// Продолжительность визуального эффекта элемента
            /// </summary>
            public Single EffectDuration
			{
				get { return mEffectDuration; }
				set
				{
					mEffectDuration = value;
				}
			}

			/// <summary>
			/// Статус визуальной активации элемента
			/// </summary>
			public Boolean IsVisualActive
			{
				get { return mIsVisualActive; }
				set
				{
					// Только если элемент включен
					if (enabled)
					{
						if (mIsVisualActive != value)
						{
							mIsVisualActive = value;
							if (mInteractiveSource != TInteractiveSource.Event && mInteractiveMode == TInteractiveMode.Toogle)
							{
								if (mIsVisualActive)
								{
									SetActivedEffect(TInteractiveState.Selected);
								}
								else
								{
									SetDeactivedEffect(TInteractiveState.Selected);
								}
							}

							if (mOnVisualActived != null) mOnVisualActived(value);
						}
					}
				}
			}

			/// <summary>
			/// Список визуальных эффектов
			/// </summary>
			public List<ILotusVisualEffect> VisualEffects
			{
				get { return mVisualEffects; }
			}

			/// <summary>
			/// Список визуальных эффектов
			/// </summary>
			public IList<ILotusVisualEffect> IEffects
			{
				get { return mVisualEffects; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации об изменение визуальной активности элемента. Аргумент - статус визуальной активности элемента
			/// </summary>
			public Action<Boolean> OnVisualActived
			{
				get { return mOnVisualActived; }
				set { mOnVisualActived = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void Reset()
			{
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				if (mVisualEffects == null) mVisualEffects = new List<ILotusVisualEffect>();

				LotusGraphics2DVisualEffectBase[] effects = this.GetComponentsInChildren<LotusGraphics2DVisualEffectBase>();
				if (effects != null && effects.Length > 0)
				{
					for (Int32 i = 0; i < effects.Length; i++)
					{
						effects[i].Element = this;
						effects[i].SetOriginalValue();
						mVisualEffects.Add(effects[i]);
					}
				}
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение источника инициализации процесса реагирования.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void RaiseInteractiveSourceChanged()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение типа визуального реагирование.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void RaiseInteractiveModeChanged()
			{
			}
			#endregion

			#region ======================================= МЕТОДЫ IEventSystemHandler ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Нажатие на элементе
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnPointerDown(PointerEventData event_data)
			{
				if (enabled)
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
									mIsVisualActive = !mIsVisualActive;
									if (mInteractiveMode == TInteractiveMode.Toogle)
									{
										if (mIsVisualActive)
										{
											SetActivedEffect(TInteractiveState.Selected);
										}
										else
										{
											SetDeactivedEffect(TInteractiveState.Selected);
										}
									}

									if (mOnVisualActived != null) mOnVisualActived(mIsVisualActive);
								}
								break;
							default:
								break;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отпускание кнопки мыши/тача
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnPointerUp(PointerEventData event_data)
			{
				if (enabled)
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

									if (mOnVisualActived != null) mOnVisualActived(mIsVisualActive);
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
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Наведение указателя на область элемента
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnPointerEnter(PointerEventData event_data)
			{
				if (enabled)
				{
					if (mInteractiveSource != TInteractiveSource.Manual)
					{
						SetActivedEffect(TInteractiveState.Hover);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Покидание указателя области элемента
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnPointerExit(PointerEventData event_data)
			{
				if (enabled)
				{
					if (mInteractiveSource != TInteractiveSource.Manual)
					{
						SetDeactivedEffect(TInteractiveState.Hover);
					}
				}
			}
			#endregion

			#region ======================================= ВИЗУАЛЬНЫЕ ЭФФЕКТЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта активации элемента
			/// </summary>
			/// <param name="state">Состояние</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetActivedEffect(TInteractiveState state)
			{
				for (Int32 i = 0; i < mVisualEffects.Count; i++)
				{
					if (mVisualEffects[i].InteractiveState == state)
					{
						mVisualEffects[i].SetActivedEffect();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта деактивации элемента
			/// </summary>
			/// <param name="state">Состояние</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetDeactivedEffect(TInteractiveState state)
			{
				for (Int32 i = 0; i < mVisualEffects.Count; i++)
				{
					if (mVisualEffects[i].InteractiveState == state)
					{
						mVisualEffects[i].SetDeactivedEffect();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Визуальная активация при нажатии на элемент
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void VisualActivedFromDown()
			{
				if (enabled)
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
								break;
							default:
								break;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Визуальная активация при отпускании кнопки мыши/тача над элементом
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void VisualActivedFromUp()
			{
				if (enabled)
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
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Визуальная активация при заходе указателя в область элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void VisualActivedFromEnter()
			{
				if (enabled)
				{
					if (mInteractiveSource != TInteractiveSource.Manual)
					{
						SetActivedEffect(TInteractiveState.Hover);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Визуальная активация при покидании указателя области элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void VisualActivedFromExit()
			{
				if (enabled)
				{
					if (mInteractiveSource != TInteractiveSource.Manual)
					{
						SetDeactivedEffect(TInteractiveState.Hover);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Визуальная активация при перетаскивании элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void VisualActivedFromDragged()
			{
				if (enabled)
				{
					if (mInteractiveSource != TInteractiveSource.Manual)
					{
						SetActivedEffect(TInteractiveState.Dragged);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Визуальная активация при покидании указателя области элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void VisualActivedFromDropped()
			{
				if (enabled)
				{
					if (mInteractiveSource != TInteractiveSource.Manual)
					{
						SetDeactivedEffect(TInteractiveState.Dragged);
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