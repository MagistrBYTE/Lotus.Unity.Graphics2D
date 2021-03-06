//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные эффекты
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualEffectBase.cs
*		Определение базового класса для реализации визуальных эффектов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
using TMPro;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DCommonVisualEffect Визуальные эффекты
		//! Визуальные эффекты представляют собой различный эффекты, связанные со всем аспектами отображения элемента 
		//! включая его размер и местоположение, которые обусловлены действиями пользователя или программными событиями
		//! \ingroup Unity2DCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Режим выполнения визуального эффекта
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TVisualEffectMode
		{
			/// <summary>
			/// Визуальный эффект сменяется мгновенно
			/// </summary>
			Swap,

			/// <summary>
			/// Плавное выполнение визуального эффекта от начального до конечного значения
			/// </summary>
			Interpolation,

			/// <summary>
			/// Режим мигания визуального эффекта при его активации
			/// </summary>
			/// <remarks>
			/// Применяется только для определённых эффектов
			/// </remarks>
			Blink,

			/// <summary>
			/// Режим постоянного применения визуального эффекта при его активации
			/// </summary>
			/// <remarks>
			/// Применяется только для определённых эффектов
			/// </remarks>
			Loop
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый компонент для определения визуальных эффектов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/VisualEffect/Base")]
		public class LotusGraphics2DVisualEffectBase : MonoBehaviour, ILotusVisualEffect
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[LotusDisplayName(nameof(InteractiveState))]
			internal TInteractiveState mInteractiveState;
			[SerializeField]
			[LotusDisplayName(nameof(EffectDuration))]
			internal Single mEffectDuration = 0.3f;
			[SerializeField]
			[LotusDisplayName(nameof(EffectMode))]
			internal TVisualEffectMode mEffectMode;
			[SerializeField]
			[LotusDisplayName(nameof(EasingType))]
			internal TEasingType mEasingType;
			[NonSerialized]
			internal Boolean mIsVisualActive;
			[NonSerialized]
			internal MonoBehaviour mElement;

			[NonSerialized]
			internal TextMeshProUGUI mTextMeshProUGU;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Состояния для реагирования на действия пользователя
			/// </summary>
			public TInteractiveState InteractiveState
			{
				get { return mInteractiveState; }
				set
				{
					mInteractiveState = value;
				}
			}

			/// <summary>
			/// Продолжительность визуального эффекта элемента, в секундах
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
			/// Режим выполнения визуального эффекта элемента
			/// </summary>
			public TVisualEffectMode EffectMode
			{
				get { return mEffectMode; }
				set
				{
					mEffectMode = value;
				}
			}

			/// <summary>
			/// Режим выполнения визуального эффекта элемента
			/// </summary>
			public TEasingType EasingType
			{
				get { return mEasingType; }
				set
				{
					mEasingType = value;
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
					if (mIsVisualActive != value)
					{
						mIsVisualActive = value;
						if (mIsVisualActive)
						{
							SetActivedEffect();
						}
						else
						{
							SetDeactivedEffect();
						}
					}
				}
			}

			/// <summary>
			/// Элемент для применения визуального эффекта
			/// </summary>
			public MonoBehaviour Element
			{
				get { return mElement; }
				set
				{
					mElement = value;
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				SetOriginalValue();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установить оригинальное значение эффекта на основании текущего значения элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetOriginalValue()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта активации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetActivedEffect()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта деактивации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetDeactivedEffect()
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