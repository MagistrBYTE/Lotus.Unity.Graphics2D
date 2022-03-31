//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные эффекты
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualEffectReplace.cs
*		Визуальный эффект изменения позиции элемента.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using UnityEngine;
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
		/// Визуальный эффект изменения размера элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/VisualEffect/Replace")]
		public class LotusGraphics2DVisualEffectReplace : LotusGraphics2DVisualEffectBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Vector2 mOriginalPosition;
			[SerializeField]
			internal Vector2 mDeltaPosition;
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
			/// Оригинальная позиция элемента
			/// </summary>
			public Vector2 OriginalPosition
			{
				get { return mOriginalPosition; }
				set
				{
					mOriginalPosition = value;
				}
			}

			/// <summary>
			/// Дельта изменения позиции элемента
			/// </summary>
			public Vector2 DeltaPosition
			{
				get { return mDeltaPosition; }
				set
				{
					mDeltaPosition = value;
				}
			}

			/// <summary>
			/// Целевая позиция элемента
			/// </summary>
			public Vector2 TargetPosition
			{
				get { return (mOriginalPosition + mDeltaPosition); }
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
					mOriginalPosition = mPlaceable2D.Location;
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
							mPlaceable2D.Location = TargetPosition;
						}
						break;
					case TVisualEffectMode.Interpolation:
						{
							IEnumerator routine = mPlaceable2D.MoveToLocalDesignBackInIteration(mEffectDuration, TargetPosition);
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
							mPlaceable2D.Location = OriginalPosition;
						}
						break;
					case TVisualEffectMode.Interpolation:
						{
							IEnumerator routine = mPlaceable2D.MoveToLocalDesignBackInIteration(mEffectDuration, OriginalPosition);
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
				IEnumerator routine = mPlaceable2D.MoveToLocalDesignLinearIteration(mEffectDuration, OriginalPosition);
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
					IEnumerator routine_active = mPlaceable2D.MoveToLocalDesignLinearIteration(mEffectDuration, TargetPosition);
					StartCoroutine(routine_active);
					yield return new WaitForSeconds(mEffectDuration);

					IEnumerator routine_deactive = mPlaceable2D.MoveToLocalDesignLinearIteration(mEffectDuration, OriginalPosition);
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