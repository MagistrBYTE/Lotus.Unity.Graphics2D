//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные эффекты
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualEffectResize.cs
*		Визуальный эффект изменения размера элемента.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DCommonVisualEffect
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для объекта который поддерживает визуальный эффект изменения размера
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusVisualEffectResize
		{

		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Визуальный эффект изменения размера элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/VisualEffect/Resize")]
		public class LotusGraphics2DVisualEffectResize : LotusGraphics2DVisualEffectBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal Vector2 mOriginalSize;
			[SerializeField]
			[LotusDisplayName(nameof(DeltaSize))]
			internal Vector2 mDeltaSize;
			[NonSerialized]
			internal ILotusPlaceable2D mPlaceable2D;

			// Служебные данные
			[NonSerialized]
			internal Coroutine mCoroutineBlink;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Оригинальный размер элемента
			/// </summary>
			public Vector2 OriginalSize
			{
				get { return mOriginalSize; }
				set
				{
					mOriginalSize = value;
				}
			}

			/// <summary>
			/// Дельта изменения размеров элемента
			/// </summary>
			public Vector2 DeltaSize
			{
				get { return mDeltaSize; }
				set
				{
					mDeltaSize = value;
				}
			}

			/// <summary>
			/// Целевой размер элемента
			/// </summary>
			public Vector2 TargetSize
			{
				get { return (mOriginalSize + mDeltaSize); }
			}

			/// <summary>
			/// Интерфейс адаптивного элемента
			/// </summary>
			public ILotusPlaceable2D Placeable2D
			{
				get { return (mPlaceable2D); }
				set
				{
					mPlaceable2D = value;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установить оригинальное значение эффекта на основании текущего значения элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetOriginalValue()
			{
				mPlaceable2D = mElement as ILotusPlaceable2D;
				if (mPlaceable2D == null)
				{
					if (mElement != null)
					{
						mPlaceable2D = mElement.GetComponent<ILotusPlaceable2D>();
					}
					else
					{
						mPlaceable2D = this.GetComponent<ILotusPlaceable2D>();
					}
				}
				if (mPlaceable2D != null)
				{
					mOriginalSize = mPlaceable2D.Size;
				}
				else
				{
					Debug.LogErrorFormat("Element <{0}> does not support interface ILotusPlaceable2D", mElement);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта активации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetActivedEffect()
			{
				switch (mEffectMode)
				{
					case TVisualEffectMode.Swap:
						{
							mPlaceable2D.Size = TargetSize;
						}
						break;
					case TVisualEffectMode.Interpolation:
						{
							IEnumerator routine = mPlaceable2D.ResizeToLocalDesignLinearIteration(mEffectDuration, TargetSize);
							StartCoroutine(routine);
						}
						break;
					case TVisualEffectMode.Blink:
						{
							StartBlinkEffect();
						}
						break;
					case TVisualEffectMode.Loop:
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта деактивации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetDeactivedEffect()
			{
				switch (mEffectMode)
				{
					case TVisualEffectMode.Swap:
						{
							mPlaceable2D.Size = OriginalSize;
						}
						break;
					case TVisualEffectMode.Interpolation:
						{
							IEnumerator routine = mPlaceable2D.ResizeToLocalDesignLinearIteration(mEffectDuration, OriginalSize);
							StartCoroutine(routine);
						}
						break;
					case TVisualEffectMode.Blink:
						{
							StopBlinkEffect();
						}
						break;
					case TVisualEffectMode.Loop:
						break;
					default:
						break;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОДПРОГРАММ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт мигания
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartBlinkEffect()
			{
				mCoroutineBlink = StartCoroutine(BlinkingCoroutine());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Приостановление мигания
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopBlinkEffect()
			{
				StopCoroutine(mCoroutineBlink);
				IEnumerator routine = mPlaceable2D.ResizeToLocalDesignLinearIteration(mEffectDuration, OriginalSize);
				StartCoroutine(routine);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма мигания
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator BlinkingCoroutine()
			{
				while (true)
				{
					IEnumerator routine_active = mPlaceable2D.ResizeToLocalDesignLinearIteration(mEffectDuration, TargetSize);
					StartCoroutine(routine_active);
					yield return new WaitForSeconds(mEffectDuration);

					IEnumerator routine_deactive = mPlaceable2D.ResizeToLocalDesignLinearIteration(mEffectDuration, OriginalSize);
					StartCoroutine(routine_deactive);
					yield return new WaitForSeconds(mEffectDuration);
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