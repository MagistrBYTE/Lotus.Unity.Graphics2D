//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteControl.cs
*		Определение типов и структур данных для управляющих элементов предназначенных для взаимодействия с пользователем.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DSpriteControls Общие элементы интерфейса
		//! Общие управляющие элементы предназначенные для взаимодействия с пользователем
		//! \ingroup Unity2DSprite
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Control - базовый компонент для реализации управляющего элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XSpriteEditorSettings.MenuPathCommon + "Control")]
		public class LotusSpriteControl : LotusSpriteElement, ILotusInteractive
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента Control
			/// </summary>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public new static LotusSpriteControl Create(Single left, Single top, Single width, Single height, Transform parent = null)
			{
				// 1) Создание объекта
				LotusSpriteControl element = LotusSpriteDispatcher.CreateElement<LotusSpriteControl>("Control", left, top, width, height);

				// 2) Определение в иерархии
				element.SetParent(parent);

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal TInteractiveSource mInteractiveSource;
			[SerializeField]
			internal TInteractiveMode mInteractiveMode;
			[SerializeField]
			internal Single mEffectDuration = 0.3f;
			[NonSerialized]
			internal Boolean mIsVisualActive;
			[NonSerialized]
			internal List<ILotusVisualEffect> mVisualEffects;
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
					if (mIsEnabled)
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
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ConstructorElement()
			{
				base.ConstructorElement();

				if (mVisualEffects == null) mVisualEffects = new List<ILotusVisualEffect>();

				LotusGraphics2DVisualEffectBase[] effects = this.GetComponents<LotusGraphics2DVisualEffectBase>();
				if(effects != null && effects.Length > 0)
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

			#region ======================================= МЕТОДЫ ILotusVisibility ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавное скрытие элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void Hide()
			{
				Hide(mEffectDuration);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный показ элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void Show()
			{
				Show(mEffectDuration);
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение источника инициализации процесса реагирования.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseInteractiveSourceChanged()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение типа визуального реагирование.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseInteractiveModeChanged()
			{
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
					if(mVisualEffects[i].InteractiveState == state)
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
				if (mIsEnabled)
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
				if (mIsEnabled)
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
				if (mIsEnabled)
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
				if (mIsEnabled)
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